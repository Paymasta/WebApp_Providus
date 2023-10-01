
let IsTermConditionCheckedNuban = false;
let IsppCheckedNuban = false;
let IsCardCheckedNuban = false;
let fNameErrorNuban = true;
let lnameErrorNuban = true;
let emailFormateErrorNuban = true;
let mobileFormateErrorNuban = true;
let emailErrorNuban = true;
let passwordErrorNuban = true;
let mobileNumberErrorNuban = true;
let accountNumberErrorNuban = true;
let ConpasswordError1Nuban = true
let ConpasswordError2Nuban = true
var mobileVerifyNuban = "";
let logintypeNuban = 0;
var OtpPassNuban = "";
var passwordStrenghNuban = false;
let timerOnNuban = false;
let bankNameNubanError = false;
var BankNameNuban = "";
var BankCodeNuban = "";
var SystemSpecsBankcode = "";
var finalNumber = "";
let userTypeForSignup = 4;
//var id = "";
$(document).ready(function () {
    getQoreIdBankList();
    $("#testssssssssss").click(function () {
        //timer2(59);
        $("#addOtherDetailsModal").modal('show');
    })
    $('#divMobile').hide();
    $('#passcheck').hide();
    $('#usercheck').hide();
    $('#mobilechk').hide();
    $('#passError').hide();
    $('#strengthMessage').hide();
    /*signup errors*/
    $('#txtFnameNubanError').hide();
    $('#txtMobileRegisternubanError').hide();
    $('#ddlbankListQoreIdError').hide();
    $('#txtPasswordRegisternubanError').hide();
    $('#cb1Error').hide();
    $('#cb2Error').hide();
    $('#txtLnmeNubanError').hide();
    $('#txtEmailNubanRegisterError').hide();
    $('#txtAccountNumberNubanError').hide();
    $('#txtConfirmPasswordnubanError').hide();

    $('#tandcError').hide();
    $('#ppnubanError').hide();
    $('#closeNubanModal').click(function () {
        $('#registerModalByNuban').hide();
        window.location.reload();
    })

    //$("#closeaddOtherDetailsModal").click(function () {
    //    $('#registerModalByNuban').hide();
    //})

    // Submit button
    $('#btnRegisterByNuban').click(function () {
        debugger
     
        $('#btnRegisterByNuban').prop('disabled', true);
        var txtFnameNuban = $('#txtFnameNuban').val();
        var txtMobileRegisternuban = $('#txtMobileRegisternuban').val();
        var txtPasswordRegisternuban = $('#txtPasswordRegisternuban').val();
        var txtLnmeNuban = $('#txtLnmeNuban').val();
        var txtEmailNubanRegister = $('#txtEmailNubanRegister').val();
        var txtAccountNumberNuban = $('#txtAccountNumberNuban').val();
        var txtIsCardChecked = IsCardCheckedNuban;
        var txtbankCode = BankCodeNuban;
        var txtbankName = BankNameNuban;
        validateEmailForSignupNuban();
        validatFields();
        if (fNameErrorNuban == true && lnameErrorNuban == true && emailErrorNuban && accountNumberErrorNuban && passwordErrorNuban && mobileNumberError && ConpasswordError1Nuban
            && ConpasswordError2Nuban && IsTermConditionCheckedNuban && IsppCheckedNuban && passwordStrenghNuban && bankNameNubanError) {
            /*var req = { Email: email, Password: password }*/
            $('#loader1').show();
            var finalNumber = "";
            if (txtMobileRegisternuban.indexOf('0') == 0) {
                finalNumber = txtMobileRegisternuban.slice(1);
            } else {
                finalNumber = txtMobileRegisternuban;
            }
            mobileVerifyNuban = finalNumber;
            var formData = new FormData();
            formData.append("Firstname", txtFnameNuban);
            formData.append("Lastname", txtLnmeNuban);
            formData.append("AccountNumber", txtAccountNumberNuban);
            formData.append("BankCode", txtbankCode);
            formData.append("Email", txtEmailNubanRegister);
            formData.append("Password", txtPasswordRegisternuban);
            formData.append("BankName", txtbankName);
            formData.append("CountryCode", "+234");
            formData.append("IsCardChecked", txtIsCardChecked);
            formData.append("DeviceType", 3);
            formData.append("DeviceId", "Web");
            formData.append("DeviceToken", "Web");
            formData.append("RoleId", 4);
            formData.append("PhoneNumber", finalNumber);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/RegisterWithNuban/RegisterWithNuban",
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

                        $("#registerModalByNuban").modal('hide');
                        $("#chkTandCNuban").prop('checked', false);
                        swal({
                            title: "Success",
                            text: "OTP has been sent to your mobile number",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#verificationmodalNuban").modal('show');
                            timerOnNuban = true;
                            $('#btnResendNuban').css("color", "#D3D3D3");
                            $('#btnResendNuban').prop('disabled', true);
                            timer2(59);
                        })
                    }
                    if (response.RstKey == 11 && response.RoleId == 3) {
                        var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
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
                            $("#verificationmodalNuban").modal('show');
                            timerOnNuban = true;
                            $('#btnResend').css("color", "#D3D3D3");
                            $('#btnResend').prop('disabled', true);
                            timer(59);
                        })
                    }
                    else if (response.RstKey == 7) {
                        $('#btnRegisterByNuban').prop('disabled', false);
                        $("#registerModal").modal('hide');
                        swal(
                            'Error!',
                            'Email already exists.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    } else if (response.RstKey == 8) {
                        $('#btnRegisterByNuban').prop('disabled', false);
                        $("#registerModal").modal('hide');
                        swal(
                            'Error!',
                            'Mobile number already exists.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    else if (response.RstKey == 2) {
                       // $("#registerModal").modal('hide');
                        $('#btnRegisterByNuban').prop('disabled', false);
                        swal(
                            'Error!',
                            'Failed',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    else if (response.RstKey == 9) {
                        // $("#registerModal").modal('hide');
                        $('#btnRegisterByNuban').prop('disabled', false);
                        swal(
                            'Error!',
                            'Account number already exists.',
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
            $('#btnRegister').prop('disabled', false);
        } else {
            $('#btnRegister').prop('disabled', false);
            return false;
        }
    });

    $("#chkApplyCardNuban").change(function () {
        if ($(this).prop('checked')) {

            IsCardCheckedNuban = true;
        } else {
            IsCardCheckedNuban = false;
        }
    });
    $("#chkTandCNuban").change(function () {
        if ($(this).prop('checked')) {

            IsTermConditionCheckedNuban = true;
        } else {
            IsTermConditionCheckedNuban = false;
        }
    });
    $("#chkPrivacyNuban").change(function () {
        if ($(this).prop('checked')) {

            IsppCheckedNuban = true;
        } else {
            IsppCheckedNuban = false;
        }
    });

    function validatFields() {

        var txtFnameNuban = $('#txtFnameNuban').val();
        var txtMobileRegisternuban = $('#txtMobileRegisternuban').val();
        var txtPasswordRegisternuban = $('#txtPasswordRegisternuban').val();
        var txtLnmeNuban = $('#txtLnmeNuban').val();
        var txtEmailNubanRegister = $('#txtEmailNubanRegister').val();
        var txtAccountNumberNuban = $('#txtAccountNumberNuban').val();
        var txtConfirmPasswordnuban = $('#txtConfirmPasswordnuban').val();
        if (txtEmailNubanRegister == '' || txtEmailNubanRegister == "" || txtEmailNubanRegister == null) {
            // $('#usercheck').show();
            emailErrorNuban = false;
            $('#txtEmailNubanRegisterError').show();
            $('#btnRegisterByNuban').prop('disabled', false);
            // return false;
        }
        else {
            // $('#usercheck').hide();
            emailErrorNuban = true;
            $('#txtEmailNubanRegisterError').hide();
            // return true;

        }
        if (txtAccountNumberNuban == '' || txtAccountNumberNuban == "" || txtAccountNumberNuban == null) {
            // $('#usercheck').show();
            accountNumberErrorNuban = false;
            $('#txtAccountNumberNubanError').show();
            $('#btnRegisterByNuban').prop('disabled', false);
            // return false;
        }
        else {
            // $('#usercheck').hide();
            accountNumberErrorNuban = true;
            $('#txtAccountNumberNubanError').hide();
            // return true;

        }
        if (txtFnameNuban == '' || txtFnameNuban == "" || txtFnameNuban == null) {
            // $('#usercheck').show();
            fNameErrorNuban = false;
            $('#txtFnameNubanError').show();
            $('#btnRegisterByNuban').prop('disabled', false);
        }
        else {
            fNameErrorNuban = true;
            $('#txtFnameNubanError').hide();
        }
        if (txtLnmeNuban == '' || txtLnmeNuban == "" || txtLnmeNuban == null) {
            // $('#usercheck').show();
            lnameErrorNuban = false;
            $('#txtLnmeNubanError').show();
            $('#btnRegisterByNuban').prop('disabled', false);
        }
        else {
            lnameErrorNuban = true;
            $('#txtLnmeNubanError').hide();
        }
        if (txtPasswordRegisternuban == '' || txtPasswordRegisternuban == "" || txtPasswordRegisternuban == null) {
            $('#btnRegisterByNuban').prop('disabled', false);
            $('#txtPasswordRegisternubanError').show();
            $('#txtPasswordRegisternubanError').text("Please enter your password.");
            passwordErrorNuban = false;
        } else {
            if (txtPasswordRegisternuban.length < 8) {
                $('#txtPasswordRegisternubanError').prop('disabled', false);
                $('#txtPasswordRegisternubanError').show();
                $('#txtPasswordRegisternubanError').text("Password should be 8 characters");
                passwordErrorNuban = false;
            } else {
                $('#txtPasswordRegisternubanError').hide();
                passwordErrorNuban = true;
            }
        }
        if (txtMobileRegisternuban == '' || txtMobileRegisternuban == "" || txtMobileRegisternuban == null) {
            $('#btnRegisterByNuban').prop('disabled', false);
            $('#txtMobileRegisternubanError').show();
            mobileNumberError = false;
        } else {
            var dd = txtMobileRegisternuban.indexOf('0');
            if (txtMobileRegisternuban.indexOf('0') == 0 && txtMobileRegisternuban.length < 10) {
                mobileNumberError = false;
                $('#btnRegisterByNuban').prop('disabled', false);
                $("#txtMobilelengthErrorNuban").text("Mobile number should be 11 digit with zero.");
            }
            else if (txtMobileRegisternuban.indexOf('0') == 0 && txtMobileRegisternuban.length == 11) {
                mobileNumberError = true;
                $('#txtMobilelengthErrorNuban').hide();
                $("#txtMobilelengthErrorNuban").hide();
            }
            else if (txtMobileRegisternuban.indexOf('0') != 0 && txtMobileRegisternuban.length == 10) {
                mobileNumberError = true;
                $('#txtMobilelengthErrorNuban').hide();
                $("#txtMobileRegisternubanError").hide();
            }
            else {
                mobileNumberError = false;
                $('#btnRegisterByNuban').prop('disabled', false);
                $("#txtMobilelengthErrorNuban").show()
                $("#txtMobilelengthErrorNuban").text("Mobile number should be 10 digit without zero.");
            }
        }
        if (txtConfirmPasswordnuban == '' || txtConfirmPasswordnuban == "" || txtConfirmPasswordnuban == null) {
            //$('#passcheck').show();
            $('#btnRegisterByNuban').prop('disabled', false);
            $('#txtConfirmPasswordnubanError').show();
            $('#txtConfirmPasswordnubanError').text("Please enter confirm password.");
            ConpasswordError1Nuban = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError1Nuban = true;
            $('#txtConfirmPasswordnubanError').hide();
            // return true;
        }
        if (txtConfirmPasswordnuban != "" && txtPasswordRegisternuban != txtConfirmPasswordnuban) {
            //$('#passcheck').show();
            $('#btnRegister').prop('disabled', false);
            $('#txtConfirmPasswordnubanError1').show();
            $('#txtConfirmPasswordnubanError').hide();
            $('#txtConfirmPasswordnubanError1').text("Password does not match!");
            ConpasswordError2Nuban = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError2Nuban = true;
            $('#txtConfirmPasswordnubanError1').hide();
            // return true;
        }
        if (IsTermConditionCheckedNuban == false) {
            $('#btnRegisterByNuban').prop('disabled', false);
            $('#tandcError').text("Please accept terms and conditions");
            $('#tandcError').show();
        } else {
            $('#tandcError').hide();
        }
        if (IsppCheckedNuban == false) {
            $('#btnRegisterByNuban').prop('disabled', false);
            $('#ppnubanError').text("Please accept privacy policy");
            $('#ppnubanError').show();
        } else {
            $('#ppnubanError').hide();
        }
        if (passwordStrenghNuban == false) {
            $('#txtPasswordRegisternubanError1').show();
            $('#txtPasswordRegisternubanError1').text("Password must contain at least one uppercase,one lowercase,one special character and one number.")
        }
        if (BankNameNuban == '' || BankNameNuban == "" || BankNameNuban == null) {
            //$('#passcheck').show();
            $('#btnRegisterByNuban').prop('disabled', false);
            $('#ddlbankListQoreIdError').show();
            // $('#ddlbankListQoreIdError').text("Please enter confirm password.");
            bankNameNubanError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            bankNameNubanError = true;
            $('#ddlbankListQoreIdError').hide();
            // return true;
        }
    }

    function validateEmailForSignupNuban() {
        var userinput = $('#txtEmailNubanRegister').val();
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
                    emailFormateErrornuban = false;
                    $('#btnRegister').prop('disabled', false);
                })
            }
            else {
                emailFormateErrornuban = true;
            }
        }


    }

    $('#txtPasswordRegisternuban').keyup(function () {
        $('#strengthMessage').show();
        $('#strengthMessage').html(checkStrength($('#txtPasswordRegisternuban').val()))
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
            $('#txtPasswordRegisternubanError1').hide()
            passwordStrenghNuban = true;
            return 'Strong'
        } else {
            $('#txtPasswordRegisternubanError1').show();
            $('#txtPasswordRegisternubanError1').text("Password must contain at least one uppercase,one lowercase,one special character and one number.password should be 8 characters")
            // $('#strengthMessage').addClass('Weak')
            passwordStrenghNuban = false;
            // return false;

        }

    }

    $('#ddlbankListQoreId').change(function () {

        BankCodeNuban = $('select#ddlbankListQoreId option:selected').val();
        BankNameNuban = $('select#ddlbankListQoreId option:selected').text();

    });

    $('#btnOtpVerifyNuban').click(function () {

        $('#btnOtpVerifyNuban').prop('disabled', true);
        var FirstDigit = $('#txtFirstDigitNuban').val();
        var SecondDigit = $('#txtSecondDigitNuban').val();
        var ThirdDigit = $('#txtThirdDigitNuban').val();
        var FourthDigit = $('#txtFourthDigitNuban').val();


        if (FirstDigit == '' || FirstDigit == "" || FirstDigit == null) {
            $('#btnOtpVerifyNuban').prop('disabled', false);
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
            $('#btnOtpVerifyNuban').prop('disabled', false);
            swal(
                'Error!',
                'Second digit can not empty',
                'error'
            ).catch(swal.noop);
            return false;
        }
        if (ThirdDigit == '' || ThirdDigit == "" || ThirdDigit == null) {
            //$('#passcheck').show();
            $('#btnOtpVerifyNuban').prop('disabled', false);
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
            $('#btnOtpVerifyNuban').prop('disabled', false);
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
            formData.append("MobileNumber", mobileVerifyNuban);
            formData.append("OtpCode", otp);
            formData.append("DeviceType", 3);
            formData.append("DeviceId", "Web");
            formData.append("DeviceToken", "Web");

            $.ajax({
                type: "POST",
                cache: false,
                url: "/RegisterWithNuban/VerifyOTPWeb",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('#loader1').hide();

                    $('#btnOtpVerify').prop('disabled', false);
                    if (response.RstKey == 6) {
                        $("#verificationmodalNuban").modal('hide');
                        $(".modal-body input").val("");
                        swal({
                            title: "Success",
                            text: "OTP verified",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            if (userTypeForSignup == 4) {
                                $("#addOtherDetailsModal").modal('show');
                               // window.location.href = "Home/Index";
                            }
                            else {
                               // window.location.href = "Account/CompleteEmployerProfile";
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

    $("#btnResendNuban").click(function () {
        $('#btnResendNuban').prop('disabled', true);

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
                        $('#btnResendNuban').prop('disabled', true);
                        $('#btnResendNuban').css("color", "#D3D3D3");
                        timer2(59);
                        $("#verificationmodalNuban").show();
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
})

function getQoreIdBankList() {
    var formData = new FormData();
    formData.append("guid", 0);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/RegisterWithNuban/BankListForRegister/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            debugger
            let ddlCountryOptions = "";// '<option value="">Please select city</option>';
            // $('#ddlCity').append(ddlCountryOptions);
            $.each(response.bankListResponse.data, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddlCountryOptions += '<option value="' + value.QoreIdBankCode + '">' + value.bankName + '</option>';
            });
            $('#ddlbankListQoreId').append(ddlCountryOptions);
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

function timer2(remaining) {
    //$('#btnResendNuban').css("color", "#D3D3D3");
    //$('#btnResendNuban').prop('disabled', true);
    var m = Math.floor(remaining / 60);
    var s = remaining % 60;

    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    document.getElementById('timer22').innerHTML = m + ':' + s;
    remaining -= 1;
    if (remaining == -1) {
        // IsOtpResent = true;
        $('#btnResendNuban').css("color", "#32A4FF");
        $('#btnResendNuban').prop('disabled', false);
    }

    if (remaining >= 0 && timerOnNuban) {
        setTimeout(function () {
            timer2(remaining);
        }, 1000);
        return;
    }

    if (!timerOnNuban) {

        return;
    }

    // Do timeout stuff here

}