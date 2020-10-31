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
    $("#addTrainingType").modal('show');
}


function showEditCardioModal(id, name, kcal, visible) {
    setAllCardioModalData(id, name, kcal, visible);
    $("#addTrainingType").modal('show');
}

function clearAllCardioModalData() {
    document.getElementById("addOrEditCardioTitle").innerHTML = "Tworzenie ćwiczenia";
    document.getElementById("cardioTypeName").value = '';
    document.getElementById("expenditure").value = '';
    document.getElementById("visible").checked = true;
}

function setAllCardioModalData(id, name, kcal, visible) {
    document.getElementById("addOrEditCardioTitle").innerHTML = "Edycja ćwiczenia";
    $("#cardioTypeId").val(id);
    $("#cardioTypeName").val(name);
    $("#expenditure").val(kcal);
    document.getElementById("visible").checked = visible;
}


function addCardioType(url) {
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            name: $("#cardioTypeName").val(),
            kcal: $("#expenditure").val(),
            visible: document.getElementById("visible").checked
        },
        success: function () {
            location.reload();
        }
    });
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