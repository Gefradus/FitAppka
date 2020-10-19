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

function insertParam(key, value) {
    key = escape(key); value = escape(value);

    var kvp = document.location.search.substr(1).split('&');
    if (kvp == '') {
        document.location.search = '?' + key + '=' + value;
    }
    else {

        var i = kvp.length; var x; while (i--) {
            x = kvp[i].split('=');

            if (x[0] == key) {
                x[1] = value;
                kvp[i] = x.join('=');
                break;
            }
        }

        if (i < 0) { kvp[kvp.length] = [key, value].join('='); }

        //this will reload the page, it's likely better to store this until finished
        document.location.search = kvp.join('&');
    }
}