function showModalAddTraining() {
    $("#addTrainingType").modal('show');
}

function addStrengthTraining() {
    var sets = document.getElementById("sets").value;
    var reps = document.getElementById("reps").value;

    if (!isEmpty(sets) && !isEmpty(reps)) {
        document.getElementById("formAdd").submit();
    }
    else {
        if (isEmpty(sets) && isEmpty(reps)) {
            validation("Podaj liczbę serii i powtórzeń");
        }
        else if (isEmpty(sets)) {
            validation("Podaj liczbę serii");
        }
        else {
            validation("Podaj liczbę powtórzeń");
        }
    }
}

function addStrengthTrainingType(dayID, url) {
    var name = document.getElementById("strengthTrainingTypeName").value;
    var sets = document.getElementById("setsT").value;
    var reps = document.getElementById("repsT").value;
    var weight = document.getElementById("weightT").value;
    
    if (areRequiredFieldsValid(name, sets, reps)) {
        if (areValuesValid(weight, sets, reps)) {
            if (name.length >= 3) {
                ajaxAddStrengthTrainingTypePost(url, dayID, name, sets, reps, weight);
            }
            else {
                validation("Nazwa ćwiczenia musi wynosić min. 3 znaki");
            }
        }
        else {
            validation("Błędy walidacji - popraw pola numeryczne");
        }
    }
    else {
        validation("Należy uzupełnić obowiązkowe pola")
    }
}

function ajaxAddStrengthTrainingTypePost(url, dayID, name, sets, reps, weight) {
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            dayID: dayID,
            name: name,
            sets: parseInt(sets),
            reps: parseInt(reps),
            weight: parseInt(weight)
        },
        success: function () {
            location.reload();
        }
    });
}

function isEmpty(string) {
    return string.length == 0;
}

function areRequiredFieldsValid(name, sets, reps) {
    return !isEmpty(name) && !isEmpty(sets) && !isEmpty(reps);
}

function areValuesValid(weight, sets, reps) {
    return isValueInteger(weight) && isValueInteger(sets) && isValueInteger(reps);
}

function isValueInteger(value) {
    return !isNaN(parseInt(value))
}

function validation(validMsg) {
    $("#validationModal").modal('show');
    document.getElementById("validatMsg").innerHTML = "<p>" + validMsg + "</p>";
    $("#closeVal").click(function () {
        $("#validationModal").modal('hide');
    })
}

function setParams(name, typeId, dayID) {
    if (document.getElementById("strengthTrainingTypeId").value != parseInt(typeId)) {
        document.getElementById("dayId").value = parseInt(dayID);
        document.getElementById("strengthTrainingTypeId").value = parseInt(typeId);
        document.getElementById("trainingName").innerHTML = name;
        document.getElementById("sets").value = null;
        document.getElementById("reps").value = null;
        document.getElementById("weight").value = null;
        document.getElementById("setsArea").hidden = false;
        document.getElementById("repsArea").hidden = false;
        document.getElementById("weightArea").hidden = false;
        document.getElementById("add").hidden = false;
    } 
}
