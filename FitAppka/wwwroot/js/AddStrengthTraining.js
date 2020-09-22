function showModalAddTraining() {
    $("#addTrainingType").modal('show');
}

function addTraining() {
    if (strengthTrainingVal()) {
        document.getElementById("formAdd").submit();
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
        success: function (response) {
            window.location.href = response.redirectToUrl;
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
    return (isValueInteger(weight) || isEmpty(weight)) && isValueInteger(sets) && isValueInteger(reps);
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
    if (document.getElementById("trainingTypeId").value != parseInt(typeId)) {
        document.getElementById("dayId").value = parseInt(dayID);
        document.getElementById("trainingTypeId").value = parseInt(typeId);
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
