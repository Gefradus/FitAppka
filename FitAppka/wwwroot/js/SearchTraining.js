function showModalAddTraining() {
    $("#addTraining").modal('show');
}

function dodaj() {
    var czas = document.getElementById("czasWMinutach").value;
    var kalorie = document.getElementById("spaloneKcal").value;

    if (!isEmpty(czas) && !isEmpty(kalorie)) {
        document.getElementById("formAdd").submit();
    }
    else
    {
        if (isEmpty(czas) && isEmpty(kalorie)) {
            validation("Podaj czas trwania ćwiczenia i spalone kcal");
        }
        else if (isEmpty(czas)){
            validation("Podaj czas trwania ćwiczenia");
        }
        else{
            validation("Podaj ilość spalonych kcal");
        }
    }
}

function addTraining(dzienID, url) {

    var nazwa = document.getElementById("nazwaTreningu").value;
    var wydatek = document.getElementById("wydatek").value;
    var wydatekInteger = parseInt(wydatek);

    if (!isEmpty(nazwa) && !isEmpty(wydatek)) {
        if (!isNaN(wydatekInteger)) {
            if (nazwa.length >= 3) {
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: { dzienID: dzienID, nazwa: nazwa, wydatek: wydatekInteger },
                    success: function () {
                        location.reload();
                    }
                });
            }
            else {
                validation("Nazwa ćwiczenia musi wynosić min. 3 znaki");
            }
        }
        else {
            validation("Wydatek kcal musi być liczbą");
        }
    }
    else {
        if (isEmpty(nazwa) && isEmpty(wydatek)) {
            validation("Podaj nazwę ćwiczenia i wydatek kcal/min.");
        }
        else if (isEmpty(nazwa)) {
            validation("Podaj nazwę ćwiczenia.");
        }
        else {
            validation("Podaj wydatek kcal/min.");
        }
    }
}

function isEmpty(string) {
    return string.length == 0;
}


function validation(validMsg) {
    $("#validationModal").modal('show');
    document.getElementById("validatMsg").innerHTML = "<p>" + validMsg + "</p>";
    $("#closeVal").click(function () {
        $("#validationModal").modal('hide');
    })
}

function setParams(nazwa, kcalNaMin, treningTypId, dzienId) {
    document.getElementById("dzienId").value = dzienId;
    document.getElementById("treningTypId").value = treningTypId;
    document.getElementById("czasWMinutach").value = null;
    document.getElementById("spaloneKcal").value = null;
    document.getElementById("nazwacwiczenia").innerHTML = nazwa;
    document.getElementById("czas").hidden = false;
    document.getElementById("spalonekalorie").hidden = false;
    document.getElementById("dodajtrening").hidden = false;
    document.getElementById("kcalNaMin").value = kcalNaMin;
}

function changeCalorie() {
    var minuty = parseInt(document.getElementById("czasWMinutach").value);
    var kcalNaMin = document.getElementById("kcalNaMin").value;
    var spalonekalorie = parseInt(minuty * kcalNaMin);
    document.getElementById("spaloneKcal").value = spalonekalorie;
}