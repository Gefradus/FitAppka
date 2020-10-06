function onload() {
    checkIfModalWasOpen();
    onResizeEvent();

    if (window.addEventListener) {
        window.addEventListener("resize", onResizeEvent, true);
    } else {
        if (window.attachEvent) {
            window.attachEvent("onresize", onResizeEvent);
        }
    }

    $('#addProductModal').on('hide.bs.modal', function () {
        window.history.replaceState(null, null, window.location.pathname);
    });
}

function onResizeEvent() {
    var protein = document.getElementById("proteinHeader");
    var fats = document.getElementById("fatHeader");
    var carbs = document.getElementById("carbsHeader");

    if (document.getElementsByTagName("body")[0].offsetWidth < 500)
    {
        protein.innerHTML = "B";
        fats.innerHTML = "T";
        carbs.innerHTML = "W";
    }
    else if (document.getElementsByTagName("body")[0].offsetWidth < 800)
    {
        protein.innerHTML = "Białko";
        fats.innerHTML = "Tłuszcze";
        carbs.innerHTML = "Węgl.";
    }
    else {
        protein.innerHTML = "Białko";
        fats.innerHTML = "Tłuszcze";
        carbs.innerHTML = "Węglowodany";
    }
}

function addProduct(url, id, grammage) {
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            id: id,
            grammage: grammage
        },
        success: function () {}
    });
}

function getWebRootPath(path) {
    return path;
}

function showAddOrEditModal(id) {
    prepareModalData(id);
    $("#addOrEditModal").modal('show');
}

function prepareModalData(id) {
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

var IDarray = [];
var nameArray = [];
var pathArray = [];
var kcalArray = [];
var proteinsArray = [];
var fatsArray = [];
var carbsArray = [];