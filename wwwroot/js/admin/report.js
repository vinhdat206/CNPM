// =====================================
// FILE: wwwroot/js/admin/report.js
// =====================================
// File JavaScript này dùng để vẽ các biểu đồ thống kê trong trang báo cáo admin.
// Các biểu đồ được tạo bằng thư viện Chart.js.
// Dữ liệu biểu đồ được lấy từ các biến global trong window,
// ví dụ: window.topProductNames, window.orderStatusLabels,...


// =============================
// TOP PRODUCT CHART
// BIỂU ĐỒ SẢN PHẨM BÁN CHẠY
// =============================

// Lấy phần tử HTML có id="topProductChart"
// Đây thường là thẻ <canvas> dùng để vẽ biểu đồ
const topProductChart =
    document.getElementById('topProductChart');

// Kiểm tra xem phần tử topProductChart có tồn tại trên trang hay không
// Nếu tồn tại thì mới khởi tạo biểu đồ
if (topProductChart)
{
    // Tạo một biểu đồ mới bằng Chart.js
    new Chart(topProductChart,
        {
            // Loại biểu đồ là bar - biểu đồ cột
            type: 'bar',

            // Phần dữ liệu của biểu đồ
            data:
                {
                    // Nhãn hiển thị trên biểu đồ
                    // Mỗi nhãn là tên của một sản phẩm
                    labels: window.topProductNames,

                    // Danh sách các bộ dữ liệu được hiển thị
                    datasets:
                        [{
                            // Tên dữ liệu hiển thị trong chú giải biểu đồ
                            label: 'Số lượng bán',

                            // Dữ liệu số lượng bán tương ứng với từng sản phẩm
                            data: window.topProductSales,

                            // Độ dày viền của các cột
                            borderWidth: 1
                        }]
                },

            // Phần cấu hình giao diện và hành vi biểu đồ
            options:
                {
                    // Cho phép biểu đồ tự thay đổi kích thước theo màn hình/container
                    responsive: true,

                    // Hiển thị biểu đồ cột ngang
                    // Mặc định bar là cột dọc, indexAxis: 'y' sẽ đổi thành thanh ngang
                    indexAxis: 'y',

                    // Cấu hình các trục của biểu đồ
                    scales:
                        {
                            // Cấu hình trục X
                            x:
                                {
                                    // Trục X bắt đầu từ giá trị 0
                                    beginAtZero: true
                                }
                        }
                }
        });
}


// =============================
// ORDER STATUS CHART
// BIỂU ĐỒ TRẠNG THÁI ĐƠN HÀNG
// =============================

// Lấy phần tử HTML có id="orderStatusChart"
// Đây là canvas dùng để vẽ biểu đồ trạng thái đơn hàng
const orderStatusChart =
    document.getElementById('orderStatusChart');

// Kiểm tra nếu canvas tồn tại thì mới tạo biểu đồ
if (orderStatusChart)
{
    // Khởi tạo biểu đồ trạng thái đơn hàng
    new Chart(orderStatusChart,
        {
            // Loại biểu đồ là doughnut - biểu đồ vòng tròn rỗng ở giữa
            type: 'doughnut',

            // Dữ liệu của biểu đồ
            data:
                {
                    // Nhãn trạng thái đơn hàng
                    // Ví dụ: Chờ xác nhận, Đang giao, Hoàn thành, Đã hủy,...
                    labels: window.orderStatusLabels,

                    datasets:
                        [{
                            // Giá trị tương ứng với từng trạng thái đơn hàng
                            // Ví dụ: số lượng đơn hàng ở mỗi trạng thái
                            data: window.orderStatusValues,

                            // Độ dày viền giữa các phần trong biểu đồ
                            borderWidth: 1
                        }]
                },

            // Cấu hình biểu đồ
            options:
                {
                    // Biểu đồ tự co giãn theo kích thước màn hình/container
                    responsive: true
                }
        });
}


// =============================
// PRODUCT REVENUE CHART
// BIỂU ĐỒ DOANH THU THEO SẢN PHẨM
// =============================

// Lấy phần tử HTML có id="productRevenueChart"
// Đây là canvas dùng để vẽ biểu đồ doanh thu sản phẩm
const productRevenueChart =
    document.getElementById('productRevenueChart');

// Kiểm tra nếu phần tử canvas tồn tại thì mới khởi tạo biểu đồ
if (productRevenueChart)
{
    // Tạo biểu đồ doanh thu sản phẩm
    new Chart(productRevenueChart,
        {
            // Loại biểu đồ là bar - biểu đồ cột
            type: 'bar',

            // Dữ liệu hiển thị trên biểu đồ
            data:
                {
                    // Tên các sản phẩm hiển thị trên trục X
                    labels: window.productRevenueNames,

                    datasets:
                        [{
                            // Tên dataset hiển thị trong chú giải
                            label: 'Doanh thu',

                            // Giá trị doanh thu tương ứng với từng sản phẩm
                            data: window.productRevenueValues,

                            // Độ dày viền của cột
                            borderWidth: 1
                        }]
                },

            // Cấu hình biểu đồ
            options:
                {
                    // Cho phép biểu đồ responsive theo kích thước màn hình/container
                    responsive: true,

                    // Cấu hình các trục
                    scales:
                        {
                            // Cấu hình trục Y
                            y:
                                {
                                    // Trục Y bắt đầu từ 0
                                    beginAtZero: true,

                                    // Cấu hình cách hiển thị giá trị trên trục Y
                                    ticks:
                                        {
                                            // Hàm callback dùng để định dạng từng giá trị trên trục Y
                                            callback: function (value)
                                            {
                                                // Chuyển số sang định dạng tiền Việt Nam
                                                // Ví dụ: 1000000 -> 1.000.000đ
                                                return value.toLocaleString('vi-VN') + 'đ';
                                            }
                                        }
                                }
                        }
                }
        });
}