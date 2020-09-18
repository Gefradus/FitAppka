function showModalAddMeasurement(){
    showModal(true);
}

function showEditMeasurementModal(id){
    showModal(false);
    $("#editMeasurementId").val(id);
}

function showModal(bool){
    $("#addOrEditModal").modal('show');
    document.getElementById("btnAddMeasurement").hidden = !bool;
    document.getElementById("btnEditMeasurement").hidden = bool;
}

function addMeasurement(url){
    if(addOrEditMeasurementValidation()){
        addMeasurementAjax(url);
    } 
}

function editMeasurement(url){
    if(addOrEditMeasurementValidation()){
        editMeasurementAjax(url);
    }
}

function addOrEditMeasurementValidation() {
    var weight = parseInt($("#weight").val());
    var waist = parseInt($("#waist").val());
    if (isNumeric(weight) && isEmptyOrNumeric(waist)) {
        return true;
    }
    else {
        if (isEmpty(weight)) {
            validation("Należy podać wagę ciała");
        }
        else if (!isEmptyOrNumeric(waist)) {
            validation("Należy podać obwód w pasie jako liczbę");
        }
        else {
            validation("Należy podać wagę jako liczbę");
        }
        return false;
    }
}

function addMeasurementAjax(url) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { 
            weight: parseInt($("#weight").val()), 
            waist: parseInt($("#waist").val()) 
        },
        success: function () {
            location.reload();
        }
    });
}

function editMeasurementAjax(url){
    $.ajax({
        type: 'PUT',
        url: url,
        data: { 
            id: parseInt($("#editMeasurementId").val()), 
            weight: parseInt($("#weight").val()), 
            waist: parseInt($("#waist").val()) 
        },
        success: function () {
            location.reload();
        }
    });
}
