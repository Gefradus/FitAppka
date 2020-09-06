function changeDay(url) {
    window.location.href = url + "?day=" + $("#datepicker").val();
}

function colorEverySecondRow() {
    var lista = document.getElementsByClassName("trainingRow");
    for (var i = 0; i < lista.length; i++) {
        if (i % 2 == 0) {
            lista[i].style.backgroundColor = "rgb(227, 227, 227)";
        }
    }
}

function changeCalorie() {
    var minutes = parseInt($("#timeInMinutes").val());
    var kcalPerMin = $("#kcalPerMin").val();
    $("#burnedKcal").val(parseInt(minutes * kcalPerMin));
}

function showEditCardioModal(id, time, burnedKcal, name, kcalPerMin) {
    document.getElementById("cardioName").innerHTML = name;
    $("#kcalPerMin").val(parseInt(kcalPerMin));
    $("#timeInMinutes").val(parseInt(time));
    $("#burnedKcal").val(parseInt(burnedKcal));
    $("#editCardioModal").modal('show');
    $("#editingCardioID").val(parseInt(id));
}

function showDeleteCardioModal(id, name) {
    document.getElementById("formDeleteCardio").hidden = false;
    document.getElementById("formDeleteStrength").hidden = true;
    document.getElementById("deletingTrainingName").innerHTML = ": " + name + ' ?';
    $("#deleteModal").modal('show');
    $("#deletingCardioID").val(parseInt(id));
}

function showEditStrengthModal(id, sets, reps, weight, name) {
    document.getElementById("strengthName").innerHTML = name;
    $("#sets").val(parseInt(sets));
    $("#reps").val(parseInt(reps));
    $("#weight").val(parseInt(weight));
    $("#editStrengthModal").modal('show');
    $("#editingStrengthTrainingID").val(parseInt(id));
}

function showDeleteStrengthModal(id, name) {
    document.getElementById("formDeleteCardio").hidden = true;
    document.getElementById("formDeleteStrength").hidden = false;
    document.getElementById("deletingTrainingName").innerHTML = ": " + name + ' ?';
    $("#deleteModal").modal('show');
    $("#deletingStrengthTrainingID").val(parseInt(id));
}

function deleteStrengthTraining(url) {
    $.ajax({
        type: 'DELETE',
        url: url,
        data: { id: parseInt($("#deletingStrengthTrainingID").val()) },
        success: function () {
            location.reload();
        }
    });
}

function deleteCardio(url) {
    $.ajax({
        type: 'DELETE',
        url: url,
        data: { id: parseInt($("#deletingCardioID").val()) },
        success: function () {
            location.reload();
        }
    });
}

function editCardioVal(url, time, burnedKcal) {
    $.ajax({
        type: 'PUT',
        url: url,
        data: {
            id: parseInt($("#editingCardioID").val()),
            time: parseInt(time),
            burnedKcal: parseInt(burnedKcal)
        },
        success: function () {
            location.reload();
        }
    });
}

function editCardio(url) {
    var time = document.getElementById("timeInMinutes").value;
    var burnedKcal = document.getElementById("burnedKcal").value;

    if (!isEmpty(time) && !isEmpty(burnedKcal)) {
        editCardioVal(url, time, burnedKcal);
    }
    else {
        if (isEmpty(time) && isEmpty(burnedKcal)) {
            validation("Podaj czas trwania æwiczenia i spalone kcal");
        }
        else if (isEmpty(time)) {
            validation("Podaj czas trwania æwiczenia");
        }
        else {
            validation("Podaj iloœæ spalonych kcal");
        }
    }
}