function showModalAddMeasurement(){
    showModal(true);
    clearModalField();
    document.getElementById("modal-title").innerHTML = "Tworzenie pomiaru";
}

function colorEverySecondRow() {
    var rows = document.getElementsByClassName("rowM");
    for (var i = 0; i < rows.length; i++) {
        if (i % 2 == 1) {
            rows[i].style.backgroundColor = "rgb(238 238 238)";
        }
    }
}

function showEditMeasurementModal(id, weight, waist){
    showModal(false);
    fillModalFields(id, weight, waist);
    document.getElementById("modal-title").innerHTML = "Edytowanie pomiaru";
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
    if (waist != '-') {
        $("#waist").val(parseInt(waist));
    } else {
        $("#waist").val("");
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

function addOrEditMeasurementValidation() 
{
    if (isNumeric(parseInt($("#weight").val())) && isEmptyOrNumeric($("#waist").val())) {
        return true;
    }
    else {
        if (isEmpty($("#weight").val())) {
            validation("Należy podać wagę ciała");
        }
        else if (!isEmptyOrNumeric($("#waist").val())) {
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
            weight: $("#weight").val(), 
            waist: parseInt($("#waist").val()) 
        },
        success: function () {
            $('body').removeClass('loaded');
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
            $('body').removeClass('loaded');
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
        success: function (response) {
            if (response == true) {
                location.reload();
            } else {
                validation("Należy pozostawić jeden pomiar wagi");
            }
            
        }
    });
}

function onload() {
    colorEverySecondRow();
    $(".rowM").click(function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected').siblings().removeClass("selected");
        } else {
            $(this).addClass('selected').siblings().removeClass("selected");
        }
        colorEverySecondRow();
    });
}