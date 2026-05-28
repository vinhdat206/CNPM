/* =========================
   BIỂU ĐỒ DOANH THU
========================= */

// Dữ liệu doanh thu theo từng năm
const revenueByYear = {
    // Doanh thu 12 tháng của năm 2024
    2024: [12000000, 15000000, 18000000, 14000000, 21000000, 26000000, 30000000, 28000000, 32000000, 35000000, 40000000, 45000000],

    // Doanh thu 12 tháng của năm 2025
    2025: [18000000, 20000000, 22000000, 25000000, 27000000, 30000000, 33000000, 31000000, 36000000, 39000000, 42000000, 48000000],

    // Doanh thu 12 tháng của năm 2026
    2026: [25000000, 28000000, 30000000, 34000000, 37000000, 40000000, 43000000, 46000000, 50000000, 54000000, 58000000, 62000000]
};

// Nhãn cho trục X, tương ứng từ tháng 1 đến tháng 12
const revenueLabels = [
    'T1', 'T2', 'T3', 'T4', 'T5', 'T6',
    'T7', 'T8', 'T9', 'T10', 'T11', 'T12'
];

// Lấy phần tử canvas dùng để vẽ biểu đồ doanh thu
const revenueCtx = document.getElementById('revenueChart');

// Lấy phần tử select dùng để lọc doanh thu theo năm
const revenueYearFilter = document.getElementById('revenueYearFilter');

// Khai báo biến lưu đối tượng biểu đồ doanh thu
let revenueChart;

// Kiểm tra nếu canvas tồn tại thì mới khởi tạo biểu đồ
if (revenueCtx) {
    revenueChart = new Chart(revenueCtx, {
        // Loại biểu đồ: bar là biểu đồ cột
        // Có thể đổi thành 'line' nếu muốn biểu đồ đường
        type: 'bar',

        data: {
            // Nhãn hiển thị trên trục X
            labels: revenueLabels,

            datasets: [{
                // Tên dữ liệu hiển thị trong chú giải
                label: 'Doanh thu',

                // Dữ liệu doanh thu theo năm đang được chọn trong select
                data: revenueByYear[revenueYearFilter.value],

                // Độ dày viền của cột
                borderWidth: 2
            }]
        },

        options: {
            // Biểu đồ tự co giãn theo kích thước màn hình
            responsive: true,

            // Không giữ tỉ lệ mặc định, giúp tùy chỉnh chiều cao tốt hơn
            maintainAspectRatio: false,

            scales: {
                y: {
                    // Trục Y bắt đầu từ 0
                    beginAtZero: true,

                    ticks: {
                        // Định dạng số tiền trên trục Y
                        callback: function(value) {
                            // Chuyển số sang định dạng Việt Nam và thêm ký hiệu tiền tệ
                            return value.toLocaleString('vi-VN') + ' ₫';
                        }
                    }
                }
            }
        }
    });
}

// Kiểm tra nếu bộ lọc năm tồn tại
if (revenueYearFilter) {
    // Lắng nghe sự kiện khi người dùng thay đổi năm
    revenueYearFilter.addEventListener('change', function () {
        // Lấy năm đang được chọn
        const selectedYear = this.value;

        // Cập nhật dữ liệu biểu đồ theo năm được chọn
        revenueChart.data.datasets[0].data = revenueByYear[selectedYear];

        // Cập nhật tên dataset
        revenueChart.data.datasets[0].label = 'Doanh thu ' + selectedYear;

        // Vẽ lại biểu đồ sau khi thay đổi dữ liệu
        revenueChart.update();
    });
}


/* =========================
   BIỂU ĐỒ SỐ ĐƠN HÀNG THEO THÁNG
========================= */

// Lấy phần tử canvas dùng để vẽ biểu đồ đường số đơn hàng
const ordersLineCtx = document.getElementById('ordersLineChart');

// Kiểm tra nếu canvas tồn tại thì mới khởi tạo biểu đồ
if (ordersLineCtx) {
    new Chart(ordersLineCtx, {
        // Loại biểu đồ: line là biểu đồ đường
        type: 'line',

        data: {
            // Lấy danh sách tháng từ biến window.ordersLineData
            labels: window.ordersLineData.map(x => x.month),

            datasets: [{
                // Tên dữ liệu hiển thị trong chú giải
                label: 'Số đơn hàng',

                // Lấy số đơn hàng tương ứng từng tháng
                data: window.ordersLineData.map(x => x.orders),

                // Làm đường biểu đồ cong mềm hơn
                tension: 0.4,

                // Tô màu vùng bên dưới đường biểu đồ
                fill: true,

                // Độ dày của đường biểu đồ
                borderWidth: 3,

                // Kích thước các điểm dữ liệu trên đường
                pointRadius: 5,

                // Màu nền vùng bên dưới đường biểu đồ
                backgroundColor: 'rgba(59, 130, 246, 0.15)',

                // Màu đường biểu đồ
                borderColor: '#3b82f6',

                // Màu của các điểm dữ liệu
                pointBackgroundColor: '#3b82f6'
            }]
        },

        options: {
            // Biểu đồ tự co giãn theo kích thước màn hình
            responsive: true,

            plugins: {
                legend: {
                    // Hiển thị chú giải của biểu đồ
                    display: true
                }
            },

            scales: {
                y: {
                    // Trục Y bắt đầu từ 0
                    beginAtZero: true
                }
            }
        }
    });
}