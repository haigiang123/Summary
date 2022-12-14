var user = (function ($) {

    var jsonData = [];

    var init = function () {
        console.info("userController is called");
        update();
    };

    var update = function () {
        

        jsonData = JSON.stringify({
            NewsID: 1,
            Title: "",
            Content: ""
        });

        $("#btn-test-ajax").on("click", function (e) {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: "/Login/TestAjax",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                //headers: AddAntiForegyToken({}), // can use IAuthorizationFilter to validate Token
                success: function (msg) {
                    console.log(msg);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    alert("Status: " + textStatus + "\n\n Error: " + errorThrown);
                }
            });
        });
    };

    var updateImage = function (e) {

        var imageData = [];
        $(e).parentsUntil("#div-update-image").find("td input[class^='description']").each(function (i, el) {
            imageData.push({ "id": i, "text": $(el).val() });
        });

        console.log(imageData);
    }

    return {
        Init: init,
        Update: update,
        UpdateImage: updateImage
    }
})(jQuery);

user.Init();