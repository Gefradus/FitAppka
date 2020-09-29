function onload() {
    transparentWhenScrollDown();
    colorEverySecondRow();
}

function showModalAddMeal(id) {
    prepareModalAddMeal(id);
    $("#addMealModal").modal('show');
}

function getWebRootPath(path) {
    return path;
}

function prepareModalAddMeal(id) {
    var iter = giveIterFromID(id);
    if (pathArray[iter] == 'null') {
        document.getElementById("photoArea").innerHTML = '<img id="zdjecie" src="' + getRootPath() + '/img/miss.png" class="img-fluid" asp-append-version="true" />';
    }
    else {
        document.getElementById("photoArea").innerHTML = '<img id="zdjecie" src="' + getRootPath() + "/photos/" + pathArray[iter] + '" class="img-fluid" asp-append-version="true" />';
    }

    document.getElementById("productId").value = id;
    document.getElementById("name").innerHTML = nameArray[iter];
    document.getElementById("kcal").innerHTML = kcalArray[iter] + ' kcal';
    document.getElementById("proteins").innerHTML = 'Białko: ' + proteinsArray[iter] + ' g,';
    document.getElementById("fats").innerHTML = 'Tł.: ' + fatsArray[iter] + ' g,';
    document.getElementById("carbs").innerHTML = 'Węgl.: ' + carbsArray[iter] + ' g';
}

function addMeal(url) {
    var grammage = parseInt($("#grammage").val());
    if (grammage < 1) {
        validation("Gramatura posiłku musi wynosić przynajmniej 1 gram.");
    }
    else if (grammage > 9999) {
        validation("Gramatura posiłku nie może przekraczać 9999 gram.");
    }
    else if (grammage >= 1 && grammage <= 9999) {
        addMealAjax(url, grammage);
    }
    else {
        validation("Należy podać liczbę");
    }
};

function addMealAjax(url, grammage) {
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            productID: $("#productId").val(),
            grammage: grammage,
            dayID: $("#dayId").val(),
            atWhichMealOfTheDay: $("#atWhichMealOfTheDay").val()
        },
        success: function (response) {
            window.location.href = response.redirectToUrl;
        }
    });
}

var IDarray = [];
var nameArray = [];
var pathArray = [];
var kcalArray = [];
var proteinsArray = [];
var fatsArray = [];
var carbsArray = [];