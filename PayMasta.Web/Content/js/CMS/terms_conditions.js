$(document).ready(function () {

    getterms();

});


function getterms() {
    var formData = new FormData();
    formData.append("id", 1);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/CMS/GetTermAndCondition/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                var value = response.getCms;
                $("#termsConditionDetails").html(value.Detail);

            }
            else {

            }
        }
    });
}