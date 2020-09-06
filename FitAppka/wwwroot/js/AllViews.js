function kolorujCoDrugiWiersz() {
    var wszystkie = document.getElementsByClassName("wieeersz");
    for (var i = 0; i < wszystkie.length; i++) {
        if (i % 2 == 0) {
            wszystkie[i].style.backgroundColor = "rgb(239, 239, 239)";
        }
    }
}


