var BankNameNubanVendor = "";
var BankCodeNubanVendor = "";
let IsCardCheckedNubanVendor = false;
let IsppCheckedNubanVendor = false;
let IsTermConditionCheckedNubanVendor = false;
let emailErrorNubanVendor = true;


let fNameErrorNubanVendor = true;
let lnameErrorNubanVendor = true;
let emailFormateErrorNubanVendor = true;
let mobileFormateErrorNubanVendor = true;
let passwordErrorNubanVendor = true;
let mobileNumberErrorNubanVendor = true;
let accountNumberErrorNubanVendor = true;
let ConpasswordError1NubanVendor = true
let ConpasswordError2NubanVendor = true
var mobileVerifyNubanVendor = "";
let logintypeNubanVendor = 0;
var OtpPassNubanVendor = "";
var passwordStrenghNubanVendor = false;
let timerOnNubanVendor = false;
let bankNameNubanErrorVendor = false;
let mobileNumberErrorVendor = true;
let userTypeForVendorSignup = 5;
$(document).ready(function () {

    getQoreIdBankListForVendor();
    /*signup errors*/
    $('#txtFnameNubanVendorError').hide();
    $('#txtMobileRegisternubanVendorError').hide();
    $('#ddlbankListQoreIdVendorError').hide();
    $('#txtPasswordRegisternubanVendorError').hide();
    $('#cb1Error').hide();
    $('#cb2Error').hide();
    $('#txtLnmeNubanVendorError').hide();
    $('#txtEmailNubanRegisterVendorError').hide();
    $('#txtAccountNumberNubanVendorError').hide();
    $('#txtConfirmPasswordnubanVendorError').hide();

    $('#tandcVendorError').hide();
    $('#ppnubanVendorError').hide();
    

    $("#registerDivVendorUser").click(function () {
        $("#registerModalByNubanVendor").modal('show');
    });
    $("#closeNubanModalVendor").click(function () {
        $("#registerModalByNubanVendor").modal('hide');
        window.location.reload();
    });


    $('#ddlbankListQoreIdVendor').change(function () {

        BankCodeNubanVendor = $('select#ddlbankListQoreIdVendor option:selected').val();
        BankNameNubanVendor = $('select#ddlbankListQoreIdVendor option:selected').text();

    });

    $("#chkApplyCardNubanVendor").change(function () {
        if ($(this).prop('checked')) {

            IsCardCheckedNubanVendor = true;
        } else {
            IsCardCheckedNubanVendor = false;
        }
    });
    $("#chkTandCNubanVendor").change(function () {
        if ($(this).prop('checked')) {

            IsTermConditionCheckedNubanVendor = true;
        } else {
            IsTermConditionCheckedNubanVendor = false;
        }
    });
    $("#chkPrivacyNubanVendor").change(function () {
        if ($(this).prop('checked')) {

            IsppCheckedNubanVendor = true;
        } else {
            IsppCheckedNubanVendor = false;
        }
    });

    function validatFieldsVendor() {

        var txtFnameNuban = $('#txtFnameNubanVendor').val();
        var txtMobileRegisternuban = $('#txtMobileRegisternubanVendor').val();
        var txtPasswordRegisternuban = $('#txtPasswordRegisternubanVendor').val();
        var txtLnmeNuban = $('#txtLnmeNubanVendor').val();
        var txtEmailNubanRegister = $('#txtEmailNubanRegisterVendor').val();
        var txtAccountNumberNuban = $('#txtAccountNumberNubanVendor').val();
        var txtConfirmPasswordnuban = $('#txtConfirmPasswordnubanVendor').val();
        if (txtEmailNubanRegister == '' || txtEmailNubanRegister == "" || txtEmailNubanRegister == null) {
            // $('#usercheck').show();
            emailErrorNubanVendor = false;
            $('#txtEmailNubanRegisterVendorError').show();
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            // return false;
        }
        else {
            // $('#usercheck').hide();
            emailErrorNubanVendor = true;
            $('#txtEmailNubanRegisterVendorError').hide();
            // return true;

        }
        if (txtAccountNumberNuban == '' || txtAccountNumberNuban == "" || txtAccountNumberNuban == null) {
            // $('#usercheck').show();
            accountNumberErrorNubanVendor = false;
            $('#txtAccountNumberNubanVendorError').show();
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            // return false;
        }
        else {
            // $('#usercheck').hide();
            accountNumberErrorNubanVendor = true;
            $('#txtAccountNumberNubanVendorError').hide();
            // return true;

        }
        if (txtFnameNuban == '' || txtFnameNuban == "" || txtFnameNuban == null) {
            // $('#usercheck').show();
            fNameErrorNubanVendor = false;
            $('#txtFnameNubanVendorError').show();
            $('#btnRegisterByNubanVendor').prop('disabled', false);
        }
        else {
            fNameErrorNubanVendor = true;
            $('#txtFnameNubanVendorError').hide();
        }
        if (txtLnmeNuban == '' || txtLnmeNuban == "" || txtLnmeNuban == null) {
            // $('#usercheck').show();
            lnameErrorNubanVendor = false;
            $('#txtLnmeNubanVendorError').show();
            $('#btnRegisterByNubanVendor').prop('disabled', false);
        }
        else {
            lnameErrorNubanVendor = true;
            $('#txtLnmeNubanVendorError').hide();
        }
        if (txtPasswordRegisternuban == '' || txtPasswordRegisternuban == "" || txtPasswordRegisternuban == null) {
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            $('#txtPasswordRegisternubanVendorError').show();
            $('#txtPasswordRegisternubanVendorError').text("Please enter your password.");
            passwordErrorNubanVendor = false;
        } else {
            if (txtPasswordRegisternuban.length < 8) {
                $('#txtPasswordRegisternubanVendorError').prop('disabled', false);
                $('#txtPasswordRegisternubanVendorError').show();
                $('#txtPasswordRegisternubanVendorError').text("Password should be 8 characters");
                passwordErrorNubanVendor = false;
            } else {
                $('#txtPasswordRegisternubanVendorError').hide();
                passwordErrorNubanVendor = true;
            }
        }
        if (txtMobileRegisternuban == '' || txtMobileRegisternuban == "" || txtMobileRegisternuban == null) {
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            $('#txtMobileRegisternubanVendorError').show();
            mobileNumberErrorNubanVendor = false;
        } else {
            var dd = txtMobileRegisternuban.indexOf('0');
            if (txtMobileRegisternuban.indexOf('0') == 0 && txtMobileRegisternuban.length < 10) {
                mobileNumberErrorNubanVendor = false;
                $('#btnRegisterByNubanVendor').prop('disabled', false);
                $("#txtMobilelengthErrorVendorNuban").text("Mobile number should be 11 digit with zero.");
            }
            else if (txtMobileRegisternuban.indexOf('0') == 0 && txtMobileRegisternuban.length == 11) {
                mobileNumberErrorNubanVendor = true;
                $('#txtMobilelengthErrorVendorNuban').hide();
                $("#txtMobilelengthErrorVendorNuban").hide();
            }
            else if (txtMobileRegisternuban.indexOf('0') != 0 && txtMobileRegisternuban.length == 10) {
                mobileNumberErrorNubanVendor = true;
                $('#txtMobilelengthErrorVendorNuban').hide();
                $("#txtMobileRegisternubanVendorError").hide();
            }
            else {
                mobileNumberErrorNubanVendor = false;
                $('#btnRegisterByNubanVendor').prop('disabled', false);
                $("#txtMobilelengthErrorVendorNuban").show()
                $("#txtMobilelengthErrorVendorNuban").text("Mobile number should be 10 digit without zero.");
            }
        }
        if (txtConfirmPasswordnuban == '' || txtConfirmPasswordnuban == "" || txtConfirmPasswordnuban == null) {
            //$('#passcheck').show();
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            $('#txtConfirmPasswordnubanVendorError').show();
            $('#txtConfirmPasswordnubanVendorError').text("Please enter confirm password.");
            ConpasswordError1NubanVendor = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError1NubanVendor = true;
            $('#txtConfirmPasswordnubanVendorError').hide();
            // return true;
        }
        if (txtConfirmPasswordnuban != "" && txtPasswordRegisternuban != txtConfirmPasswordnuban) {
            //$('#passcheck').show();
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            $('#txtConfirmPasswordnubanVendorError1').show();
            $('#txtConfirmPasswordnubanError').hide();
            $('#txtConfirmPasswordnubanVendorError1').text("Password does not match!");
            ConpasswordError2NubanVendor = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError2NubanVendor = true;
            $('#txtConfirmPasswordnubanVendorError1').hide();
            // return true;
        }
        if (IsTermConditionCheckedNubanVendor == false) {
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            $('#tandcVendorError').text("Please accept terms and conditions");
            $('#tandcVendorError').show();
        } else {
            $('#tandcVendorError').hide();
        }
        if (IsppCheckedNubanVendor == false) {
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            $('#ppnubanVendorError').text("Please accept privacy policy");
            $('#ppnubanVendorError').show();
        } else {
            $('#ppnubanVendorError').hide();
        }
        if (passwordStrenghNuban == false) {
            $('#txtPasswordRegisternubanError1').show();
            $('#txtPasswordRegisternubanError1').text("Password must contain at least one uppercase,one lowercase,one special character and one number.")
        }
        if (BankNameNubanVendor == '' || BankNameNubanVendor == "" || BankNameNubanVendor == null) {
            //$('#passcheck').show();
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            $('#ddlbankListQoreIdVendorError').show();
            // $('#ddlbankListQoreIdError').text("Please enter confirm password.");
            bankNameNubanErrorVendor = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            bankNameNubanErrorVendor = true;
            $('#ddlbankListQoreIdVendorError').hide();
            // return true;
        }
    }

    function validateEmailForSignupNubanVendor() {
        var userinput = $('#txtEmailNubanRegisterVendor').val();
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
                    $('#btnRegisterByNubanVendor').prop('disabled', false);
                })
            }
            else {
                emailFormateErrornuban = true;
            }
        }


    }

    $('#txtPasswordRegisternubanVendor').keyup(function () {
        $('#strengthMessage').show();
        $('#strengthMessage').html(checkStrengthVendor($('#txtPasswordRegisternubanVendor').val()))
    })
    function checkStrengthVendor(password) {

        var strength = 0
        if (password.length < 7) {
            $('#strengthMessage').removeClass()
            //$('#strengthMessage').addClass('Short')
            return 'Too short'
        }

        var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*_])(?=.{8,30}$)");

        if (regex.test(password)) {
            // $('#txtPasswordRegisterError').removeClass()
            $('#txtPasswordRegisternubanVendorError1').hide()
            passwordStrenghNubanVendor = true;
            return 'Strong'
        } else {
            $('#txtPasswordRegisternubanVendorError1').show();
            $('#txtPasswordRegisternubanVendorError1').text("Password must contain at least one uppercase,one lowercase,one special character and one number.password should be 8 characters")
            // $('#strengthMessage').addClass('Weak')
            passwordStrenghNubanVendor = false;
            // return false;

        }

    }

    // Submit button
    $('#btnRegisterByNubanVendor').click(function () {
        debugger

        $('#btnRegisterByNubanVendor').prop('disabled', true);
        var txtFnameNuban = $('#txtFnameNubanVendor').val();
        var txtMobileRegisternuban = $('#txtMobileRegisternubanVendor').val();
        var txtPasswordRegisternuban = $('#txtPasswordRegisternubanVendor').val();
        var txtLnmeNuban = $('#txtLnmeNubanVendor').val();
        var txtEmailNubanRegister = $('#txtEmailNubanRegisterVendor').val();
        var txtAccountNumberNuban = $('#txtAccountNumberNubanVendor').val();
        var txtIsCardChecked = IsCardCheckedNuban;
        var txtbankCode = BankCodeNubanVendor;
        var txtbankName = BankNameNubanVendor;
        validateEmailForSignupNubanVendor();
        validatFieldsVendor();
        if (fNameErrorNubanVendor == true && lnameErrorNubanVendor == true && emailErrorNubanVendor && accountNumberErrorNubanVendor && passwordErrorNubanVendor && mobileNumberErrorVendor && ConpasswordError1NubanVendor
            && ConpasswordError2NubanVendor && IsTermConditionCheckedNubanVendor && IsppCheckedNubanVendor && passwordStrenghNubanVendor && bankNameNubanErrorVendor) {
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
            formData.append("RoleId", 5);
            formData.append("PhoneNumber", finalNumber);
            $.ajax({
                type: "POST",
                cache: false,
                url: "",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('#btnRegisterByNubanVendor').prop('disabled', false);
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
                        $('#btnRegisterByNubanVendor').prop('disabled', false);
                        $("#registerModal").modal('hide');
                        swal(
                            'Error!',
                            'Email already exists.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    } else if (response.RstKey == 8) {
                        $('#btnRegisterByNubanVendor').prop('disabled', false);
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
                        $('#btnRegisterByNubanVendor').prop('disabled', false);
                        swal(
                            'Error!',
                            'Failed',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    else if (response.RstKey == 9) {
                        // $("#registerModal").modal('hide');
                        $('#btnRegisterByNubanVendor').prop('disabled', false);
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
        } else {
            $('#btnRegisterByNubanVendor').prop('disabled', false);
            return false;
        }
    });
})


function getQoreIdBankListForVendor() {
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
            $('#ddlbankListQoreIdVendor').append(ddlCountryOptions);
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