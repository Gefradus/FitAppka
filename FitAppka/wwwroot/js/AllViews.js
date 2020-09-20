function setDisabled(element) {
    element.readOnly = true;
    element.style.backgroundColor = "#e9ecef";
}

function setEnabled(element) {
    element.readOnly = false;
    element.style.backgroundColor = 'white';
}

function setNotAllowedCursor(mouseTarget) {
    mouseTarget.style.cursor = 'not-allowed';
}

function setPointerCursor(mouseTarget) {
    mouseTarget.style.cursor = 'pointer';
}

function setNormalCursor(mouseTarget) {
    mouseTarget.style.cursor = 'text';
}