function blockIfAutoChecked() {
    block(document.getElementById("auto").checked);
}

function block(autoChecked) {
    var inputs = document.getElementsByClassName("form-control");
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].id != "burnGoal" && inputs[i].id != "timeGoal") {
            blockInputIfAutoChecked(autoChecked, inputs[i]);
        }
    }
}

function blockInputIfAutoChecked(autoChecked, element) {
    if (autoChecked) {
        setNotAllowedCursor(element);
        element.value = element.defaultValue;
        element.title = "Pole zablokowane - cele dietetyczne ustawiane automatycznie"
        setDisabled(element);
        hideOrShowStars(true);
    }
    else {
        setNormalCursor(element);
        element.title = "";
        setEnabled(element);
        hideOrShowStars(false);
    }
}


function hideOrShowStars(boolean) {
    var list = document.getElementsByClassName("star");
    for (var i = 0; i < list.length; i++) {
        list[i].hidden = boolean;
    }
}

function setNotAllowedCursor(mouseTarget) {
    mouseTarget.style.cursor = 'not-allowed';
}

function saveGoal() {
    if (document.getElementById("auto").checked) {
        document.getElementById("form").submit();
    }
    else {
        var kcal = $("#caloriesTarget").val();
        var proteins = $("#proteinsTarget").val();
        var carbs = $("#carbsTarget").val();
        var fats = $("#fatsTarget").val();
        var burn = $("#burnGoal").val();
        var time = $("#timeGoal").val();

        if (isNumeric(kcal) && isNumeric(proteins) && isNumeric(fats) && isNumeric(carbs)) {
            if (isEmptyOrNumeric(burn) && isEmptyOrNumeric(time)) {
                if (setMinValue(500, "Kcal muszą wynosić min. 500", kcal)) {
                    if (setMaxValue(20000, "Kcal może wynosić max. 20000", kcal)) {
                        if (setMaxValue(5000, "Białko może wynosić max. 5000g", proteins)) {
                            if (setMaxValue(5000, "Węglowodany mogą wynosić max. 5000g", carbs)) {
                                if (setMaxValue(2222, "Tłuszcze mogą wynosić max. 2222g", fats)) {
                                    document.getElementById("form").submit();
                                }
                            }
                        }
                    }
                }
            }
            else {
                validation("Błędy walidacji - uzupełnij pola liczbami");
            }
        }
        else {
            if (isEmpty(kcal) || isEmpty(proteins) || isEmpty(carbs) || isEmpty(fats)) {
                validation("Uzupełnij obowiązkowe pola");
            }
            else {
                validation("Błędy walidacji - uzupełnij pola liczbami");
            }
        }
    }
}

function setMinValue(minValue, message, element) {
    if (parseInt(element) < minValue) {
        validation(message);
        return false;
    }
    else {
        return true;
    }
}

function setMaxValue(maxValue, message, element) {
    if (parseInt(element) > maxValue) {
        validation(message);
        return false;
    }
    else {
        return true;
    }
}

function moreInfo() {
    var moreInfoDiv = document.getElementById("moreInfoDiv");
    var imgDown = document.getElementById("imgDown");
    var moreOrLess = document.getElementById("moreOrLess");

    if (moreInfoDiv.hidden == false) {
        moreInfoDiv.hidden = true;
        imgDown.src = 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9d/Arrow-down.svg/200px-Arrow-down.svg.png';
        imgDown.style.height = '15px';
        imgDown.style.width = '27px';
        imgDown.style.marginTop = '5px';
        imgDown.style.marginRight = '5px';
        imgDown.style.marginLeft = '0px';
        moreOrLess.innerHTML = "Pokaż więcej celów dziennego spożycia";
    }
    else {
        imgDown.src = 'https://www.pinclipart.com/picdir/big/84-848449_png-file-icon-arrow-up-svg-clipart.png';
        moreInfoDiv.hidden = false;
        imgDown.style.height = '10px';
        imgDown.style.width = '16px';
        imgDown.style.marginTop = '6px';
        imgDown.style.marginRight = '7px';
        imgDown.style.marginLeft = '5px';
        moreOrLess.innerHTML = "Pokaż mniej celów dziennego spożycia";
    }
}

function inputsOnChange() {
    var $j = jQuery.noConflict();
    $j(".target").on("change", function () {
        this.value = changeToInteger(this.value)
    });

    $j("#timeGoal").on("change", function () {
        this.value = changeToMaxIfGreaterInt(this.value, 1440);
    });

    $j("#burnGoal").on("change", function () {
        this.value = changeToMaxIfGreaterInt(this.value, 5000);
    });

    $j(".micro").on("change", function () {
        this.value = changeToMaxIfGreaterFloat(this.value, 5, 10000);
    });
}