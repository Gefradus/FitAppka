function showModalAddMeasurement(){
    $("#addMeasurement").modal('show');
}

function addMeasurement(url) {
    var weight = parseInt($("#weight").val());
    var waist = parseInt($("#waist").val());
    if (isNumeric(weight) && isEmptyOrNumeric(waist)) {
        addMeasurementAjax(url, weight, waist);
    }
    else {
        if (isEmpty(weight)) {
            validation("Nale¿y podaæ wagê cia³a");
        }
        else if (!isEmptyOrNumeric(waist)) {
            validation("Nale¿y podaæ obwód w pasie jako liczbê");
        }
        else {
            validation("Nale¿y podaæ wagê jako liczbê");
        }
    }
}

function addMeasurementAjax(url, weight, waist) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { weight: weight, waist: waist },
        success: function () {
            location.reload();
        }
    });
}

