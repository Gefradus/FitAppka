function transparentWhenScrollDown() {
    $(document).scroll(function () {
        var $nav = $(".navbar-fixed-top");
        $nav.toggleClass('scrolled', $(this).scrollTop() > $nav.height());
    });
}

function setDisabled(element) {
    element.readOnly = true;
    element.style.backgroundColor = "#e9ecef";
}

function setEnabled(element) {
    element.readOnly = false;
    element.style.backgroundColor = 'white';
}

function setNotAllowedCursor(mouseTarget) {
    mouseTarget.style.cursor = 'not-allowed';
}

function setPointerCursor(mouseTarget) {
    mouseTarget.style.cursor = 'pointer';
}

function setNormalCursor(mouseTarget) {
    mouseTarget.style.cursor = 'text';
}

function giveIterFromID(id) {
    var iter = 0;
    for (var i = 0; i < IDarray.length; i++) {
        if (IDarray[i] == id) {
            iter = i;
        }
    }
    return iter;
}

function colorEverySecondRow() {
    var rows = document.getElementsByClassName("tableRow");
    for (var i = 0; i < rows.length; i++) {
        if (i % 2 == 0) {
            rows[i].style.backgroundColor = "rgb(243, 243, 243)";
        }
    }
}