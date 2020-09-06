function showModalAddTraining() {
    $("#addTrainingType").modal('show');
}

function addCardioTraining() {
    var time = document.getElementById("timeInMinutes").value;
    var burnedKcal = document.getElementById("burnedKcal").value;

    if (!isEmpty(time) && !isEmpty(burnedKcal)) {
        document.getElementById("formAdd").submit();
    }
    else
    {
        if (isEmpty(time) && isEmpty(burnedKcal)) {
            validation("Podaj czas trwania ćwiczenia i spalone kcal");
        }
        else if (isEmpty(time)){
            validation("Podaj czas trwania ćwiczenia");
        }
        else{
            validation("Podaj ilość spalonych kcal");
        }
    }
}

function addCardioType(dayID, url) {

    var cardioTypeName = document.getElementById("cardioTypeName").value;
    var expenditure = document.getElementById("expenditure").value;
    var expenditureInteger = parseInt(expenditure);

    if (!isEmpty(cardioTypeName) && !isEmpty(expenditure)) {
        if (!isNaN(expenditureInteger)) {
            if (cardioTypeName.length >= 3) {
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: { dayID: dayID, name: cardioTypeName, kcalPerMin: expenditureInteger },
                    success: function () {
                        location.reload();
                    }
                });
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
        if (isEmpty(cardioTypeName) && isEmpty(expenditure)) {
            validation("Podaj nazwę ćwiczenia i wydatek kcal/min.");
        }
        else if (isEmpty(cardioTypeName)) {
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