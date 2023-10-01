let isCountrySelected = false;
var UserGuid = "";
var country = "";
var state = "";
let IsEwaPermission = false;
var city = "";
var gender = "";
var employerName = "";
var employerId = 0;
var bankName = "";
var Days = 0;
let usernameError = true;
let MobileNoError = true;
let PayFromError = true;
let PayToError = true;
let StateError = true;
let CountryError = true;
let AddressError = true;
let PostalCodeError = true;
let WorkingHourError = true;
let WorkingDaysError = true;
var payCycleFromId = 0;
var payCycleToId = 0;
$.ajaxSetup({ async: false });
$(document).ready(function () {
    var countryGuid = "";
    var stateGuid = "";
    $('#txttxtorganisationError').hide();
    $('#ddlCountryError').hide();
    $('#ddlStateError').hide();
    $('#txtAddressError').hide();
    $('#txtPostalCodeError').hide();
    $('#txtWorkingDaysError').hide();
    $('#ddlWorkingDaysError').hide();
    $('#txtPayCycleFromError').hide();
    $('#txtPayCycleToError').hide();
    $('#txttxtorganisationError').hide();
    $('#txttxtorganisationError').hide();


    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);

    getCountries();


    //var startDate = new Date('01/01/2012');
    //var FromEndDate = new Date();
    //var ToEndDate = new Date();
    //// ToEndDate.setDate(ToEndDate.getDate() + 365);
    //ToEndDate.setDate(ToEndDate.getDate());

    //$('#txtPayCycleFrom').datepicker({
    //    weekStart: 1,
    //    startDate: '01/01/2012',
    //    endDate: FromEndDate,
    //    autoclose: true,
    //    format: "dd/mm/yyyy"
    //}).on('changeDate', function (selected) {
    //    startDate = new Date(selected.date.valueOf());
    //    // fromDate = selected.date.valueOf();
    //    startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
    //    $('#txtPayCycleTo').datepicker('setStartDate', startDate);
    //});

    //$('#txtPayCycleTo').datepicker({
    //    weekStart: 1,
    //    startDate: startDate,
    //    endDate: ToEndDate,
    //    autoclose: true,
    //    format: "dd/mm/yyyy"
    //}).on('changeDate', function (selected) {
    //    FromEndDate = new Date(selected.date.valueOf());
    //    //toDate = FromEndDate.parse('dd-mm-yy') ;
    //    FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
    //    $('#txtPayCycleFrom').datepicker('setEndDate', FromEndDate);
    //});



    $('#ddlcountry').change(function () {
        //if (isCountrySelected == true) {
        //    $('#ddlState').empty();
        //}
        if (isCountrySelected == true) {
            $('#ddlState').empty();

            //$('select#ddlState option:selected').text('');
            //$('select#ddlCity option:selected').text('');
            state = '';

            $('#ddlState').append('<option value="">Please select state</option>');
        }
        isCountrySelected = true;
        var value = $('#ddlcountry').val();
        countryGuid = value;
        country = $('#ddlcountry option:selected').text();
        getStateByCountryGuid(countryGuid);

    });
    $('#ddlState').change(function () {

        var value = $('#ddlState').val();
        stateGuid = value;
        state = $('#ddlState option:selected').text();


    });

    $('#payCycleFrom').change(function () {
        
        payCycleFromId = $('#payCycleFrom').val();
        //payCycleFromId = value;
        var value = $('#payCycleFrom option:selected').text();


    });

    $('#payCycleTo').change(function () {
        payCycleToId = $('#payCycleTo').val();
        var value = $('#payCycleTo option:selected').text();


    });

    $('#ddlWorkingDays').change(function () {

        Days = $('#ddlWorkingDays option:selected').val();
        //  state = $('select#ddlWorkingDays option:selected').text();
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


    $(function () {
        $('#txtorganisation').on('keypress', function (e) {
            /*if (event.target.value.substr(-1) === ' ' && event.code === 'Space') {
                return false;
            }*/
            if (this.value.length === 0 && e.which === 32) { e.preventDefault(); }
        });
    });
    $(function () {
        $('#txtAddress').on('keypress', function (e) {
            if (event.target.value.substr(-1) === ' ' && event.code === 'Space') {
                return false;
            }
        });
    });

    $('#btnUpdateProfile').click(function () {

        var txtorganisation = $('#txtorganisation').val().trim();
        var txtcountryGuid = countryGuid;
        var txtstateGuid = stateGuid;
        var txtAddress = $('#txtAddress').val().trim();
        var txtState = state;
        var txtCountry = country;
        var txtWorkingDays = Days;
        var txtAddress = $('#txtAddress').val().trim();
        var txtPostalCode = $('#txtPostalCode').val().trim();
        var txtWorkingHours = $('#txtWorkingDays').val().trim();
        var txtPayCycleFrom = payCycleFromId; //$('#txtPayCycleFrom').val();
        var txtPayCycleTo = payCycleToId;//$('#txtPayCycleTo').val();

        //mobileVerify = mobile;
        if (txtorganisation == '' || txtorganisation == "" || txtorganisation == null) {
            $('#txttxtorganisationError').show();
            usernameError = false;
            $('#txttxtorganisationError').text("Please enter organisation name");
            // alert("Organisation name can not empty")
            // return false;
        }
        else {
            $('#txttxtorganisationError').hide();
            usernameError = true;
            // return true;
        }
        if (txtWorkingHours == '' || txtWorkingHours == "" || txtWorkingHours == null) {
            $('#txtWorkingDaysError').show();
            $('#txtWorkingDaysError').text("Please enter working hours");
            WorkingHourError = false;
            // return false;
        } else {
            if (txtWorkingHours > 24) {
                $('#txtWorkingDaysError').show();
                $('#txtWorkingDaysError').text("Working hours can not be greater then 24");
                WorkingHourError = false;
            } else {
                $('#txtWorkingDaysError').hide();
                WorkingHourError = true;

            }

        }
        if (txtPayCycleFrom == '0' || txtPayCycleFrom == "0" || txtPayCycleFrom == 0) {
            //$('#passcheck').show();
            $('#txtPayCycleFromError').show();
            $('#txtPayCycleFromError').text("Please select pay cycle from.");
            PayFromError = false;
            // return false;
        } else {
            if (parseInt(txtPayCycleFrom) > parseInt(txtPayCycleTo)) {
                $('#txtPayCycleFromError').show();
                $('#txtPayCycleFromError').text("Pay cycle from can not be greater.");
                PayFromError = false;
            } else {
                $('#txtPayCycleFromError').hide();
                PayFromError = true;
            }
            // $('#passcheck').hide();

            // return true;
        }
        if (txtPayCycleTo == '0' || txtPayCycleTo == "0" || txtPayCycleTo == 0) {
            //$('#passcheck').show();
            $('#txtPayCycleToError').show();
            $('#txtPayCycleToError').text("Please select pay cycle to.");
            PayToError = false;
            // return false;
        } else {
            
            if (parseInt(txtPayCycleFrom) < parseInt(txtPayCycleTo)) {
                $('#txtPayCycleToError').show();
                $('#txtPayCycleToError').text("Pay cycle to can not be less then from date.");
                PayToError = false;
            }
            else if (parseInt(txtPayCycleTo) == parseInt(txtPayCycleFrom)) {
                $('#txtPayCycleFromError').show();
                $('#txtPayCycleFromError').text("Pay cycle to and Pay cycle from date can not be same.");
                PayToError = false;
            }
            else {
                $('#txtPayCycleToError').hide();
                // $('#passcheck').hide();
                PayToError = true;
            }

            $('#txtPayCycleToError').hide();
            // $('#passcheck').hide();
            PayToError = true;
            // return true;
        }
        if (txtCountry == '' || txtCountry == "" || txtCountry == null || txtCountry == "Please select country") {
            $('#ddlCountryError').show();
            $('#ddlCountryError').text("Please select country.");
            CountryError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            $('#ddlCountryError').hide();
            CountryError = true;
            // return true;
        }
        if (txtState == '' || txtState == "" || txtState == null || txtState == "Please select state") {
            $('#ddlStateError').show();

            $('#ddlStateError').text("Please select state.");
            StateError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            StateError = true;
            $('#ddlStateError').hide();
            // return true;
        }
        if (txtAddress == '' || txtAddress == "" || txtAddress == null) {
            //$('#passcheck').show();
            $('#txtAddressError').show();
            $('#txtAddressError').text("Please enter address.");

            AddressError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            $('#txtAddressError').hide();
            AddressError = true;
            // return true;
        }
        if (txtPostalCode == '' || txtPostalCode == "" || txtPostalCode == null) {
            //$('#passcheck').show();
            $('#txtPostalCodeError').show();
            $('#txtPostalCodeError').text("Please enter post code.");
            PostalCodeError = false;
            // return false;
        } else {
            if (txtPostalCode.length < 6) {
                $('#txtPostalCodeError').show();
                $('#txtPostalCodeError').text("Post code should be in 6 digit");
                PostalCodeError = false;
            } else {
                if (txtPostalCode === "000000") {
                    $('#txtPostalCodeError').show();
                    $('#txtPostalCodeError').text("Please enter valid post code.");
                    PostalCodeError = false;
                } else {
                    $('#txtPostalCodeError').hide();
                    PostalCodeError = true;
                }

            }
            // $('#passcheck').hide();

            // return true;
        }
        if (Days == '' || Days == "" || Days == null || Days === "Select") {
            //$('#passcheck').show();
            $('#ddlWorkingDaysError').show();
            $('#ddlWorkingDaysError').text("Please select working days.");
            WorkingDaysError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            $('#ddlWorkingDaysError').hide();
            WorkingDaysError = true;
            // return true;
        }

        // validateEmail();
        if (usernameError == true && MobileNoError == true && AddressError == true && PostalCodeError == true && CountryError == true &&
            StateError == true && PayFromError == true && PayToError == true && WorkingDaysError == true && WorkingHourError == true) {
            $("#btnUpdateProfile").attr('disabled', true);
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            //formData.append("PhoneNumber", txtMobile);
            formData.append("State", txtState);
            formData.append("Country", txtCountry);
            formData.append("Address", txtAddress);
            formData.append("PostalCode", txtPostalCode);
            formData.append("OrganisationName", txtorganisation);
            formData.append("WorkingHoursOrDays", txtWorkingHours);
            formData.append("WorkingDaysInWeek", Days);
            formData.append("CountryCode", "+234");
            formData.append("StartDate", txtPayCycleFrom);
            formData.append("EndDate", txtPayCycleTo);
            formData.append("CountryGuid", txtcountryGuid);
            formData.append("StateGuid", txtstateGuid);
            formData.append("IsEwaApprovalAccess", IsEwaPermission);
            
            $.ajax({
                type: "POST",
                cache: false,
                url: "CompleteEmployerProfile",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {

                        var url = window.location.origin;
                        window.location.href = url + "/EmployerDashbord/Index";
                    }
                    else if (response.RstKey == 8) {

                        swal(
                            'Error!',
                            'Mobile already exists.',
                            'error'
                        ).catch(swal.noop);
                        $("#btnUpdateProfile").attr('disabled', false);
                        // window.location.href = "Account/Index";
                    } else if (response.RstKey == 2) {

                        swal(
                            'Error!',
                            'Profile not completed',
                            'error'
                        ).catch(swal.noop);
                        $("#btnUpdateProfile").attr('disabled', false);
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
            return false;
        }
    });

   
    $("#chkEWAapprovalauthority").change(function () {
        if (this.checked) {
            IsEwaPermission = true;
        }
        else {
            IsEwaPermission = false;
        }
    });
    /* $("#btnHelp").click(function () {
         $("#helpModal").modal('show');
     })*/
});


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
            //$.each(response.countryResponses, function (index, value) {
            //    // APPEND OR INSERT DATA TO SELECT ELEMENT.
            //    $('#ddlcountry').append('<option value="' + value.Guid + '">' + value.Name + '</option>');
            //});

            let ddOptions = '<option value="">Please select country</option>'
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