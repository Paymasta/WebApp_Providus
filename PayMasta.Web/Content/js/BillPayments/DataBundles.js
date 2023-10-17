var UserGuid = "";
var currentPageNumber = 1;
var ProductFee = '';
var ProductCommision = '';
var ProductTotAmount = '';
var productcode = "";
var BankCode = "";
var ServiceName = "";
var OperatorId = "";
var productCode = "";
var Code = "";
var productname = "";


var txtMobileNumberAirtimeError = true;
var txtAmountError = true;
var ServiceNameError = true;
var ProductError = true;
//$.ajaxSetup({ async: false });
var accountNumberData = "";
var serviceTypeData = 0;
var walletTransIdData = 0;
let IsServiceData = false;
$(document).ready(function () {
    $('.loader').hide();
    UserGuid = sessionStorage.getItem("User1");

    var dataBundle = getUrlVarsData();
    if (dataBundle) {
        getDataBundleOperator();
        $("#dataBundleDiv").trigger("click");
       
      
        $("#dataBundleMobileNumber").val(accountNumberData);
    }

    $(document).on('click', '#dataBundleDiv', function () {
        getDataBundleOperator();
    });

    $('#ddlDataBundleOperator').change(function () {
        var OperatorBankCode = $('select#ddlDataBundleOperator option:selected').val();
        ServiceName = $('select#ddlDataBundleOperator option:selected').text();
        OperatorId = $('select#ddlDataBundleOperator option:selected').attr('data-operator');
        console.log(OperatorId);
        if (ServiceName != "Select Operator") {

            var formData = new FormData();
            formData.append("service", ServiceName);
            formData.append("SubCategoryId", 5);
            formData.append("UserGuid", UserGuid);
            $.ajax({
                type: "POST",
                cache: false,
                url: "DataPaybills/GetDataOperatorPlanList",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    $('#ddlDataBundleProduct').empty();
                   // productCode = response.planListResponse.data.productCode;
                    let ddOptions = "";//'<option value="">Please select country</option>'
                    ddOptions += '<option value="">Select Product</option>';
                    $.each(response.dataResponse.data, function (index, value) {
                        // APPEND OR INSERT DATA TO SELECT ELEMENT.
                        ddOptions += '<option value="' + value.bundle + '" data-amount ="' + value.price + '" >' + value.bundle + ' ' + value .validity+ '(₦' + value.price + ')';
                      //  ddOptions += '(';
                       // if (value.amount != '') { ddOptions += ' Amount : ' + value.amount; }
                      //  ddOptions += ')';
                        ddOptions += '</option>';
                    });
                    $('#ddlDataBundleProduct').append(ddOptions);
                }
            });
        }
    });

    $('#ddlDataBundleProduct').change(function () {
        Code = $('select#ddlDataBundleProduct option:selected').val();
        // ServiceName = $('select#ddlDataBundleOperator option:selected').text();
        var amount = $('select#ddlDataBundleProduct option:selected').attr('data-amount');
        $("#dataBundleAmount").val(amount);
        if (amount > 0) {
            //var commissionAmount = parseFloat(amount) + 107.5;
            //$('#DataBundleAmountError').text('Fees : ' + 107.5+ '  Total Amount :  ' + commissionAmount);
        }
        else {
           // $('#DataBundleAmountError').text('');
        }
    });

    $("#dataBundleAmount").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;

        if (val > 0) {
            //var commissionAmount = parseFloat(val) +107.5;
            //$('#DataBundleAmountError').text('Fees : ' + 107 + '  Commision :  ' + 7.5 + '  Total Amount :  ' + commissionAmount);
        }
        else {
            // $('#DataBundleAmountError').text('');
            return false;
        }
        if (!valid) {
            console.log("Invalid input!");
            this.value = val.substring(0, val.length - 1);
        }
    });

    $("#btnDataBundlePay").click(function () {
      
        $("#dataBundleModal").modal('hide');
        var txtMobileNumberAirtime = $("#dataBundleMobileNumber").val();
        var txtAmount = $("#dataBundleAmount").val();

        if (txtMobileNumberAirtime == null || txtMobileNumberAirtime == "" || txtMobileNumberAirtime == '') {
            txtMobileNumberAirtimeError = false;
            $("#DataBundleMobileError").show();
            $("#DataBundleMobileError").text("Please enter mobile number.");
        } else {
            txtMobileNumberAirtimeError = true;
            $("#DataBundleMobileError").hide();
        }
        if (txtAmount == null || txtAmount == "" || txtAmount == '') {
            txtAmountError = false;
            $("#DataBundleAmountError").show();
            $("#DataBundleAmountError").text("Please enter amount.");
        } else {
            txtAmountError = true;
            $("#DataBundleAmountError").hide();
        }
        if (ServiceName == null || ServiceName == "" || ServiceName == '' || ServiceName == "Select Operator") {
            ServiceNameError = false;
            $("#DataBundleOperatorError").show();
            $("#DataBundleOperatorError").text("Please select operator.");
        } else {
            ServiceNameError = true;
            $("#DataBundleOperatorError").hide();
        }
        if (Code == null || Code == "" || Code == '') {
            ProductError = false;
            $("#DataBundleProductError").show();
            $("#DataBundleProductError").text("Please select product.");
        } else {
            ProductError = true;
            $("#DataBundleProductError").hide();
        }
        //  var validateData = AirtimeValidation(txtMobileNumberAirtime, txtAmount, billerName, billerCode, 'DataBundle');
        if (txtMobileNumberAirtimeError && txtAmountError && ServiceNameError && ProductError) {
            $('.loader').show();
            $("#btnDataBundlePay").prop('disabled', true);
            var formData = new FormData();
            formData.append("Service", ServiceName);
            formData.append("UserGuid", UserGuid);
            formData.append("SubCategoryId", 5);
            formData.append("AccountType", "");
            formData.append("phone", txtMobileNumberAirtime);
            formData.append("Amount", txtAmount);
            formData.append("SmartCardCode", txtMobileNumberAirtime);
            formData.append("redeemBonus", true);
            formData.append("bonusAmount", 0);
            formData.append("code", Code);
            formData.append("productCode", Code);
            $.ajax({
                type: "POST",
                cache: false,
                url: "DataPaybills/DataRechargePayment",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $("#btnDataBundlePay").prop('disabled', false);
                    $('.loader').hide();
                    $(".modal").modal('hide');
                    if (response.RstKey == 1) {
                        $("#airtimeModal").modal('hide');


                        swal({
                            title: "Success!",
                            text: "Payment done successfully!",
                            icon: "success",
                            button: "Aww yiss!",
                        }, function succes(isDone) { location.reload(); });
                    }
                    else if (response.RstKey == 2) {
                        swal(
                            'Error!',
                            "Failed",
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 4) {
                        swal(
                            'Error!',
                            "Failed",
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 5) {
                        swal(
                            'Error!',
                            "Failed",
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 6) {
                        swal(
                            'Error!',
                            "Failed",
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 7) {
                        swal(
                            'Error!',
                            "Insufficient funds",
                            'error'
                        ).catch(swal.noop);
                    } else if (response.RstKey == 8) {
                        swal(
                            'Error!',
                            "Amount should be greater then 100 Naira",
                            'error'
                        ).catch(swal.noop);
                    }
                    else {
                        swal(
                            'Error!',
                            "Insufficient funds",
                            'error'
                        ).catch(swal.noop);
                    }
                }
            });
            $("#btnDataBundlePay").prop('disabled', false);
        }

    });

});


function getDataBundleOperator() {
    $('.loader').show();
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "DataPaybills/GetDataOperatorList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            let ddOptions = "";//'<option value="">Please select country</option>'
            $.each(response.operatorResponse, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.BillerName + '" data-operator = "' + value.BankCode + '">' + value.ServiceName + '</option>';
            });
            $('#ddlDataBundleOperator').append(ddOptions);
            $('.loader').hide();
        }
    });

}


function getUrlVarsData() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        if (hash[0] === "Id") {
            walletTransIdData = hash[1];
        }
        if (hash[0] === "AccountNumber") {
            accountNumberData = hash[1];
        }
        if (hash[0] === "Type") {
            serviceTypeData = hash[1];
        }
       
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    if (serviceTypeData == "5") {
        IsServiceData = true;
      
    }


    return IsServiceData;
}