function noYesActive() {
    var noYesActive = document.getElementsByClassName("noYesActive");
    for (var i = 0; i < noYesActive.length; i++) {
        if (noYesActive[i].innerHTML == 'NIE') {
            noYesActive[i].style.color = '#31c131';
        } else {
            noYesActive[i].style.color = 'rgb(224 102 102)';
        }
    }
}

function onload() {
    colorEverySecondRow();
    yesNoActive();
    noYesActive();
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

function banUser(url, id, username) {
    document.getElementById("message").innerHTML = "Czy na pewno chcesz zbanować użytkownika " + username + " ?"; 
    banUnban(id, url);
}

function unbanUser(url, id, username) {
    document.getElementById("message").innerHTML = "Czy na pewno chcesz odbanować użytkownika " + username + " ?";
    banUnban(id, url);
}

function banUnban(id, url) {
    $("#userId").val(id);
    $("#deleteModal").modal("show");
    $("#banOrUnban").click(function () {
        $.ajax({
            type: 'POST',
            url: url,
            data: { id: $("#userId").val() },
            success: function () {
                location.reload();
            }
        });
    });
}