function deleteDiet(url, id) {
    $("#DietId").val(id);
    $("#deleteModal").modal("show");

    $("#delete").click(function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            data: {
                id: $("#DietId").val()
            },
            success: function () {
                location.reload();
            }
        });
    });
}

function onload() {
    colorEverySecondRow();
    yesNoActive();
    $(".tableRow").click(function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected').siblings().removeClass("selected");
        } else {
            $(this).addClass('selected').siblings().removeClass("selected");
        }
        colorEverySecondRow();
    });
}

function yesNoActive() {
    var yesNoActive = document.getElementsByClassName("yesNoActive");
    for (var i = 0; i < yesNoActive.length; i++) {
        if (yesNoActive[i].innerHTML == 'TAK') {
            yesNoActive[i].style.color = '#31c131';
        } else {
            yesNoActive[i].style.color = 'rgb(224 102 102)';
        }
    }
}

