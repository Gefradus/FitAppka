function changeDay(url) {
    window.location.href = url + "?day=" + document.getElementById("datepicker").value;
}

function kolorujCoDrugiWiersz() {
    var lista = document.getElementsByClassName("wierszTreningu");
    for (var i = 0; i < lista.length; i++) {
        if (i % 2 == 0) {
            lista[i].style.backgroundColor = "rgb(227, 227, 227)";
        }
    }
}

function usunTrening(id, url) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { treningID: id },
        success: function () {
            location.reload();
        }
    });
}

function hideModal() {
    location.reload();
}
