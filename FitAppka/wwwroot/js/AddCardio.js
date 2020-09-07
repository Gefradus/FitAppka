function showModalAddTraining() {
    $("#addTrainingType").modal('show');
}

function addCardioTraining() {
    if (cardioVal()) {
        document.getElementById("formAdd").submit();
    }  
}

function addCardioTypeVal(dayID, url, name, kcalPerMin) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { dayID: dayID, name: name, kcalPerMin: kcalPerMin },
        success: function () {
            location.reload();
        }
    });
}

function addCardioType(dayID, url) {

    var name = document.getElementById("cardioTypeName").value;
    var expenditure = document.getElementById("expenditure").value;
    var expenditureInt = parseInt(expenditure);

    if (!isEmpty(name) && !isEmpty(expenditure)) {
        if (!isNaN(expenditureInt)) {
            if (name.length >= 3) {
                addCardioTypeVal(dayID, url, name, kcalPerMin);
            }
            else {
                validation("Nazwa ćwiczenia musi wynosić min. 3 znaki");
            }
        }
        else {
            validation("Wydatek kcal musi być liczbą");
        }
    }
    else {
        if (isEmpty(name) && isEmpty(expenditure)) {
            validation("Podaj nazwę ćwiczenia i wydatek kcal/min.");
        }
        else if (isEmpty(name)) {
            validation("Podaj nazwę ćwiczenia.");
        }
        else {
            validation("Podaj wydatek kcal/min.");
        }
    }
}


function setParams(name, kcalPerMin, cardioTypeId, dayID) {
    if (document.getElementById("cardioTypeId").value != parseInt(cardioTypeId)) {
        document.getElementById("dayId").value = parseInt(dayID);
        document.getElementById("cardioTypeId").value = parseInt(cardioTypeId);
        document.getElementById("timeInMinutes").value = null;
        document.getElementById("burnedKcal").value = null;
        document.getElementById("trainingName").innerHTML = name;
        document.getElementById("time").hidden = false;
        document.getElementById("kcal").hidden = false;
        document.getElementById("add").hidden = false;
        document.getElementById("kcalPerMin").value = parseInt(kcalPerMin);
    }
}

function changeCalorie() {
    var minutes = parseInt(document.getElementById("timeInMinutes").value);
    var kcalPerMin = document.getElementById("kcalPerMin").value;
    var burnedKcal = parseInt(minutes * kcalPerMin);
    document.getElementById("burnedKcal").value = burnedKcal;
}