function inputBlocking() {
    setCursor(document.getElementById("auto").checked);
}

function setCursor(autoChecked) {
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
    }
    else {
        setNormalCursor(element);
        element.disabled = false;
    }
}

function setNotAllowedCursor(mouseTarget) {
    mouseTarget.style.cursor = 'not-allowed';
}

function setNormalCursor(mouseTarget) {
    mouseTarget.style.cursor = 'text';
}