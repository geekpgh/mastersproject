$(function () {
    $("#ratings a").click(function () {
        var href = $(this).attr("href");
        $.get(href)
            .done(function(data) {
                $("#modal")
                    .html(data)
                    .modal();
            })
            .fail(function() {
                alert("An error occurred");
            });
        return false;
    });
});