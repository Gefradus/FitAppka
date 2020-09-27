function choice() {
    var select = document.getElementById("choice");
    var selectedValue = select.options[select.selectedIndex].value;

    document.getElementById("product").hidden = selectedValue != 0;
    document.getElementById("calorie").hidden = selectedValue != 1;
    document.getElementById("waterConsumption").hidden = selectedValue != 2;
}


function findDays(url, searchType) {
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

    url = url + "?findBy=" + searchType + "&from=" + from + "&to=" + to;
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
}

function restore(searchType) {
    if (searchType == 0) {
        document.getElementById("choice").value = 0;
        choice();
    }
    if (searchType == 1) {
        document.getElementById("choice").value = 1;
        choice();
    }
    if (searchType == 2) {
        document.getElementById("choice").value = 2;
        choice();
    }
}

function setInputsValidation() {
    $(".int").on("change", function () {
        this.value = changeToMaxIfGreaterInt(this.value, 10000);
    });

    $(".int").attr("max", 999999);
}