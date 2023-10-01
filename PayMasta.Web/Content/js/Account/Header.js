var notificationCount = 0;
$(document).ready(function () {
    $("#IsShowCountInRed").hide();
    let ipAddress = "";
    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
        console.log(e.ip);
    });
    UserGuid = sessionStorage.getItem("User1");
    $("#spnHeaderName").text(sessionStorage.getItem("UserName"));
    $("#spnHeaderName1").text(sessionStorage.getItem("UserName"));
    $('.img-circle').attr('src', sessionStorage.getItem("ProfileImage"));
    $('.user-image').attr('src', sessionStorage.getItem("ProfileImage"));

    $('#btnEmployerLogout').click(function () {


        // var UserGuid = sessionStorage.getItem("User1");
        swal({
            title: "Are you sure you want to logout?",
            // text: "Logout",
            icon: "warnning",
            buttons: true,
            dangerMode: true,
            showCancelButton: true,
        }, function succes(isDone) {
            logout(UserGuid, ipAddress);

        })

    });

    //
    getNotifications(UserGuid);


    $("#btnReadMessage").click(function () {

        updateNotifications(UserGuid);
    })

    //document.onkeypress = function (event) {
    //    event = (event || window.event);
    //    if (event.keyCode == 123) {
    //        return false;
    //    }
    //}
    //document.onmousedown = function (event) {
    //    event = (event || window.event);
    //    if (event.keyCode == 123) {
    //        return false;
    //    }
    //}
    //document.onkeydown = function (event) {
    //    event = (event || window.event);
    //    if (event.keyCode == 123) {
    //        return false;
    //    }
    //}

    //$(document).bind('contextmenu.namespace', function () {
    //    return false;
    //});

});

function logout(UserGuid, ipAddress) {
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    formData.append("DeviceId", ipAddress);
    $.ajax({
        type: "POST",
        //cache: false,
        url: "/Account/Logout/",
        //contentType: false,
        processData: false,
        data: formData,
        //  timeout: 40000,

        success: function (response) {

            if (response == 1) {
                console.log(response);
                sessionStorage.clear();
                var base_url = window.location.origin;
                window.location.href = '/Account';

                //window.location.reload();
            }
            else {

                swal(
                    'Error!',
                    'Please Try again.',
                    'error'
                ).catch(swal.noop);
                // window.location.href = "Account/Index";
            }

        }
    });
}

function getNotifications(id) {
    var formData = new FormData();
    formData.append("UserGuid", id);

    $.ajax({
        //type: "POST",
        //cache: false,
        //url: "/Support/GetNotificationByUserGuid",
        //contentType: false,
        //processData: false,
        //data: formData,
        //  timeout: 40000,
        type: "POST",
        cache: false,
        url: "/Support/GetNotificationByUserGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                var item = "";
                if (response.TotalCount > 0) {
                    notificationCount = response.TotalCount;
                    $("#IsShowCountInRed").show();
                    $("#noRecordFound").hide();
                }
                else { $("#IsShowCountInRed").hide(); }
                $.each(response.notificationsResponses, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class="notification-main-container"><div class="ntype"> <img class="nimage" src="../Content/images/mobile-icon.svg" />' +
                        '<p class="ndata" > ' + value.AlterMessage + '</p >' + '</div><p class="nday">' + value.NotificationTime + '</p></div>';
                });
                //$('#divBankList').append('<div value="' + value.BankCode + '">' + value.BankName + '</div>');
                $('#divNotifications').append(item).show();
                $("#noRecordFound").hide();
            }
            else {
                $("#noRecordFound").show();
                $("#IsShowCountInRed").hide();
            }

        }, error: function (error) {
            alert("session expires");
}
    });
}

function updateNotifications(id) {
    var formData = new FormData();
    formData.append("UserGuid", id);

    $.ajax({
        //type: "POST",
        //cache: false,
        //url: "/Support/GetNotificationByUserGuid",
        //contentType: false,
        //processData: false,
        //data: formData,
        //  timeout: 40000,
        type: "POST",
        cache: false,
        url: "/Support/UpdateNotificationByUserGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                var item = "";
                if (response.TotalCount > 0) {
                    $("#IsShowCountInRed").show();
                }
                else { $("#IsShowCountInRed").hide(); }
                $.each(response.notificationsResponses, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class="notification-main-container"><div class="ntype"> <img class="nimage" src="../Content/images/mobile-icon.svg" />' +
                        '<p class="ndata" > ' + value.AlterMessage + '</p >' + '</div><p class="nday">' + value.NotificationTime + '</p></div>';
                });
                //$('#divBankList').append('<div value="' + value.BankCode + '">' + value.BankName + '</div>');
                $('#divNotifications').append(item).show();
            }
            else {

            }

        }
    });
}