
var UserGuid1 = "";
let isCountrySelected = false;
let isStateSelected = false;
var state = "";
var city = "";
let addressNameError = true;
let postcodeError = true;
let ninNumberError = true;
let CountryError = true;
let StateError = true;
let CityError = true;
let verificationTypeError = true;
var country = "";
var cGuid = "";
var sGuid = "";
var verificationType;
var VerificationId;
$(document).ready(function () {
    debugger

    //$("#testssssssssss").click(function () {
    //    $("#addOtherDetailsModal").show();
    //});
   
    $("#divtxtNinNumber").hide();
    $("#divtxtDL").hide();
    $("#divtxtVC").hide();
    $("#divtxtIP").hide();
    $(".IdTypeDoc").hide();
    
    getCountries();
    $("#addOtherDetailsDiv").click(function () {
        $("#addOtherDetailsModal").modal('show');
    });
    $("#addDocDiv").click(function () {
        $("#addDocsModal").modal('show');
    });

    $('#ddlcountry').change(function () {

        if (isCountrySelected == true) {
            $('#ddlState').empty();
            $('#ddlCity').empty();
            state = '';
            city = ''
            $('#ddlCity').append('<option value="">Please select city</option>');
            $('#ddlState').append('<option value="">Please select state</option>');
        }
        isCountrySelected = true;
        var value = $('select#ddlcountry option:selected').val();
        country = $('select#ddlcountry option:selected').text();
        cGuid = value;
        getStateByCountryGuid(value);

    });


    $('#ddlState').change(function () {
        //alert("State");
        //  $('#ddlState').prop('selectedIndex', 0);
        if (isStateSelected == true) {
            $('#ddlCity').empty();
            city = ''
            $('#ddlCity').append('<option value="">Please select city</option>');
        }
        isStateSelected = true;


        var value = $('select#ddlState option:selected').val();
        state = $('select#ddlState option:selected').text();
        sGuid = value;
        getCityByState(value);

    });

    $('#ddlCity').change(function () {

        var value = $('select#ddlCity option:selected').val();
        city = $('select#ddlCity option:selected').text();

    });

    $('#ddlVerificationType').change(function () {

        verificationType = $('select#ddlVerificationType option:selected').val();
        var verificationTypeText = $('select#ddlVerificationType option:selected').text();
        if (verificationType === "DL") {
            $("#divtxtDL").show();
            $("#divtxtNinNumber").hide();
            $("#divtxtVC").hide();
            $("#divtxtIP").hide();
        } else if (verificationType === "VNIN") {
            $("#divtxtNinNumber").show();
            $("#divtxtDL").hide();
            $("#divtxtVC").hide();
            $("#divtxtIP").hide();
        } else if (verificationType === "VC") {
            $("#divtxtVC").show();
            $("#divtxtNinNumber").hide();
            $("#divtxtDL").hide();
            $("#divtxtIP").hide();

        } else if (verificationType === "IP") {
            $("#divtxtIP").show();
            $("#divtxtVC").hide();
            $("#divtxtNinNumber").hide();
            $("#divtxtDL").hide();
        }

    });

    $("#btnSaveOtherDetails").click(function () {
        UserGuid1 = sessionStorage.getItem("User1");
        var addressName = $("#txtAddress").val();
        var txtPosrCode = $("#txtPostCode").val();
        var txtNinNumber = $("#txtNinNumber").val();
        var txtDriversLicense = $("#txtDriversLicense").val();
        var txtVotersCard = $("#txtVotersCard").val();
        var txtInternationalpassport = $("#txtInternationalpassport").val();
        if (verificationType=== "DL") {
            VerificationId = txtDriversLicense;
        }
        else if (verificationType=== "VC") {
            VerificationId = txtVotersCard;
        }
        else if (verificationType=== "VNIN") {
            VerificationId = txtNinNumber;
        }
        else if (verificationType=== "IP") {
            VerificationId = txtInternationalpassport;
        }
        validateFields();
        if (addressNameError == true && postcodeError == true && ninNumberError == true && CountryError == true && StateError == true && CityError && verificationTypeError == true) {
            $("#btnSaveOtherDetails").attr('disabled', true);
            var formData = new FormData();
            formData.append("UserGuid", UserGuid1);
            //formData.append("NinNo", txtNinNumber);
            formData.append("Address", addressName);
            formData.append("State", state);
            formData.append("city", city);
            formData.append("Countryname", country);
            formData.append("CountryGuid", cGuid);
            formData.append("StateGuid", sGuid);
            formData.append("PostalCode", txtPosrCode);
            formData.append("UserVerificationType", verificationType);
            formData.append("VerificationId", VerificationId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/RegisterWithNuban/AddUserOtherDetail",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {
                        swal({
                            title: "Success",
                            text: "Your profile is created successfully.",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            var base_url = window.location.origin;
                            window.location.href = base_url + "/Home/Index";
                            sessionStorage.setItem("UserName", txtFirstName + " " + txtLastName);
                            //$("#verificationmodal").modal('show');
                        })

                    } else {
                        swal({
                            title: "Failed",
                            text: "Failed.",
                            icon: "Error",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#btnSaveOtherDetails").attr('disabled', false);
                            //$("#verificationmodal").modal('show');
                        })
                    }

                }
            });

        } else {
            $("#btnSaveOtherDetails").attr('disabled', false);
            return false;
        }
    });


    $('#btnUploadDocs').click(function () {
       // $('.img1 img').attr('src');
        var file = $('#IdType')[0].files[0]
        var images = $('.img1 img').attr('src');
        console.log(file)
       // alert(images);
    });


    //upload profile pic
    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                //uncomment this for shoing file which user uploaded
               // $('.IdTypeDoc').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }


    $("#IdType").on('change', function () {
        $(".IdTypeDoc").show();
        var ext = $('#IdType').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg', 'jfif', 'webp']) == -1) {
            alert('Invalid format! Please use these formats(gif,png,jp,jpeg).');
            return false;
        } else {
            readURL(this);
        }


    });

    $("input:file").change(function () {
        readURL(this);
    });

})


function getCountries() {
    debugger
    var id = 1;
    var formData = new FormData();
    formData.append("id", id);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Account/GetCountry",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            //$.each(response.countryResponses, function (index, value) {
            //    // APPEND OR INSERT DATA TO SELECT ELEMENT.
            //    $('#ddlcountry').append('<option value="' + value.Guid + '">' + value.Name + '</option>');
            //});
            let ddOptions = "";//'<option value="">Please select country</option>'
            $.each(response.countryResponses, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.Guid + '">' + value.Name + '</option>';
            });
            $('#ddlcountry').append(ddOptions);
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

function getStateByCountryGuid(id) {
    var id = id;
    var formData = new FormData();
    formData.append("guid", id);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Account/GetStateByCountryGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            let ddlStateOptions = "";// '<option value="">Please select State</option>';
            //$('#ddlState').append(ddlStateOptions);
            $.each(response.stateResponses, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddlStateOptions += '<option value="' + value.Guid + '">' + value.Name + '</option>';
            });
            $('#ddlState').append(ddlStateOptions);
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
function getCityByState(id) {
    var id = id;
    var formData = new FormData();
    formData.append("guid", id);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Account/GetCityByStateGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            let ddlCountryOptions = "";// '<option value="">Please select city</option>';
            // $('#ddlCity').append(ddlCountryOptions);
            $.each(response.cityResponses, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddlCountryOptions += '<option value="' + value.Guid + '">' + value.Name + '</option>';
            });
            $('#ddlCity').append(ddlCountryOptions);
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


function validateFields() {
    var addressName = $("#txtAddress").val();
    var txtPosrCode = $("#txtPostCode").val();
    var txtNinNumber = $("#txtNinNumber").val();

    var txtDriversLicense = $("#txtDriversLicense").val();
    var txtVotersCard = $("#txtVotersCard").val();
    var txtInternationalpassport = $("#txtInternationalpassport").val();

    if (addressName == '' || addressName == "" || addressName == null) {
        $('#txtAddressError').show();
        $('#txtAddressError').text("Please enter address");
        addressNameError = false;
        $('#btnSaveOtherDetails').prop('disabled', false);
        // return false;
    } else {
        $('#txtAddressError').hide();
        addressNameError = true;
        // return true;
    }
    if (txtPosrCode == '' || txtPosrCode == "" || txtPosrCode == null) {
        $('#txtPostCodeError').show();
        $('#txtPostCodeError').text("Please enter post code");
        postcodeError = false;
        $('#btnSaveOtherDetails').prop('disabled', false);
        // return false;
    } else {
        $('#txtPostCodeError').hide();
        postcodeError = true;
        // return true;
    }
    if (verificationType === "VNIN") {
        if (txtNinNumber == '' || txtNinNumber == "" || txtNinNumber == null) {
            $('#txtNINError').show();
            $('#txtNINError').text("Please enter NIN number");
            ninNumberError = false;
            $('#btnSaveOtherDetails').prop('disabled', false);
            // return false;
        } else {
            $('#txtNINError').hide();
            ninNumberError = true;
            // return true;
        }
    }
    if (verificationType === "DL") {
        if (txtDriversLicense == '' || txtDriversLicense == "" || txtDriversLicense == null) {
            $('#txtDriversLicenseError').show();
            $('#txtDriversLicenseError').text("Please enter DL number");
            ninNumberError = false;
            $('#btnSaveOtherDetails').prop('disabled', false);
            // return false;
        } else {
            $('#txtDriversLicenseError').hide();
            ninNumberError = true;
            // return true;
        }
    }
    if (verificationType === "VC") {
        if (txtVotersCard == '' || txtVotersCard == "" || txtVotersCard == null) {
            $('#txtVotersCardError').show();
            $('#txtVotersCardError').text("Please enter voter card number");
            ninNumberError = false;
            $('#btnSaveOtherDetails').prop('disabled', false);
            // return false;
        } else {
            $('#txtVotersCardError').hide();
            ninNumberError = true;
            // return true;
        }
    }
    if (verificationType === "IP") {
        if (txtInternationalpassport == '' || txtInternationalpassport == "" || txtInternationalpassport == null) {
            $('#txtInternationalpassportError').show();
            $('#txtInternationalpassportError').text("Please enter international passport number");
            ninNumberError = false;
            $('#btnSaveOtherDetails').prop('disabled', false);
            // return false;
        } else {
            $('#txtInternationalpassportError').hide();
            ninNumberError = true;
            // return true;
        }
    }


    if (country == '' || country == "" || country == null) {
        $('#ddlddlcountryError').show();
        $('#ddlddlcountryError').text("Please select country");
        CountryError = false;
        $('#btnSaveOtherDetails').prop('disabled', false);
        // return false;
    } else {
        $('#ddlddlcountryError').hide();
        CountryError = true;
        // return true;
    }
    if (state == '' || state == "" || state == null) {
        $('#ddlStateError').show();
        $('#ddlStateError').text("Please select state");
        StateError = false;
        $('#btnSaveOtherDetails').prop('disabled', false);
        // return false;
    } else {
        $('#ddlStateError').hide();
        StateError = true;
        // return true;
    }
    if (city == '' || city == "" || city == null) {
        $('#ddlCityError').show();
        $('#ddlCityError').text("Please select city");
        CityError = false;
        $('#btnSaveOtherDetails').prop('disabled', false);
        // return false;
    } else {
        $('#ddlCityError').hide();
        CityError = true;
        // return true;
    }
    if (verificationType == '' || verificationType == "" || verificationType == null) {
        $('#ddlVerificationTypeError').show();
        $('#ddlVerificationTypeError').text("Please select verification type");
        verificationTypeError = false;
        $('#btnSaveOtherDetails').prop('disabled', false);
        // return false;
    } else {
        $('#ddlVerificationTypeError').hide();
        verificationTypeError = true;
        // return true;
    }
}