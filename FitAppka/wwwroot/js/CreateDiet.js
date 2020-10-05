function onload() {
    onResizeEvent();

    if (window.addEventListener) {
        window.addEventListener("resize", onResizeEvent, true);
    } else {
        if (window.attachEvent) {
            window.attachEvent("onresize", onResizeEvent);
        }
    }
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