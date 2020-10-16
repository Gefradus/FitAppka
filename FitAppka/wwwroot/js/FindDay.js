function choice() {
    var select = document.getElementById("choice");
    var selectedValue = select.options[select.selectedIndex].value;

    document.getElementById("product").hidden = selectedValue != 0;
    document.getElementById("calorie").hidden = selectedValue != 1;
    document.getElementById("waterConsumption").hidden = selectedValue != 2;
}

function loadDatePickers(dateFrom, dateTo, findBy) {
    createDatePicker(".datepicker");

    if (findBy == 0) {
        changeDatePickers("#datepickerFrom_product", "#datepickerTo_product", dateFrom, dateTo);
    }
    if (findBy == 1) {
        changeDatePickers("#datepickerFrom_calorie", "#datepickerTo_calorie", dateFrom, dateTo);
    }
    if (findBy == 2) {
        changeDatePickers("#datepickerFrom_water", "#datepickerTo_water", dateFrom, dateTo);
    }

    var $j = jQuery.noConflict();
    $j(".datepicker").on("change", changeDatePickers(findBy));
}


function changeDatePickers(datepickerFrom, datepickerTo, dateFrom, dateTo) {
    setMaxDate(datepickerFrom, new Date(4000, 12, 30));
    setMinDate(datepickerFrom, new Date(1900, 12, 30));
    setDate(datepickerTo, dateTo);
    setDate(datepickerFrom, dateFrom);
    updateDatePickers(datepickerFrom, datepickerTo);
}

function updateDatePickers(datepickerFrom, datepickerTo) {
    var $j = jQuery.noConflict();
    setMinDate(datepickerTo, $j(datepickerFrom).datepicker("getDate"));
    setMaxDate(datepickerFrom, $j(datepickerTo).datepicker("getDate"));
}

function findDays(url, searchType) {
    var select = document.getElementById("normalDropDown");
    var from = 0;
    var to = 0;
    var dateFrom;
    var dateTo;

    if (searchType == 0) {
        from = getValueFromInput("#from_product");
        to = getValueFromInput("#to_product");
        dateFrom = $("#datepickerFrom_product").val();
        dateTo = $("#datepickerTo_product").val();
    }
    if (searchType == 1) {
        from = getValueFromInput("#from_calorie");
        to = getValueFromInput("#to_calorie");
        dateFrom = $("#datepickerFrom_calorie").val();
        dateTo = $("#datepickerTo_calorie").val();
    }
    if (searchType == 2) {
        from = getValueFromInput("#from_water");
        to = getValueFromInput("#to_water");
        dateFrom = $("#datepickerFrom_water").val();
        dateTo = $("#datepickerTo_water").val();
    }

    url = url + "?findBy=" + searchType + "&from=" + from + "&to=" + to + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&wasSearchedFor=true";
    location.href = searchType == 0 ? url + "&productId=" + select.options[select.selectedIndex].value : url;
}

function getValueFromInput(selector) {
    var value = $(selector).val();
    return isNaN(value) ? 0 : value;
}


function onload() {
    colorEverySecondRow();
    restoreState();
    $('#normalDropDown').chosen({ search_contains: true });
    transparentWhenScrollDown();
    $(".datepicker, .int").addClass("form-control");
    $(".dayRow").click(function () {
        if ($(this).hasClass('selected'))
        {
            $(this).removeClass('selected').siblings().removeClass("selected");
        }
        else
        {
            $(this).addClass('selected').siblings().removeClass("selected");
        }
        colorEverySecondRow();
    });
}

function restore(searchType, from, to) {
    searchType = parseInt(searchType);
    from = parseInt(from);
    to = parseInt(to);

    $("#choice").val(searchType);
    choice();

    if (searchType == 0) {
        $("#from_product").val(from);
        $("#to_product").val(to); 
    }
    if (searchType == 1) {
        $("#from_calorie").val(from);
        $("#to_calorie").val(to);
    }
    if (searchType == 2) {
        $("#from_water").val(from);
        $("#to_water").val(to);
    }
}

function setInputsValidation() {
    $(".int").on("change", function () {
        this.value = changeToMaxIfGreaterInt(this.value, 10000);
    });

    $(".int").attr("max", 999999);
}

