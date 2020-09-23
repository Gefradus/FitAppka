function createChart() {
    Chart.defaults.global.defaultFontColor = 'rgb(255, 255, 255)';
    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Poniedziałek', 'Wtorek', 'Środa', 'Czwartek', 'Piątek', 'Sobota', 'Niedziela'],
            datasets: [{
                label: 'Pomiar wagi',
                data: [90, 89, 89, 89.5, 88.7, 88],
                borderColor: 'rgb(3, 202, 86)',
                backgroundColor: 'rgba(0, 50, 0, 0.0)',
            },

            
            ]
        },
        options: {
            /*scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }*/
        }
    });
}