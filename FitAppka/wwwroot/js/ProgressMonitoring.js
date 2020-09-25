function createChart(weight, date) {
    var measurements = [];
    var dates = [];
    weight.forEach(w => { measurements.push(w); });
    date.forEach(d => { dates.push(d); })

    Chart.defaults.global.defaultFontColor = 'rgb(255, 255, 255)';
    Chart.defaults.global.elements.point.radius = 4;
    Chart.defaults.global.elements.point.borderWidth = 2;
    
    var ctx = document.getElementById('myChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: dates,
            datasets: [{
                label: 'Pomiar wagi [kg]',
                data: measurements,
                borderColor: 'rgb(3, 202, 86)',
                backgroundColor: 'rgba(0, 50, 0, 0.0)',
                color: 'rgb(0, 0, 0)',
                pointBorderColor: "lightgreen",
            },
            ]
        },
        options: { 
            scales: {
                xAxes: [{ gridLines: { color: 'rgba(197, 201, 206, 0.3)' }}],
                yAxes: [{ gridLines: { color: 'rgba(197, 201, 206, 0.3)' }}]
            }
        }
    });
}

var weight = [];
var date = [];