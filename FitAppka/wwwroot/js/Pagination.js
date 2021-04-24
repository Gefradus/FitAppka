var paginationDiv = document.getElementsByClassName("pagination")[0];
var pageCount = paginationDiv.getElementsByTagName("li").length;
pageCount = pageCount - paginationDiv.getElementsByClassName("PagedList-skipToPrevious").length - paginationDiv.getElementsByClassName("PagedList-skipToNext").length;
if (pageCount < 2) {
    document.getElementsByClassName("pagination-container")[0].hidden = true;
}
