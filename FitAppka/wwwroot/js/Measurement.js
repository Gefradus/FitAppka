function showModalAddMeasurement(){
    showModal(true);
    clearModalField();
}

function showEditMeasurementModal(id, weight, waist){
    showModal(false);
    fillModalFields(id, weight, waist);
}

function showDeleteModal(id){
    $("#deleteModal").modal('show');
    $("#deleteMeasurementId").val(id);
}

function clearModalField(){
    $("#weight").val("");
    $("#waist").val("");
}

function fillModalFields(id, weight, waist){
    $("#editMeasurementId").val(id);
    $("#weight").val(weight);
    if(waist != '-'){
        $("#waist").val(waist);
    }
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

function deleteMeasurement(url){
    $.ajax({
        type: 'DELETE',
        url: url,
        data: { 
            id: parseInt($("#deleteMeasurementId").val())
        },
        success: function () {
            location.reload();
        }
    });
}