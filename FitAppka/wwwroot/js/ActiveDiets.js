function restoreDay() {
    var dayOfWeek = new URL(location.href).searchParams.get("dayOfWeek");
    if (dayOfWeek == null) { dayOfWeek = new Date().getDay() };
    if (dayOfWeek == 1) { $("#monday").attr("checked", true); }
    if (dayOfWeek == 2) { $("#tuesday").attr("checked", true); }
    if (dayOfWeek == 3) { $("#wednesday").attr("checked", true); }
    if (dayOfWeek == 4) { $("#thursday").attr("checked", true); }
    if (dayOfWeek == 5) { $("#friday").attr("checked", true); }
    if (dayOfWeek == 6) { $("#saturday").attr("checked", true); }
    if (dayOfWeek == 0) { $("#sunday").attr("checked", true); }
}