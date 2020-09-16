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

function setFlag(flag) {
    if (flag == '1') {
        return true;
    }
}

function setWeightAndGrowth(weight, growth) {
    document.getElementById("weight").value = parseInt(weight);
    document.getElementById("growth").value = parseInt(growth);
}

function setActivityLevel(activity) {
    var activityList = document.getElementsByName("LevelOfActivity");
    for (let i = 0; i < activityList.length; i++) {
        if (activityList[i].value == activity) {
            activityList[i].checked = true;
        }
    }
}

function setSex(sex) {
    if (setFlag(sex)) {
        document.getElementById("men").checked = true;
    }
    else {
        document.getElementById("women").checked = true;
    }
}

function setPace(pace) {
    document.getElementById("pace").value = parseFloat(pace).toFixed(1)
}

function setWeightChangeGoal(changeGoal) {
    if (changeGoal == 2) {
        document.getElementById("maintenance").checked = true;
    }
    if (changeGoal == 3) {
        document.getElementById("gainWeight").checked = true;
    }
}

function setDatepickerValue(value) {
    document.getElementById("datepicker").value = value;
}

function setMeals(breakfast, lunch, dinner, dessert, snack, supper) {
    document.getElementById("breakfast").checked = setFlag(breakfast);
    document.getElementById("lunch").checked = setFlag(lunch);
    document.getElementById("dinner").checked = setFlag(dinner);
    document.getElementById("dessert").checked = setFlag(dessert);
    document.getElementById("snack").checked = setFlag(snack);
    document.getElementById("supper").checked = setFlag(supper);
}

function setNavbar() {
    document.getElementById("headerFirstRun").hidden = true;
    document.getElementById("allViewsNav").hidden = false;
}

function blockWeightInput() {
    let weight = document.getElementById("weight");
    setNotAllowedCursor(weight);
    weight.readOnly = true;
    weight.style.backgroundColor = "#e9ecef";
    document.getElementById("weightTip").hidden = false;
}

function setNotAllowedCursor(mouseTarget) {
    mouseTarget.style.cursor = 'not-allowed';
}

function setNormalCursor(mouseTarget) {
    mouseTarget.style.cursor = 'pointer';
}

function setTitle(element1, element2, title) {
    element1.title = title;
    element2.title = title;
}

function blockPace() {
    var incBtn = document.getElementById("incBtn");
    var decBtn = document.getElementById("decBtn");
    
    if (document.getElementById("maintenance").checked) {
        document.getElementById("pace").value = 0.4;
        setNotAllowedCursor(incBtn);
        setNotAllowedCursor(decBtn);
        setTitle(incBtn, decBtn, "Zmiany zablokowane gdyż celem jest utrzymanie wagi")
    }
    else {
        setNormalCursor(incBtn);
        setNormalCursor(decBtn);
        setTitle(incBtn, decBtn, "");
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

function incrementPace() {
    if (!document.getElementById("maintenance").checked) {
        var pace = document.getElementById("pace");
        var value = pace.value;
        if (value <= 0.9) {
            value = parseFloat(value) + 0.1;
            pace.value = value.toFixed(1);
        }
    }
}

function birthDateDatepicker() {
    var $j = jQuery.noConflict();
    $j("#datepicker").datepicker({
        minDate: new Date(1850, 1, 1),
        maxDate: '0',
        dateFormat: 'yy-mm-dd',
        monthNames: ["Styczeń", "Luty", "Marzec", "Kwiecień",
            "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień",
            "Październik", "Listopad", "Grudzień"],
        monthNamesShort: ["Sty", "Lut", "Mar", "Kwi",
            "Maj", "Cze", "Lip", "Sie", "Wrz",
            "Paź", "Lis", "Gru"],
        dayNames: ['Niedziela', 'Poniedziałek', 'Wtorek', 'Środa', 'Czwartek', 'Piątek', 'Sobota'],
        dayNamesShort: ['Ndz', 'Pon', 'Wto', 'Śro', 'Czw', 'Pt', 'Sob'],
        dayNamesMin: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],
        changeYear: true,
        changeMonth: true,
        yearRange: "1970:+nn"
    });
    $j("#datepicker").datepicker("setDate", new Date(2000,1,1));
}