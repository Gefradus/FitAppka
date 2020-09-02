function choice() {
    var select = document.getElementById("choice");
    var selectedValue = select.options[select.selectedIndex].value;

    hideEverything();

    if (selectedValue == 1) {
        document.getElementById("product").hidden = false;
    }
    if (selectedValue == 2) {
        document.getElementById("calorie").hidden = false;
    }
    if (selectedValue == 3) {
        document.getElementById("waterConsumption").hidden = false;
    }
}

function hideEverything() {
    document.getElementById("product").hidden = true;
    document.getElementById("calorie").hidden = true;
    document.getElementById("waterConsumption").hidden = true;
}

function searchDay(searchType, htmlUrl) {
    var select = document.getElementById("normalDropDown");
    var selectedValue = select.options[select.selectedIndex].value;
    var from = 0;
    var to = 0;

    if (searchType == 1) {
        from = parseInt(document.getElementById("from_product").value, 10);
        to = parseInt(document.getElementById("to_product").value, 10);
    }
    if (searchType == 2) {
        from = parseInt(document.getElementById("from_calorie").value, 10);
        to = parseInt(document.getElementById("to_calorie").value, 10);
    }
    if (searchType == 3) {
        from = parseInt(document.getElementById("from_water").value, 10);
        to = parseInt(document.getElementById("to_water").value, 10);
    }

    var url = htmlUrl;
    window.location.href = url.replace('_id_', selectedValue).replace('_from_', from).replace('_to_', to).replace('_type_', searchType);
}

function onload() {
    $('#normalDropDown').chosen({ search_contains: true });
    restoreState();
}

function restore(searchType) {
    if (searchType == 2) {
        var select = document.getElementById("choice");
        select.value = '2';
        hideEverything();
        document.getElementById("calorie").hidden = false;
    }
    if (searchType == 3) {
        var select = document.getElementById("choice");
        select.value = '3';
        hideEverything();
        document.getElementById("waterConsumption").hidden = false;
    }
}

function colorEverySecondRow() {
    var daysRows = document.getElementsByClassName("dayRow");
    for (var i = 0; i < daysRows.length; i++) {
        if (i % 2 == 0) {
            daysRows[i].style.backgroundColor = "rgb(224, 221, 221)";
        }
    }
}