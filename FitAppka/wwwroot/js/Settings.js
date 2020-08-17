function isDate(date) {
    return (new Date(date) !== "Invalid Date" && !isNaN(new Date(date))) ? true : false;
}  

function checkForm() {
    let waga = document.getElementById("waga").value;
    let wzrost = document.getElementById("wzrost").value;
    let dataUro = document.getElementById("datepicker").value;

    if (!isNumeric(waga) || !isNumeric(wzrost) || !isDate(dataUro.toString())) {

        let invalidWzrost = document.getElementById("invalidWzrost");

        document.getElementById("invalidWaga").hidden = isNumeric(waga);
        invalidWzrost.hidden = isNumeric(wzrost);
        document.getElementById("invalidDate").hidden = isDate(dataUro.toString());

        if (isNumeric(waga)) {
            invalidWzrost.style.position = 'relative';
        }
        else {
            invalidWzrost.style.position = 'absolute';
        }

        $('html, body').animate({ scrollTop: 0 }, 'slow');

        return false;
    }

    return true;
} 

function ustawFlage(flaga) {
    if (flaga == '1') {
        return true;
    }
}

function zablokujTempo() {
    tempo = document.getElementById("tempo");

    if (document.getElementById("utrzymanie").checked) {
        //tempo.disabled = true;
        tempo.value = 0.4;
    }
/*    else {
        tempo.disabled = false;
    }*/
}


function ustawDefault() {
    if (document.getElementById("tempo") == null) {
        document.getElementById("tempo") = 0.4;
    }
}


function dekrementuj() {
    if (!document.getElementById("utrzymanie").checked) {
        var tempo = document.getElementById("tempo");
        var value = parseFloat(tempo.value).toFixed(1);
        if (value > 0.1) {
            tempo.value = parseFloat(value - 0.1).toFixed(1);
        }
    }
}

function inkrementuj() {
    if (!document.getElementById("utrzymanie").checked) {
        var tempo = document.getElementById("tempo");
        var value = tempo.value;
        if (value <= 0.9) {
            value = parseFloat(value) + 0.1;
            tempo.value = value.toFixed(1);
        }
    }
}