$(document).ready(function () {
    let ddlBankError = true;
    let txtCustomerIdError = true;
    let txtbvnCodeError = true;
    let txtAccountNumberError = true;
    let txtAccountHolderNameError = true;
    var bankName = "";
    var bankId = "";
    var D2cbankId = "";
    var D2cbankName = "";
    var UserGuid = "";
    var EmployerName = "";
    var EmployerId = 0;
    //  var UserGuid = "";
    let employerError = true;
    let employeridError = true;
    let staffIdError = true;
    var img_resp = [];
    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");
    $("#imgVerified").hide();
    $("#imgNotVerified").hide();
    $("#imgKYCVerified").hide();
    $("#imgKYCNotVerified").hide();
    UserGuid = sessionStorage.getItem("User1");

    getBankList();
    getEmployerList();
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);
    getProfile();
    getBankListByUserGuid();

    function getProfile() {

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
                $('#loader1').hide();
                if (response.ProfileImage == "") {
                    $('.profile-pic').attr('src', '/Content/images/avatar.jpg');

                } else {
                    $('.profile-pic').attr('src', response.ProfileImage);
                    sessionStorage.setItem("ProfileImage", response.ProfileImage);
                }
                $("#spnUserName").text(response.FirstName + " " + response.LastName);
                $("#spnEmail").text(response.Email);
                $("#spnAddess").text(response.Address);
                if (response.EmployerName != "") {
                    $("#EmployerDetail").text(response.EmployerName + '-' + response.EmployerId);
                }
                else {
                    $("#EmployerDetail").text('');
                }
                if (response.IsEmailVerified) {
                    $("#imgVerified").show();
                    $("#imgNotVerified").hide();
                } else {
                    $("#imgNotVerified").show();
                    $("#imgVerified").hide();
                }
                if (response.IsKycVerified) {
                    $("#imgKYCVerified").show();
                    $("#imgKYCNotVerified").hide();
                } else {
                    $("#imgKYCNotVerified").show();
                    $("#imgKYCVerified").hide();
                }
                // $("#spnHeaderName").text(response.FirstName + " " + response.LastName);
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


    $('#btnViewProfile').click(function () {
        window.location.href = "ViewProfile";
    });

    $("#addBankDiv").click(function () {
        $("#addBankModal").modal('show');
    });

    $("#addEmployerDiv").click(function () {
        $("#addEmployerModal").modal('show');
    });

    $("#addD2CEmployerDiv").click(function () {
        $("#addD2CEmployerModal").modal('show');
    });

    function getBankList() {
        var id = 1;
        var formData = new FormData();
        formData.append("request", UserGuid);
        $.ajax({
            type: "POST",
            cache: false,
            /* url: "/BillAndPayment/GetBankList/",*/
            url: "/ExpressWalletServices/GetExpressBankList/",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {

                $.each(response.expressBankList.banks, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    //ddOptions += '<option value="' + value.bankCode + '">' + value.bankName + '</option>';
                    $('#ddlBankAdd').append('<option value="' + value.code + '">' + value.name + '</option>');
                    $('#ddlD2cBankAdd').append('<option value="' + value.code + '">' + value.name + '</option>');
                });
            },
            //complete: function (resp) {
            //    $("#ddlBank").select2({
            //        templateResult: formatOptions,
            //        templateSelection: formatOptions,
            //        theme: 'input-text'


            //    });
            //    var count = 0;
            //    function formatOptions(state) {

            //        if (!state.id) { return state.text; }
            //        var optimage = $(state.element).attr('data-img');
            //        var $state = $(
            //            '<span ><img sytle="display: inline-block;" src="' + optimage + '" width="20" height="20"/> ' + state.text + '</span>'
            //        );


            //        count++;
            //        return $state;

            //    }


            //}
        });
    }



    function getBankListByUserGuid() {

        var formData = new FormData();
        formData.append("guid", UserGuid);
        $.ajax({
            type: "POST",
            cache: false,
            url: "GetBankListByUserGuid",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                var item = "";

                $.each(response.getBankList, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class="bank-name-container"><div class=""><img src="../Content/images/bank-logo.png" alt="" width="36px" height="36px">' +
                        '<div class="bank-content">' + value.BankName + '</div></div><span class="delete-bank" data-val=' + value.Id + '><img src="../Content/images/delete-icon.png" alt="" style="cursor:pointer;"></span></div>';
                });
                //$('#divBankList').append('<div value="' + value.BankCode + '">' + value.BankName + '</div>');
                $('#divBankList').append(item).show();
            }
        });
    }

    $('body').on('click', 'span.delete-bank', function (e) {
        e.preventDefault;

        var data = $(this).data('val')
        var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you sure you want to delete this bank account?",
            // text: "Warnning",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("BankId", data);
            $.ajax({
                type: "POST",
                cache: false,
                url: "DeleteBankByBankDetailId",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        window.location.reload();
                        swal({
                            title: "Good job!",
                            text: "Bank deleted successfully!",
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Bank not deleted !",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        })

    });

    $('#ddlBankAdd').change(function () {

        bankId = $('select#ddlBankAdd option:selected').val();
        bankName = $('select#ddlBankAdd option:selected').text();

    });

    $('#ddlD2cBankAdd').change(function () {

        D2cbankId = $('select#ddlD2cBankAdd option:selected').val();
        D2cbankName = $('select#ddlD2cBankAdd option:selected').text();

    });

    $('#ddlEmployer').change(function () {

        EmployerId = $('select#ddlEmployer option:selected').val();
        EmployerName = $('select#ddlEmployer option:selected').text();

    });

    $('#btnSaveBanks').click(function () {

        $('#btnSaveBanks').prop('disabled', true);

        var bank = bankName;
        //var txtCustomerId = $('#txtCustomerId').val();
        var txtbvnCode = $('#txtbvnCode').val();
        var txtAccountHolderName = $('#txtAccountHolderName').val();
        var txtAccountNumber = $('#txtAccountNumber').val();
        var BankCode = bankId;
        if (bank == undefined || bank == "") {
            $('#ddlBankError').show();
            $('#ddlBankError').text("Please select a bank");
            ddlBankError = false;
            $('#btnSaveBanks').prop('disabled', false);
            // return false;
        }
        else {
            $('#ddlBankError').hide();
            ddlBankError = true;
            // return true;

        }
        //if (txtCustomerId == '' || txtCustomerId == "" || txtCustomerId == null) {
        //    $('#txtCustomerIdError').show();
        //    $('#txtCustomerIdError').text("Please enter customer id");
        //    txtCustomerIdError = false;
        //    $('#btnSaveBanks').prop('disabled', false);
        //    // return false;
        //} else {
        //    $('#txtCustomerIdError').hide();
        //    txtCustomerIdError = true;
        //    // return true;
        //}
        if (txtbvnCode == '' || txtbvnCode == "" || txtbvnCode == null) {
            $('#txtbvnCodeError').show();
            $('#txtbvnCodeError').text("Please enter BVN number");
            txtbvnCodeError = false;
            $('#btnSaveBanks').prop('disabled', false);
            // return false;
        } else {
            $('#txtbvnCodeError').hide();
            txtbvnCodeError = true;
            // return true;
        }
        if (txtAccountHolderName == '' || txtAccountHolderName == "" || txtAccountHolderName == null) {
            $('#txtAccountHolderNameError').show();
            $('#txtAccountHolderNameError').text("Please enter username");
            txtbvnCodeError = false;
            $('#btnSaveBanks').prop('disabled', false);
            // return false;
        } else {
            $('#txtAccountHolderNameError').hide();
            txtbvnCodeError = true;
            // return true;
        }
        if (txtAccountNumber == '' || txtAccountNumber == "" || txtAccountNumber == null) {
            $('#txtAccountNumberError').show();
            $('#txtAccountNumberError').text("Please enter your account number");
            txtAccountNumberError = false;
            $('#btnSaveBanks').prop('disabled', false);
            // return false;
        } else {
            $('#txtAccountNumberError').hide();
            txtAccountNumberError = true;
            // return true;
        }


        if ((ddlBankError == true) && (txtCustomerIdError == true) && txtbvnCodeError == true && txtAccountHolderNameError == true && txtAccountNumberError == true) {


            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("BankName", bank);
            formData.append("AccountNumber", txtAccountNumber);
            formData.append("BVN", txtbvnCode);
            formData.append("BankAccountHolderName", txtAccountHolderName);
            // formData.append("CustomerId", txtCustomerId);
            formData.append("BankCode", BankCode);
            formData.append("ImageUrl", "");
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/AddBanks/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {

                    $('#btnSaveBanks').prop('disabled', false);

                    if (response.RstKey == 1) {

                        $("#addBankModal").modal('hide');

                        $('#divBankList').empty();
                        getBankListByUserGuid();
                        //swal({
                        //    title: "Success",
                        //    text: "Your bank added successfully.",
                        //    icon: "Success",
                        //    buttons: true,
                        //    dangerMode: true,
                        //}, function succes(isDone) {
                        //    $('#divBankList').empty();
                        //    getBankListByUserGuid();

                        //})
                        swal(
                            'Success!',
                            'Your bank added successfully.',
                            'success'
                        ).catch(swal.noop);


                    }
                    if (response.RstKey == 3) {
                        $("#addBankModal").modal('hide');
                        swal({
                            title: "Error",
                            text: response.Message,
                            icon: "error",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            window.location.reload();
                        })


                    }
                    else {

                        swal(
                            'Error!',
                            'Please Try Again.',
                            'error'
                        ).catch(swal.noop);
                        //window.location.href = "Account/Index";
                    }

                }
            });

        } else {
            $('#btnSaveBanks').prop('disabled', false);
            return false;
        }
    });

    $('#btnSaveEmployer').click(function () {
        debugger


        var employerName = EmployerName;
        var txtstaffId = $('#txtstaffId').val();
        var employerId = EmployerId;
        //let employerError = true;
        //let employeridError = true;
        //let staffIdError = true;
        if (employerName == null || employerName == "" || employerName == '' || employerName === "Select") {
            employerError = false;
            $("#ddlEmployerError").show();
            $("#ddlEmployerError").text("Please select employer");
        } else {
            $("#ddlEmployerError").hide();
            employerError = true;
        }
        if (txtstaffId == null || txtstaffId == "" || txtstaffId == '') {
            staffIdError = false;
            $("#txtStaffIdError").show();
            $("#txtStaffIdError").text("Please enter your staff id");
        } else {
            staffIdError = true;
            $("#txtStaffIdError").hide();
        }

        if (employerError && staffIdError) {
            $('#btnSaveEmployer').prop('disabled', true);

            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("StaffId", txtstaffId);
            formData.append("EmployerName", employerName);
            formData.append("EmployerId", employerId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/UpdateEmployerByUserGuid/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {

                    $('#btnSaveEmployer').prop('disabled', false);

                    if (response.RstKey == 1) {

                        $("#addEmployerModal").modal('hide');

                        getProfile();
                        //swal({
                        //    title: "Success",
                        //    text: "Your bank added successfully.",
                        //    icon: "Success",
                        //    buttons: true,
                        //    dangerMode: true,
                        //}, function succes(isDone) {
                        //    $('#divBankList').empty();
                        //    getBankListByUserGuid();

                        //})
                        swal(
                            'Success!',
                            'Your employer added successfully.',
                            'success'
                        ).catch(swal.noop);


                    }
                    if (response.RstKey == 3) {
                        $("#addEmployerModal").modal('hide');
                        swal({
                            title: "Error",
                            text: response.Message,
                            icon: "error",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            window.location.reload();
                        })


                    }
                    else {

                        swal(
                            'Error!',
                            'Please Try Again.',
                            'error'
                        ).catch(swal.noop);
                        //window.location.href = "Account/Index";
                    }

                }
            });

        } else {
            // $('#addEmployerModal').prop('disabled', false);
            return false;
        }
    });

    $('#btnSaveOtherEmployer').click(function () {
        debugger

        // var NetSalary = $('#txtD2CNetSalary').val();
        var GrossSalary = $('#txtD2CGrossSalary').val();
        var EmployerName = $('#txtD2CEmployerName').val();
        var StaffId = $('#txtD2CStaffId').val();
        var AccountNumber = $('#txtD2CAccountNumber').val();

        if (EmployerName === undefined || EmployerName === null || EmployerName === '') {
            $("#txtD2CEmployerNameError").show();
            $("#txtD2CEmployerNameError").text("Please Enter Employer Name");
            return;
        } else {
            $("#txtD2CEmployerNameError").hide();
        }

        if (StaffId === undefined || StaffId === null || StaffId === '') {
            $("#txtD2CStaffIdError").show();
            $("#txtD2CStaffIdError").text("Please Enter StaffId");
            return;
        } else {
            $("#txtD2CStaffIdError").hide();
        }
        //if (D2cbankId === undefined || D2cbankId === null || D2cbankId === '') {
        //    $("#ddlD2cBankError").show();
        //    $("#ddlD2cBankError").text("Please select bank");
        //    return;
        //} else {
        //    $("#ddlD2cBankError").hide();
        //}
        if (AccountNumber === undefined || AccountNumber === null || AccountNumber === '') {
            $("#txtD2CAccountNumberError").show();
            $("#txtD2CAccountNumberError").text("Please Enter AccountNumber");
            return;
        } else {
            $("#txtD2CAccountNumberError").hide();
        }

        if (GrossSalary === undefined || GrossSalary === null || GrossSalary === '' || parseFloat(GrossSalary) <= 0) {
            $("#txttxtD2CGrossSalary").show();
            $("#txttxtD2CGrossSalary").text("Please Enter Monthly Salary");
            return;
        } else {
            $("#txttxtD2CGrossSalary").hide();
        }

       


        $('#btnSaveOtherEmployer').prop('disabled', true);

        /*var req = { Email: email, Password: password }*/
        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append("StaffId", StaffId);
        formData.append("EmployerName", EmployerName);
        formData.append("NetSalary", GrossSalary);
        formData.append("GrossSalary", GrossSalary);
        formData.append("AccountNumber", AccountNumber);
        formData.append("BankCode", D2cbankId);
        $.ajax({
            type: "POST",
            cache: false,
            url: "/Account/CreateD2CEmployer/",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {

                $('#btnSaveOtherEmployer').prop('disabled', false);

                if (response.RstKey === 3) {

                    $("#addD2CEmployerModal").modal('hide');

                    getProfile();
                    swal(
                        'Success!',
                        'Your employer added successfully.',
                        'success'
                    ).catch(swal.noop);
                }
                else {
                    $("#addD2CEmployerModal").modal('hide');
                    swal({
                        title: "Error",
                        text: response.Message,
                        icon: "error",
                        buttons: true,
                        dangerMode: true,
                    }, function succes(isDone) {
                        /*window.location.reload();*/
                    })
                }
            }
        });


    });


    $(function () {
        $('#txtCustomerId').on('keypress', function (e) {
            if (e.which == 32) {
                // alert('Space not allowed');
                return false;
            }
        });
    });


    $(function () {
        $('#txtAccountHolderName').on('keypress', function (e) {
            if (event.target.value.substr(-1) === ' ' && event.code === 'Space') {
                return false;
            }
        });
    });

    $("#txtAccountNumber").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;
        if (val.length > 9) {
            $('.loader').show();
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("AccountNumber", val);
            formData.append("BankCode", bankId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/BillAndPayment/VerifyAccount/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('.loader').hide();
                    if (response.RstKey == 1) {
                        $("#txtAccountHolderName").val(response.verifyAccountResponse.accountName);
                    } else {
                        $("#txtAccountHolderName").val("NA");
                    }

                }
            });
        }

    });
});

function getEmployerList() {
    var id = 1;
    var formData = new FormData();
    formData.append("id", id);
    $.ajax({
        type: "POST",
        cache: false,
        url: "GetEmployerList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            //$.each(response.employerResponses, function (index, value) {
            //    // APPEND OR INSERT DATA TO SELECT ELEMENT.
            //    $('#ddlEmployer').append('<option value="' + value.Id + '">' + value.OrganisationName + '</option>');
            //});
            let ddOptions = '<option value=""  class="mt">Please select employer</option>';
            $.each(response.employerResponses, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.Id + '">' + value.OrganisationName + '</option>';
            });
            $('#ddlEmployer').append(ddOptions);
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