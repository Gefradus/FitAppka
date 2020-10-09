function onload() {
    checkIfModalWasOpen();
    showAddedProducts();
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


function getWebRootPath(path) {
    return path;
}

function closeAddModal() {
    $("#addOrEditModal").modal('hide');
    $("#grammage").val("");
}

function tryAddProduct(url) {
    let grammage = $("#grammage").val();

    if (isNumeric(grammage)) {
        addProduct(url, grammage,);
    } else {
        if (isEmpty(grammage)) {
            validation("Podaj ilość produktu");
        }
    }
}

function addProduct(url, grammage)
{
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            addedProducts: JSON.parse(localStorage.products),
            productId: $("#productId").val(),
            grammage: grammage
        },
        success: function (response) {
            localStorage.setItem("products", JSON.stringify(response.addedProducts));
        }
    });
}

function showAddedProducts() {
    let products = JSON.parse(localStorage.products);
    for (var i = 0; i < products.length; i++)
    {
        let name = products[i].productName;
        let grammage = products[i].grammage;
        let kcal = products[i].calories;
        let proteins = products[i].proteins;
        let carbs = products[i].carbohydrates;
        let fats = products[i].fats;
        let panel = document.getElementById("productsPanel");

        panel.innerHTML = panel.innerHTML +
            '<div class="row no-gutters productRow">' +
            '<div class="col-2 col-lg-3 h-100 acent cent">'+ name +'</div>' +
            '<div class="col-9 col-lg-8 container h-100">' +
                '<div class="row no-gutters h-100">' +
                    '<div class="col-3 acent cent">' + grammage + '</div>' +
                        '<div class="col-9 container row no-gutters acent">' +
                            '<div class="col-3 cent">'+ kcal +'</div>' +
                            '<div class="col-3 cent">'+ proteins +'</div>' +
                            '<div class="col-3 cent">'+ fats +'</div>' +
                            '<div class="col-3 cent">'+ carbs +'</div>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
                '<div class="col-1 acent cent">' +
                    '<button type="button" class="btnDelete"><img class="delete" src="https://cdn.onlinewebfonts.com/svg/img_416864.png"></button>' +
                '</div>' +
            '</div>' + 
            '</div>';
    }
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
