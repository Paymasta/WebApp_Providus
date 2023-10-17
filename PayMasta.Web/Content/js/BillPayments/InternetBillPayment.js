var UserGuid = "";
var ProductFee = '';
var ProductCommision = '';
var ProductTotAmount = '';
var productcode = "";
var txtMobileNumberAirtimeError = true;
var txtAmountError = true;
var ServiceNameError = true;
var ProductError = true;
$(document).ready(function () {

    UserGuid = sessionStorage.getItem("User1");

    $(document).on('click', '#InternetDiv', function () {
        getInternetOperator(UserGuid);
    });

    $('#ddlInternetOperator').change(function () {
        var OperatorBankCode = $('select#ddlInternetOperator option:selected').val();
        ServiceName = $('select#ddlInternetOperator option:selected').text();
        OperatorId = $('select#ddlInternetOperator option:selected').attr('data-operator');
        console.log(OperatorId);
        if (ServiceName != "Select Operator") {
            var formData = new FormData();
            formData.append("Service", ServiceName);
            formData.append("UserGuid", UserGuid);
            formData.append("SubCategoryId", 2);
            formData.append("AccountType", OperatorId);
            formData.append("Account", OperatorBankCode);
            formData.append("Amount", 0);
            formData.append("SmartCardCode", "");
            $.ajax({
                type: "POST",
                cache: false,
                url: "BillAndPayment/GetInternetBundles",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    $('#ddlInternetProduct').empty();
                    let ddOptions = "";//'<option value="">Please select country</option>'
                    ddOptions += '<option value="">Select Product</option>';
                    $.each(response.internetBundlesResponse.data.bundles, function (index, value) {
                        // APPEND OR INSERT DATA TO SELECT ELEMENT.
                        if (value != null) {
                            ddOptions += '<option value="' + value.code + '" data-amount="' + value.displayPrice + '" data-fee= "' + value.fee + '" data-commision="' + value.fee + '">' + value.name + '(' + value.validity + ')';
                            ddOptions += '( ';
                            if (value.displayPrice != '') { ddOptions += ' Amount : ' + value.displayPrice + ','; }
                            if (value.fee != '') { ddOptions += ' Fee :' + value.fee + ','; }
                            //if (value.Commision != '') { ddOptions += ' Commission :' + value.Commision; }
                            ddOptions += ')';
                            ddOptions += '</option>';
                        }

                    });
                    $('#ddlInternetProduct').append(ddOptions);
                }
            });
        }
    });

    $('#ddlInternetProduct').change(function () {
        debugger
        var ProductVal = $('select#ddlInternetProduct option:selected').val();//.split(',');
        var amount = $('select#ddlInternetProduct option:selected').attr('data-amount');
        productcode = $('select#ddlInternetProduct option:selected').val();
        var txtAmount = $("#InternetAmount").val(amount);
        if (amount > 0) {
            var commissionAmount = parseFloat(amount)+107.5;
            $('#InternetCommissionAmount').text('Fees : ' + 107.5+  '  Total Amount :  ' + commissionAmount);
        }
        else {
            $('#InternetCommissionAmount').text('');
        }
    });

    $("#btnInternetPay").click(function () {
        $('.loader').show();
      
        $("#internetModal").modal('hide');
        var txtMobileNumberAirtime = $("#txtMobileNumberInternet").val();
        var txtAmount = $("#InternetAmount").val();
        // var validateData = AirtimeValidation(txtMobileNumberAirtime, txtAmount, billerName, billerCode, 'Internet');
        if (txtMobileNumberAirtime == null || txtMobileNumberAirtime == "" || txtMobileNumberAirtime == '') {
            txtMobileNumberAirtimeError = false;
            $("#InternetMobileError").show();
            $("#InternetMobileError").text("Please enter mobile number.");
        } else {
            txtMobileNumberAirtimeError = true;
            $("#InternetMobileError").hide();
        }
        if (txtAmount == null || txtAmount == "" || txtAmount == '') {
            txtAmountError = false;
            $("#InternetAmountError").show();
            $("#InternetAmountError").text("Please enter amount.");
        } else {
            txtAmountError = true;
            $("#InternetAmountError").hide();
        }
        if (ServiceName == null || ServiceName == "" || ServiceName == '' || ServiceName == "Select Operator") {
            ServiceNameError = false;
            $("#InternetOperatorError").show();
            $("#InternetOperatorError").text("Please select operator.");
        } else {
            ServiceNameError = true;
            $("#InternetOperatorError").hide();
        }
        if (productcode == null || productcode == "" || productcode == '') {
            ProductError = false;
            $("#InternetProductError").show();
            $("#InternetProductError").text("Please select product.");
        } else {
            ProductError = true;
            $("#InternetProductError").hide();
        }
        if (txtMobileNumberAirtimeError && txtAmountError && ServiceNameError && ProductError) {
            $("#btnInternetPay").prop('disabled', true);
            var formData = new FormData();
            formData.append("Service", ServiceName);
            formData.append("UserGuid", UserGuid);
            formData.append("SubCategoryId", 2);
            formData.append("AccountType", OperatorId);
            formData.append("phone", txtMobileNumberAirtime);
            formData.append("Amount", txtAmount);
            formData.append("SmartCardCode", "");
            formData.append("redeemBonus", true);
            formData.append("bonusAmount", 0);
            formData.append("code", productcode);
            $.ajax({
                type: "POST",
                cache: false,
                url: "BillAndPayment/InternetRechargePayment",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $("#btnInternetPay").prop('disabled', false);
                    $('.loader').hide();
                    $(".modal").modal('hide');
                    if (response.RstKey == 1) {
                        $("#airtimeModal").modal('hide');
                        swal({
                            title: "Success!",
                            text: response.internetPurchaseResponse.message,
                            icon: "success",
                            button: "Aww yiss!",
                        }, function succes(isDone) { location.reload(); });
                    }
                    else if (response.RstKey == 2) {
                        swal(
                            'Error!',
                            response.internetPurchaseResponse.message,
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 3) {
                        swal(
                            'Error!',
                            response.internetPurchaseResponse.message,
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
        $("#btnInternetPay").prop('disabled', false);
    });

    $("#InternetAmount").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;
        if (val > 0) {
            var commissionAmount = parseFloat(val) * parseFloat(ProductCommision) / 100;
            ProductTotAmount = parseFloat(commissionAmount) + parseFloat(ProductFee) + parseFloat(val);
            $('#InternetAmountError').text('Fees : ' + ProductFee + '  Commision :  ' + ProductCommision + '  Total Amount :  ' + ProductTotAmount);
        }
        else {
            $('#InternetAmountError').text('');
        }

        if (!valid) {
            this.value = val.substring(0, val.length - 1);
        }
    });
})

function getInternetOperator() {
    $('.loader').show();
    //  var id = 1;
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "BillAndPayment/GetWifiInternetOperatorList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            let ddOptions = "";//'<option value="">Please select country</option>'
            $.each(response.operatorResponse, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.ServiceName + '" data-operator = "' + value.AccountType + '">' + value.ServiceName + '</option>';
            });
            $('#ddlInternetOperator').append(ddOptions);
            $('.loader').hide();
        }
    });

}