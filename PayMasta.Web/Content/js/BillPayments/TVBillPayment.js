var UserGuid = "";
var servicename = "";
var OperatorId = "";
var OperatorBankCode = "";
var productCode = "";
var amount = "";
var subsctiptionType = 1;

var txtMobileNumberAirtimeError = true;
var txtAmountError = true;
var ServiceNameError = true;
var ProductError = true;
var accountNumberTV = "";
var serviceTypeTV = 0;
var walletTransIdTV = 0;
let IsServiceTV = false;
$(document).ready(function () {
    $('.loader').hide();
    //$('#divMain').hide();
    UserGuid = sessionStorage.getItem("User1");

    var dataTv = getUrlVarsTv();
    if (dataTv) {
        getTVOperator();
        $("#cableTvDiv").trigger("click");


        $("#cableTvSmartCardNumber").val(accountNumberTV);
    }

    $(document).on('click', '#cableTvDiv', function () {
        getTVOperator();
    });


    $('#ddlTVOperator').change(function () {

        OperatorBankCode = $('select#ddlTVOperator option:selected').val();
        servicename = $('select#ddlTVOperator option:selected').text();
        OperatorId = $('select#ddlTVOperator option:selected').attr('data-operator');
        /*if (servicename != "Select Operator" && servicename.toLowerCase() != "startimes".toLowerCase()) {*/
        $('.loader').show();
        $('#divTVProduct1').show();
        $('#cableTvAmount').attr('readonly', true);
        var formData = new FormData();
        //formData.append("Service", OperatorBankCode);
        //formData.append("UserGuid", UserGuid);
        //formData.append("SubCategoryId", 3);
        formData.append("product", OperatorId);
        $.ajax({
            type: "POST",
            cache: false,
            url: "ZealvendBillsPay/GetPayTvProductList",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                $('.loader').hide();
                $('#ddlTVProduct1').empty();
                let ddOptions = "";//'<option value="">Please select country</option>'
                ddOptions += '<option value="">Select Product</option>';
                $.each(response.Data.Products, function (index, value) {
                    ddOptions += '<option value="' + value.Code + '" data-amount = "' + value.Amount + '">' + value.Name + ":" + "₦ " + value.Amount + '</option>';
                });
                $('#ddlTVProduct1').append(ddOptions);
                // console.log(ddOptions);

                if (parseInt(subsctiptionType) === 2) {
                    $('#divTVProduct1').hide();
                }
            }
        });
        //}
        //else if (servicename.toLowerCase() === "startimes".toLowerCase()) {
        //    $('#divTVProduct1').hide();
        //    $('#cableTvAmount').attr('readonly', false);
        //}
    });

    $('#ddlTVProduct1').change(function () {
        productCode = $('select#ddlTVProduct1 option:selected').val();
        amount = $('select#ddlTVProduct1 option:selected').attr('data-amount');
        $("#cableTvAmount").val(amount);
        //var amount = $('select#ddlTVProduct1 option:selected').attr('data-amount');
        //ProductFee = $('select#ddlTVProduct1 option:selected').attr('data-fee');
        //ProductCommision = $('select#ddlTVProduct1 option:selected').attr('data-commision');
        //var commissionAmount = parseFloat(amount) * parseFloat(ProductCommision) / 100;
        //ProductTotAmount = parseFloat(commissionAmount) + parseFloat(ProductFee) + parseFloat(amount);
        //if (amount > 0) {
        //    $("#cableTvAmount").val(amount);
        //    $("#cableTvAmount").prop('disabled', true);
        //    $('#CableTvAmountError').text('Fees : ' + ProductFee + '  Commision :  ' + ProductCommision + '  Total Amount :  ' + ProductTotAmount);

        //}
        //else {
        //    $("#cableTvAmount").prop('disabled', false);
        //}
        if (amount > 0) {
            var commissionAmount = parseFloat(amount) + 107.5;
            $('#CableTvAmountError').text('Fees : ' + 107.5 + '  Total Amount :  ' + commissionAmount);
        }
        else {
            $('#CableTvAmountError').text('');
        }
    });

    $('#ddlSubsctiptionType').change(function () {
        subsctiptionType = $('select#ddlSubsctiptionType option:selected').val();
        $('#cableTvAmount').val('');
        $('#CableTvAmountError').text('');

        if (parseInt(subsctiptionType) === 1) {
            $('#divTVProduct1').show();
            $('#cableTvAmount').attr('readonly', true);
        }

        if (parseInt(subsctiptionType) === 2) {
            $('#divTVProduct1').hide();
            $('#cableTvAmount').attr('readonly', false);
        }
    });

    $('#cableTvAmount').change(function () {
        debugger
        if (parseInt(subsctiptionType) === 2) {
            var amount = $("#cableTvAmount").val();
            if (amount > 0) {
                var commissionAmount = parseFloat(amount) + 107.5;
                $('#CableTvAmountError').text('Fees : ' + 107.5 + '  Total Amount :  ' + commissionAmount);
            }
            else {
                $('#CableTvAmountError').text('');
            }
        }
    });

    $("#btnCableTvPay").click(function () {
        debugger
      
        $("#cableTvModal").modal('hide');
        var txtMobileNumberAirtime = $("#cableTvSmartCardNumber").val();
        var txtAmount = $("#cableTvAmount").val();
        if (servicename.toLowerCase() == "startimes".toLowerCase()) {

            if (txtMobileNumberAirtime == null || txtMobileNumberAirtime == "" || txtMobileNumberAirtime == '') {
                txtMobileNumberAirtimeError = false;
                $("#CableTvMobileError").show();
                $("#CableTvMobileError").text("Please enter smart card number.");
            } else {
                txtMobileNumberAirtimeError = true;
                $("#CableTvMobileError").hide();
            }
            if (txtAmount == null || txtAmount == "" || txtAmount == '') {
                txtAmountError = false;
                $("#CableTvAmountError").show();
                $("#CableTvAmountError").text("Please enter amount.");
            } else {
                txtAmountError = true;
                $("#CableTvAmountError").hide();
            }
            if (servicename == null || servicename == "" || servicename == '' || servicename == "Select Operator") {
                ServiceNameError = false;
                $("#CableTvOperatorError").show();
                $("#CableTvOperatorError").text("Please select operator.");
            } else {
                ServiceNameError = true;
                $("#CableTvOperatorError").hide();
            }
            //if (productcode == null || productcode == "" || productcode == '') {
            //    ProductError = false;
            //    $("#InternetProductError").show();
            //    $("#InternetProductError").text("Please select product.");
            //} else {
            //    ProductError = true;
            //    $("#InternetProductError").hide();
            //}
        } else {
            if (txtMobileNumberAirtime == null || txtMobileNumberAirtime == "" || txtMobileNumberAirtime == '') {
                txtMobileNumberAirtimeError = false;
                $("#CableTvMobileError").show();
                $("#CableTvMobileError").text("Please enter smart card number.");
            } else {
                txtMobileNumberAirtimeError = true;
                $("#CableTvMobileError").hide();
            }
            if (txtAmount == null || txtAmount == "" || txtAmount == '') {
                txtAmountError = false;
                $("#CableTvAmountError").show();
                $("#CableTvAmountError").text("Please enter amount.");
            } else {
                txtAmountError = true;
                $("#CableTvAmountError").hide();
            }
            if (servicename == null || servicename == "" || servicename == '' || servicename == "Select Operator") {
                ServiceNameError = false;
                $("#CableTvOperatorError").show();
                $("#CableTvOperatorError").text("Please select operator.");
            } else {
                ServiceNameError = true;
                $("#CableTvOperatorError").hide();
            }
            if (productCode == null || productCode == "" || productCode == '') {

                if (parseInt(subsctiptionType) === 2) {
                    ProductError = true;
                    $("#CableTvProductError").hide();
                } else {
                    ProductError = false;
                    $("#CableTvProductError").show();
                    $("#CableTvProductError").text("Please select product.");
                }

            } else {
                ProductError = true;
                $("#CableTvProductError").hide();
            }
        }
        if (txtMobileNumberAirtimeError && txtAmountError && ServiceNameError && servicename.toLowerCase() == "startimes".toLowerCase()) {
            $("#btnCableTvPay").prop('disabled', true);
            $('.loader').show();
            var formData = new FormData();
            formData.append("Service", servicename);
            formData.append("UserGuid", UserGuid);
            formData.append("SubCategoryId", 3);
            formData.append("AccountType", OperatorId);
            formData.append("phone", txtMobileNumberAirtime);
            formData.append("Amount", txtAmount);
            formData.append("SmartCardCode", txtMobileNumberAirtime);
            formData.append("redeemBonus", true);
            formData.append("bonusAmount", 0);
            formData.append("code", productCode);
            formData.append("subsctiptionType", subsctiptionType);
            $.ajax({
                type: "POST",
                cache: false,
                url: "ZealvendBillsPay/PayTvVendPayment",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $("#btnCableTvPay").prop('disabled', false);
                    $('.loader').hide();
                    $(".modal").modal('hide');
                    if (response.RstKey == 1) {
                        $("#airtimeModal").modal('hide');
                        swal({
                            title: "Success!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!",
                        }, function succes(isDone) { location.reload(); });
                    }
                    else if (response.RstKey == 2) {
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 101) {
                        swal(
                            'Error!',
                            response.Message,
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
                }
            });

            $("#btnCableTvPay").prop('disabled', false);
        }
        else if (ProductError && txtMobileNumberAirtimeError && txtAmountError && ServiceNameError && servicename.toLowerCase() != "startimes".toLowerCase()) {
            var formData = new FormData();
            formData.append("Service", servicename);
            formData.append("UserGuid", UserGuid);
            formData.append("SubCategoryId", 3);
            formData.append("AccountType", OperatorId);
            formData.append("phone", txtMobileNumberAirtime);
            formData.append("Amount", txtAmount);
            formData.append("SmartCardCode", txtMobileNumberAirtime);
            formData.append("redeemBonus", true);
            formData.append("bonusAmount", 0);
            formData.append("code", productCode);
            formData.append("subsctiptionType", subsctiptionType);
            $.ajax({
                type: "POST",
                cache: false,
                url: "ZealvendBillsPay/PayTvVendPayment",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('.loader').hide();
                    $(".modal").modal('hide');
                    if (response.RstKey == 1) {
                        $("#airtimeModal").modal('hide');
                        swal({
                            title: "Success!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!",
                        }, function succes(isDone) { location.reload(); });
                    }
                    else if (response.RstKey == 2) {
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 101) {
                        swal(
                            'Error!',
                            response.Message,
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
                }
            });
        }
    });

    $("#cableTvAmount").on("keyup", function () {
        debugger
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;
        if (val > 0) {
            $('#CableTvAmountError').show();
            var commissionAmount = parseFloat(val) + 107.5;
            ProductTotAmount = parseFloat(commissionAmount);// + parseFloat(ProductFee) + parseFloat(val);
            $('#CableTvAmountError').text('Fees : ' + ProductFee + '  Commision :  ' + ProductCommision + '  Total Amount :  ' + ProductTotAmount);
        }
        else {
            $('#CableTvAmountError').text('');
        }

        if (!valid) {
            console.log("Invalid input!");
            this.value = val.substring(0, val.length - 1);
        }
    });

    $("#cableTvSmartCardNumber").on("keyup", function () {
        debugger
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;
        if (val.length >= 10) {
            // if (txtMobileNumberAirtimeError && txtAmountError && ServiceNameError && servicename.toLowerCase() == "startimes".toLowerCase()) {
            $('.loader').show();
            var formData = new FormData();
            formData.append("Product", OperatorId);
            formData.append("CardNumber", val);
            
            $.ajax({
                type: "POST",
                cache: false,
                url: "ZealvendBillsPay/VerifyPayTv",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('.loader').hide();
                    if (response.RstKey == 1) {
                        $("#ownerName").text(response.Data.Data.Name)
                    }
                }
            });
            // }
        }

    });

    $("#btnVerifyPasscode").click(function () {

        var txtPasscode = $("#txtPasscode").val();

        if (txtPasscode.length < 4) {
            $('#txtPasscodeError').text('Please enter 4 digit Passcode');
            return;
        } else {
            $('#txtPasscodeError').text('');
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
                        $('#divPasscode').hide();
                        $('#divMain').show();
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

function getTVOperator() {
    $('.loader').show();
    var id = 1;
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    $.ajax({
        type: "POST",
        //cache: false,
        url: "/BillAndPayment/GetTVOperatorList/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#ddlTVOperator').empty();
            console.log(response.operatorResponse);

            let ddOptions = '<option value="">Select Operator</option>'
            $.each(response.operatorResponse, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.ServiceName + '" data-operator = "' + value.AccountType + '">' + value.ServiceName + '</option>';
            });
            $('#ddlTVOperator').append(ddOptions);
            $('.loader').hide();
        }
    });

}

function getUrlVarsTv() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        if (hash[0] === "Id") {
            walletTransIdTV = hash[1];
        }
        if (hash[0] === "AccountNumber") {
            accountNumberTV = hash[1];
        }
        if (hash[0] === "Type") {
            serviceTypeTV = hash[1];
        }

        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    if (serviceTypeTV == "3") {
        IsServiceTV = true;

    }


    return IsServiceTV;
}