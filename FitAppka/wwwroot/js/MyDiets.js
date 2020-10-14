function deleteDiet(url, id) {
    $("#DietId").val(id);
    $("#deleteModal").modal("show");

    $("#delete").click(function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            data: {
                id: $("#DietId").val()
            },
            success: function () {
                location.reload();
            }
        });
    });
}