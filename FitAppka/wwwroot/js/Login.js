function openModelAfterSubmit(wasOpened) {
    var modal = $("#loginModal");
    if (wasOpened == "True") {
        modal.modal("show");
    } else {
        if (!modal.hasClass("fade")) {
            modal.addClass("fade");
        }
    }
}

function modalDismiss(wasOpened, url) {
    $("#loginModal").modal("hide");
    if (wasOpened == "True") {
        document.location = url;
    }
}