function cardioVal() {
    var time = document.getElementById("timeInMinutes").value;
    var burnedKcal = document.getElementById("burnedKcal").value;

    if (!isEmpty(time) && !isEmpty(burnedKcal)) {
        return true;
    }
    else {
        if (isEmpty(time) && isEmpty(burnedKcal)) {
            validation("Podaj czas trwania ćwiczenia i spalone kcal");
        }
        else if (isEmpty(time)) {
            validation("Podaj czas trwania ćwiczenia");
        }
        else {
            validation("Podaj ilość spalonych kcal");
        }
        return false;
    }
}

function strengthTrainingVal() {
    var sets = document.getElementById("sets").value;
    var reps = document.getElementById("reps").value;

    if (!isEmpty(sets) && !isEmpty(reps)) {
        return true;
    }
    else {
        if (isEmpty(sets) && isEmpty(reps)) {
            validation("Podaj liczbę serii i powtórzeń");
        }
        else if (isEmpty(sets)) {
            validation("Podaj liczbę serii"); 
        }
        else {
            validation("Podaj liczbę powtórzeń");
        }
        return false;
    }
}