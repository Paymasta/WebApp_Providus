var UserGuid = "";
var Code = "";
var ServiceName = "";
let txtAccountNumberError = true
let txtBankAmountError = true
let txtBeneficiaryError = true
let txtAccounttypeError = true
let txtBankddlError = true
var BankList = "";
var IsSelfAccount = false;
var accountTypeDetail = "";

let txtwalletAccountNumberError = true
let txtwalletBankAmountError = true
let txtwalletBeneficiaryError = true

$(document).ready(function () {
    //$('#divmain_content').hide();
    $('.loader').hide();
    UserGuid = sessionStorage.getItem("User1");



    $(document).on('click', '#WalletToWalletDiv', function () {
        $("#walletToWalletModal").show();
    });

    $(document).on('click', '#BtnClosewallettowallet', function () {
        $("#walletToWalletModal").hide();
    });



    $("#bankTransferAmount").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;

        if (val > 1 && val <= 5000) {
            var commissionAmount = parseFloat(val) + 32.25
            $('#bankTransferAmountError').text('Fees : ' + 32.25 + '  Total Amount :  ' + commissionAmount);
        }
        else if (val > 5000 && val <= 50000) {
            var commissionAmount = parseFloat(val) + 53.74;
            $('#bankTransferAmountError').text('Fees : ' + 53.74 + '  Total Amount :  ' + commissionAmount);
        }
        else if (val > 50000 && val <= 100000) {
            var commissionAmount = parseFloat(val) + 107.5;
            $('#bankTransferAmountError').text('Fees : ' + 107.5 + '  Total Amount :  ' + commissionAmount);
        }
        else {
            $('#bankTransferAmountError').text('');
        }
        if (!valid) {
            console.log("Invalid input!");
            this.value = val.substring(0, val.length - 1);
        }
    });

    $("#txtBankAccountNumber").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;
        if (val.length > 9) {
            $('.loader').show();
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("accountNumber", val);
            formData.append("bankCode", Code);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/ExpressWalletServices/VerifyAccount/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('.loader').hide();
                    if (response != null) {
                        $("#txtBankBeneficiaryName").val(response.account.accountName);
                    } else {
                        $("#txtBankBeneficiaryName").val("NA");
                    }

                }
            });
        }

    });

    $('#ddlbankList').change(function () {
        debugger
        Code = $('select#ddlbankList option:selected').val();
        ServiceName = $('select#ddlbankList option:selected').text();
        var amount = $('select#ddlbankList option:selected').attr('data-operator');

        if (IsSelfAccount == true) {
            $.each(BankList, function (index, value) {
                if (value.BankCode === Code) {
                    $("#txtBankAccountNumber").val(value.AccountNumber);
                    $("#txtBankBeneficiaryName").val(value.BankAccountHolderName);
                }
            });
        } else {

        }

    });

    $('#ddlAccountTypeList').change(function () {
        debugger
        $('#ddlbankList').empty();
        var accountType = $('select#ddlAccountTypeList option:selected').val();
        accountTypeDetail = accountType
        //var accountType = $('select#ddlAccountTypeList option:selected').text();
        if (accountType.toUpperCase() == "self".toUpperCase()) {
            $('.loader').hide();
            getAddedBankList();
            IsSelfAccount = true;
            $("#txtBankBeneficiaryName").prop("readonly", true);
            $("#txtBankAccountNumber").prop("readonly", true);
        }
        else if (accountType.toUpperCase() == "other".toUpperCase()) {
            $("#txtBankBeneficiaryName").prop("readonly", false);
            $("#txtBankAccountNumber").prop("readonly", false);
            $("#txtBankAccountNumber").val('');
            $("#txtBankBeneficiaryName").val('');
            $('.loader').hide();
            IsSelfAccount = false;
            getBankList();
        }
    });

    $("#btnWalletToBankPay").click(function () {
        //  $("#walletToBankModal").modal('hide');
        var txtAccountNumber = $("#txtBankAccountNumber").val();
        var txtAmount = $("#bankTransferAmount").val();
        var amt = parseInt(txtAmount);
        var isValidamt = parseFloat(txtAmount);
        var txtBankBeneficiaryName = $("#txtBankBeneficiaryName").val();
        if (txtAccountNumber == null || txtAccountNumber == "" || txtAccountNumber == '') {
            txtAccountNumberError = false;
            $("#bankAccountNumberError").show();
            $("#bankAccountNumberError").text("Please enter mobile number.");
        } else {
            txtAccountNumberError = true;
            $("#bankAccountNumberError").hide();
        }
        if (txtAmount == null || txtAmount == "" || txtAmount == '') {
            txtBankAmountError = false;
            $("#bankTransferAmountError").show();
            $("#bankTransferAmountError").text("Please enter amount.");
        } else {
            if (amt == 0) {
                txtBankAmountError = false;
                $("#bankTransferAmountError").show();
                $("#bankTransferAmountError").text("Please enter amount greater then zero.");
            } else {
                txtBankAmountError = true;
                $("#bankTransferAmountError").hide();
            }

        }

        if (Code == null || Code == "" || Code == '') {
            txtBankddlError = false;
            $("#BankError").show();
            $("#BankError").text("Please select product.");
        } else {
            txtBankddlError = true;
            $("#BankError").hide();
        }
        if (txtBankBeneficiaryName == null || txtBankBeneficiaryName == "" || txtBankBeneficiaryName == '') {
            txtBeneficiaryError = false;
            $("#txtBankBeneficiaryNameError").show();
            $("#txtBankBeneficiaryNameError").text("Please enter beneficiary name.");
        } else {
            txtBeneficiaryError = true;
            $("#txtBankBeneficiaryNameError").hide();
        }
        if (txtBankBeneficiaryName == null || txtBankBeneficiaryName == "" || txtBankBeneficiaryName == '') {
            txtBeneficiaryError = false;
            $("#txtBankBeneficiaryNameError").show();
            $("#txtBankBeneficiaryNameError").text("Please enter beneficiary name.");
        } else {
            txtBeneficiaryError = true;
            $("#txtBankBeneficiaryNameError").hide();
        }
        if (accountTypeDetail == null || accountTypeDetail == "" || accountTypeDetail == '') {
            txtAccounttypeError = false;
            $("#AccountTypeError").show();
            $("#AccountTypeError").text("Please select account type.");
        } else {
            txtAccounttypeError = true;
            $("#AccountTypeError").hide();
        }
        //  var validateData = AirtimeValidation(txtMobileNumberAirtime, txtAmount, billerName, billerCode, 'DataBundle');
        if (txtAccountNumberError && txtBankAmountError && txtBankddlError) {
            $("#btnWalletToBankPay").prop('disabled', true);
            if (isValidamt <= 100000) {
                $('.loader').show();
                var formData = new FormData();
                formData.append("Service", "Wallet To Bank Transfer");
                formData.append("UserGuid", UserGuid);
                /* formData.append("SubCategoryId", 11);*/
                formData.append("SubCategoryId", 6);
                formData.append("AccountType", "");
                formData.append("AccountNumber", txtAccountNumber);
                formData.append("Amount", txtAmount);
                formData.append("destBankCode", Code);
                formData.append("beneficiaryName", txtBankBeneficiaryName);
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/ExpressWalletServices/WalletToBankTransfer/",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        $("#btnWalletToBankPay").prop('disabled', false);
                        $('.loader').hide();
                        $(".modal").modal('hide');
                        if (response.RstKey == 1) {
                            $("#walletToBankModal").modal('hide');


                            swal({
                                title: "Success!",
                                text: response.expressWalletToBankResponse.message,
                                icon: "success",
                                button: "Aww yiss!",
                            }, function succes(isDone) { location.reload(); });
                        }
                        else if (response.RstKey == 2) {
                            swal(
                                'Error!',
                                response.expressWalletToBankResponse.message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 4) {
                            swal(
                                'Error!',
                                response.Message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 5) {
                            swal(
                                'Error!',
                                response.Message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 6) {
                            swal(
                                'Error!',
                                response.Message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 7) {
                            swal(
                                'Error!',
                                "Insufficient  fund",
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 31) {
                            swal(
                                'Error!',
                                "Amount should be greater then 100 Naira",
                                'error'
                            ).catch(swal.noop);
                        }
                    }
                });
            } else {
                $("#btnWalletToBankPay").prop('disabled', false);
                swal(
                    'Error!',
                    "You can not transfer more than 100000",
                    'error'
                ).catch(swal.noop);
            }
        }

    });

    $("#btnWalletToWalletPay").click(function () {
        debugger
        //  $("#walletToBankModal").modal('hide');
        var txtAccountNumber = $("#txtWalletAccountNumber").val();
        var txtAmount = $("#walletTransferAmount").val();
        var amt = parseInt(txtAmount);
        var isValidamt = parseFloat(txtAmount);
        var txtBankBeneficiaryName = $("#txtWalletBeneficiaryName").val();
        if (txtAccountNumber == null || txtAccountNumber == "" || txtAccountNumber == '') {
            txtwalletAccountNumberError = false;
            $("#walletAccountNumberError").show();
            $("#walletAccountNumberError").text("Please enter wallet account number.");
        } else {
            txtwalletAccountNumberError = true;
            $("#walletAccountNumberError").hide();
        }
        if (txtAmount == null || txtAmount == "" || txtAmount == '') {
            txtwalletBankAmountError = false;
            $("#walletTransferAmountError").show();
            $("#walletTransferAmountError").text("Please enter amount.");
        } else {
            if (amt == 0) {
                txtwalletBankAmountError = false;
                $("#walletTransferAmountError").show();
                $("#walletTransferAmountError").text("Please enter amount greater then zero.");
            } else {
                txtwalletBankAmountError = true;
                $("#walletTransferAmountError").hide();
            }

        }
        if (txtBankBeneficiaryName == null || txtBankBeneficiaryName == "" || txtBankBeneficiaryName == '') {
            txtwalletBeneficiaryError = false;
            $("#txtwalletBeneficiaryNameError").show();
            $("#txtwalletBeneficiaryNameError").text("Please enter beneficiary name.");
        } else {
            txtwalletBeneficiaryError = true;
            $("#txtwalletBeneficiaryNameError").hide();
        }

        //  var validateData = AirtimeValidation(txtMobileNumberAirtime, txtAmount, billerName, billerCode, 'DataBundle');
        if (txtwalletAccountNumberError && txtwalletBankAmountError && txtwalletBeneficiaryError) {
            if (isValidamt <= 100000) {
                $('.loader').show();
                var formData = new FormData();
                formData.append("Service", "Wallet To Wallet Transfer");
                formData.append("UserGuid", UserGuid);
                formData.append("SubCategoryId", 7);
                // formData.append("SubCategoryId", 12);
                formData.append("AccountType", "");
                formData.append("WalletAccountNumber", txtAccountNumber);
                formData.append("Amount", txtAmount);
                formData.append("beneficiaryName", txtBankBeneficiaryName);
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/ExpressWalletServices/WalletToWalletTransfer/",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        $('.loader').hide();
                        $(".modal").modal('hide');
                        if (response.RstKey == 1) {
                            $("#walletToWalletModal").modal('hide');


                            swal({
                                title: "Success!",
                                text: response.vTUResponse.message,
                                icon: "success",
                                button: "Aww yiss!",
                            }, function succes(isDone) { location.reload(); });
                        }
                        else if (response.RstKey == 2) {
                            swal(
                                'Error!',
                                response.vTUResponse.message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 4) {
                            swal(
                                'Error!',
                                response.Message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 5) {
                            swal(
                                'Error!',
                                response.Message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 6) {
                            swal(
                                'Error!',
                                response.Message,
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 7) {
                            swal(
                                'Error!',
                                "Insufficient  fund",
                                'error'
                            ).catch(swal.noop);
                        }
                        else if (response.RstKey == 31) {
                            swal(
                                'Error!',
                                "Amount should be greater then 100 Naira",
                                'error'
                            ).catch(swal.noop);
                        }
                    }
                });
            } else {
                swal(
                    'Error!',
                    "You can not transfer more than 100000",
                    'error'
                ).catch(swal.noop);
            }
        }

    });

    $("#btnVerifyPasscode2").click(function () {

        var txtPasscode = $("#txtPasscode2").val();

        if (txtPasscode.length < 4) {
            $('#txtPasscodeError2').text('Please enter 4 digit Passcode');
            return;
        } else {
            $('#txtPasscodeError2').text('');
        }

        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append('Passcode', txtPasscode);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/Account/VerifyPasscode",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.RstKey === 1) {
                    swal({
                        title: "Success",
                        text: "Passcode verification completed, Click Ok to continue!",
                        icon: "success",
                        buttons: true,
                        dangerMode: true,
                    }, function succes(isDone) {
                        $('#divPasscode2').hide();
                        $('#divmain_content').show();
                    })

                }
                else {

                    swal(
                        'Error!',
                        response.Message,
                        'error'
                    ).catch(swal.noop);
                    // window.location.href = "Account/Index";
                }

            }
        });
    });

});


function getBankList() {
    $('.loader').show();
    var formData = new FormData();
    formData.append("request", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/ExpressWalletServices/GetExpressBankList/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            let ddOptions = '<option value="">Please select bank</option>'
            $.each(response.expressBankList.banks, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.code + '">' + value.name + '</option>';
            });
            $('#ddlbankList').append(ddOptions);
            $('.loader').hide();
        }
    });

}

function getAddedBankList() {
    $('.loader').show();
    var formData = new FormData();
    formData.append("guid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetAddedBankList/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            BankList = response.addedBanListResponses;
            let ddOptions = '<option value="">Please select bank</option>'
            $.each(response.addedBanListResponses, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.BankCode + '" data-operator = "' + value.BankName + '">' + value.BankName + '</option>';
            });
            $('#ddlbankList').append(ddOptions);
            $('.loader').hide();
        }
    });

}