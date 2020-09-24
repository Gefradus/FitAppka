function createChart(weight, date) {
    var measurements = [];
    var dates = [];
    weight.forEach(w => {
        measurements.push(parseInt(w));
    });
    date.forEach(d => {
        dates.push(d);
    })


    Chart.defaults.global.defaultFontColor = 'rgb(255, 255, 255)';
    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: dates,
            datasets: [{
                label: 'Pomiar wagi',
                data: measurements,
                borderColor: 'rgb(3, 202, 86)',
                backgroundColor: 'rgba(0, 50, 0, 0.0)',
                color: 'rgb(0, 0, 0)'
            },
            ]
        },
        options: {
            scales: {
                xAxes: [{
                    gridLines: {
                        display: true,
                        color: 'rgba(197, 201, 206, 0.3)',
                        lineWidth: 1
                    }
                }],
                yAxes: [{
                    gridLines: {
                        display: true,
                        color: 'rgba(197, 201, 206, 0.3)',
                        lineWidth: 1
                    }
                }]
            }
        }
    });
}

var weight = [];
var date = [];