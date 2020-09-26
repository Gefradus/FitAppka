function createChart(chartType) {
    if (chartType == 0) {
        createKcalConsumedChart();  
    }
    if (chartType == 1) {
        createKcalBurnedChart();
    }
    if (chartType == 2) {
        createWeightChart();
    }
    if (chartType == 3) {
        createWaistCircumferenceChart();
    }
    if (chartType == 4) {
        createBodyFatChart();
    }
    if (chartType == 5) {
        createWaterConsumedChart();
    }
    if (chartType == 6) {
        createTrainingTimeChart();
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
    $j("#select, #datepickerFrom, #datepickerTo").change(function () {
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
    defaultChart('Pomiar wagi [kg]', 'rgb(3, 202, 86)', "lightgreen", 'rgba(3, 202, 86, 0.12)', 'removeThisLabel');
}

function createKcalBurnedChart() {
    defaultChart('Spalone kalorie [kcal]', 'rgb(191, 99, 8)', "orange", 'rgba(191, 99, 8, 0.12)', 'Cel spalonych kalorii [kcal]');
}

function createKcalConsumedChart() {
    defaultChart('Spożyte kalorie [kcal]', 'rgb(222, 209, 27)', "yellow", 'rgba(222, 209, 27, 0.2)', 'Cel spożycia kalorii [kcal]');
}

function createWaterConsumedChart() {
    defaultChart('Spożycie płynów [ml]', 'rgb(8, 87, 191)', "lightblue", 'rgba(8, 87, 191, 0.2)', 'Cel spożycia płynów [ml]');
}

function createWaistCircumferenceChart() {
    defaultChart('Pomiar obwodu talii [cm]', 'rgba(241, 239, 239, 1)', "white", 'rgba(241, 239, 239, 0.12)', 'removeThisLabel');
}

function createTrainingTimeChart() {
    defaultChart('Czas treningów [min]', 'rgb(172, 46, 245)', "violet", 'rgba(172, 46, 245, 0.12)', 'Cel czasu treningów [min]');
}

function createBodyFatChart() {
    defaultChart('Szacowany poziom tłuszczu w organizmie[%]', 'rgb(144, 55, 39)', "brown", 'rgba(144, 55, 39, 0.12)', 'removeThisLabel');
}

function defaultChart(label, borderColor, pointBorderColor, backgroundColor, labelGoal) {
    Chart.defaults.global.defaultFontColor = 'rgb(255, 255, 255)';
    Chart.defaults.global.elements.point.radius = 4;
    Chart.defaults.global.elements.point.borderWidth = 2;

    var ctx = document.getElementById('myChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: date,
            datasets: [
            {
                label: label,
                data: data,
                borderColor: borderColor,
                color: 'rgb(0, 0, 0)',
                backgroundColor: backgroundColor,
                pointBorderColor: pointBorderColor,
            },
            {
                label: labelGoal,
                data: goals,
                borderColor: 'pink',
                color: 'rgb(0, 0, 0)',
                pointBorderColor: 'pink',
                backgroundColor: 'rgba(193,66,66,0.12)',
            }]
        },
        options: {
            scales: {
                xAxes: [{ gridLines: { color: 'rgba(197, 201, 206, 0.3)' } }],
                yAxes: [{ gridLines: { color: 'rgba(197, 201, 206, 0.3)' } }]
            },
            legend: {
                labels: {
                    filter: function (item) {
                        return !item.text.includes('removeThisLabel');
                    }
                }
            }
        }
    });
}


var data = [];
var date = [];
var goals = [];