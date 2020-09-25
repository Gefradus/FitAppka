function createChart(chartType) {
    if (chartType == 0) {
        createWeightChart();
    }
    if (chartType == 1) {
        createKcalConsumedChart();
    }
    if (chartType == 2) {
        createKcalBurnedChart();
    }
}

function createDatePickers(dateFrom, dateTo, chartType) {
    createDatePicker("#datepickerFrom");
    createDatePicker("#datepickerTo");
    setMaxDate("#datepickerTo", new Date());
    setMinDate("#datepickerTo", dateFrom);
    setMaxDate("#datepickerFrom", dateTo);
    setDate("#datepickerTo", dateTo);
    setDate("#datepickerFrom", dateFrom);
    document.getElementById("select").value = chartType;
    setOnChange();
}

function setOnChange() {
    var $j = jQuery.noConflict();
    $j("#select").change(function () {
        var dateFrom = $j("#datepickerFrom").datepicker("getDate").toISOString();
        var dateTo = $j("#datepickerTo").datepicker("getDate").toISOString();
        var chartType = document.getElementById("select").options[select.selectedIndex].value;
        reloadPage(dateFrom, dateTo, chartType);
    });

    $j("#datepickerFrom").change(function () {
        var dateFrom = $j("#datepickerFrom").datepicker("getDate").toISOString();
        var dateTo = $j("#datepickerTo").datepicker("getDate").toISOString();
        var chartType = document.getElementById("select").options[select.selectedIndex].value;
        reloadPage(dateFrom, dateTo, chartType);
    });

    $j("#datepickerTo").change(function () {
        var dateFrom = $j("#datepickerFrom").datepicker("getDate").toISOString();
        var dateTo = $j("#datepickerTo").datepicker("getDate").toISOString();
        var chartType = document.getElementById("select").options[select.selectedIndex].value;
        reloadPage(dateFrom, dateTo, chartType);
    });
}

function setMaxDate(datepicker, max) {
    var $j = jQuery.noConflict();
    $j(datepicker).datepicker('option', 'maxDate', max);
}

function setDate(datepicker, date) {
    var $j = jQuery.noConflict();
    $j(datepicker).datepicker("setDate", date);
}

function setMinDate(datepicker, min) {
    var $j = jQuery.noConflict();
    $j(datepicker).datepicker('option', 'minDate', min);
}

function SubWeekFromToday() {
    var oneWeekAgo = new Date();
    oneWeekAgo.setDate(oneWeekAgo.getDate() - 7);
    return oneWeekAgo;
}


function createWeightChart() {
    defaultChart('Pomiar wagi [kg]', 'rgb(3, 202, 86)', "lightgreen");
}

function createKcalBurnedChart() {
    defaultChart('Spalone kalorie [kcal]', 'rgba(191, 99, 8, 1)', "orange");
}

function createKcalConsumedChart() {
    defaultChart('Spożyte kalorie [kcal]', 'rgba(8, 87, 191, 1)', "lightblue");
}

function defaultChart(label, borderColor, pointBorderColor) {
    Chart.defaults.global.defaultFontColor = 'rgb(255, 255, 255)';
    Chart.defaults.global.elements.point.radius = 4;
    Chart.defaults.global.elements.point.borderWidth = 2;

    var ctx = document.getElementById('myChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: date,
            datasets: [{
                label: label,
                data: data,
                borderColor: borderColor,
                color: 'rgb(0, 0, 0)',
                pointBorderColor: pointBorderColor,
            },
            ]
        },
        options: {
            scales: {
                xAxes: [{ gridLines: { color: 'rgba(197, 201, 206, 0.3)' } }],
                yAxes: [{ gridLines: { color: 'rgba(197, 201, 206, 0.3)' } }]
            }
        }
    });
}



var data = [];
var date = [];