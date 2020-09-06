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

function contains(string, substring) {
    return string.indexOf(substring) !== -1
}

function changeToInteger(value){
    var convertedInteger = parseInt(value.replace('e','').replace('E','')).toString();

    if (convertedInteger.length > 5){
        return removeNonNumericChars(convertedInteger.slice(0, -(convertedInteger.length - 5)));
    }
    else 
    {
        return removeNonNumericChars(convertedInteger);
    }
}

function changeToNumeric(value, intMaxLength)
{
    var convertedNumeric = value.replace(/[^0-9$.,]/g, '').replace('.', ',');

    if (countCommaInString(convertedNumeric) > 0 && convertedNumeric.length > (intMaxLength+2)) //przecinek
    {
        return removeNonNumericChars(convertedNumeric.slice(0, -(convertedNumeric.length - (intMaxLength+2))));
    }
    else if (countCommaInString(convertedNumeric) == 0 && convertedNumeric.length > intMaxLength) //bez przecinka
    {
        return removeNonNumericChars(convertedNumeric.slice(0, -(convertedNumeric.length - intMaxLength)));
    }
    else {
        return removeNonNumericChars(convertedNumeric);
    }
}

function removeNonNumericChars(string){
    var newString = '';
    var commaCount = 0;
    var numbersCount = 0;
    var zerosCount = 0;

    for(var i = 0; i < string.length; i++){
        let char = string[i];
        if(isNumeric(char) || isCommaOrDot(char)) {
            if(isCommaOrDot(char)) {
                if(commaCount < 1 && numbersCount > 0) {
                    commaCount++;
                    newString += char;
                }
            }
            else if(!(zerosCount == numbersCount && zerosCount > 0 && char == 0 && commaCount == 0)) {
                newString += char;
                numbersCount++;
                if(char == 0) {
                    zerosCount++;
                }
            }
        }
    }

    if (newString[0] == 0 && !(isCommaOrDot(newString[1])) && newString.length > 1) {
        newString = newString.substring(1);
    }

    return newString;
}

function countCommaInString(string) {
    var counter = 0;

    for(var i = 0; i < string.length; i++) {
        if(isCommaOrDot(string[i])) {
            counter++;
        }
    }

    return counter;
}

function changeToMaxIfGreater(value, max){
    if(parseFloat(value.replace(',','.'), 2) > max) {
        return max;
    }
    else {
        return value;
    }
}

function isCommaOrDot(char){
    return char == ',' || char == '.';
}

function isNumeric(s) {
    return !isNaN(s - parseFloat(s));
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