function drawChart(proteins, carbs, fats) {
    var data = google.visualization.arrayToDataTable([
        ['Task', 'Gramy'],
        ['Białko', proteins],
        ['Węglowodany', carbs],
        ['Tłuszcze', fats]
    ]);

    var options = {
        legend: { position: 'none' },
        backgroundColor: { fill: 'transparent' },
        chartArea: { width: '65%', height: '75%' },
        colors: ['#3936dc', '#b017c2', 'orange']
    };

    var chart = new google.visualization.PieChart(document.getElementById('piechart'));
    chart.draw(data, options);
}


$(window).resize(function () {
    createChart();
})

$(document).scroll(function () {
    localStorage['page'] = document.URL;
    localStorage['scrollTop'] = $(document).scrollTop();
});

$(document).ready(function () {
    if (localStorage['page'] == document.URL) {
        $('html, body').animate({ scrollTop: localStorage['scrollTop'] }, 'slow');
    }
});


function colorItRed(proteins, carbs, fats, calories) {
    if (proteins > 100) {
        document.getElementById("proteins").style.backgroundColor = "red";
    }
    if (carbs > 100) {
        document.getElementById("carbs").style.backgroundColor = "red";
    }
    if (fats > 100) {
        document.getElementById("fats").style.backgroundColor = "red";
    }
    if (calories > 100) {
        document.getElementById("calories").style.backgroundColor = "red";
    }
}

function glass(water, caloriesTarget) {
    var percentage = (water) / (caloriesTarget);

    if (percentage > 0.005 && percentage < 0.20) {
        document.getElementById("glass").src = "/FitAppka/img/10.png";
    }
    if (percentage >= 0.20 && percentage < 0.5) {
        document.getElementById("glass").src = "/FitAppka/img/35.png";
    }
    if (percentage >= 0.5 && percentage < 0.75) {
        document.getElementById("glass").src = "/FitAppka/img/50.png";
    }
    if (percentage >= 0.75 && percentage < 0.97) {
        document.getElementById("glass").src = "/FitAppka/img/75.png";
    }
    if (percentage >= 0.97) {
        document.getElementById("glass").src = "/FitAppka/img/100.png";
    }
}


function hidePanel(id) {
    if (document.getElementById(id).hidden == false) {
        document.getElementById(id).hidden = true;
    }
    else {
        document.getElementById(id).hidden = false;
    }
}

function editingWater() {
    document.getElementById("EditedWater").value = document.getElementById("waterCount").value;
    document.getElementById("showWater").hidden = true;
    document.getElementById("waterEdit").hidden = false;
}

function closeEditingWater(water) {
    document.getElementById("waterCount").value = water;
    document.getElementById("showWater").hidden = false;
    document.getElementById("waterEdit").hidden = true;
}

function hideChart(proteins, carbs, fats) {
    if (!(proteins == 0 && carbs == 0 && fats == 0)) {
        document.getElementById("chartArea").style.display = "block";
    }
    else {
        document.getElementById("waterHeader").style.borderTopWidth = 0;
    }
}

function onload() {
    showWater();
    hide();
    overflow();
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(createChart);
}

function deleteMealUrl(id, url) {
    $("#mealID").val(id);
    $("#deleteModal").modal('show');
    $("#delete").click(function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            data: { id: $("#mealID").val() },
            success: function () {
                location.reload();
            }
        });
    });
}

function giveIterFromID(id){
    var iter = 0;
    for (var i = 0; i < IDarray.length; i++) {
        if (IDarray[i] == id) {
            iter = i;
        }
    }
    return iter;
}

function addWater(url, dayID, water, target) {
    var addedWater = parseInt($("#AddedWater").val());

    $.ajax({
        type: 'POST',
        url: url,
        data: {
            dayID: dayID,
            addedWater: addedWater
        },
        success: function () {
            //location.reload();
            document.getElementById("water").innerHTML = parseInt(addedWater + water) + " ml";
            glass(water, target);
        }
    });
}

function editWater(url, dayID, target) {
    var editedWater = parseInt($("#EditedWater").val());

    $.ajax({
        type: 'PUT',
        url: url,
        data: {
            dayID: dayID,
            editedWater: editedWater
        },
        success: function () {
            //location.reload();
            document.getElementById("water").innerHTML = editedWater + " ml";
            glass(water, target);
            closeEditingWater(editedWater);
        }
    });
}

function showEditModalUrl(id, url) {
    prepareModalData(id);

    $("#editModal").modal('show');
    $("#editMeal").click(function () {
        var gram = parseInt(document.getElementById("grammage").value, 10);
        if (gram < 1) {
            validation("Gramatura posiłku musi wynosić przynajmniej 1 gram.");
        }
        else if (gram > 9999) {
            validation("Gramatura posiłku nie może przekraczać 9999 gram.");
        }
        else if (gram >= 1 && gram <= 9999) {
            editMeal(url, gram);
        }
        else {
            validation("Należy podać liczbę");
        }
    });
}

function editMeal(url, gram) {
    $.ajax({
        type: 'PUT',
        url: url,
        data: {
            id: $("#mealID").val(),
            grammage: gram
        },
        success: function () {
            location.reload();
        }
    });
}

function prepareModalData(id) {
    var iter = giveIterFromID(id);
    var photoPath = pathArray[iter];
    var gram = gramArray[iter];

    if (photoPath == 'null') {
        document.getElementById("photoArea").innerHTML = '<img id="zdjecie" src="/FitAppka/img/miss.png" class="img-fluid" asp-append-version="true" />';
    }
    else {
        document.getElementById("photoArea").innerHTML = '<img id="zdjecie" src="' + "/FitAppka/photos/" + photoPath + '" class="img-fluid" asp-append-version="true" />';
    }

    document.getElementById("mealID").value = id;
    document.getElementById("grammage").value = gram;
    document.getElementById("name").innerHTML = nameArray[iter] + ", " + gram + "g";
    document.getElementById("kcalHeader").innerHTML = kcalArray[iter] + ' kcal';
    document.getElementById("proteinsHeader").innerHTML = 'Białko: ' + proteinsArray[iter] + ' g,';
    document.getElementById("fatsHeader").innerHTML = 'Tł.: ' + fatsArray[iter] + ' g,';
    document.getElementById("carbsHeader").innerHTML = 'Węgl.: ' + carbsArray[iter] + ' g';
}


var IDarray = [];
var nameArray = [];
var gramArray = [];
var pathArray = [];
var kcalArray = [];
var proteinsArray = [];
var fatsArray = [];
var carbsArray = []; 