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

        $("#btn-test-ajax").off("click").on("click", function () {
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

    return {
        Init: init,
        Update: update
    }
})(jQuery);

user.Init();