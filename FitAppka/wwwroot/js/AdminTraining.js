function onload() {
    colorEverySecondRow();
    $(".tableRow").click(function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected').siblings().removeClass("selected");
        } else {
            $(this).addClass('selected').siblings().removeClass("selected");
        }
        colorEverySecondRow();
    });
}


function textboxresize() {
    $("#searchStrength").width($("#searchCardio").width());
    window.addEventListener('resize', function () {
        $("#searchStrength").width($("#searchCardio").width());
    });
}

function showAddCardioModal() {
    clearAllCardioModalData();
    $("#addOrEditCardioTrainingType").modal('show');
}


function showEditCardioModal(id, name, kcal, visible) {
    setAllCardioModalData(id, name, kcal, visible);
    $("#addOrEditCardioTrainingType").modal('show');
}

function clearAllCardioModalData() {
    document.getElementById("addOrEditCardioTitle").innerHTML = "Tworzenie ćwiczenia";
    document.getElementById("addCardioArea").hidden = false;
    document.getElementById("editCardioArea").hidden = true;
    document.getElementById("cardioTypeName").value = '';
    document.getElementById("expenditure").value = '';
    document.getElementById("visible").checked = true;
}

function setAllCardioModalData(id, name, kcal, visible) {
    document.getElementById("addOrEditCardioTitle").innerHTML = "Edycja ćwiczenia";
    document.getElementById("addCardioArea").hidden = true;
    document.getElementById("editCardioArea").hidden = false;
    $("#cardioTypeId").val(id);
    $("#cardioTypeName").val(name);
    $("#expenditure").val(kcal);
    document.getElementById("visible").checked = visible == "true";
}


function addCardioType(url) {
    if (checkIfCardioModalValid()) {
        $.ajax({
            type: 'POST',
            url: url,
            data: {
                name: $("#cardioTypeName").val(),
                kcalPerMin: $("#expenditure").val(),
                visibleToAll: document.getElementById("visible").checked
            },
            success: function () {
                location.reload();
            }
        });
    }
}

function clearAllStrengthModalData() {
    document.getElementById("editStrengthArea").hidden = true;
    document.getElementById("addStrengthArea").hidden = false;
    document.getElementById("strengthModalTitle").innerHTML = "Tworzenie ćwiczenia";
    document.getElementById("strengthTrainingTypeName").value = '';
    document.getElementById("visibleStrength").checked = true;
}


function showAddStrengthModal() {
    clearAllStrengthModalData()
}

function showEditStrengthModal(id, name, visible) {
    setAllStrengthModalData(id, name, visible);
}

function setAllStrengthModalData(id, name, visible) {
    document.getElementById("editStrengthArea").hidden = false;
    document.getElementById("addStrengthArea").hidden = true;
    document.getElementById("strengthModalTitle").innerHTML = "Edycja ćwiczenia";
    document.getElementById("strengthTypeId").value = id;
    document.getElementById("strengthTrainingTypeName").value = name;
    document.getElementById("visibleStrength").checked = visible == 'true';
}


function editCardioType(url) {
    if (checkIfCardioModalValid()) {
        $.ajax({
            type: 'PUT',
            url: url,
            data: {
                id: $("#cardioTypeId").val(),
                name: $("#cardioTypeName").val(),
                kcalPerMin: $("#expenditure").val(),
                visibleToAll: document.getElementById("visible").checked
            },
            success: function () {
                location.reload();
            }
        });
    }
}

function addStrengthTrainingType(url) {
    if (checkIfStrengthModalValid()) {
        $.ajax({
            type: 'POST',
            url: url,
            data: {
                name: $("#strengthTrainingTypeName").val(),
                visibleToAll: document.getElementById("visible").checked
            },
            success: function () {
                location.reload();
            }
        });
    }
}

function editStrengthTrainingType(url) {
    if (checkIfStrengthModalValid()) {
        $.ajax({
            type: 'PUT',
            url: url,
            data: {
                id: $("#strengthTypeId").val(),
                name: $("#strengthTrainingTypeName").val(),
                visibleToAll: document.getElementById("visible").checked
            },
            success: function () {
                location.reload();
            }
        });
    }
}



function checkIfStrengthModalValid() {
    if (isEmpty($("#strengthTrainingTypeName").val())) {
        validation("Podaj nazwę treningu");
        return false;
    }
    return true;
}

function checkIfCardioModalValid() {
    if (isEmpty($("#cardioTypeName").val())) {
        validation("Podaj nazwę cardio");
        return false;
    }
    else if (isEmpty($("#expenditure").val())) {
        validation("Podaj kcal/min");
        return false;
    } 
    return true; 
}


function showDeleteStrengthModal(id, name) {
    document.getElementById("formDeleteStrength").hidden = false;
    document.getElementById("formDeleteCardio").hidden = true;
    document.getElementById("deletingTrainingName").innerHTML = ": " + name + " ?";
    document.getElementById("deletingStrengthTrainingID").value = id;
    $("#deleteModal").modal("show");
}

function showDeleteCardioModal(id, name) {
    document.getElementById("formDeleteCardio").hidden = false;
    document.getElementById("formDeleteStrength").hidden = true
    document.getElementById("deletingTrainingName").innerHTML = ": " + name + " ?";
    document.getElementById("deletingCardioID").value = id;
    $("#deleteModal").modal("show");
}

function deleteCardio(url) {
    var id = document.getElementById("deletingCardioID").value;
    $.ajax({
        type: 'DELETE',
        url: url,
        data: { id: id },
        success: function () {
            location.reload();
        }
    });
}

function deleteStrengthTraining(url) {
    var id = document.getElementById("deletingStrengthTrainingID").value;
    $.ajax({
        type: 'DELETE',
        url: url,
        data: { id: id },
        success: function () {
            location.reload();
        }
    });
}