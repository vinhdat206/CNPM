/*Doanh thu*/
const revenueByYear = {
    2024: [12000000, 15000000, 18000000, 14000000, 21000000, 26000000, 30000000, 28000000, 32000000, 35000000, 40000000, 45000000],
    2025: [18000000, 20000000, 22000000, 25000000, 27000000, 30000000, 33000000, 31000000, 36000000, 39000000, 42000000, 48000000],
    2026: [25000000, 28000000, 30000000, 34000000, 37000000, 40000000, 43000000, 46000000, 50000000, 54000000, 58000000, 62000000]
};

const revenueLabels = [
    'T1', 'T2', 'T3', 'T4', 'T5', 'T6',
    'T7', 'T8', 'T9', 'T10', 'T11', 'T12'
];

const revenueCtx = document.getElementById('revenueChart');
const revenueYearFilter = document.getElementById('revenueYearFilter');

let revenueChart;

if (revenueCtx) {
    revenueChart = new Chart(revenueCtx, {
        type: 'bar', // hoặc 'line'
        data: {
            labels: revenueLabels,
            datasets: [{
                label: 'Doanh thu',
                data: revenueByYear[revenueYearFilter.value],
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function(value) {
                            return value.toLocaleString('vi-VN') + ' ₫';
                        }
                    }
                }
            }
        }
    });
}

if (revenueYearFilter) {
    revenueYearFilter.addEventListener('change', function () {
        const selectedYear = this.value;

        revenueChart.data.datasets[0].data = revenueByYear[selectedYear];
        revenueChart.data.datasets[0].label = 'Doanh thu ' + selectedYear;
        revenueChart.update();
    });
}
/*số đơn hàng theo tháng*/
const ordersLineCtx = document.getElementById('ordersLineChart');

if (ordersLineCtx) {
    new Chart(ordersLineCtx, {
        type: 'line',
        data: {
            labels: window.ordersLineData.map(x => x.month),
            datasets: [{
                label: 'Số đơn hàng',
                data: window.ordersLineData.map(x => x.orders),
                tension: 0.4,
                fill: true,
                borderWidth: 3,
                pointRadius: 5,
                backgroundColor: 'rgba(59, 130, 246, 0.15)',
                borderColor: '#3b82f6',
                pointBackgroundColor: '#3b82f6'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: true
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}