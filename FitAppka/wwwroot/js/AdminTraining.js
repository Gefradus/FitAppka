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


function showDeleteStrengthModal(id, name) {
    document.getElementById("formDeleteStrength").hidden = false;
    document.getElementById("formDeleteCardio").hidden = true;
    document.getElementById("deletingTrainingName").innerHTML = ": " + name + " ?";
    document.getElementById("deletingCardioID").value = id;
    $("#deleteModal").modal("show");
}

function showDeleteCardioModal(id, name) {
    document.getElementById("formDeleteCardio").hidden = false;
    document.getElementById("formDeleteStrength").hidden = true
    document.getElementById("deletingTrainingName").innerHTML = ": " + name + " ?";
    document.getElementById("deletingStrengthTrainingID").value = id;
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