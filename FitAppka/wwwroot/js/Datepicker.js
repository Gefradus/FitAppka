function createDatePicker(selector) {
    var $j = jQuery.noConflict();
    $j(selector).datepicker({
        minDate: new Date(1900, 1, 1),
        dateFormat: 'dd.mm.yy',
        monthNames: ["Styczeń", "Luty", "Marzec", "Kwiecień",
            "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień",
            "Październik", "Listopad", "Grudzień"],
        monthNamesShort: ["Sty", "Lut", "Mar", "Kwi",
            "Maj", "Cze", "Lip", "Sie", "Wrz",
            "Paź", "Lis", "Gru"],
        dayNames: ['Niedziela', 'Poniedziałek', 'Wtorek', 'Środa', 'Czwartek', 'Piątek', 'Sobota'],
        dayNamesShort: ['Ndz', 'Pon', 'Wto', 'Śro', 'Czw', 'Pt', 'Sob'],
        dayNamesMin: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],
        changeYear: true,
        changeMonth: true
    });
}

function createDatePickers() {
    createDatePicker("#datepickerFrom");
    createDatePicker("#datepickerTo");
}