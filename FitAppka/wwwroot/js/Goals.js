function inputBlocking() {
    block(document.getElementById("auto").checked);
}

function block(autoChecked) {
    blockInputIfAutoChecked(autoChecked, document.getElementById("caloriesTarget"));
    blockInputIfAutoChecked(autoChecked, document.getElementById("proteinsTarget"));
    blockInputIfAutoChecked(autoChecked, document.getElementById("carbsTarget"));
    blockInputIfAutoChecked(autoChecked, document.getElementById("fatsTarget"));
}

function blockInputIfAutoChecked(autoChecked, element) {
    if (autoChecked) {
        setNotAllowedCursor(element);
        element.value = '';
        element.disabled = true;
        hideOrShowStars(true);
    }
    else {
        setNormalCursor(element);
        element.disabled = false;
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

function setNormalCursor(mouseTarget) {
    mouseTarget.style.cursor = 'text';
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