$(document).ready(function () {

    getPrivacy();

});


function getPrivacy() {
    var formData = new FormData();
    formData.append("id", 1);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/CMS/GetPrivacyPolicy/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                var value = response.getCms;
                $("#privacyPolicyDetails").html(value.Detail);

            }
            else {

            }
        }
    });
}