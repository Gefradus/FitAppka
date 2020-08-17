function moreInfo() {
    var moreInfoDiv = document.getElementById("moreInfoDiv");
    var imgDown = document.getElementById("imgDown");

    if (moreInfoDiv.hidden == false) {
        moreInfoDiv.hidden = true;
        imgDown.src = 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9d/Arrow-down.svg/200px-Arrow-down.svg.png';
        imgDown.style.height = '15px';
        imgDown.style.width = '27px';
        imgDown.style.marginTop = '5px';
        imgDown.style.marginRight = '5px';
        imgDown.style.marginLeft = '0px';
    }
    else {
        imgDown.src = 'https://www.pinclipart.com/picdir/big/84-848449_png-file-icon-arrow-up-svg-clipart.png';
        moreInfoDiv.hidden = false;
        imgDown.style.height = '10px';
        imgDown.style.width = '16px';
        imgDown.style.marginTop = '6px';
        imgDown.style.marginRight = '7px';
        imgDown.style.marginLeft = '5px';
    }
}

function onlyInt(evt){
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
            return false;
    return true;
}

function onlyNumbers(evt) {
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        if (ASCIICode != 44 && ASCIICode != 46) //44 - przecinek, 46 - kropka
            return false;
    return true;
}

function notCommaOrDot(value, evt){
    var ifContains = contains(value,'.') && contains(value, ',')
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    return ((ASCIICode == 46 || ASCIICode == 44) && ifContains);
}

function checkValidity(isValid, event) {
    if (isValid || event.keyCode == 44) {
        return true;
    }
    else {
        return false;
    }
}

function contains(string, substring){
    return string.indexOf(substring) !== -1
}

function changeToInteger(value){
    var convertedInteger = parseInt(value.replace('e','').replace('E',''));

    if (convertedInteger.length > 5){
        return convertedInteger.slice(0, -(convertedInteger.length - 5))
    }
    else 
    {
        return convertedInteger;
    }
}

function changeToNumeric(value)
{
    var convertedNumeric = value.replace(/[^0-9$.,]/g, '').replace('/./g', ',');

    if (convertedNumeric.length > 9){
        return removeNonNumericChars(convertedNumeric.slice(0, -(convertedNumeric.length - 9)));
    }
    else 
    {
        return removeNonNumericChars(convertedNumeric);
    }
    
}

function removeNonNumericChars(string){
    var newString = '';

    for(var i = 0; i < string.length; i++){
        if(isNumeric(string[i]) || string[i] == ',' || string[i] == '.'){
            newString += string[i];
        }
    }

    return newString;
}

function isNumeric(s) {
    return !isNaN(s - parseFloat(s));
}