function createDatePicker(selector) {
    var $j = jQuery.noConflict();
    $j(selector).datepicker({
        minDate: new Date(1900, 1, 1),
        dateFormat: 'dd.mm.yy',
        monthNamesShort: ["Sty", "Lut", "Mar", "Kwi",
            "Maj", "Cze", "Lip", "Sie", "Wrz",
            "Paź", "Lis", "Gru"],
        dayNamesMin: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],
        changeYear: true,
        changeMonth: true
    });
}

function createDatePickers(dateFrom, dateTo)
{
    createDatePicker("#datepickerFrom");
    createDatePicker("#datepickerTo");
    setMaxDate("#datepickerTo", dateTo);
    setMinDate("#datepickerTo", dateFrom);
    setMaxDate("#datepickerFrom", dateTo);
    setDate("#datepickerTo", dateTo);
    setDate("#datepickerFrom", dateFrom);
    setOnChange();
}

function setOnChange() {
    var $j = jQuery.noConflict();
    $j("#datepickerFrom").change(function () {
        //updateDates();
        var date = $j("#datepickerFrom").datepicker("getDate");
        $j("#datepickerTo").datepicker('option', 'minDate', date);
    });

    $j("#datepickerTo").change(function () {
        //updateDates();
        var date = $j("#datepickerTo").datepicker("getDate");
        $j("#datepickerFrom").datepicker('option', 'maxDate', date);
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

