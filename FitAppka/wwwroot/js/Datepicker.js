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

