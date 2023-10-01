var country = "";
var state = "";
var countryGuid = "";
var stateGuid = "";
var waorkingDays = 0;
let txtOrganisationError = true;
let txtAddressError = true;
let txtMobileError = true;
let dataError = true;
let txtEmailError = true;
let txtPostalCodeError = true;
let txtWorkingHoursError = true;

let waorkingDaysError = true;
let stateError = true;
let countryError = true;
let isCountrySelected = false;
let emailFormateError = true;
//let txtOrganisationError = true;
//let txtOrganisationError = true;
$.ajaxSetup({ async: false });

$(document).ready(function () {
    var UserGuid = "";
    UserGuid = sessionStorage.getItem("User1");
    getCountries();
    $('#ddlcountry').change(function () {
        if (isCountrySelected == true) {
            $('#ddlState').empty();

            //$('select#ddlState option:selected').text('');
            //$('select#ddlCity option:selected').text('');
            state = '';

            $('#ddlState').append('<option value="">Please select state</option>');
        }
        isCountrySelected = true;
        var value = $('#ddlcountry option:selected').val();
        countryGuid = value;
        country = $('#ddlcountry option:selected').text();
        getStateByCountryGuid(value);
    });
    $('#ddlState').change(function () {

        var value = $('select#ddlState option:selected').val();
        stateGuid = value;
        state = $('select#ddlState option:selected').text();

    });
    $('#ddlWorkingDaysinaWeek').change(function () {

        waorkingDays = $('select#ddlWorkingDaysinaWeek option:selected').val();
        var value = $('select#ddlWorkingDaysinaWeek option:selected').text();

    });

    $(function () {
        $('#txtEmail').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtOrganisation').on('keyup', function (e) {
            var str = "";
            str = $('#txtOrganisation').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtOrganisation').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtOrganisation').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });

    $("#btnUpdateProfile").click(function () {
        var txtOrganisation = $("#txtOrganisation").val();
        var txtAddress = $("#txtAddress").val();
        var txtMobile = $("#txtMobile").val();
        //var data = txtMobile.split('-');
        var txtEmail = $("#txtEmail").val();
        var txtPostalCode = $("#txtPostalCode").val();
        var txtWorkingHours = $("#txtWorkingHours").val();
        var txtwaorkingDays = waorkingDays;


        if (txtOrganisation == null || txtOrganisation == '' || txtOrganisation == "") {
            $("#txttxtOrganisationError").show();
            $("#txttxtOrganisationError").text("Please enter organisation name");
            txtOrganisationError = false;
        }
        else {
            $("#txttxtOrganisationError").hide();
            txtOrganisationError = true;
        }
        if (txtAddress == null || txtAddress == '' || txtAddress == "") {
            $("#txtAddressError").show();
            $("#txtAddressError").text("Please enter address");
            txtAddressError = false;
        }
        else {
            if (txtAddress.length < 10) {
                $('#txtAddressError').show();
                $('#txtAddressError').text("Please enter atleast 10 characters");
                AddressError1 = false;
            } else {
                $("#txtAddressError").hide();
                txtAddressError = true;
            }


        }
        if (txtMobile == null || txtMobile == '' || txtMobile == "") {
            $("#txtMobileError").show();
            $("#txtMobileError").text("Please enter mobile Number");
            txtMobileError = false;
        }
        else {
            if (txtMobile.length < 10) {
                $("#txtMobileError").show();
                $("#txtMobileError").text("Mobile number can not be less then 10 digits");
                txtMobileError = false;
            } else {
                $("#txtMobileError").hide();
                txtMobileError = true;
            }
        }
        if (txtEmail == null || txtEmail == '' || txtEmail == "") {
            $("#txtEmailError").show();
            $("#txtEmailError").text("Please enter email");
            txtEmailError = false;
        }
        else {
            $("#txtEmailError").hide();
            txtEmailError = true;
        }
        if (txtPostalCode == null || txtPostalCode == '' || txtPostalCode == "") {
            $("#txtPostalCodeError").show();
            $("#txtPostalCodeError").text("Please enter post code");
            txtPostalCodeError = false;
        } else {
            if (txtPostalCode.length < 6) {
                $("#txtPostalCodeError").show();
                $("#txtPostalCodeError").text("Post code should be 6 digits long");
            } else {
                $("#txtPostalCodeError").hide();
                txtPostalCodeError = true;
            }

        }
        if (txtWorkingHours == null || txtWorkingHours == '' || txtWorkingHours == "") {
            $("#txtWorkingHoursError").show();
            $("#txtWorkingHoursError").text("Please enter working hours");
            txtWorkingHoursError = false;
        } else {
            $("#txtWorkingHoursError").hide();
            txtWorkingHoursError = true;
        }
        if (txtwaorkingDays == "0") {
            $("#ddlWorkingDaysinaWeekError").show();
            $("#ddlWorkingDaysinaWeekError").text("Please select working days");
            waorkingDaysError = false;
        } else {
            $("#ddlWorkingDaysinaWeekError").hide();
            waorkingDaysError = true;
        }
        if (state == null || state == '' || state == "") {
            $("#ddlStateError").show();
            $("#ddlStateError").text("Please select state");
            stateError = false;
        } else {
            $("#ddlStateError").hide();
            stateError = true;
        }
        if (country == null || country == '' || country == "" || country == "select") {
            $("#ddlcountryError").show();
            $("#ddlcountryError").text("Please select country");
            countryError = false;
        } else {
            $("#ddlcountryError").hide();
            countryError = true;
        }
        validateEmailForSignup(txtEmail);
        if (emailFormateError == true && countryError == true && stateError == true && txtOrganisationError == true && txtAddressError == true && txtMobileError == true && txtEmailError == true && txtPostalCodeError == true && txtWorkingHoursError == true && waorkingDaysError == true) {
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("PhoneNumber", txtMobile.trim());
            formData.append("State", state);
            formData.append("Country", country);
            formData.append("WorkingDaysInWeek", waorkingDays.trim());
            formData.append("Address", txtAddress.trim());
            formData.append("PostalCode", txtPostalCode.trim());
            formData.append("OrganisationName", txtOrganisation.trim());
            formData.append("WorkingHoursOrDays", txtWorkingHours.trim());
            formData.append("CountryCode", "+234");
            formData.append("Email", txtEmail.trim());
            formData.append("CountryGuid", countryGuid);
            formData.append("StateGuid", stateGuid);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Profile/UpdateEmployerProfile/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        swal({
                            title: "Success",
                            text: "Profile updated successfully.",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            var url = window.location.origin;
                            window.location.href = url + "/Profile/Index";
                        })
                    } else {
                        swal({
                            title: "Error",
                            text: "Profile updated successfully.",
                            icon: "error",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {

                        })
                    }
                }
            });
        }
        else {
            return false;
        }
    });


    getEmployerProfileForUpdate(UserGuid);
});

function getEmployerProfileForUpdate(UserGuid) {
    var formData = new FormData();
    formData.append("guid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "GetEmployerProfileForUpdate",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {


                $("#ddlcountry").val(response.getEmployerProfileDetailResponse.CountryGuid).trigger('change');
                setTimeout(function () {
                    $("#ddlState").val(response.getEmployerProfileDetailResponse.StateGuid).trigger('change');
                }, 1000);
                $("#ddlWorkingDaysinaWeek").val(response.getEmployerProfileDetailResponse.WorkingDaysInWeek).trigger('change');
                $("#txtOrganisation").val(response.getEmployerProfileDetailResponse.OrganisationName);
                $("#txtAddress").val(response.getEmployerProfileDetailResponse.Address);
                /*  $("#txtMobile").val(response.getEmployerProfileDetailResponse.CountryCode + '-' + response.getEmployerProfileDetailResponse.PhoneNumber);*/
                $("#countruCode").text(response.getEmployerProfileDetailResponse.CountryCode);
                $("#txtMobile").val(response.getEmployerProfileDetailResponse.PhoneNumber);
                $("#txtEmail").val(response.getEmployerProfileDetailResponse.Email);
                $("#txtPostalCode").val(response.getEmployerProfileDetailResponse.PostalCode);
                $("#txtWorkingHours").val(response.getEmployerProfileDetailResponse.WorkingHoursOrDays);

            } else {

            }

        }
    });
}

function getCountries() {
    var id = 1;
    var formData = new FormData();
    formData.append("id", id);
    $.ajax({
        type: "POST",
        cache: false,
        url: "GetCountry",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#ddlState').append('<option value="">Please select state</option>');
            $.each(response.countryResponses, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                $('#ddlcountry').append('<option value="' + value.Guid + '">' + value.Name + '</option>');
            });
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
        url: "GetStateByCountryGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $.each(response.stateResponses, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                $('#ddlState').append('<option value="' + value.Guid + '">' + value.Name + '</option>');
            });
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

function validateEmailForSignup(email) {
    var userinput = email;



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
                emailFormateError = false;
                $('#btnRegister').prop('disabled', false);
            })
        }
        else {
            emailFormateError = true;
        }
    }


}