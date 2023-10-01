
$(document).ready(function () {
    let ipAddress = "";
    // $('#loader').hide();
    var UserGuid = "";

    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
        console.log(e.ip);
    });

    UserGuid = sessionStorage.getItem("User1");

    $('#btnLogout').click(function () {
       
        var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you Sure you want to logout",
            text: "Logout",
            icon: "warnning",
            buttons: true,
            dangerMode: true,
        }, function succes(isDone) {
            logout(UserGuid);
        })

    });


    function logout(UserGuid) {
        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append("DeviceId", ipAddress);
        $.ajax({
            type: "POST",
            cache: false,
            url: "Logout",
            contentType: false,
            processData: false,
            data: formData,
            timeout: 40000,

            success: function (response) {
                console.log(response);
                sessionStorage.clear();
                // window.location.href = "Account/Index";
                if (response != null) {
                    //swal(
                    //    'Error!',
                    //    'Invalid OTP',
                    //    'error'
                    //).catch(swal.noop);
                }
                else {

                    swal(
                        'Error!',
                        ' OTP not sent',
                        'error'
                    ).catch(swal.noop);
                    // window.location.href = "Account/Index";
                }

            }
        });
    }

});

