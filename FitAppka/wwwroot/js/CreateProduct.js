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
