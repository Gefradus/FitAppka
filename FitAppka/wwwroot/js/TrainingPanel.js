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

function editCardio(url) {
    if (cardioVal()) {
        $.ajax({
            type: 'PUT',
            url: url,
            data: {
                id: parseInt($("#editingCardioID").val()),
                time: parseInt($("#timeInMinutes").val()),
                burnedKcal: parseInt($("#burnedKcal").val())
            },
            success: function () {
                location.reload();
            }
        });
    }
}

function editStrengthTraining(url) {
    if (strengthTrainingVal()) {
        $.ajax({
            type: 'PUT',
            url: url,
            data: {
                id: parseInt($("#editingStrengthTrainingID").val()),
                sets: parseInt($("#sets").val()),
                reps: parseInt($("#reps").val()),
                weight: parseInt($("#weight").val())
            },
            success: function () {
                location.reload();
            }
        });
    }
}

function setInputsValidation() {
    $j(".int1").on("change", function () {
        this.value = changeToMaxIfGreaterInt(this.value, 1000);
    });
    $j("#burnedKcal").on("keypress", function () {
        this.value = changeToMaxIfGreaterInt(this.value, 1000000);
    });
    $j("#timeInMinutes").on("keypress", function () {
        this.value = changeToMaxIfGreaterInt(this.value, 1000000);
    });
}