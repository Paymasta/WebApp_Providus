var UserGuid = "";
var ServiceName = "";
var ServiceNameText = "";
var BankCode = "";
var AccountType = "";

var txtMobileNumberAirtimeError = true;
var txtAmountError = true;
var ServiceNameError = true;
var ProductError = true;
var txtmeterNumberError = true;
//$.ajaxSetup({ async: false });
var accountNumber = "";
var serviceType = 0;
var walletTransId = 0;
let IsService = false;
var meterNumber = "";
//$.ajaxSetup({ async: false });
$(document).ready(function () {
    $('.loader').hide();
    $('#meterVerifiyDiv').hide();

    var data = getUrlVars();
    if (data) {
        getElectricityOperator();
        $("#electricityDiv").trigger("click");

        $("#electricityMobileNumber").val(accountNumber);
        $("#electricityMeterNumber").val(meterNumber);
    }
    $(document).on('click', '#electricityDiv', function () {
        getElectricityOperator();
    });


    $('#ddlElectricityOperator').change(function () {
        ServiceName = $('select#ddlElectricityOperator option:selected').val();
        ServiceNameText = $('select#ddlElectricityOperator option:selected').text();
        AccountType = $('select#ddlElectricityOperator option:selected').attr('data-operator');

        //if (ServiceName != null) {
        //    ("#ElectricityCommissionAmount").text('')

        //}

    });

    $("#btnElectricityPay").click(function () {
      
        $("#electricityModal").modal('hide');
        var txtmeterNumber = $("#electricityMeterNumber").val();
        var txtMobileNumberAirtime = $("#electricityMobileNumber").val();
        var txtAmount = $("#electricityAmount").val();

        if (txtMobileNumberAirtime == null || txtMobileNumberAirtime == "" || txtMobileNumberAirtime == '') {
            txtMobileNumberAirtimeError = false;
            $("#ElectricityProductError").show();
            $("#ElectricityProductError").text("Please enter mobile number.");
        } else {
            txtMobileNumberAirtimeError = true;
            $("#ElectricityProductError").hide();
        }
        if (txtAmount == null || txtAmount == "" || txtAmount == '') {
            txtAmountError = false;
            $("#ElectricityAmountError").show();
            $("#ElectricityAmountError").text("Please enter amount.");
        } else {
            txtAmountError = true;
            $("#ElectricityAmountError").hide();
        }
        if (ServiceNameText == null || ServiceNameText == "" || ServiceNameText == '' || ServiceNameText == "Select Operator") {
            ServiceNameError = false;
            $("#ElectricityOperatorError").show();
            $("#ElectricityOperatorError").text("Please select operator.");
        } else {
            ServiceNameError = true;
            $("#ElectricityOperatorError").hide();
        }
        if (txtmeterNumber == null || txtmeterNumber == "" || txtmeterNumber == '') {
            txtmeterNumberError = false;
            $("#ElectricityMobileError").show();
            $("#ElectricityMobileError").text("Please enter meter number.");
        } else {
            txtmeterNumberError = true;
            $("#ElectricityMobileError").hide();
        }
        //  var validateData = AirtimeValidation(txtMobileNumberAirtime, txtAmount, billerName, billerCode, 'Electricity');
        if (txtMobileNumberAirtimeError && txtAmountError && ServiceNameError && txtmeterNumberError) {
            $('.loader').show();
            var formData = new FormData();
            formData.append("Service", ServiceName);
            formData.append("UserGuid", UserGuid);
            formData.append("SubCategoryId", 4);
            formData.append("AccountType", AccountType);
            formData.append("phone", txtMobileNumberAirtime);
            formData.append("Amount", txtAmount);
            formData.append("meterNo", txtmeterNumber);
            formData.append("redeemBonus", true);
            formData.append("bonusAmount", 0);
            formData.append("code", "");
            $.ajax({
                type: "POST",
                cache: false,
                url: "ElectricityBillPayment/ElectricityRechargePayment",
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
                            text: response.electricityPurchaseResponse.message,
                            icon: "success",
                            button: "Aww yiss!",
                        }, function succes(isDone) { location.reload(); });
                    }
                    else if (response.RstKey == 2) {
                        swal(
                            'Error!',
                            response.electricityPurchaseResponse.message,
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 99) {
                        swal(
                            'Error!',
                            "Please enter valid mobile number",
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 992) {
                        swal(
                            'Error!',
                            "The meter does not exists, please input the correct meter number",
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 991) {
                        swal(
                            'Error!',
                            "insufficient funds",
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


    $("#electricityAmount").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;
        if (val > 0) {
            var commissionAmount = parseFloat(val) + 107.5;
            $('#ElectricityCommissionAmount').text('Fees : ' + 107.5 + '  Total Amount :  ' + commissionAmount);
        }
        else {
            $('#ElectricityCommissionAmount').text('');
        }
        if (!valid) {
            console.log("Invalid input!");
            this.value = val.substring(0, val.length - 1);
        }
    });

    $("#electricityMeterNumber").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;
        if (val.length > 9) {
            $('.loader').show();
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("meterNo", val);
            formData.append("disco", ServiceName);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/ElectricityBillPayment/MeterVerify/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    if (response.data.error == false) {
                        $('#meterVerifiyDiv').show();
                        $('.loader').hide();
                        $("#txtZeavCustomerMeterName").text(response.data.name + " (" + response.data.vendType + ")");
                        $("#txtCustomerMeterAddress").text(response.data.address);
                        $("#txtCustomerMeterOutstanding").text("Outstanding:₦" + response.data.outstanding);
                        $("#txtCustomerMeterRepayment").text("Repayment:₦" + response.data.debtRepayment);
                    }

                }
            });
        }

    });
});

function getElectricityOperator() {
    $('.loader').show();
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/BillAndPayment/GetElectricityOperatorList/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            debugger
            let ddOptions = "";//'<option value="">Please select country</option>'
            $.each(response.operatorResponse, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.BillerName + '" data-operator = "' + value.AccountType + '">' + value.ServiceName + '</option>';
            });
            $('#ddlElectricityOperator').append(ddOptions);
            $('.loader').hide();
        }
    });

}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        if (hash[0] === "Id") {
            walletTransId = hash[1];
        }
        if (hash[0] === "AccountNumber") {
            accountNumber = hash[1];
        }
        if (hash[0] === "Type") {
            serviceType = hash[1];
        }
        if (hash[0] === "meterNumber") {
            meterNumber = hash[1];
        }
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    if (serviceType == "4") {
        IsService = true;
    }


    return IsService;
}