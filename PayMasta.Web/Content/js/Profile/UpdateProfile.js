let FirstNameError = true;
let LastNameError = true;
let MobileNumberError = true;
let NinNoError = true;
let EmailError = true;
let CountryCodeError = true;
var UserGuid = "";
$(document).ready(function () {
    let ipAddress = "";
    // $('#loader').hide();
    $('.profile-pic').attr('src', '/Content/images/avatar.jpg');

    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
        console.log(e.ip);
    });
    $("#AddressDiv").hide();
   

    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);
    getProfile(UserGuid);
    viewProfile(UserGuid);


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
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg', 'jfif', 'webp']) == -1) {
            alert('Invalid format! Please use these formats(gif,png,jp,jpeg).');
            return false;
        } else {
            readURL(this);
        }


    });

    //$(".upload-button").on('click', function () {

    //    $(".file-upload").click();


    //});

    $("input:file").change(function () {
        readURL(this);
    });
    //end profile pic upload block



    $("#btnSaveImage").click(function () {

        var fi = $('#profile-pic')[0].files[0];
        if (fi == null) {
            swal(
                'Error!',
                'Please select Image',
                'error'
            ).catch(swal.noop);
            return false;
        }
        var formData = new FormData();
        formData.append('file', $('#profile-pic')[0].files[0]);
        formData.append('guid', UserGuid);
        $.ajax({
            url: '/Account/UploadFiles/',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (response) {
                $('#uploadProfileModal').modal('hide')
                if (response.RstKey == 1) {

                    //swal(
                    //    'Success!',
                    //    response.Message,
                    //    'success'
                    //).catch(swal.noop);

                    swal({
                        title: "Success",
                        text: "Your profile picture updated successfully",
                        icon: "Success",
                        buttons: true,
                        dangerMode: true,
                    }, function succes(isDone) {
                        $('#uploadProfileModal').modal('hide');
                        window.location.reload();
                    })

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

    $(function () {
        $('#txtFirstName').on('keyup', function (e) {
            var str = "";
            str = $('#txtFirstName').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtFirstName').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtFirstName').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });

    $(function () {
        $('#txtLastName').on('keyup', function (e) {
            var str = "";
            str = $('#txtLastName').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtLastName').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtLastName').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });
    $(function () {
        $('#txtMiddleName').on('keyup', function (e) {
            var str = "";
            str = $('#txtMiddleName').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtMiddleName').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtMiddleName').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });


    $("#btnRequestUpdateProfile").click(function () {

        var txtFirstName = $("#txtFirstName").val();
        var txtLastName = $("#txtLastName").val();
        var txtNinNumber = $("#txtNinNumber").val();
        var txtEmail = $("#txtEmail").val();
        var txtMobileNumber = $("#txtMobileNumber").val().split('-')[1];
        var txtMiddleName = $("#txtMiddleName").val();
        var txtComment = $("#txtComment").val();

        if (txtFirstName == '' || txtFirstName == "" || txtFirstName == null && txtFirstName.length < 2) {
            FirstNameError = false;
            $('#txtFirstNameError').show();
            $('#txtFirstNameError').text("Please enter first name");
        }
        else {
            $('#txtFirstNameError').hide();
            FirstNameError = true;
        }
        if (txtNinNumber == '' || txtNinNumber == "" || txtNinNumber == null) {

            $('#txtNinNumberError').show();
            $('#txtNinNumberError').text("Please enter NIN number");
            NinNoError = false;

        } else {
            $('#txtNinNumberError').hide();
            NinNoError = true;

        }
        if (txtLastName == '' || txtLastName == "" || txtLastName == null && txtFirstName.length < 2) {

            $('#txtLastNameError').show();
            $('#txtLastNameError').text("Please enter last name");
            LastNameError = false;

        } else {
            $('#txtLastNameError').hide();
            LastNameError = true;

        }

        if (txtEmail == '' || txtEmail == "" || txtEmail == null) {

            $('#txtEmailError').show();
            $('#txtEmailError').text("Please enter email");
            EmailError = false;

        } else {
            validateEmail();
            $('#txtEmailError').hide();
            EmailError = true;

        }

        if (txtMobileNumber == '' || txtMobileNumber == "" || txtMobileNumber == null) {

            $('#txtMobileNumberError').show();
            $('#txtEmailError').text("Please enter mobile number");
            MobileNumberError = false;
            $('#txtMobileNumberError').hide();
        } else {

            MobileNumberError = true;

        }

        if (FirstNameError == true && LastNameError == true && NinNoError == true && EmailError == true && MobileNumberError == true) {
            var formData = new FormData();
            formData.append('UserGuid', UserGuid);
            formData.append('FirstName', txtFirstName);
            formData.append('LastName', txtLastName);
            formData.append('CountryCode', "+234");
            formData.append('PhoneNumber', txtMobileNumber);
            formData.append('MiddleName', txtMiddleName);
            formData.append('Email', txtEmail);
            formData.append('Comment', txtComment);

            $.ajax({
                url: '/Account/UpdateUserProfileRequest',
                type: 'POST',
                data: formData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (response) {
                    $('#uploadProfileModal').modal('hide')
                    if (response.RstKey == 1) {
                        $("#rqstSentAdminModal").modal('show');

                        $("#rqstSentAdminModalBtn").click(function () {
                            window.location.href = "/Account/ViewProfile"
                        })


                        /*  swal({
                              title: "Success",
                              text: "Your profile update request sent to the admin successfully",
                              icon: "Success",
                              buttons: true,
                              dangerMode: true,
                          }, function succes(isDone) {
                              window.location.href ="/Account/ViewProfile"
                          })*/

                    }
                    else if (response.RstKey == 3) {
                        $("#rqstSentAdminModal").modal('hide');
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                    }
                    else {
                        $("#rqstSentAdminModal").modal('hide');
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                    }
                }
            });
        }
    });

    $("#openPasscodeModal").click(function () {
        $('#passcodeModal').modal('show');
        sendPassCodeOTP();
    });

    $("#btnResendPasscodeOtp").click(function () {
        sendPassCodeOTP();
    });

    let timerOn = false;

    function timer(remaining) {

        var m = Math.floor(remaining / 60);
        var s = remaining % 60;

        m = m < 10 ? '0' + m : m;
        s = s < 10 ? '0' + s : s;
        document.getElementById('timer').innerHTML = m + ':' + s;
        remaining -= 1;
        if (remaining == -1) {
            // IsOtpResent = true;
            $('#btnResend').css("color", "#32A4FF");
            $('#btnResend').prop('disabled', false);
        }

        if (remaining >= 0 && timerOn) {
            setTimeout(function () {
                timer(remaining);
            }, 1000);
            return;
        }

        if (!timerOn) {

            return;
        }

        // Do timeout stuff here

    }

  

    $("#btnSetPasscode").click(function () {
        var txtPasscode = $("#txtPasscode").val();
        var txtOTP = $("#txtOTP").val();

        if (txtPasscode.length < 4) {
            $('#txtPasscodeError').text('Please enter 4 digit Passcode');
            return;
        } else {
            $('#txtPasscodeError').text('');
        }

        if (txtOTP.length < 4) {
            $('#txtOTPError').text('Please enter 4 digit OTP');
            return;
        } else {
            $('#txtOTPError').text('');
        }


        $('.loader').show();
        var formData = new FormData();
        formData.append('UserGuid', UserGuid);
        formData.append('Passcode', txtPasscode);
        formData.append('OtpCode', txtOTP);

        $.ajax({
            url: '/Account/GeneratePasscode',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (response) {
                $('.loader').hide();
                if (response.RstKey === 3) {
                    $('#passcodeModal').modal('hide');
                    swal(
                        'Success!',
                        response.Message,
                        'success'
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

    });
   
});


function getProfile(UserGuid) {

    var formData = new FormData();
    formData.append("guid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "MyProfile",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.ProfileImage == "") {
                $('.profile-pic').attr('src', '/Content/images/avatar.jpg');

            } else {
                $('.profile-pic').attr('src', response.ProfileImage);
                sessionStorage.setItem("ProfileImage", response.ProfileImage);
            }

            $('#txtFirstName').text(response.FirstName);
            if (response.MiddleName == "" || response.MiddleName == null) {
                $('#middleNameDiv').hide();
            }
            else {
                $('#txtMiddleName').text(response.MiddleName);
            }

            $('#txtLastName').text(response.LastName);
            $('#txtNinNumber').text(response.NINNumber);
            $('#txtEmail').text(response.Email);
            $('#txtMobileNumber').text(response.CountryCode + "-" + response.PhoneNumber);
            $('#txtAddess').text(response.Address);
            $('#txtdob').text(response.DOB);
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
function viewProfile(UserGuid) {

    var formData = new FormData();
    formData.append("guid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "MyProfile",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            $("#txtFirstName").val(response.FirstName);
            $('#txtLastName').val(response.LastName);
            $('#txtNinNumber').val(response.NINNumber);
            $('#txtEmail').val(response.Email);
            $('#txtMobileNumber').val(response.CountryCode + "-" + response.PhoneNumber);
            $('#txtAddess').val(response.Address);
            $("#txtMiddleName").val(response.MiddleName);

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

function removeEmojis(string) {
    var regex = /(?:[\u2700-\u27bf]|(?:\ud83c[\udde6-\uddff]){2}|[\ud800-\udbff][\udc00-\udfff]|[\u0023-\u0039]\ufe0f?\u20e3|\u3299|\u3297|\u303d|\u3030|\u24c2|\ud83c[\udd70-\udd71]|\ud83c[\udd7e-\udd7f]|\ud83c\udd8e|\ud83c[\udd91-\udd9a]|\ud83c[\udde6-\uddff]|\ud83c[\ude01-\ude02]|\ud83c\ude1a|\ud83c\ude2f|\ud83c[\ude32-\ude3a]|\ud83c[\ude50-\ude51]|\u203c|\u2049|[\u25aa-\u25ab]|\u25b6|\u25c0|[\u25fb-\u25fe]|\u00a9|\u00ae|\u2122|\u2139|\ud83c\udc04|[\u2600-\u26FF]|\u2b05|\u2b06|\u2b07|\u2b1b|\u2b1c|\u2b50|\u2b55|\u231a|\u231b|\u2328|\u23cf|[\u23e9-\u23f3]|[\u23f8-\u23fa]|\ud83c\udccf|\u2934|\u2935|[\u2190-\u21ff])/g;
    return string.replace(regex, '');
}

function validateEmail() {
    var userinput = $('#txtEmail').val();



    if (userinput.length != '') {

        var pattern = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;

        if (!pattern.test(userinput)) {
            emailFormateError = false;
            swal({
                title: "Email format is not valid",
                text: "Try again",
                type: "warning",
                //showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                //confirmButtonText: "Delete",
                //closeOnConfirm: false
            }, function succes(isDone) {
                emailFormateError = false;
            })
        }
        else {
            emailFormateError = true
                ;
        }
    }


}

function sendPassCodeOTP() {
    $('#btnResendPasscodeOtp').prop('disabled', true);

    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    formData.append("Type", 2);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Account/ResendOtp",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            if (response == 1) {
                swal({
                    title: "Success",
                    text: "Verification otp sent to your mobile number",
                    icon: "Success",
                    buttons: true,
                    dangerMode: true,
                }, function succes(isDone) {
                    $('#btnResendPasscodeOtp').prop('disabled', true);
                    $('#btnResendPasscodeOtp').css("color", "#D3D3D3");
                    passcodeTimer(59);
                })
                //swal(
                //    'Success!',
                //    'Verifucation OTP sent to your email or mobile',
                //    'success'
                //).catch(swal.noop);

            }
            else {

                swal(
                    'Error!',
                    'Verification otp not sent!',
                    'error'
                ).catch(swal.noop);
                // window.location.href = "Account/Index";
            }

        }
    });
};

function passcodeTimer(remaining) {
    timerOn = true;
    var m = Math.floor(remaining / 60);
    var s = remaining % 60;

    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    document.getElementById('Passcodetimer').innerHTML = m + ':' + s;
    remaining -= 1;
    if (remaining == -1) {
        // IsOtpResent = true;
        $('#btnResendPasscodeOtp').css("color", "#32A4FF");
        $('#btnResendPasscodeOtp').prop('disabled', false);
    }

    if (remaining >= 0 && timerOn) {
        setTimeout(function () {
            passcodeTimer(remaining);
        }, 1000);
        return;
    }

    if (!timerOn) {

        return;
    }

    // Do timeout stuff here

}