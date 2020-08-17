function wybor() {
    var select = document.getElementById("wybor");
    var selectedValue = select.options[select.selectedIndex].value;

    ukryjWszystko();

    if (selectedValue == '1') {
        document.getElementById("produkt").hidden = false;
    }
    if (selectedValue == '2') {
        document.getElementById("kalorie").hidden = false;
    }
    if (selectedValue == '3') {
        document.getElementById("plyny").hidden = false;
    }
}

function ukryjWszystko() {
    document.getElementById("produkt").hidden = true;
    document.getElementById("kalorie").hidden = true;
    document.getElementById("plyny").hidden = true;
}

function szukajDnia(typSzukan, htmlUrl) {
    var select = document.getElementById("normalDropDown");
    var selectedValue = select.options[select.selectedIndex].value;
    var od = 0;
    var dO = 0;

    if (typSzukan == 1) {
        od = parseInt(document.getElementById("odProdukt").value, 10);
        dO = parseInt(document.getElementById("doProdukt").value, 10);
    }
    if (typSzukan == 2) {
        od = parseInt(document.getElementById("odKalorie").value, 10);
        dO = parseInt(document.getElementById("doKalorie").value, 10);
    }
    if (typSzukan == 3) {
        od = parseInt(document.getElementById("odWoda").value, 10);
        dO = parseInt(document.getElementById("doWoda").value, 10);
    }

    var url = htmlUrl;
    window.location.href = url.replace('_id_', selectedValue).replace('_od_', od).replace('_dO_', dO).replace('_typ_', typSzukan);
}

function zaladuj() {
    $('#normalDropDown').chosen();
    przywrocStan();
}

function przywroc(typSzukania) {
    if (typSzukania == 2) {
        var select = document.getElementById("wybor");
        select.value = '2';
        ukryjWszystko();
        document.getElementById("kalorie").hidden = false;
    }
    if (typSzukania == 3) {
        var select = document.getElementById("wybor");
        select.value = '3';
        ukryjWszystko();
        document.getElementById("plyny").hidden = false;
    }
}
