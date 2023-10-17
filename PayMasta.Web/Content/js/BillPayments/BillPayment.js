var UserGuid = "";
var currentPageNumber = 1;
var ProductFee = '';
var ProductCommision = '';
var ProductTotAmount = '';
var productcode = "";
//$.ajaxSetup({ async: false });
$(document).ready(function () {
    $('.loader').hide();

    var BankCode = "";
    var ServiceName = "";
    var OperatorId = "";
    UserGuid = sessionStorage.getItem("User1");

    $(document).on('click', '#airtimeDiv', function () {
        getAirtimeOperator();
    });
   
   
   
   

    $('#ddlAirtimeOperator').change(function () {
        BankCode = $('select#ddlAirtimeOperator option:selected').val();
        ServiceName = $('select#ddlAirtimeOperator option:selected').text();
        OperatorId = $('select#ddlAirtimeOperator option:selected').attr('data-operator');
        console.log(OperatorId);
        var amount = $('select#ddlAirtimeOperator option:selected').attr('data-amount');
        if (amount > 0) {
            $("#txtAmount").val(amount);
            $("#txtAmount").prop('disabled', true);
        }
        else {
            $("#txtAmount").prop('disabled', false);
        }

    });


    //$("#divShowTvModal").click(function () {
    //    alert();
    //    $("#cableTvModal").modal('show');
    //});

    //$("#divShowTvModal").click(function () {
    //    //getTVOperator();
    //    $("#cableTvModal").modal('show');

    //});



   

  

   

   

  

    $("#btnAirtimePay").click(function () {
        debugger
       
        var txtMobileNumberAirtime = $("#txtMobileNumberAirtime").val();
        var txtAmount = $("#txtAmount").val();
        var billerCode = BankCode;
        var billerName = ServiceName;
        var validateData = AirtimeValidation(txtMobileNumberAirtime, txtAmount, billerName, billerCode, 'Airtime');

        if (validateData) {
            $("#btnAirtimePay").prop('disabled', true);
            $('.loader').show();
            var formData = new FormData();
            formData.append("Service", billerCode);
            formData.append("UserGuid", UserGuid);
            formData.append("SubCategoryId", 1);
            formData.append("AccountType", "");
            formData.append("phone", txtMobileNumberAirtime);
            formData.append("Amount", txtAmount);
            formData.append("SmartCardCode", "");
            formData.append("redeemBonus", true);
            formData.append("bonusAmount", 1.0);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/ZealvendBillsPay/AirtimePayment/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $("#btnAirtimePay").prop('disabled', false);
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
                            "Insuficient fund in your wallet",
                            'error'
                        ).catch(swal.noop);
                    }
                }
            });

            $("#btnAirtimePay").prop('disabled', false);
        }

    });

  

   
    $('#BtnCloseAirtime').on('click', function () {
        $('#AirtimeOperatorError').text('');
        $('#AirtimeMobileError').text('');
        $('#AirtimeProductError').text('');
        $('#AirtimeAmountError').text('');
        $("select#ddlAirtimeOperator").prop('selectedIndex', 0);
        $('#txtMobileNumberAirtime').val('');
        $('#txtAmount').val('');
        BankCode = ''
        ServiceName = '';
        $("#txtAmount").attr('disabled', true);
    });
    $('#BtnCloseInternet').on('click', function () {
        $('#InternetOperatorError').text('');
        $('#InternetMobileError').text('');
        $('#InternetProductError').text('');
        $('#InternetAmountError').text('');
        BankCode = ''
        ServiceName = '';
        $("select#ddlInternetOperator").prop('selectedIndex', 0);
        $("select#ddlInternetProduct").prop('selectedIndex', 0);
        $('#txtMobileNumberInternet').val('');
        $('#InternetAmount').val('');
        $("#InternetAmount").prop('disabled', false);
        ProductFee = '';
        ProductCommision = '';
        ProductTotAmount = '';
    });
    $('#BtnCloseCableTv').on('click', function () {
        $('#CableTvOperatorError').text('');
        $('#CableTvMobileError').text('');
        $('#CableTvProductError').text('');
        $('#CableTvAmountError').text('');
        BankCode = ''
        ServiceName = '';
        $("select#ddlTVOperator").prop('selectedIndex', 0);
        $("select#ddlTVProduct").prop('selectedIndex', 0);
        $('#cableTvSmartCardNumber').val('');
        $('#cableTvAmount').val('');
        $("#cableTvAmount").prop('disabled', false);
        ProductFee = '';
        ProductCommision = '';
        ProductTotAmount = '';
    });
    $('#BtnCloseElectricity').on('click', function () {
        $('#ElectricityOperatorError').text('');
        $('#ElectricityMobileError').text('');
        $('#ElectricityProductError').text('');
        $('#ElectricityAmountError').text('');
        BankCode = ''
        ServiceName = '';
        $("select#ddlElectricityOperator").prop('selectedIndex', 0);
        $("select#ddlElectricityProduct").prop('selectedIndex', 0);
        $('#electricityMeterNumber').val('');
        $('#electricityAmount').val('');
        $("#electricityAmount").prop('disabled', false);
        ProductFee = '';
        ProductCommision = '';
        ProductTotAmount = '';
    });
    $('#BtnCloseDataBundle').on('click', function () {
        $('#DataBundleOperatorError').text('');
        $('#DataBundleMobileError').text('');
        $('#DataBundleProductError').text('');
        $('#DataBundleAmountError').text('');
        BankCode = ''
        ServiceName = '';
        $("select#ddlDataBundleOperator").prop('selectedIndex', 0);
        $("select#ddlDataBundleProduct").prop('selectedIndex', 0);
        $('#dataBundleMobileNumber').val('');
        $('#dataBundleAmount').val('');
        $("#dataBundleAmount").prop('disabled', false);
        ProductFee = '';
        ProductCommision = '';
        ProductTotAmount = '';
    });

    $('#txtMobileNumberAirtime').on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
    $("#txtAmount").on("keyup", function () {
        var valid = /^\d{0,6}(\.\d{0,2})?$/.test(this.value),
            val = this.value;

        if (!valid) {
            console.log("Invalid input!");
            this.value = val.substring(0, val.length - 1);
        }
    });
    $('#txtMobileNumberInternet').on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
  
    $('#cableTvSmartCardNumber').on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
   
    $('#electricityMeterNumber').on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
   
    $('#dataBundleMobileNumber').on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
  
    //----------------------------------------------
    //var currentscrollHeight = 0;
    //var count = 0;

    //jQuery(document).ready(function ($) {
    //    for (var i = 0; i < 8; i++) {
    //        callData(count);//Call 8 times on page load
    //        count++;
    //    }
    //});

    //$(window).on("scroll", function () {
    //    const scrollHeight = $(document).height();
    //    const scrollPos = Math.floor($(window).height() + $(window).scrollTop());
    //    const isBottom = scrollHeight - 100 < scrollPos;

    //    if (isBottom && currentscrollHeight < scrollHeight) {
    //        //alert('calling...');
    //        for (var i = 0; i < 6; i++) {
    //            callData(count);//Once at bottom of page -> call 6 times
    //            count++;
    //        }
    //        currentscrollHeight = scrollHeight;
    //    }
    //});

    //----------------------------------------------

});


function getAirtimeOperator() {
    $('.loader').show();
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "BillAndPayment/GetAirtimeOperatorList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            let ddOptions = "";//'<option value="">Please select country</option>'
            $.each(response.operatorResponse, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.ServiceName + '" data-amount="' + value.ServiceName + '" data-operator = "' + value.ServiceName + '">' + value.ServiceName + '</option>';
            });
            $('#ddlAirtimeOperator').append(ddOptions);
            $('.loader').hide();

        }
    });

}




function validateMobile(mobile) {
    var txtMobile = mobile;
    var mobileFormateError;
    var phoneno = /^\d{11}$/;
    if (mobile.indexOf('0') == 0 && mobile.length < 10) {
        mobileFormateError = 1;
    }
    else if (mobile.indexOf('0') == 0 && mobile.length == 11) {
        mobileFormateError = 2;

    }
    else if (mobile.indexOf('0') != 0 && mobile.length == 10) {
        mobileFormateError = 2;

    }
    else {
        mobileFormateError = 0;
    }
    /*if (txtMobile.length >= 9 && txtMobile.length <= 11 && txtMobile.match(phoneno)) {
        mobileFormateError = true;
    } else {
        mobileFormateError = false;
    }*/
    return mobileFormateError;
}

function validateCard(mobile) {
    var txtMobile = mobile;
    var mobileFormateError;
    var phoneno = /^\d{11}$/;
    if (txtMobile.length == 11 && txtMobile.match(phoneno)) {
        mobileFormateError = true;
    } else {
        mobileFormateError = false;
    }
    return mobileFormateError;
}

function AirtimeValidation(mobile, amount, billername, billType, param) {
    flag = true;
    $('#' + param + 'OperatorError').text('');
    $('#' + param + 'MobileError').text('');
    $('#' + param + 'ProductError').text('');
    $('#' + param + 'AmountError').text('');
    console.log('validate' + param);
    if (billername == '' || billername == "" || billername == null) {
        $('#' + param + 'OperatorError').text('Operator is required');
        flag = false;
    }
    if (billType == '' || billType == "" || billType == null) {
        $('#' + param + 'ProductError').text('Product is required');
        flag = false;
    }

    if (mobile == '' || mobile == "" || mobile == null) {
        if (param == 'CableTv') {
            $('#' + param + 'MobileError').text('Smart Card Number is required');
        }
        else if (param == 'Electricity') {
            $('#' + param + 'MobileError').text('Meter Number is required');
        }
        else {
            $('#' + param + 'MobileError').text('Mobile Number is required');
        }
        flag = false;

    }
    if (mobile != '' || mobile != "" || mobile != null) {
        if (param == 'CableTv') {
            card_validation = validateCard(mobile)
            if (card_validation) {
                $('#' + param + 'MobileError').text('');
            }
            else {
                $('#' + param + 'MobileError').text('Please put 11 digit smart card number');
                flag = false;
            }
        }
        else if (param == 'Electricity') {
            card_validation = validateCard(mobile)
            if (card_validation) {
                $('#' + param + 'MobileError').text('');
                flag = true;
            }
            else {
                $('#' + param + 'MobileError').text('Please put 11 digit meter number');
                flag = false;
            }
        }
        else {
            mobile_validation = validateMobile(mobile)
            console.log(mobile_validation);
            if (mobile_validation == 1) {
                $('#' + param + 'MobileError').text('Please enter 11 digit number with zero');
                flag = false;
            }
            else if (mobile_validation == 0) {
                $('#' + param + 'MobileError').text('Please enter 10 digit number');
                flag = false;
            }
            else if (mobile_validation == 2) {
                $('#' + param + 'MobileError').text('');

            }
            /*if (mobile_validation) {
                $('#' + param + 'MobileError').text('');
            }
            else {
                $('#' + param + 'MobileError').text('Please put  10  digit mobile number');
                flag = false;
            }*/
        }

    }

    if (amount == '' || amount == "" || amount == null) {
        $('#' + param + 'AmountError').text('Amount is required');
        flag = false;
    }
    if (amount != '' || amount != "" || amount != null) {
        if (amount <= 0) {
            $('#' + param + 'AmountError').text('Amount should be more than 0');
            flag = false;
        }
        else {
            $('#' + param + 'AmountError').text('');

        }

    }
    return flag;

}
