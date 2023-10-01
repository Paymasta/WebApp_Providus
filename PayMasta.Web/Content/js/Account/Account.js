
$(document).ready(function () {

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

    let ipAddress = "NA";
    $('#loader1').hide();



    //$.getJSON("https://api.ipify.org/?format=json", function (e) {
    //    ipAddress = e.ip;
    //    console.log(e.ip);
    //});
    let userTypeForSignup = 4;
    $('#divMobile').hide();
    $('#passcheck').hide();
    $('#usercheck').hide();
    $('#mobilechk').hide();
    $('#passError').hide();
    $('#strengthMessage').hide();
    /*signup errors*/
    $('#txtEmailRegisterError').hide();
    $('#txtMobileRegisterError').hide();
    $('#txtPasswordRegisterError').hide();
    $('#txtConfirmPasswordError').hide();
    $('#txtForgetMobileError').hide();
    $('#txtForgetEmailError').hide();
    $('#spntxtForgeContPasswordError').hide();


    $('#TestBnt').click(function () {

        $("#verificationmodal").modal('show');

    });
    let IsOtpResent = false;
    let usernameError = true;
    let emailFormateError = true;
    let mobileFormateError = true;
    let ChnageOldpasswordError = true;
    let ChnageNewpasswordError = true;

    let passwordError = true;
    let mobileNumberError = true;
    let ConpasswordError = true;
    let ConpasswordError1 = true
    var mobileVerify = "";
    let logintype = 0;
    var OtpPass = "";
    var passwordStrengh = false;
    var ChangepasswordStrengh = false;
    let timerOn = false;
    var forgetOtpEmailOrMobile = "";
    let IsTermConditionChecked = false;
    let IsppChecked = false;
    let IsCardChecked = false;
    $("#registerDiv").click(function () {
        $("#registerModal").modal('show');
    });

    $("#registerDivUser").click(function () {
        $("#registerModalByNuban").modal('show');
    });
    $("#registerDivUser1").click(function () {
        $("#registerModalByNuban").modal('show');
    });
    $("#sidebar-registerDiv").click(function () {
        $("#registerModal").modal('show');
    });

    //$("#item-blog").click(function () {
    //    window.location.href = "http://www.paymasta.co/";
    //})
    //-------------------------------------------space validation


    //$('#txtConfirmPassword').keypress(function (e) {
    //    var $this = $(this);
    //    $(this).val($this.val().replace(/(\s{2,})|[^a-zA-Z0-9_']/g, ' ').replace(/^\s*/, ''));
    //});

    //-------------------------------------------

  

    $(function () {
        $('#txtPasswordRegister').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtForgetPassword').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });
    $(function () {
        $('#txtNewChangePassword').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });
    $(function () {
        $('#txtChangeConfirmPassword').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });
    $(function () {
        $('#txtForgetConfirmPassword').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtConfirmPassword').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtEmailRegister').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtEmail').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });
    $('#txtPasswordRegister').keyup(function () {
        $('#strengthMessage').show();
        $('#strengthMessage').html(checkStrength($('#txtPasswordRegister').val()))
    })
    function checkStrength(password) {

        var strength = 0
        if (password.length < 7) {
            $('#strengthMessage').removeClass()
            //$('#strengthMessage').addClass('Short')
            return 'Too short'
        }

        var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*_])(?=.{8,30}$)");

        if (regex.test(password)) {
            // $('#txtPasswordRegisterError').removeClass()
            $('#txtPasswordRegisterError').hide()
            passwordStrengh = true;
            return 'Strong'
        } else {
            $('#txtPasswordRegisterError').show();
            $('#txtPasswordRegisterError').text("Password must contain at least one uppercase,one lowercase,one special character and one number.password should be 8 characters")
            // $('#strengthMessage').addClass('Weak')
            passwordStrengh = false;
            // return false;

        }

    }
    function checkStrength1(password) {

        var strength = 0
        if (password.length < 7) {
            $('#strengthMessage1').removeClass()
            // $('#strengthMessage1').addClass('Short')
            return false;//'Too short'
        }

        var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*_])(?=.{8,30}$)");

        if (regex.test(password)) {
            $('#strengthMessage1').hide()
            //$('#strengthMessage1').addClass('Strong')
            passwordStrengh = true;
            // return 'Strong'
        } else {
            $('#strengthMessage1').show()
            $('#strengthMessage1').text('Password must contain at least one uppercase,one lowercase,one special character and one number.')
            //return 'Weak'
            passwordStrengh = false;
        }

    }
    function checkStrength1ForChange(password) {

        var strength = 0
        if (password.length < 7) {
            $('#strengthMessage1').removeClass()
            // $('#strengthMessage1').addClass('Short')
            return false;//'Too short'
        }

        var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*_])(?=.{8,30}$)");

        if (regex.test(password)) {
            $('#strengthMessage11').hide()
            //$('#strengthMessage1').addClass('Strong')
            ChangepasswordStrengh = true;
            // return 'Strong'
        } else {
            $('#strengthMessage11').show()
            $('#strengthMessage11').text('Password must contain at least one uppercase,one lowercase,one special character and one number.')
            //return 'Weak'
            ChangepasswordStrengh = false;
        }

    }


    $('#txtForgetPassword').keyup(function () {
        $('#strengthMessage').show();
        $('#strengthMessage1').html(checkStrength1($('#txtForgetPassword').val()))
    })
    $('#txtNewChangePassword').keyup(function () {
        $('#strengthMessage').show();
        $('#strengthMessage1').html(checkStrength1ForChange($('#txtNewChangePassword').val()))
    })
    $("#closeModal").click(function () {

        $('#txtEmailRegister').val('');
        $('#txtMobileRegister').val('');
        $('#txtPasswordRegister').val('');
        $('#txtConfirmPassword').val('');

        $('#txtEmailRegisterError').hide();
        $('#txtMobileRegisterError').hide();
        $('#txtPasswordRegisterError').hide();
        $('#txtConfirmPasswordError').hide();
        $("#txtMobilelengthError").hide();

        window.location.reload();
    });

    $("#logincloseModal").click(function () {
        $('#txtEmail').val('');
        $('#txtMobile').val('');
        $('#txtPassword').val('');
        $('#usercheck').hide();
        $("#mobilechk").hide();
        $("#passcheck").hide();
        window.location.reload();
    });

    $("#btnRegister3").click(function () {
        $('#txtEmail').val('');
        $('#txtMobile').val('');
        $('#txtPassword').val('');
        $('#usercheck').hide();
        $("#mobilechk").hide();
        $("#passcheck").hide();
        $("#loginModal").modal('hide');
        $("#registerModal").modal('show');
    });


    $("#btnOpenSignUpmodalFromCreatePassword").click(function () {
        $("#createpasswordmodal").modal('hide');
        $("#registerModal").modal('show');
    });
    $("#btnOpenSignUpmodalFromChangePassword").click(function () {
        $("#Changecreatepasswordmodal").modal('hide');
        $("#registerModal").modal('show');
    });




    $("#btnRegister1").click(function () {
        $("#loginModal").modal('hide');
        $("#registerModal").modal('show');
        $("#forgetPasswordModal").modal('hide');
    });

    $("#LoginDiv").click(function () {
        $("#loginModal").modal('show');
    });

    $("#sidebar-loginDiv").click(function () {
        $("#loginModal").modal('show');
    });

    $("#btnLoginModal").click(function () {
        $(".modal-body input").val("");
        $("#loginModal").modal('show');
        $("#registerModal").modal('hide');


    });

    $("#forgetPasswordAnchor").click(function () {
        $('#forgetPasswordModalloader1').hide();
        $("#forgetPasswordModal").modal('show');
        $("#registerModal").modal('hide');
        $("#loginModal").modal('hide');
    });


    $('input:radio[name="userLogin"]').change(function () {

        if ($(this).val() == 0) {
            $('#divMobile').hide();
            $('#divEmail').show();
            $('#usercheck').hide();
            logintype = 0
        }
        else if ($(this).val() == 1) {
            $('#divEmail').hide();
            $('#divMobile').show();
            $('#usercheck').hide();
            logintype = 1;
        }
        else if ($(this).val() == 2) {
           
            $('#divEmail').hide();
            $('#divMobile').show();
            $('#usercheck').hide();
            logintype = 1;
        }
    });

    $('input:radio[name="userSignup"]').change(function () {

        if ($(this).val() == 0) {
            userTypeForSignup = 4;
            $('#mainDivCardApply').show();
            $('#txtEmailRegister').val('');
            $('#txtMobileRegister').val('');
            $('#txtPasswordRegister').val('');
            $('#txtConfirmPassword').val('');

            $('#txtEmailRegisterError').hide();
            $('#txtMobileRegisterError').hide();
            $('#txtMobilelengthError').hide();
            $('#txtPasswordRegisterError').hide();
            $('#txtConfirmPasswordError').hide();
            $('#txtConfirmPasswordError1').hide();
            $('#chkTermConditionError').hide();
            $('#chkppnError').hide();

        }
        else if ($(this).val() == 1) {
            $('#txtEmailRegister').val('');
            $('#txtMobileRegister').val('');
            $('#txtPasswordRegister').val('');
            $('#txtConfirmPassword').val('');
            userTypeForSignup = 3;
            $('#mainDivCardApply').hide();//Raaz

            $('#txtEmailRegisterError').hide();
            $('#txtMobileRegisterError').hide();
            $('#txtMobilelengthError').hide();
            $('#txtPasswordRegisterError').hide();
            $('#txtConfirmPasswordError').hide();
            $('#txtConfirmPasswordError1').hide();
            $('#chkTermConditionError').hide();
            $('#chkppnError').hide();
        }
        else if ($(this).val() == 2) {
            $("#registerModalByNuban").modal('show');
            $("#registerModal").modal('hide');

            $('#txtEmailRegister').val('');
            $('#txtMobileRegister').val('');
            $('#txtPasswordRegister').val('');
            $('#txtConfirmPassword').val('');
            userTypeForSignup = 3;
            $('#mainDivCardApply').hide();//Raaz

            $('#txtEmailRegisterError').hide();
            $('#txtMobileRegisterError').hide();
            $('#txtMobilelengthError').hide();
            $('#txtPasswordRegisterError').hide();
            $('#txtConfirmPasswordError').hide();
            $('#txtConfirmPasswordError1').hide();
            $('#chkTermConditionError').hide();
            $('#chkppnError').hide();
        }

      

    });

    //------------Auto switch
    $(".otp").keyup(function () {

        if (this.value.length == this.maxLength) {
            $(this).next('.otp').select();
        }
    });




    // Validate Confirm Password
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
    // Validate Confirm Password
    function validateEmailForSignup() {
        var userinput = $('#txtEmailRegister').val();



        if (userinput.length != '') {

            var pattern = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;// /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i

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
                    $('#btnRegister').prop('disabled', false);
                })
            }
            else {
                emailFormateError = true;
            }
        }


    }

    function validateMobile() {
        var txtMobile = $('#txtMobile').val();
        if (txtMobile.length >= 9 && txtMobile.length <= 11) {
            mobileFormateError = true;
        } else {
            alert('Please put  11  digit mobile number');
            mobileFormateError = false;
        }
    }


    //}
    // Submit button
    $('#btnLogin').click(function () {
       
       // $('#loader1').show();
        $('#btnLogin').prop('disabled', true);
        var EmailOrMobile = "";
        var email = $('#txtEmail').val();
        var txtMobile = $('#txtMobile').val();
        var password = $('#txtPassword').val();

        if (logintype == 0) {
            if (email == "") {
                $('#usercheck').show();
                usernameError = false;
                $('#btnLogin').prop('disabled', false);
                // return false;
            }
            else {
                $('#usercheck').hide();
                usernameError = true;
                // return true;

            }
            if (password == '' || password == "" || password == null) {
                $('#passcheck').show();
                passwordError = false;
                $('#btnLogin').prop('disabled', false);
                // return false;
            } else {
                $('#passcheck').hide();
                passwordError = true;
                // return true;
            }

            validateEmail();
        }
        else if (logintype == 1) {
            if (txtMobile == "") {
                $('#mobilechk').show();
                usernameError = false;
                $('#btnLogin').prop('disabled', false);
            }
            else {
                $('#mobilechk').hide();
                usernameError = true;
                // return true;

            }
            if (password == '' || password == "" || password == null) {
                $('#passcheck').show();
                passwordError = false;
                $('#btnLogin').prop('disabled', false);
                // return false;
            } else {
                $('#passcheck').hide();
                passwordError = true;
                //validateMobile();
                // return true;
            }

        }
        if ((usernameError == true) && (passwordError == true) && emailFormateError == true && mobileFormateError == true) {


            if (email != "") {
                EmailOrMobile = email;
            } else if (txtMobile != "") {

                /*var req = { Email: email, Password: password }*/
                var finalNumber = "";
                if (txtMobile.indexOf('0') == 0) {
                    finalNumber = txtMobile.slice(1);
                } else {
                    finalNumber = txtMobile;
                }
                // mobileVerify = finalNumber;

                EmailOrMobile = finalNumber;

            }
            $('#loader1').show();
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("Email", EmailOrMobile);
            formData.append("Password", password);
            formData.append("DeviceType", 3);
            formData.append("DeviceId", "web");
            formData.append("DeviceToken", "web");

            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/Login",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    mobileVerify = response.MobileNumber;
                    userTypeForSignup = response.RoleId;
                    $('#btnLogin').prop('disabled', false);
                    $('#loader').hide();
                    if (response.IsBulkUpload == false) {
                        if (response.RstKey == 1) {
                            $('#loader1').hide();
                            $("#loginModal").modal('hide');
                            var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
                            //alert(encrypted.toString());
                            //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");
                            sessionStorage.setItem("User", encrypted.toString());
                            sessionStorage.setItem("User1", response.UserGuid);
                            if (response.ProfileImage == "" || response.ProfileImage == null) {
                                sessionStorage.setItem("ProfileImage", "/Content/images/avatar.jpg");
                            } else {
                                sessionStorage.setItem("ProfileImage", response.ProfileImage);
                            }
                            if (response.IsPhoneVerified == true) {
                                if (response.IsProfileCompleted == true) {
                                    $('#loader1').hide();
                                    sessionStorage.setItem("UserName", response.FirstName + " " + response.LastName);
                                    // $("#spnUserName").text(response.FirstName + " " + response.LastName);
                                    $("#loginModal").modal('hide');
                                    if (response.RoleId == 3) {
                                        var url = window.location.origin;
                                        window.location.href = url + "/EmployerDashbord/Index";
                                    }
                                    //if (response.RoleId == 5) {
                                    //    var url = window.location.origin;
                                    //    window.location.href = url + "/VendorDashboard/Index";
                                    //}
                                    else {
                                        var url1 = window.location.origin;
                                        window.location.href = url1 + "/Home/Index";
                                    }
                                    //swal(
                                    //    'Success!',
                                    //    'Logged in.',
                                    //    'success'
                                    //).catch(swal.noop);
                                    swal({
                                        title: "Success",
                                        text: "Logged in",
                                        icon: "Success",
                                        buttons: true,
                                        dangerMode: true,
                                    }, function succes(isDone) {

                                    })
                                }
                                else if (response.RoleId == 4 && response.IsProfileCompleted == false) {
                                    $("#addOtherDetailsModal").modal('show');
                                    //var baseUrl = window.location.origin;
                                    //window.location.href = baseUrl + "/Account/CompleteProfile";
                                }
                                else if (response.RoleId == 3 && response.IsProfileCompleted == false) {
                                    window.location.href = "Account/CompleteEmployerProfile";
                                }
                            } else {
                                $('#loader1').hide();
                                swal({
                                    title: "Success",
                                    text: "OTP has been sent to your mobile number",
                                    icon: "Success",
                                    buttons: true,
                                    dangerMode: true,
                                }, function succes(isDone) {
                                    $("#verificationmodal").modal('show');
                                    timerOn = true;
                                    $('#btnResend').css("color", "#D3D3D3");
                                    $('#btnResend').prop('disabled', true);
                                    timer(59);
                                })
                                //write code here for OTP verifivation
                            }

                        }
                        else if (response.RstKey == 2) {
                            $('#loader1').hide();
                            //$("#loginModal").modal('hide');
                            swal(
                                'Error!',
                                'Email or Password is not correct,Please Try Again.',
                                'error'
                            ).catch(swal.noop);
                            window.location.href = "Account/Index";
                        }
                        else if (response.RstKey == 3) {
                            $('#loader1').hide();
                            $("#loginModal").modal('hide');
                            swal(
                                'Error!',
                                'You can not login,you are blocked by admin.',
                                'error'
                            ).catch(swal.noop);
                            window.location.href = "Account/Index";
                        }
                        else if (response.RstKey == 4) {
                            $('#loader1').hide();
                            $("#loginModal").modal('hide');
                            swal(
                                'Error!',
                                'You can not login,your account is deleted by admin.',
                                'error'
                            ).catch(swal.noop);
                            window.location.href = "Account/Index";
                        }
                        else if (response.RstKey == 5) {
                            $('#loader1').hide();
                            $("#loginModal").modal('hide');
                            swal(
                                'Error!',
                                'User not registered with this credentials',
                                'error'
                            ).catch(swal.noop);
                            // window.location.href = "Account/Index";
                        }
                        else {
                            $('#loader1').hide();
                            swal(
                                'Error!',
                                'Please Try Again.',
                                'error'
                            ).catch(swal.noop);
                            window.location.href = "Account/Index";
                        }
                    } else {
                        $('#loader1').hide();
                        $("#loginModal").modal('hide');
                        var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
                        //alert(encrypted.toString());
                        //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");
                        sessionStorage.setItem("User", encrypted.toString());
                        sessionStorage.setItem("User1", response.UserGuid);
                        if (response.ProfileImage == "" || response.ProfileImage == null) {
                            sessionStorage.setItem("ProfileImage", "/Content/images/avatar.jpg");
                        } else {
                            sessionStorage.setItem("ProfileImage", response.ProfileImage);
                        }
                        sessionStorage.setItem("UserName", response.FirstName + " " + response.LastName);

                        $('#Changecreatepasswordmodal').modal('show');
                        //var baseUrl = window.location.origin;
                        //window.location.href = baseUrl +'/Account/ChangePassword'
                    }
                }
            });

        } else {
            return false;
        }
    });
    $('#btnTesting').click(function () {
        $('#Changecreatepasswordmodal').modal('show');
    })
    $('#btnChangeSavePassword').click(function () {
        var txtOldPassword = $('#txtOldChangePassword').val();
        var txtNewPassword = $('#txtNewChangePassword').val();
        var txtconPassword = $('#txtChangeConfirmPassword').val();
        let isConPassword = true;
        var userid = sessionStorage.getItem("User1");
        //  var txtconfirmPassword = $('#txtNewChangePassword').val();
        if (txtOldPassword == '' || txtOldPassword == "" || txtOldPassword == null) {

            $('#spntxtIsbulkPasswordError').show();
            $('#spntxtIsbulkPasswordError').text("Please enter old password");
            ChnageOldpasswordError = false;
        }
        else {
            ChnageOldpasswordError = true;
            $('#spntxtIsbulkPasswordError').hide();
        }
        if (txtNewPassword == '' || txtNewPassword == "" || txtNewPassword == null) {
            $('#spntxtIsbullContPasswordError').show();
            $('#spntxtIsbullContPasswordError').text("Please enter new password.");
            ChnageNewpasswordError = false;
        } else {
            if (txtNewPassword.length < 8) {
                $('#spntxtIsbullContPasswordError').show();
                $('#spntxtIsbullContPasswordError').text("Password should be 8 characters");
                ChnageNewpasswordError = false;
            } else {
                $('#spntxtIsbullContPasswordError').hide();
                ChnageNewpasswordError = true;
            }
        }
        if (txtNewPassword != "" && txtNewPassword != txtconPassword) {
            $('#spntxtIsbullConfirmPasswordError').show();
            $('#spntxtIsbullConfirmPasswordError').text("Password does not match!");
            isConPassword = false;
        } else {
            if (txtNewPassword.length > 0 && txtNewPassword.length < 8) {
                $('#spntxtIsbullConfirmPasswordError').show();
                $('#spntxtIsbullConfirmPasswordError').text("Password should be 8 characters");
                isConPassword = false;
            }
            else if (txtconPassword == '' || txtconPassword == "" || txtconPassword == null) {
                $('#spntxtIsbullConfirmPasswordError').show();
                $('#spntxtIsbullConfirmPasswordError').text("Please enter confirm password");
                isConPassword = false;
            }
            else {
                $('#spntxtIsbullConfirmPasswordError').hide();
                isConPassword = true;
            }
        }

        if (ChnageOldpasswordError == true && ChnageNewpasswordError == true && ChangepasswordStrengh == true && isConPassword == true) {
            $('#loader1').show();
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("OldPassword", txtOldPassword);
            formData.append("Password", txtNewPassword);
            formData.append("UserGuid", userid);


            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/ChangePassword/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    $('#loader1').hide();
                    if (response == 1) {

                        swal({
                            title: "Success",
                            text: "Your password has been changed successfully.",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $('#Changecreatepasswordmodal').modal('hide');
                            $('#loginModal').modal('show');
                            //var url1 = window.location.origin;
                            //window.location.href = url1 + "/Account/Index";
                        })
                        //swal(
                        //    'Success!',
                        //    'Your password changed successfully.',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else if (response == 2) {

                        swal(
                            'Error!',
                            'Old Password is not correct.',
                            'error'
                        ).catch(swal.noop);
                        window.location.href = "Account/Index";
                    }
                    else if (response == 3) {

                        swal(
                            'Error!',
                            'Old password and new password can not be same.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    //if (response == "success") {
                    //    ClearControls();
                    //    swal(
                    //        'Success!',
                    //        'Ticket Created.',
                    //        'success'
                    //    ).catch(swal.noop);
                    //}
                    //else if (response == "fail") {
                    //    swal(
                    //        'Error!',
                    //        'Please Try Again.',
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                    //else {
                    //    swal(
                    //        'Error!',
                    //        response,
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                }
            });

        } else {
            return false;
        }
    });

    // Submit button
    $('#btnRegister').click(function () {
      
        $('#btnRegister').prop('disabled', true);
        var email = $('#txtEmailRegister').val();
        var password = $('#txtPasswordRegister').val();
        var mobile = $('#txtMobileRegister').val();
        var confirmPassword = $('#txtConfirmPassword').val();
        var txtIsCardChecked = IsCardChecked;

        if (email == '' || email == "" || email == null) {
            // $('#usercheck').show();
            usernameError = false;
            $('#txtEmailRegisterError').show();
            $('#btnRegister').prop('disabled', false);
            // return false;
        }
        else {
            // $('#usercheck').hide();
            usernameError = true;
            $('#txtEmailRegisterError').hide();
            // return true;

        }
        if (password == '' || password == "" || password == null) {
            //$('#passcheck').show();
            $('#btnRegister').prop('disabled', false);
            $('#txtPasswordRegisterError').show();
            $('#txtPasswordRegisterError').text("Please enter your password.");
            passwordError = false;
            // return false;
        } else {
            // $('#passcheck').hide();

            if (password.length < 8) {
                $('#btnRegister').prop('disabled', false);
                $('#txtPasswordRegisterError').show();
                $('#txtPasswordRegisterError').text("Password should be 8 characters");
                passwordError = false;
            } else {
                $('#txtPasswordRegisterError').hide();
                passwordError = true;
            }

            // return true;
        }
        if (mobile == '' || mobile == "" || mobile == null) {
            //$('#passcheck').show();
            $('#btnRegister').prop('disabled', false);
            $('#txtMobileRegisterError').show();
            mobileNumberError = false;
            // return false;
        } else {
            var dd = mobile.indexOf('0');
            if (mobile.indexOf('0') == 0 && mobile.length < 10) {
                mobileNumberError = false;
                $('#btnRegister').prop('disabled', false);
                $("#txtMobilelengthError").text("Mobile number should be 11 digit with zero.");
            }
            else if (mobile.indexOf('0') == 0 && mobile.length == 11) {
                mobileNumberError = true;
                $('#txtMobileRegisterError').hide();
                $("#txtMobilelengthError").hide();
            }
            else if (mobile.indexOf('0') != 0 && mobile.length == 10) {
                mobileNumberError = true;
                $('#txtMobileRegisterError').hide();
                $("#txtMobilelengthError").hide();
            }
            else {
                mobileNumberError = false;
                $('#btnRegister').prop('disabled', false);
                $("#txtMobilelengthError").show()
                $("#txtMobilelengthError").text("Mobile number should be 10 digit without zero.");
            }
            // $('#passcheck').hide();

            // return true;
        }
        if (confirmPassword == '' || confirmPassword == "" || confirmPassword == null) {
            //$('#passcheck').show();
            $('#btnRegister').prop('disabled', false);
            $('#txtConfirmPasswordError').show();
            $('#txtConfirmPasswordError').text("Please enter confirm password.");
            ConpasswordError1 = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError1 = true;
            $('#txtConfirmPasswordError').hide();
            // return true;
        }
        if (confirmPassword != "" && password != confirmPassword) {
            //$('#passcheck').show();
            $('#btnRegister').prop('disabled', false);
            $('#txtConfirmPasswordError1').show();
            $('#txtConfirmPasswordError').hide();
            $('#txtConfirmPasswordError1').text("Password does not match!");
            ConpasswordError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError = true;
            $('#txtConfirmPasswordError1').hide();
            // return true;
        }
        if (IsTermConditionChecked == false) {
            $('#btnRegister').prop('disabled', false);
            $('#chkTermConditionError').text("Please accept terms and conditions");
            $('#chkTermConditionError').show();
        } else {
            $('#chkTermConditionError').hide();
        }
        if (IsppChecked == false) {
            $('#btnRegister').prop('disabled', false);
            $('#chkppnError').text("Please accept privacy policy");
            $('#chkppnError').show();
        } else {
            $('#chkppnError').hide();
        }
        if (passwordStrengh == false) {
            $('#txtPasswordRegisterError').show();
            $('#txtPasswordRegisterError').text("Password must contain at least one uppercase,one lowercase,one special character and one number.")
        }


        validateEmailForSignup();
        if ((usernameError == true) && (passwordError == true && passwordStrengh == true) && (mobileNumberError == true) && (ConpasswordError == true) && (emailFormateError == true) && IsTermConditionChecked == true && ConpasswordError1 == true && IsppChecked == true) {
            /*var req = { Email: email, Password: password }*/
            $('#loader1').show();
            var finalNumber = "";
            if (mobile.indexOf('0') == 0) {
                finalNumber = mobile.slice(1);
            } else {
                finalNumber = mobile;
            }
            mobileVerify = finalNumber;
            var formData = new FormData();
            formData.append("Email", email);
            formData.append("Password", password);
            formData.append("PhoneNumber", finalNumber);
            formData.append("CountryCode", "+234");
            formData.append("OtpCode", "1001");
            formData.append("DeviceId", ipAddress);
            formData.append("DeviceToken", ipAddress);
            formData.append("DeviceType", 3);
            formData.append("RoleId", 3);
            formData.append("IsCardChecked", txtIsCardChecked);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/SignUp",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('#btnRegister').prop('disabled', false);
                    console.log(response);
                    $('#loader1').hide();
                    sessionStorage.setItem("ProfileImage", "/Content/images/avatar.jpg");
                    if (response.RstKey == 11 && response.RoleId == 4) {
                        var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
                        //alert(encrypted.toString());
                        //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");
                        sessionStorage.setItem("User", encrypted.toString());
                        sessionStorage.setItem("User1", response.UserGuid);
                        $(".modal-body input").val("");

                        $("#registerModal").modal('hide');
                        $("#chkTermCondition").prop('checked', false);
                        swal({
                            title: "Success",
                            text: "OTP has been sent to your mobile number",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#verificationmodal").modal('show');
                            timerOn = true;
                            $('#btnResend').css("color", "#D3D3D3");
                            $('#btnResend').prop('disabled', true);
                            timer(59);
                        })
                    }
                    if (response.RstKey == 11 && response.RoleId == 3) {
                        var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
                        userTypeForSignup = 3;
                        //alert(encrypted.toString());
                        //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");
                        $(".modal-body input").val("");
                        $("#chkTermCondition").prop('checked', false);
                        sessionStorage.setItem("User", encrypted.toString());
                        sessionStorage.setItem("User1", response.UserGuid);
                        $("#registerModal").modal('hide');
                        swal({
                            title: "Success",
                            text: "OTP has been sent to your mobile number",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#verificationmodal").modal('show');
                            timerOn = true;
                            $('#btnResend').css("color", "#D3D3D3");
                            $('#btnResend').prop('disabled', true);
                            timer(59);
                        })
                    }
                    else if (response.RstKey == 7) {
                        $("#registerModal").modal('hide');
                        swal(
                            'Error!',
                            'Email already exists.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    } else if (response.RstKey == 8) {
                        $("#registerModal").modal('hide');
                        swal(
                            'Error!',
                            'Mobile number already exists.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }

                    //if (response == "success") {
                    //    ClearControls();
                    //    swal(
                    //        'Success!',
                    //        'Ticket Created.',
                    //        'success'
                    //    ).catch(swal.noop);
                    //}
                    //else if (response == "fail") {
                    //    swal(
                    //        'Error!',
                    //        'Please Try Again.',
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                    //else {
                    //    swal(
                    //        'Error!',
                    //        response,
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                }
            });
            //$.ajax({
            //    type: "POST",
            //    url: "AccountController/Login",
            //    data: req,
            //    success: function (data) {
            //        console.log(data)
            //    }
            //});
            //return true;
        } else {
            $('#btnRegister').prop('disabled', false);
            return false;
        }
    });

    // Submit button
    $('#btnOtpVerify').click(function () {
      
        $('#btnOtpVerify').prop('disabled', true);
        var FirstDigit = $('#txtFirstDigit').val();
        var SecondDigit = $('#txtSecondDigit').val();
        var ThirdDigit = $('#txtThirdDigit').val();
        var FourthDigit = $('#txtFourthDigit').val();


        if (FirstDigit == '' || FirstDigit == "" || FirstDigit == null) {
            $('#btnOtpVerify').prop('disabled', false);
            swal(
                'Error!',
                'First digit can not empty',
                'error'
            ).catch(swal.noop);
            // alert("FirstDigit can not empty")
            return false;
        }
        else {
            // $('#usercheck').hide();
            usernameError = true;
            // return true;

        }
        if (SecondDigit == '' || SecondDigit == "" || SecondDigit == null) {
            //$('#passcheck').show();
            //alert("SecondDigit can not empty")
            $('#btnOtpVerify').prop('disabled', false);
            swal(
                'Error!',
                'Second digit can not empty',
                'error'
            ).catch(swal.noop);
            return false;
        }
        if (ThirdDigit == '' || ThirdDigit == "" || ThirdDigit == null) {
            //$('#passcheck').show();
            $('#btnOtpVerify').prop('disabled', false);
            swal(
                'Error!',
                'Third digit can not empty',
                'error'
            ).catch(swal.noop);
            //alert("ThirdDigit can not empty")
            return false;
        }
        if (FourthDigit == '' || FourthDigit == "" || FourthDigit == null) {
            //$('#passcheck').show();
            $('#btnOtpVerify').prop('disabled', false);
            swal(
                'Error!',
                'Fourth digit can not empty',
                'error'
            ).catch(swal.noop);
            //alert("FourthDigit can not empty")
            return false;
        }
        var otp = FirstDigit.concat(SecondDigit, ThirdDigit, FourthDigit);
        if (otp.length >= 4) {
            $('#loader1').show();
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("MobileNumber", mobileVerify);
            formData.append("OtpCode", otp);
            formData.append("DeviceType", 3);
            formData.append("DeviceId", ipAddress);
            formData.append("DeviceToken", ipAddress);

            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/VerifyOTP",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('#loader1').hide();

                    $('#btnOtpVerify').prop('disabled', false);
                    if (response.RstKey == 6) {
                        $("#verificationmodal").modal('hide');
                        $(".modal-body input").val("");
                        swal({
                            title: "Success",
                            text: "OTP verified",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            if (userTypeForSignup == 4) {
                                window.location.href = "Account/CompleteProfile";
                            }
                            else {
                                window.location.href = "Account/CompleteEmployerProfile";
                            }
                        })
                        //swal(
                        //    'Success!',
                        //    'Verifucation OTP sent to your email or mobile',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else {
                        $('#btnOtpVerify').prop('disabled', false);
                        swal(
                            'Error!',
                            'Invalid OTP',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    //if (response == "success") {
                    //    ClearControls();
                    //    swal(
                    //        'Success!',
                    //        'Ticket Created.',
                    //        'success'
                    //    ).catch(swal.noop);
                    //}
                    //else if (response == "fail") {
                    //    swal(
                    //        'Error!',
                    //        'Please Try Again.',
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                    //else {
                    //    swal(
                    //        'Error!',
                    //        response,
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                }
            });
            //$.ajax({
            //    type: "POST",
            //    url: "AccountController/Login",
            //    data: req,
            //    success: function (data) {
            //        console.log(data)
            //    }
            //});
            //return true;
        } else {
            return false;
        }
    });

    $('#btnLogout').click(function () {

      
        var UserGuid = sessionStorage.getItem("User1");
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
        $('#loader1').show();
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
                $('#loader1').hide();
                if (response == 1) {
                    console.log(response);
                    sessionStorage.clear();
                    var base_url = window.location.origin;
                    window.location.href = base_url + "/Account";
                    // window.location.reload();
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

    $('#btnForgetPassword').click(function () {
        $('#forgetPasswordModalloader1').show();
        var txtForgetEmail = $('#txtForgetEmail').val();
        var txtForgetMobile = $('#txtForgetMobile').val();

        if (txtForgetEmail != "" && txtForgetMobile != "") {
            alert("Please enter email or mobile");
            return false;
        }

        var EmailOrPhone = "";
        var type = 0;
        if ((txtForgetEmail == '' || txtForgetEmail == "" || txtForgetEmail == null) && (txtForgetMobile == '' || txtForgetMobile == "" || txtForgetMobile == null)) {

            $('#txtForgetMobileError').show();
            $('#txtForgetEmailError').show();
            return false;
        }
        else {
            // $('#usercheck').hide();
            usernameError = true;
            $('#txtForgetMobileError').hide();
            $('#txtForgetEmailError').hide();
            // return true;

        }
        if (txtForgetEmail != "") {
            mobileVerify = txtForgetEmail;
            EmailOrPhone = txtForgetEmail;
            forgetOtpEmailOrMobile = txtForgetEmail;
            type = 1;
            validateEmailForgetPassword(txtForgetEmail);
        } else if (txtForgetMobile != "") {

            /*var req = { Email: email, Password: password }*/
            var finalNumber = "";
            if (txtForgetMobile.indexOf('0') == 0) {
                finalNumber = txtForgetMobile.slice(1);
            } else {
                finalNumber = txtForgetMobile;
            }
            mobileVerify = finalNumber;


            //mobileVerify = txtForgetMobile;
            EmailOrPhone = finalNumber;
            forgetOtpEmailOrMobile = txtForgetMobile;
            type = 2;
            validateMobile(txtForgetMobile);
            //mobileFormateError
        }


        if (usernameError == true && emailFormateError == true && mobileFormateError == true) {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("EmailorPhone", EmailOrPhone);
            formData.append("Type", type);


            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/ForgotPassword",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {

                    console.log(response);
                    if (response != null && response != "" && response != '') {
                        $('#forgetPasswordModalloader1').hide();
                        $('#forgetPasswordModal').modal('hide');

                        swal({
                            title: "Success",
                            text: "Verification OTP sent to your email or mobile",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            timerOn = true;
                            $('#btnResentForgetPassOtp').prop('disabled', true);
                            $('#btnResentForgetPassOtp').css("color", "#D3D3D3");
                            timer1(59);
                            $("#forgetPasswordOtpverificationmodal").modal('show');
                            $("#forgetPasswordModal").modal('hide');
                        })

                    }
                    else {

                        swal(
                            'Error!',
                            'User not registered with this credentials',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    //if (response == "success") {
                    //    ClearControls();
                    //    swal(
                    //        'Success!',
                    //        'Ticket Created.',
                    //        'success'
                    //    ).catch(swal.noop);
                    //}
                    //else if (response == "fail") {
                    //    swal(
                    //        'Error!',
                    //        'Please Try Again.',
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                    //else {
                    //    swal(
                    //        'Error!',
                    //        response,
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                }
            });
            //$.ajax({
            //    type: "POST",
            //    url: "AccountController/Login",
            //    data: req,
            //    success: function (data) {
            //        console.log(data)
            //    }
            //});
            //return true;
        } else {
            return false;
        }
    });

    $('#btnResetForgetPassValues').click(function () {

        $('#txtForgetMobile').val('');
        $('#txtForgetEmail').val('');
        window.location.reload();
    })

    $('#btnCreatPasswordModal').click(function () {

        $('#txtForgetPassword').val('');
        $('#txtForgetConfirmPassword').val('');
        window.location.reload();
    })

    function validateMobile(mobNum) {

        var dd = mobNum.indexOf('0');
        if (mobNum.indexOf('0') == 0 && mobNum.length < 10) {
            mobileFormateError = false;
            swal({
                title: "Please enter 11 digit number with zero",
                text: "Try again",
                type: "warning",
                //showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                //confirmButtonText: "Delete",
                //closeOnConfirm: false
            }, function succes(isDone) {
                mobileFormateError = false;
                //$('#btnRegister').prop('disabled', false);
            })
        }
        else if (mobNum.indexOf('0') == 0 && mobNum.length == 11) {
            mobileFormateError = true;

        }
        else if (mobNum.indexOf('0') != 0 && mobNum.length == 10) {
            mobileFormateError = true;

        }
        else {
            mobileFormateError = false;
            swal({
                title: "Please enter 10 digit number without zero",
                text: "Try again",
                type: "warning",
                //showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                //confirmButtonText: "Delete",
                //closeOnConfirm: false
            }, function succes(isDone) {
                mobileFormateError = false;
                //$('#btnRegister').prop('disabled', false);
            })
        }


        //if (mobNum.length >= 10 && mobNum.length <= 11) {
        //    mobileFormateError = true
        //}
        //else {
        //    mobileFormateError = false;

        //    swal({
        //        title: "Please enter 11 digit number",
        //        text: "Try again",
        //        type: "warning",
        //        //showCancelButton: true,
        //        confirmButtonColor: "#DD6B55",
        //        //confirmButtonText: "Delete",
        //        //closeOnConfirm: false
        //    }, function succes(isDone) {
        //        mobileFormateError = false;
        //        //$('#btnRegister').prop('disabled', false);
        //    })
        //}
    }

    // Validate Confirm email
    function validateEmailForgetPassword(email) {

        if (email.length != '') {

            var pattern = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;// /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i

            if (!pattern.test(email)) {
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
                    //$('#btnRegister').prop('disabled', false);
                })
            }
            else {
                emailFormateError = true;
            }
        }


    }

    $('#btnForgetPasswordOtpVerify').click(function () {
        $('#loader1').show();
        var FirstDigit = $('#txtForgetPasswordFirstDigit').val();
        var SecondDigit = $('#txtForgetPasswordSecondDigit').val();
        var ThirdDigit = $('#txtForgetPasswordThirdDigit').val();
        var FourthDigit = $('#txtForgetPasswordFourthDigit').val();


        if (FirstDigit == '' || FirstDigit == "" || FirstDigit == null) {

            alert("FirstDigit can not empty")
            return false;
        }
        else {
            // $('#usercheck').hide();
            usernameError = true;
            // return true;

        }
        if (SecondDigit == '' || SecondDigit == "" || SecondDigit == null) {
            //$('#passcheck').show();
            alert("SecondDigit can not empty")
            return false;
        }
        if (ThirdDigit == '' || ThirdDigit == "" || ThirdDigit == null) {
            //$('#passcheck').show();
            alert("ThirdDigit can not empty")
            return false;
        }
        if (FourthDigit == '' || FourthDigit == "" || FourthDigit == null) {
            //$('#passcheck').show();
            alert("FourthDigit can not empty")
            return false;
        }
        var otp = FirstDigit.concat(SecondDigit, ThirdDigit, FourthDigit);
        OtpPass = otp;
        if (otp.length >= 4) {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("EmailorPhone", mobileVerify);
            formData.append("OtpCode", otp);
            formData.append("Type", 1);


            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/VerifyForgetPasswordOTP",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 6) {
                        $('#loader1').hide();
                        $('#forgetPasswordOtpverificationmodal').modal('hide');
                        swal({
                            title: "Success",
                            text: "OTP verified",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#forgetPasswordOtpverificationmodal").modal('hide');
                            $("#createpasswordmodal").modal('show');

                            //empty after verified
                            var FirstDigit = $('#txtForgetPasswordFirstDigit').val('');
                            var SecondDigit = $('#txtForgetPasswordSecondDigit').val('');
                            var ThirdDigit = $('#txtForgetPasswordThirdDigit').val('');
                            var FourthDigit = $('#txtForgetPasswordFourthDigit').val('');
                            //need to open reset password modal and then hit reset password API

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
                            'Invalid OTP',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    //if (response == "success") {
                    //    ClearControls();
                    //    swal(
                    //        'Success!',
                    //        'Ticket Created.',
                    //        'success'
                    //    ).catch(swal.noop);
                    //}
                    //else if (response == "fail") {
                    //    swal(
                    //        'Error!',
                    //        'Please Try Again.',
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                    //else {
                    //    swal(
                    //        'Error!',
                    //        response,
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                }
            });
            //$.ajax({
            //    type: "POST",
            //    url: "AccountController/Login",
            //    data: req,
            //    success: function (data) {
            //        console.log(data)
            //    }
            //});
            //return true;
        } else {
            return false;
        }
    });

    $('#btnSavePassword').click(function () {
        $('#loader1').show();
        var txtForgetPassword = $('#txtForgetPassword').val();
        var txtForgetConfirmPassword = $('#txtForgetConfirmPassword').val();
        let txtForgetPasswordpasswordError = true

        if (txtForgetPassword == '' || txtForgetPassword == "" || txtForgetPassword == null) {

            $('#spntxtForgetPasswordError').show();
            $('#spntxtForgetPasswordError').text("Please enter your password.");
            txtForgetPasswordpasswordError = false;
            // return false;
        } else {
            if (txtForgetPassword.length < 8) {

                $('#spntxtForgetPasswordError').show();
                $('#spntxtForgetPasswordError').text("Password should be 8 characters");
                txtForgetPasswordpasswordError = false;
            } else {
                $('#spntxtForgetPasswordError').hide();
                txtForgetPasswordpasswordError = true;
            }

            // return true;
        }
        if (txtForgetConfirmPassword == '' || txtForgetConfirmPassword == "" || txtForgetConfirmPassword == null) {
            //$('#passcheck').show();
            // alert("Password miss match!")
            $('#spntxtForgeContPasswordError').show();
            $('#spntxtForgeContPasswordError').text("Please enter your confirm password.");

            ConpasswordError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError = true;
            // return true;
        }
        if (txtForgetPassword != "" && txtForgetPassword != txtForgetConfirmPassword && passwordStrengh == true) {
            //$('#passcheck').show();
            // alert("Password miss match!")
            $('#spntxtForgeContPasswordError').show();
            $('#spntxtForgeContPasswordError').text("Password does not match!");

            ConpasswordError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError = true;
            // return true;
        }

        if (ConpasswordError == true && txtForgetPasswordpasswordError == true) {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("EmailorPhone", mobileVerify);
            formData.append("OtpCode", OtpPass);
            formData.append("Password", txtForgetPassword);


            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/ResetPassword",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {
                        $('#loader1').hide();
                        swal({
                            title: "Success",
                            text: "Your password has been updated successfully",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#createpasswordmodal").modal('hide');
                            $("#loginModal").modal('show');

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
                            'Old password and new password can not be same.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    //if (response == "success") {
                    //    ClearControls();
                    //    swal(
                    //        'Success!',
                    //        'Ticket Created.',
                    //        'success'
                    //    ).catch(swal.noop);
                    //}
                    //else if (response == "fail") {
                    //    swal(
                    //        'Error!',
                    //        'Please Try Again.',
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                    //else {
                    //    swal(
                    //        'Error!',
                    //        response,
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                }
            });
            //$.ajax({
            //    type: "POST",
            //    url: "AccountController/Login",
            //    data: req,
            //    success: function (data) {
            //        console.log(data)
            //    }
            //});
            //return true;
        } else {
            return false;
        }
    });

    $('#testId').click(function () {

        $("#createpasswordmodal").modal('show');
        //swal({
        //    title: "Success",
        //    text: "Verifucation OTP sent to your email or mobile",
        //    icon: "Success",
        //    buttons: true,
        //    dangerMode: true,
        //}, function succes(isDone) {
        //    $("#verificationmodal").modal('show');
        //})

    });

    //password hide and show
    $("body").on('click', '#txtPasswordRegister1', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtPasswordRegister");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });



    $("#txtPasswordtoggle").click(function () {

        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#txtPassword");
        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    });

    //$(".toggle-password1").click(function () {

    //    $(this).toggleClass("fa-eye fa-eye-slash");
    //    var input = $($(this).attr("toggle"));
    //    if (input.attr("type") == "password") {
    //        input.attr("type", "text");
    //    } else {
    //        input.attr("type", "password");
    //    }
    //});
    $("body").on('click', '#txtConfirmPassword1', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtConfirmPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });

    //$("body").on('click', '.toggle-password', function () {
    //    $(this).toggleClass("fa-eye fa-eye-slash");
    //    var input = $("#txtPassword");
    //    if (input.attr("type") === "password") {
    //        input.attr("type", "text");
    //    } else {
    //        input.attr("type", "password");
    //    }

    //});


    $('#btnlandingPage').click(function () {

        var phonumber = $('#txtinviteNumber').val();
        // txtForgetEmail

        if (phonumber != "") {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("IsdCode", "+234");
            formData.append("PhoneNumber", phonumber);


            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/Invite",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {
                        $('#loader1').hide();
                        var setEmptyField = $('#txtinviteNumber').val('');
                        swal({
                            title: "Success",
                            text: "Invitation Sent.",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {

                        })
                        //swal(
                        //    'Success!',
                        //    'Verification OTP sent to your email or mobile',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else {

                        swal(
                            'Error!',
                            'Invitation not sent',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }

                }
            });

        } else {
            return false;
        }
    });




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
    function timer1(remaining) {

        var m = Math.floor(remaining / 60);
        var s = remaining % 60;

        m = m < 10 ? '0' + m : m;
        s = s < 10 ? '0' + s : s;
        document.getElementById('timer1').innerHTML = m + ':' + s;
        remaining -= 1;
        if (remaining == 0) {
            // IsOtpResent = true;
            $('#btnResentForgetPassOtp').css("color", "#32A4FF");
            $('#btnResentForgetPassOtp').prop('disabled', false);
        }

        if (remaining >= 0 && timerOn) {
            setTimeout(function () {
                timer1(remaining);
            }, 1000);
            return;
        }

        if (!timerOn) {

            return;
        }

        // Do timeout stuff here

    }
    $("#testId").click(function () {
        //$('#btnResentForgetPassOtp').prop('disabled', true);
        //timerOn = true;

        //timer1(10)
        $("#createpasswordmodal").modal('show');
    });
    $("#btnResend").click(function () {
        $('#btnResend').prop('disabled', true);

        var FirstDigit = $('#txtFirstDigit').val('');
        var SecondDigit = $('#txtSecondDigit').val('');
        var ThirdDigit = $('#txtThirdDigit').val('');
        var FourthDigit = $('#txtFourthDigit').val('');

        var id = sessionStorage.getItem("User1");
        var formData = new FormData();
        formData.append("UserGuid", id);
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
                        $('#btnResend').prop('disabled', true);
                        $('#btnResend').css("color", "#D3D3D3");
                        timer(60);
                        $("#verificationmodal").show();
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
    });


    $("#btnResetvaluesForgetPasswordOTP").click(function () {
        var FirstDigit = $('#txtForgetPasswordFirstDigit').val('');
        var SecondDigit = $('#txtForgetPasswordSecondDigit').val('');
        var ThirdDigit = $('#txtForgetPasswordThirdDigit').val('');
        var FourthDigit = $('#txtForgetPasswordFourthDigit').val('');
        window.location.reload();
    });


    $('#btnResentForgetPassOtp').click(function () {
        $('#loader1').show();
        var txtForgetEmail = forgetOtpEmailOrMobile;

        //empty after verified
        var FirstDigit = $('#txtForgetPasswordFirstDigit').val('');
        var SecondDigit = $('#txtForgetPasswordSecondDigit').val('');
        var ThirdDigit = $('#txtForgetPasswordThirdDigit').val('');
        var FourthDigit = $('#txtForgetPasswordFourthDigit').val('');
        // txtForgetEmail

        if (txtForgetEmail != "") {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("EmailorPhone", txtForgetEmail);
            formData.append("Type", 2);


            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/ForgotPassword",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response != null) {
                        $('#loader1').hide();
                        swal({
                            title: "Success",
                            text: "Verification OTP sent to your email or mobile",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            timerOn = true;
                            $('#btnResentForgetPassOtp').prop('disabled', true);
                            $('#btnResentForgetPassOtp').css("color", "#D3D3D3");
                            timer1(60);
                            $("#forgetPasswordOtpverificationmodal").modal('show');
                            $("#forgetPasswordModal").modal('hide');
                        })
                        //swal(
                        //    'Success!',
                        //    'Verification OTP sent to your email or mobile',
                        //    'success'
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

        } else {
            return false;
        }
    });


    //password hide and show
    $("body").on('click', '#txtForgetResetPassword', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtForgetPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });
    $("body").on('click', '#txtForgetResetConfirmPassword', function () {

        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#txtForgetConfirmPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });

    //password hide and show
    $("body").on('click', '#txtchangeIsbulkPassword', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtOldChangePassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });
    //password hide and show
    $("body").on('click', '#txtNewIsBulkConfirmPassword', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtNewChangePassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });
    //password hide and show
    $("body").on('click', '#txtIsBulkChangeConfirmPassword', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtChangeConfirmPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });

    $("#btnHelp").click(function () {
        swal({
            title: "Success",
            text: "Coming soon",
            icon: "Success",
            buttons: true,
            dangerMode: true,
            cancelButtonColor: "#00FFFF",
            confirmButtonColor: "#00FFFF",
        }, function succes(isDone) {

        })

    });

    $("#chkTermCondition").change(function () {
        if ($(this).prop('checked')) {

            IsTermConditionChecked = true;
        } else {
            IsTermConditionChecked = false;
        }
    });
    $("#chkPrivacyPolicy").change(function () {
        if ($(this).prop('checked')) {

            IsppChecked = true;
        } else {
            IsppChecked = false;
        }
    });
    $(document).ready(function () {
        $(".menu-container").click(function () {
            $(".side-bar").animate({ display: "block", width: "toggle" });
            $(".menu-container").hide();
            $(".close-menu-icon").show();
        });

        $(".close-menu-icon-container").click(function () {
            $(".side-bar").animate({ display: "none", width: "toggle" });
            $(".menu-container").show();
            $(".close-menu-icon").hide();
        });

        $(".menu-item").click(function () {
            $(".side-bar").animate({ display: "none", width: "toggle" });
            $(".menu-container").show();
            $(".close-menu-icon").hide();
        })

        $("#item-home").click(function () {
            $(this).css("color", "#020202B3");
            $("#item-portfolio").css("color", " #02020299");
            $("#item-pages").css("color", " #02020299");
            $("#item-elements").css("color", " #02020299");
            $("#item-news").css("color", " #02020299");
            $("#item-blog").css("color", " #02020299");

            $("#sidebar-item-home").css("color", " #41B6E6");
            $("#sidebar-item-portfolio").css("color", " #02020299");
            $("#sidebar-item-pages").css("color", " #02020299");
            $("#sidebar-item-elements").css("color", " #02020299");
            $("#sidebar-item-news").css("color", " #02020299");
            $("#sidebar-item-blog").css("color", " #02020299");

            $("#active-home-item").show();
            $("#active-portfolio-item").hide();
            $("#active-pages-item").hide();
            $("#active-elements-item").hide();
            $("#active-news-item").hide();
            $("#active-blog-item").hide();
        })










        $("#sidebar-item-home").click(function () {
            $(this).css("color", "#020202B3");
            $("#item-portfolio").css("color", " #02020299");
            $("#item-pages").css("color", " #02020299");
            $("#item-elements").css("color", " #02020299");
            $("#item-news").css("color", " #02020299");

            $("#sidebar-item-home").css("color", " #41B6E6");
            $("#sidebar-item-portfolio").css("color", " #02020299");
            $("#sidebar-item-pages").css("color", " #02020299");
            $("#sidebar-item-elements").css("color", " #02020299");
            $("#sidebar-item-news").css("color", " #02020299");
            $("#sidebar-item-blog").css("color", " #02020299");

            $("#active-home-item").show();
            $("#active-portfolio-item").hide();
            $("#active-pages-item").hide();
            $("#active-elements-item").hide();
            $("#active-news-item").hide();
            $("#active-blog-item").hide();
        })

        $("#sidebar-item-portfolio").click(function () {
            $(this).css("color", "#020202B3");
            $("#item-home").css("color", " #02020299");
            $("#item-pages").css("color", " #02020299");
            $("#item-elements").css("color", " #02020299");
            $("#item-news").css("color", " #02020299");

            $("#sidebar-item-home").css("color", " #02020299");
            $("#sidebar-item-portfolio").css("color", "#41B6E6");
            $("#sidebar-item-pages").css("color", " #02020299");
            $("#sidebar-item-elements").css("color", " #02020299");
            $("#sidebar-item-news").css("color", " #02020299");
            $("#sidebar-item-blog").css("color", " #02020299");

            $("#active-home-item").hide();
            $("#active-portfolio-item").show();
            $("#active-pages-item").hide();
            $("#active-elements-item").hide();
            $("#active-news-item").hide();
            $("#active-blog-item").hide();
        })

        $("#sidebar-item-pages").click(function () {
            $(this).css("color", "#020202B3");
            $("#item-portfolio").css("color", " #02020299");
            $("#item-home").css("color", " #02020299");
            $("#item-elements").css("color", " #02020299");
            $("#item-news").css("color", " #02020299");

            $("#sidebar-item-home").css("color", " #02020299");
            $("#sidebar-item-portfolio").css("color", "#02020299");
            $("#sidebar-item-pages").css("color", " #41B6E6");
            $("#sidebar-item-elements").css("color", " #02020299");
            $("#sidebar-item-news").css("color", " #02020299");
            $("#sidebar-item-blog").css("color", " #02020299");

            $("#active-home-item").hide();
            $("#active-portfolio-item").hide();
            $("#active-pages-item").show();
            $("#active-elements-item").hide();
            $("#active-news-item").hide();
            $("#active-blog-item").hide();
        })

        $("#sidebar-item-elements").click(function () {
            $(this).css("color", "#020202B3");
            $("#item-portfolio").css("color", " #02020299");
            $("#item-pages").css("color", " #02020299");
            $("#item-home").css("color", " #02020299");
            $("#item-news").css("color", " #02020299");

            $("#sidebar-item-home").css("color", " #02020299");
            $("#sidebar-item-portfolio").css("color", "#02020299");
            $("#sidebar-item-pages").css("color", "#02020299");
            $("#sidebar-item-elements").css("color", " #41B6E6");
            $("#sidebar-item-news").css("color", " #02020299");
            $("#sidebar-item-blog").css("color", " #02020299");

            $("#active-home-item").hide();
            $("#active-portfolio-item").hide();
            $("#active-pages-item").hide();
            $("#active-elements-item").show();
            $("#active-news-item").hide();
            $("#active-blog-item").hide();
        })

        $("#sidebar-item-news").click(function () {
            $(this).css("color", "#020202B3");
            $("#item-portfolio").css("color", " #02020299");
            $("#item-pages").css("color", " #02020299");
            $("#item-elements").css("color", " #02020299");
            $("#item-home").css("color", " #02020299");

            $("#sidebar-item-home").css("color", " #02020299");
            $("#sidebar-item-portfolio").css("color", "#02020299");
            $("#sidebar-item-pages").css("color", "#02020299");
            $("#sidebar-item-elements").css("color", "#02020299");
            $("#sidebar-item-news").css("color", " #41B6E6");
            $("#sidebar-item-blog").css("color", " #02020299");

            $("#active-home-item").hide();
            $("#active-portfolio-item").hide();
            $("#active-pages-item").hide();
            $("#active-elements-item").hide();
            $("#active-news-item").show();
            $("#active-blog-item").hide();
        })

        $("#sidebar-item-blog").click(function () {
            //$(this).css("color", "#020202B3");
            //$("#item-portfolio").css("color", " #02020299");
            //$("#item-pages").css("color", " #02020299");
            //$("#item-elements").css("color", " #02020299");
            //$("#item-home").css("color", " #02020299");

            //$("#sidebar-item-home").css("color", " #02020299");
            //$("#sidebar-item-portfolio").css("color", "#02020299");
            //$("#sidebar-item-pages").css("color", "#02020299");
            //$("#sidebar-item-elements").css("color", "#02020299");
            //$("#sidebar-item-news").css("color", " #02020299");
            //$("#sidebar-item-blog").css("color", " #41B6E6");

            //$("#active-home-item").hide();
            //$("#active-portfolio-item").hide();
            //$("#active-pages-item").hide();
            //$("#active-elements-item").hide();
            //$("#active-news-item").hide();
            //$("#active-blog-item").show();
        })
    });

    $("#chkApplyCard").change(function () {
        if ($(this).prop('checked')) {

            IsCardChecked = true;
        } else {
            IsCardChecked = false;
        }
    });
});





