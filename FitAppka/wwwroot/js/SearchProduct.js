function onload() {
    transparentWhenScrollDown();
    colorEverySecondRow();
}

function showModalAddMeal() {
    prepareModalAddProduct();
    $("#addMealModal").modal('show');
}

function prepareModalAddProduct() {
    var iter = giveIterFromID(id);
    if (pathArray[iter] == 'null') {
        document.getElementById("photoArea").innerHTML = '<img id="zdjecie" src="/FitAppka/img/miss.png" class="img-fluid" asp-append-version="true" />';
    }
    else {
        document.getElementById("photoArea").innerHTML = '<img id="zdjecie" src="' + "/FitAppka/photos/" + pathArray[iter] + '" class="img-fluid" asp-append-version="true" />';
    }

    document.getElementById("name").innerHTML = nameArray[iter];
    document.getElementById("kcal").innerHTML = kcalArray[iter] + ' kcal';
    document.getElementById("proteins").innerHTML = 'Białko: ' + proteinsArray[iter] + ' g,';
    document.getElementById("fats").innerHTML = 'Tł.: ' + fatsArray[iter] + ' g,';
    document.getElementById("carbs").innerHTML = 'Węgl.: ' + carbsArray[iter] + ' g';
}

function addProductValidation(url) {
    var grammage = parseInt($("#grammage").val());
    if (grammage < 1) {
        validation("Gramatura posiłku musi wynosić przynajmniej 1 gram.");
    }
    else if (grammage > 9999) {
        validation("Gramatura posiłku nie może przekraczać 9999 gram.");
    }
    else if (grammage >= 1 && grammage <= 9999) {
        addProduct(url, grammage);
    }
    else {
        validation("Należy podać liczbę");
    }
};

function addProduct(url, grammage) {
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            productID: id,
            grammage: grammage,
            dayID: $("#dayID").val(),
            inWhich: $("#inWhich").val()
        },
        success: function () {
            location.reload();
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