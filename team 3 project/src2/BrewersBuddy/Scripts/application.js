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

    $("#comment-form").submit(function () {
        var textarea = $("textarea", this);
        var button = $("input[type=submit]", this);
        var action = $(this).attr("action");

        button.attr("disabled", "disabled");

        $.post(action, $(this).serialize())
            .done(function (data) {
                $(textarea).val("");
                $("#no-comments").hide();
                
                var template = $("#comment-template");
                
                var html = template.html()
                    .replace("{{comment}}", data.Comment)
                    .replace("{{username}}", data.UserName)
                    .replace("{{date}}", data.PostDate);

                $("#batch-comments").append(html);
            })
            .fail(function (data) {
                alert("An error occurred. Please check that you did not enter more than 256 characters.");
                button.removeAttr("disabled");
            });

        return false;
    });
});