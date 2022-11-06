function AddAntiForegyToken(data) {
    data = data || {};
    data.__RequestVerificationToken = $("#__AjaxAntiForgeryForm input[name='__RequestVerificationToken']").val();
    return data;
}

$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        xhr.setRequestHeader('__RequestVerificationToken', $("#__AjaxAntiForgeryForm input[name='__RequestVerificationToken']").val());
        if (settings.type == "POST") {
            //settings.data = settings.data + "&__RequestVerificationToken="
        }
    }
});