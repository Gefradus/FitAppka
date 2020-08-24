function drawChart(bialko, wegle, tluszcze) {
    var data = google.visualization.arrayToDataTable([
        ['Task', 'Gramy'],
        ['Białko', bialko],
        ['Węglowodany', wegle],
        ['Tłuszcze', tluszcze]
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
    rysujWykres();
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

/*$(document).ready(function () {
    document.getElementById("body").style.display = "block";
});
*/

function kolorujNaCzerwono(bialko, wegle, tluszcze, kalorie) {
    if (bialko > 100) {
        document.getElementById("bialko").style.backgroundColor = "red";
    }
    if (wegle > 100) {
        document.getElementById("wegle").style.backgroundColor = "red";
    }
    if (tluszcze > 100) {
        document.getElementById("tluszcze").style.backgroundColor = "red";
    }
    if (kalorie > 100) {
        document.getElementById("kalorie").style.backgroundColor = "red";
    }
}

function szklanka(woda, celkalorie) {
    var procent = (woda) / (celkalorie);

    if (procent > 0.005 && procent < 0.20) {
        document.getElementById("szklanka").src = "/FitAppka/img/10.png";
    }
    if (procent >= 0.20 && procent < 0.5) {
        document.getElementById("szklanka").src = "/FitAppka/img/35.png";
    }
    if (procent >= 0.5 && procent < 0.75) {
        document.getElementById("szklanka").src = "/FitAppka/img/50.png";
    }
    if (procent >= 0.75 && procent < 0.97) {
        document.getElementById("szklanka").src = "/FitAppka/img/75.png";
    }
    if (procent >= 0.97) {
        document.getElementById("szklanka").src = "/FitAppka/img/100.png";
    }
}


function ukryj(id) {
    if (document.getElementById(id).hidden == false) {
        document.getElementById(id).hidden = true;
    }
    else {
        document.getElementById(id).hidden = false;
    }
}

function edytujWode() {
    document.getElementById("pokazWode").hidden = true;
    document.getElementById("edycja").style.display = "block";
}

function hideModal() {
    location.reload();
}


function zaladuj() {
    pokazWode();
    schowaj();
    przepelnienie();
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(rysujWykres);

}

function validation(validMsg) {
    $("#validationModal").modal('show');
    document.getElementById("validatMsg").innerHTML = "<p>" + validMsg + "</p>";
    $("#closeVal").click(function () {
        $("#validationModal").modal('hide');
    })
}

function usunPosilekUrl(id, url) {
    $("#deleteModal").modal('show');
    $("#usun").click(function () {
        $.ajax({
            type: 'POST',
            url: url,
            data: { posilekID: id },
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
    $("#editPosilek").click(function () {
        var gram = parseInt(document.getElementById("gramatura").value, 10);
        if (gram < 1) {
            validation("Gramatura posiłku musi wynosić przynajmniej 1 gram.");
        }
        else if (gram > 9999) {
            validation("Gramatura posiłku nie może przekraczać 9999 gram.");
        }
        else if (gram >= 1 && gram <= 9999) {
            $.ajax({
                type: 'POST',
                url: url,
                data: { posilekID: id, gramatura: gram },
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

    document.getElementById("gramatura").value = gram;
    document.getElementById("name").innerHTML = nameArray[iter] + ", " + gram + "g";
    document.getElementById("kcalNapis").innerHTML = kcalArray[iter] + ' kcal';
    document.getElementById("bialkoNapis").innerHTML = 'Białko: ' + bialkoArray[iter] + ' g,';
    document.getElementById("tluszczeNapis").innerHTML = 'Tł.: ' + tluszczeArray[iter] + ' g,';
    document.getElementById("wegleNapis").innerHTML = 'Węgl.: ' + wegleArray[iter] + ' g';
}



$("#editModal").on('hide.bs.modal', function () {
    location.reload();
});



var pathArray = [];
var IDarray = [];
var nameArray = [];
var gramArray = [];
var pathArray = [];
var kcalArray = [];
var bialkoArray = [];
var tluszczeArray = [];
var wegleArray = []; 