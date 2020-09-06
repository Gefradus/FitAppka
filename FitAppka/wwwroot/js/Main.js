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
        // $('html, body').scrollTop(localStorage['scrollTop']);
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

function editWater() {
    document.getElementById("showWater").hidden = true;
    document.getElementById("waterEdit").style.display = "block";
}

function hideModal() {
    location.reload();
}


function onload() {
    showWater();
    hide();
    overflow();
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(createChart);

}

function validation(validMsg) {
    $("#validationModal").modal('show');
    document.getElementById("validatMsg").innerHTML = "<p>" + validMsg + "</p>";
    $("#closeVal").click(function () {
        $("#validationModal").modal('hide');
    })
}

function deleteMealUrl(id, url) {
    $("#deleteModal").modal('show');
    $("#delete").click(function () {
        $.ajax({
            type: 'DELETE',
            url: url + "?id=" + id,
            dataType: "json",
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
            $.ajax({
                type: 'PUT',
                url: url,
                data: { id: id, grammage: gram },
                success: function () {
                    location.reload();
                }
            });
        }
        else {
            validation("Należy podać liczbę");
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

    document.getElementById("grammage").value = gram;
    document.getElementById("name").innerHTML = nameArray[iter] + ", " + gram + "g";
    document.getElementById("kcalHeader").innerHTML = kcalArray[iter] + ' kcal';
    document.getElementById("proteinsHeader").innerHTML = 'Białko: ' + proteinsArray[iter] + ' g,';
    document.getElementById("fatsHeader").innerHTML = 'Tł.: ' + fatsArray[iter] + ' g,';
    document.getElementById("carbsHeader").innerHTML = 'Węgl.: ' + carbsArray[iter] + ' g';
}

$("#editModal").on('hide.bs.modal', function () {
    location.reload();
});


var IDarray = [];
var nameArray = [];
var gramArray = [];
var pathArray = [];
var kcalArray = [];
var proteinsArray = [];
var fatsArray = [];
var carbsArray = []; 