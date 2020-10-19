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

    $("input:checkbox, #EditedDiet_DietName").on("change", function () {
        saveParamsInLocalStorage();
    });
}

function lol(deleteUrl, changed) {
    if (!changed) {
        refreshStorage();
    } 
    checkIfParamsWasSaved();
    showAddedProducts(deleteUrl);
}


function refreshStorage()
{
    localStorage.removeItem("paramsEdit");
    localStorage.setItem("productsEdit", JSON.stringify(listOfProducts));
}


function onResizeEvent() {
    var protein = document.getElementById("proteinHeader");
    var fats = document.getElementById("fatHeader");
    var carbs = document.getElementById("carbsHeader");

    if (document.getElementsByTagName("body")[0].offsetWidth < 500) {
        protein.innerHTML = "B";
        fats.innerHTML = "T";
        carbs.innerHTML = "W";
    }
    else if (document.getElementsByTagName("body")[0].offsetWidth < 800) {
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
        addProduct(url, grammage);
    } else {
        if (isEmpty(grammage)) {
            validation("Podaj ilość produktu");
        }
    }
}

function addProduct(url, grammage) {
    saveParamsInLocalStorage();
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            addedProducts: localStorage.getItem("productsEdit") === null ? null : JSON.parse(localStorage.productsEdit),
            productId: document.getElementById("productId").value,
            grammage: grammage
        },
        success: function (response) {
            localStorage.setItem("productsEdit", JSON.stringify(response.addedProducts));
            insertParam("change", "true");
        }
    });
}

function deleteProduct(id, url) {
    $.ajax({
        type: 'DELETE',
        url: url,
        data: {
            addedProducts: JSON.parse(localStorage.productsEdit),
            tempId: id
        },
        success: function (response) {
            localStorage.setItem("productsEdit", JSON.stringify(response.addedProducts));
            insertParam("change", "true");
        }
    });
}


function saveParamsInLocalStorage() {
    localStorage.setItem("paramsEdit", JSON.stringify({
        monday: document.getElementById("Monday").checked,
        tuesday: document.getElementById("Tuesday").checked,
        wednesday: document.getElementById("Wednesday").checked,
        thursday: document.getElementById("Thursday").checked,
        friday: document.getElementById("Friday").checked,
        saturday: document.getElementById("Saturday").checked,
        sunday: document.getElementById("Sunday").checked,
        name: document.getElementById("EditedDiet_DietName").value,
        active: document.getElementById("active").checked
    }));
}

function checkIfParamsWasSaved() {
    var params = localStorage.getItem("paramsEdit");
    if (params != null) {
        var parsedParams = JSON.parse(params);
        document.getElementById("Monday").checked = parsedParams.monday;
        document.getElementById("Tuesday").checked = parsedParams.tuesday;
        document.getElementById("Wednesday").checked = parsedParams.wednesday;
        document.getElementById("Thursday").checked = parsedParams.thursday;
        document.getElementById("Friday").checked = parsedParams.friday;
        document.getElementById("Saturday").checked = parsedParams.saturday;
        document.getElementById("Sunday").checked = parsedParams.sunday;
        document.getElementById("EditedDiet_DietName").value = parsedParams.name;
        document.getElementById("active").checked = parsedParams.active;
    }
}

function showAddedProducts(deleteUrl)
{
    if (localStorage.getItem("productsEdit") != null) {
        let panel = document.getElementById("productsPanel");
        
        let products = JSON.parse(localStorage.productsEdit);
        for (var i = 0; i < products.length; i++) {
            let tempId = products[i].tempId;
            let name = products[i].productName;
            let grammage = products[i].grammage;
            let kcal = products[i].calories;
            let proteins = products[i].proteins;
            let carbs = products[i].carbohydrates;
            let fats = products[i].fats;
            
            panel.innerHTML = panel.innerHTML +
                '<div class="row no-gutters productRow">' +
                '<div class="col-2 col-lg-3 h-100 acent cent">' + name + '</div>' +
                '<div class="col-9 col-lg-8 container h-100">' +
                '<div class="row no-gutters h-100">' +
                '<div class="col-3 acent cent">' + grammage + '</div>' +
                '<div class="col-9 container row no-gutters acent">' +
                '<div class="col-3 cent">' + kcal + '</div>' +
                '<div class="col-3 cent">' + parseFloat(proteins).toFixed(1).replace('.0', '') + '</div>' +
                '<div class="col-3 cent">' + parseFloat(fats).toFixed(1).replace('.0', '') + '</div>' +
                '<div class="col-3 cent">' + parseFloat(carbs).toFixed(1).replace('.0', '') + '</div>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '<div class="col-1 acent cent">' +
                '<button type="button" class="btnDelete" onclick="deleteProduct(' + tempId + ', \'' + deleteUrl + '\')">' +
                '<img class="delete" src="https://cdn.onlinewebfonts.com/svg/img_416864.png"></button>' +
                '</div>' +
                '</div>' +
                '</div>';
        }
    }
}

function savediet(url, id) {
    var monday = document.getElementById("Monday").checked;
    var tuesday = document.getElementById("Tuesday").checked;
    var wednesday = document.getElementById("Wednesday").checked;
    var thursday = document.getElementById("Thursday").checked;
    var friday = document.getElementById("Friday").checked;
    var saturday = document.getElementById("Saturday").checked;
    var sunday = document.getElementById("Sunday").checked;
    var name = document.getElementById("EditedDiet_DietName").value;
    var products = localStorage.getItem("productsEdit") === null ? null : JSON.parse(localStorage.productsEdit)

    if (!isEmpty(name)) {
        if (monday || tuesday || wednesday || thursday || friday || saturday || sunday) {
            if (products != null && products.length != 0) {
                dietDTO = {
                    DietId: id,
                    DietName: name,
                    Active: document.getElementById("active").checked,
                    Monday: monday,
                    Tuesday: tuesday,
                    Wednesday: wednesday,
                    Thursday: thursday,
                    Friday: friday,
                    Saturday: saturday,
                    Sunday: sunday
                };

                $.ajax({
                    type: 'POST',
                    url: url,
                    data: {
                        products: products,
                        dietDTO: dietDTO
                    },
                    success: function (response) {
                        if (response == false) {
                            $("#overrideModal").modal("show");
                            $("#yesOverride").click(function () {
                                $.ajax({
                                    type: 'POST',
                                    url: url,
                                    data: {
                                        products: products,
                                        dietDTO: dietDTO,
                                        overriding: true
                                    },
                                    success: function () {
                                        successSaved(url);
                                    }
                                });
                            });

                        } else {
                            successSaved(url);
                        }
                    }
                });
            } else {
                validation("Dodaj produkty do diety");
            }
        } else {
            validation("Wybierz dni obowiązywania diety");
        }
    } else {
        validation("Podaj nazwę diety");
    }
}

function successSaved(redirect) {
    localStorage.removeItem("paramsEdit")
    validation("Dieta zapisana pomyślnie ✅");
    $('#validationModal').on('hidden.bs.modal', function () {
        location.href = redirect;
    });
    setTimeout(function () {
        location.href = redirect;
    }, 3000);
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

var listOfProducts = [];
var IDarray = [];
var nameArray = [];
var pathArray = [];
var kcalArray = [];
var proteinsArray = [];
var fatsArray = [];
var carbsArray = [];
