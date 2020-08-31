function changeDay(url) {
    window.location.href = url + "?day=" + document.getElementById("datepicker").value;
}

function colorEverySecondRow() {
    var lista = document.getElementsByClassName("trainingRow");
    for (var i = 0; i < lista.length; i++) {
        if (i % 2 == 0) {
            lista[i].style.backgroundColor = "rgb(227, 227, 227)";
        }
    }
}

function deleteCardio(id, url) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { cardioID: id },
        success: function () {
            location.reload();
        }
    });
}

function hideModal() {
    location.reload();
}
