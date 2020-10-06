function onload() {
    checkIfModalWasOpen();
    onResizeEvent();

    if (window.addEventListener) {
        window.addEventListener("resize", onResizeEvent, true);
    } else {
        if (window.attachEvent) {
            window.attachEvent("onresize", onResizeEvent);
        }
    }

    $('#addProductModal').on('hide.bs.modal', function (e) {
        window.history.replaceState(null, null, window.location.pathname);
    });
}

function onResizeEvent() {
    if (document.getElementsByTagName("body")[0].offsetWidth < 500) {
        document.getElementById("protein").innerHTML = "B";
        document.getElementById("fat").innerHTML = "T";
        document.getElementById("carbs").innerHTML = "W";
    }
    else if (document.getElementsByTagName("body")[0].offsetWidth < 800) {
        document.getElementById("protein").innerHTML = "Białko";
        document.getElementById("fat").innerHTML = "Tłuszcze";
        document.getElementById("carbs").innerHTML = "Węgl.";
    }
    else {
        document.getElementById("protein").innerHTML = "Białko";
        document.getElementById("fat").innerHTML = "Tłuszcze";
        document.getElementById("carbs").innerHTML = "Węglowodany";
    }
}

function addProduct(url, id, grammage) {
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            id: id,
            grammage: grammage
        },
        success: function () {}
    });
}

