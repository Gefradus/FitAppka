function isDate(date) {
    return (new Date(date) !== "Invalid Date" && !isNaN(new Date(date))) ? true : false;
}  

function checkForm() {
    let weight = document.getElementById("weight").value;
    let growth = document.getElementById("growth").value;
    let dateOfBirth = document.getElementById("datepicker").value;

    if (!isNumeric(weight) || !isNumeric(growth) || !isDate(dateOfBirth.toString())) {

        let invalidGrowth = document.getElementById("invalidGrowth");

        document.getElementById("invalidWeight").hidden = isNumeric(waga);
        invalidGrowth.hidden = isNumeric(wzrost);
        document.getElementById("invalidDate").hidden = isDate(dateOfBirth.toString());

        if (isNumeric(weight)) { invalidGrowth.style.position = 'relative'; }
        else { invalidGrowth.style.position = 'absolute'; }

        $('html, body').animate({ scrollTop: 0 }, 'slow');

        return false;
    }

    return true;
} 

function setFlag(flaga) {
    if (flaga == '1') {
        return true;
    }
}

function blockPace() {
    if (document.getElementById("maintenance").checked) {
        document.getElementById("pace").value = 0.4;
    }
}


function setDefaultPace() {
    if (document.getElementById("pace") == null) {
        document.getElementById("pace") = 0.4;
    }
}


function decrementPace() {
    if (!document.getElementById("maintenance").checked) {
        var pace = document.getElementById("pace");
        var value = parseFloat(pace.value).toFixed(1);
        if (value > 0.1) {
            pace.value = parseFloat(value - 0.1).toFixed(1);
        }
    }
}

function incrementPage() {
    if (!document.getElementById("maintenance").checked) {
        var pace = document.getElementById("pace");
        var value = pace.value;
        if (value <= 0.9) {
            value = parseFloat(value) + 0.1;
            pace.value = value.toFixed(1);
        }
    }
}