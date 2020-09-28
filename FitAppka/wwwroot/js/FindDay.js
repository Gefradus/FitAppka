function choice() {
    var select = document.getElementById("choice");
    var selectedValue = select.options[select.selectedIndex].value;

    document.getElementById("product").hidden = selectedValue != 0;
    document.getElementById("calorie").hidden = selectedValue != 1;
    document.getElementById("waterConsumption").hidden = selectedValue != 2;
}


function findDays(url, searchType) {
    $('body').removeClass('loaded');
    var select = document.getElementById("normalDropDown");
    var from = 0;
    var to = 0;

    if (searchType == 0) {
        from = getValueFromInput("#from_product");
        to = getValueFromInput("#to_product");
    }
    if (searchType == 1) {
        from = getValueFromInput("#from_calorie");
        to = getValueFromInput("#to_calorie");
    }
    if (searchType == 2) {
        from = getValueFromInput("#from_water");
        to = getValueFromInput("#to_water");
    }

    url = url + "?findBy=" + searchType + "&from=" + from + "&to=" + to + "&wasSearchedFor=true";
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
}

function restore(searchType, from, to)
{
    searchType = parseInt(searchType);
    from = parseInt(from);
    to = parseInt(to);

    if (searchType == 0) {
        $("#choice").val(0);
        choice();
        $("#from_product").val(from);
        $("#to_product").val(to);
        
    }
    if (searchType == 1) {
        $("#choice").val(1);
        choice();
        $("#from_calorie").val(from);
        $("#to_calorie").val(to);
        
    }
    if (searchType == 2) {
        $("#choice").val(2);
        choice();
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