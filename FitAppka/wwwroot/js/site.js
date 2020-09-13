$(document).click(function (event) {
    var clickover = $(event.target);
    var $navbar = $(".navbar-collapse");
    var _opened = $navbar.hasClass("in");
    if (_opened === true && !clickover.hasClass("navbar-toggle") && !clickover.hasClass("in")) {
        $navbar.collapse('hide');
    }
});

function siteLoad() {
    $(".num").on("keypress", function (event) {
        return onlyNumbers(event);
    });
    $(".int").on("keypress", function (event) {
        return onlyInt(event);
    });
}
