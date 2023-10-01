
$.ajaxSetup({ async: false });
$(document).ready(function () {

    var UserGuid = "";
    let ipAddress = "";
    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
        console.log(e.ip);
    });
    UserGuid = sessionStorage.getItem("User1");
    getProfile(UserGuid);
    $('.profile-pic').attr('src', '/Content/images/avatar.jpg');

  
    //upload profile pic
    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.profile-pic').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }


    $(".file-upload").on('change', function () {
     
        var ext = $('#profile-pic').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            alert('invalid extension!,You can upload image in these formats(gif,png,jpg,jpeg)');
            return false;
        } else {
            readURL(this);
        }
       
       
    });

    $(".upload-button").on('click', function () {
     
        $(".file-upload").click();

       
    });
    //end profile pic upload block



    $("#btnSaveImage").click(function () {
       
        var fi = $('#profile-pic')[0].files[0];
        var formData = new FormData();
        formData.append('file', $('#profile-pic')[0].files[0]);
        formData.append('guid', UserGuid);
        $.ajax({
            url: 'Profile/UploadFiles',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (response) {
                if (response.RstKey == 1) {
                    $('#uploadProfileModal').modal('hide')
                    swal(
                        'Success!',
                        response.Message,
                        'success'
                    ).catch(swal.noop);
                }
                else if (response.RstKey == 3) {
                    swal(
                        'Error!',
                        response.Message,
                        'error'
                    ).catch(swal.noop);
                }
                else {
                    swal(
                        'Error!',
                        response.Message,
                        'error'
                    ).catch(swal.noop);
                }
            }
        });
    })

    $('#divLogout').click(function () {

        var UserGuid = sessionStorage.getItem("User1");

        //swal({
        //    title: "Are you Sure you want to logout",
        //    text: "Logout",
        //    icon: "warnning",
        //    buttons: true,
        //    dangerMode: true,
        //}, function succes(isDone) {
        //    logout(UserGuid);

        //})
        swal({
            title: "Are you sure you want to logout?",
            // text: "Logout",
            icon: "warnning",
            buttons: true,
            dangerMode: true,
            showCancelButton: true,
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
            //cache: false,
            url: "/Account/Logout/",
            //contentType: false,
            processData: false,
            data: formData,
            timeout: 40000,

            success: function (response) {

                if (response == 1) {
                    console.log(response);
                    sessionStorage.clear();
                    var base_url = window.location.origin;
                    window.location.href = base_url + "/Account/Index";
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
  

})

function getProfile(UserGuid) {

    var formData = new FormData();
    formData.append("guid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Profile/MyProfile",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.ProfileImage == "") {
                $('.profile-pic').attr('src', '/Content/images/avatar.jpg');
            } else {
                $('.profile-pic').attr('src', response.ProfileImage);
            }


            $("#txtFirstName").text(response.FirstName + " " + response.LastName);
            $("#txtEmail").text(response.Email);
            $("#txtMobileNumber").text(response.CountryCode + '-' + response.PhoneNumber);
            $("#txtState").text(response.State);
            $("#txtAddress").text(response.Address);
            $("#txtCountry").text(response.Country);
            $("#txtPostCode").text(response.PostCode);
        }
    });
    //$.ajax({
    //    url: "Account/GetCountry",
    //    cache: false,
    //    success: function (response) {
    //        console.log(response);
    //    }
    //});
}