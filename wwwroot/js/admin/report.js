// =====================================
// FILE: wwwroot/js/admin/report.js
// =====================================

// =============================
// TOP PRODUCT CHART
// =============================

const topProductChart =
    document.getElementById('topProductChart');

if (topProductChart)
{
    new Chart(topProductChart,
        {
            type: 'bar',

            data:
                {
                    labels: window.topProductNames,

                    datasets:
                        [{
                            label: 'Số lượng bán',

                            data: window.topProductSales,

                            borderWidth: 1
                        }]
                },

            options:
                {
                    responsive: true,

                    indexAxis: 'y',

                    scales:
                        {
                            x:
                                {
                                    beginAtZero: true
                                }
                        }
                }
        });
}

// =============================
// ORDER STATUS CHART
// =============================

const orderStatusChart =
    document.getElementById('orderStatusChart');

if (orderStatusChart)
{
    new Chart(orderStatusChart,
        {
            type: 'doughnut',

            data:
                {
                    labels: window.orderStatusLabels,

                    datasets:
                        [{
                            data: window.orderStatusValues,

                            borderWidth: 1
                        }]
                },

            options:
                {
                    responsive: true
                }
        });
}

// =============================
// PRODUCT REVENUE CHART
// =============================

const productRevenueChart =
    document.getElementById('productRevenueChart');

if (productRevenueChart)
{
    new Chart(productRevenueChart,
        {
            type: 'bar',

            data:
                {
                    labels: window.productRevenueNames,

                    datasets:
                        [{
                            label: 'Doanh thu',

                            data: window.productRevenueValues,

                            borderWidth: 1
                        }]
                },

            options:
                {
                    responsive: true,

                    scales:
                        {
                            y:
                                {
                                    beginAtZero: true,

                                    ticks:
                                        {
                                            callback: function (value)
                                            {
                                                return value.toLocaleString('vi-VN') + 'đ';
                                            }
                                        }
                                }
                        }
                }
        });
}