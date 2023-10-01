////$(window).on('popstate', function () {
////    alert('Back button was pressed.');
////});


$(document).ready(function () {
    $('.select2').select2({
        selectionCssClass: ":all:",
        tags: true
    });

    document.onkeypress = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onmousedown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onkeydown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }

    $(document).bind('contextmenu.namespace', function () {
        return false;
    });


    $("#txtDateOfBirth").datepicker({

        changeMonth: true,
        changeYear: true,
        maxDate: "-16y",
        minDate: "-100y",
        yearRange: "-100:-18"
        // onSelect: function (dateText, inst) {
        //     $('#txtDateOfBirth').val(dateText);
        //}
    });


    // preventBack();
    //window.onhashchange = function () {
    //    alert("pop!1111");
    //}

    //preventBack();
    //function preventBack() {
    //    window.history.back();

    //}
    //setTimeout("preventBack()", 0)
    //window.onunload = function () { null }



    $('#btnBack').hide();
    $('#txtFirstNameError').hide();
    $('#txtLastNameError').hide();
    $('#txtNinNoError').hide();
    $('#txtDateOfBirthError').hide();
    $('#ddlGenderError').hide();
    $('#ddlcountryError').hide();
    $('#ddlStateError').hide();
    $('#ddlCityError').hide();
    $('#txtAddressError').hide();
    $("#divNonRegisterEmployer").hide();
    $("#btnBackFromEmployer").hide();
    let isCountrySelected = false;
    let isStateSelected = false;
    var UserGuid = "";
    var country = "";
    var state = "";
    var city = "";
    var gender = "";
    var employerName = "";
    var employerId = 0;
    var bankName = "";
    var bankCode = "";
    let usernameError = true;
    let FirstNameError = true;
    let LastNameError = true;
    let MiddleNameError = true;
    let NinNoError = true;
    let DateOfBirthError = true;
    let StateError = true;
    let CityError = true;
    let CountryError = true;
    let AddressError = true;
    let PostalCodeError = true;
    let GenderError = true;
    let EmployerNameError = true;
    let EmployerIdError = true;
    let StaffIdError = true;
    let BankNameError = true;
    let AccountNumberError = true;
    let BVNError = true;
    let BankAccountHolderNameError = true;
    var SearchText = "";
    let isNonRegisterEmployerSelected = false
    var NonRegisterEmployerGuid = "";

    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);


    $(function () {
       
        $("#tags").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/CommonEmployer/GetNonRegisteredEmployerList/",
                    contentType: "application/json",
                    dataType: "json",
                    processData: false,
                    data: "{'searchText':'" + request.term + "'}",
                    success: function (data) {
                        response($.map(data.nonRegisterEmployerResponses, function (item) {
                            return {
                                label: item.OrganisationName,
                                value: item.OrganisationName,//Guid,
                                id: item.Guid,
                                class: item.Id
                            };
                        }))
                    }
                });
            },
            select: function (event, ui) {
                employerId = ui.item.class;
                employerName = ui.item.value;
                NonRegisterEmployerGuid = ui.item.id;
            }
        });
    });



    getCountries();
    getEmployerList();
    getOkraBankList();
    //getNonRegisterEmployerList(SearchText);
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
                let ddOptions = '<option value=""  class="mt">Please select employer</option>' + '<option value="Other"  class="mt" >Other</option>';
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


    function getNonRegisterEmployerList(d) {
        var id = 1;

        var formData = new FormData();
        formData.append("searchText", d);
        $.ajax({
            type: "POST",
            cache: false,
            url: "/CommonEmployer/GetNonRegisteredEmployerList/",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {

                let ddOptions = '';
                if (isNonRegisterEmployerSelected == true) {
                    ddOptions = '<option value=""  class="mt">Please select employer</option>';
                }
                // '<option value=""  class="mt">Please select employer</option>';
                $.each(response.nonRegisterEmployerResponses, function (index, value) {

                    ddOptions += '<option value="' + value.Id + '" data-name="Others" data-guid="' + value.Guid + '">' + value.OrganisationName + '</option>';
                });
                $('#ddlNonRegisterEmployer').append(ddOptions);
            }
        });

    }

    function getOkraBankList() {
        var id = 1;
        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        $.ajax({
            type: "POST",
            cache: false,
            url: "/BillAndPayment/GetBankList/",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {


                let ddOptions = '<option value="">Please select bank</option>'
                $.each(response.pouchiBankListResponse.data, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    ddOptions += '<option value="' + value.bankCode + '">' + value.bankName + '</option>';
                });
                $('#ddlBank').append(ddOptions);
            }
        });

    }
    $('#ddlcountry').change(function () {

        if (isCountrySelected == true) {
            $('#ddlState').empty();
            $('#ddlCity').empty();
            //$('select#ddlState option:selected').text('');
            //$('select#ddlCity option:selected').text('');
            state = '';
            city = ''
            $('#ddlCity').append('<option value="">Please select city</option>');
            $('#ddlState').append('<option value="">Please select state</option>');
        }
        isCountrySelected = true;
        var value = $('select#ddlcountry option:selected').val();
        country = $('select#ddlcountry option:selected').text();
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
        getCityByState(value);

    });

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
            url: "GetCityByStateGuid",
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
    $('#ddlCity').change(function () {

        var value = $('select#ddlCity option:selected').val();
        city = $('select#ddlCity option:selected').text();

    });
    $('#ddlGender').change(function () {

        var value = $('select#ddlGender option:selected').val();
        gender = $('select#ddlGender option:selected').text();
    });

    $('#ddlBank').change(function () {

        bankCode = $('select#ddlBank option:selected').val();
        bankName = $('select#ddlBank option:selected').text();

    });


    $(function () {
        $('#txtFirstName').on('keyup', function (e) {
            var str = "";
            str = $('#txtFirstName').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtFirstName').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtFirstName').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });
    $(function () {
        $('#txtFirstName').on('keypress', function (e) {
            if (this.value.length === 0 && e.which === 32) { e.preventDefault(); }
        });
    });

    $(function () {
        $('#txtLastName').on('keyup', function (e) {
            var str = "";
            str = $('#txtLastName').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtLastName').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtLastName').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });
    $(function () {
        $('#txtLastName').on('keypress', function (e) {
            if (this.value.length === 0 && e.which === 32) { e.preventDefault(); }
        });
    });
    $(function () {
        $('#txtMiddleName').on('keyup', function (e) {
            var str = "";
            str = $('#txtMiddleName').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtMiddleName').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtMiddleName').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });
    $(function () {
        $('#txtMiddleName').on('keypress', function (e) {
            if (this.value.length === 0 && e.which === 32) { e.preventDefault(); }
        });
    });



    $(function () {
        $('#txtAddress').on('keypress', function (e) {
            if (this.value.length === 0 && e.which === 32) { e.preventDefault(); }
        });
    });
    $(function () {
        $('#txtAddress').on('keyup', function (e) {
            var str = "";
            str = $('#txtAddress').val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            $('#txtAddress').val(str);

            var cleanStr = removeEmojis(str);
            $(this).val(cleanStr);
            $('#txtAddress').val(cleanStr);
            //if (e.which == 32) {
            //    alert('Space not allowed');
            //    return false;
            //}
        });
    });
    //$("textarea").not(".allowemoji").keyup(function () {
    //    var strng = $(this).val();
    //    var cleanStr = removeEmojis(strng);
    //    $(this).val(cleanStr);
    //});
    //$("input").not("#txtFirstName").keyup(function () {
    //    
    //    var strng = $(this).val();
    //    var cleanStr = removeEmojis(strng);
    //    $(this).val(cleanStr);
    //});
    function removeEmojis(string) {
        var regex = /(?:[\u2700-\u27bf]|(?:\ud83c[\udde6-\uddff]){2}|[\ud800-\udbff][\udc00-\udfff]|[\u0023-\u0039]\ufe0f?\u20e3|\u3299|\u3297|\u303d|\u3030|\u24c2|\ud83c[\udd70-\udd71]|\ud83c[\udd7e-\udd7f]|\ud83c\udd8e|\ud83c[\udd91-\udd9a]|\ud83c[\udde6-\uddff]|\ud83c[\ude01-\ude02]|\ud83c\ude1a|\ud83c\ude2f|\ud83c[\ude32-\ude3a]|\ud83c[\ude50-\ude51]|\u203c|\u2049|[\u25aa-\u25ab]|\u25b6|\u25c0|[\u25fb-\u25fe]|\u00a9|\u00ae|\u2122|\u2139|\ud83c\udc04|[\u2600-\u26FF]|\u2b05|\u2b06|\u2b07|\u2b1b|\u2b1c|\u2b50|\u2b55|\u231a|\u231b|\u2328|\u23cf|[\u23e9-\u23f3]|[\u23f8-\u23fa]|\ud83c\udccf|\u2934|\u2935|[\u2190-\u21ff])/g;
        return string.replace(regex, '');
    }


    //$("#TestOpenModal").click(function () {
    //    $("#nonRegisterEmployerModal").modal('show');
    //});

    //$(".bet").unbind().click(function () {
    //    //Stuff
    //});

    $('#btnUpdateProfile').unbind().click(function () {

        var txtFirstName = $('#txtFirstName').val();
        var txtLastName = $('#txtLastName').val();
        var txtMiddleName = $('#txtMiddleName').val();
        var txtNinNo = $('#txtNinNo').val();
        var txtDateOfBirth = $('#txtDateOfBirth').val();
        var txtState = state;
        var txtCity = city;
        var txtCountry = country;
        var txtAddress = $('#txtAddress').val();
        var txtPostalCode = $('#txtPostalCode').val();
        var txtGender = gender;
        var txtEmployerName = employerName;
        var txtEmployerId = employerId;
        var txtStaffId = $('#txtStaffId').val();
        var txtBankName = bankName;
        var txtbankCode = bankCode;
        var txtAccountNumber = $('#txtAccountNumber').val();
        var txtBVN = $('#txtBVN').val();
        var txtBankAccountHolderName = $('#txtBankAccountHolderName').val();
        //var txtCustomerId = $('#txtCustomerId').val();
        var txtNonRegisterdEmployerId = NonRegisterEmployerGuid;
        console.log("txtNonRegisterdEmployerId" + txtNonRegisterdEmployerId);
        //mobileVerify = mobile;
        if (txtFirstName == '' || txtFirstName == "" || txtFirstName == null && txtFirstName.length < 2) {
            // $('#usercheck').show();
            usernameError = false;//
            $('#txtFirstNameError').show();
            // return false;
        }
        else {
            // $('#usercheck').hide();
            usernameError = true;
            // return true;
        }
        if (txtNinNo == '' || txtNinNo == "" || txtNinNo == null) {
            //$('#passcheck').show();
            $('#txtNinNoError').show();
            NinNoError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            NinNoError = true;
            // return true;
        }
        if (txtDateOfBirth == '' || txtDateOfBirth == "" || txtDateOfBirth == null) {
            //$('#passcheck').show();
            $('#txtDateOfBirthError').show();
            DateOfBirthError = false;
            // return false;
        } else {

            // $('#passcheck').hide();
            DateOfBirthError = true;

            // return true;
        }
        if (txtGender == '' || txtGender == "" || txtGender == null) {
            //$('#passcheck').show();
            $('#ddlGenderError').show();
            GenderError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            GenderError = true;
            // return true;
        }
        if (txtLastName == '' || txtLastName == "" || txtLastName == null && txtFirstName.length < 2) {
            //$('#passcheck').show();
            $('#txtLastNameError').show();
            LastNameError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            LastNameError = true;
            // return true;
        }
        if (txtCountry == '' || txtCountry == "" || txtCountry == null) {
            //$('#passcheck').show();
            $('#ddlcountryError').show();
            CountryError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            CountryError = true;
            // return true;
        }
        if (txtState == '' || txtState == "" || txtState == null) {
            //$('#passcheck').show();
            $('#ddlStateError').show();
            StateError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            StateError = true;
            // return true;
        }
        if (txtCity == '' || txtCity == "" || txtCity == null) {
            //$('#passcheck').show();
            $('#ddlCityError').show();
            CityError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            CityError = true;
            // return true;
        }
        if (txtAddress == '' || txtAddress == "" || txtAddress == null) {
            //$('#passcheck').show();
            $('#txtAddressError').show();
            AddressError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            AddressError = true;
            // return true;
        }

        // validateEmail();
        if (usernameError == true && NinNoError == true && LastNameError == true && DateOfBirthError == true &&
            GenderError == true && AddressError == true && CountryError == true && StateError == true && CityError == true) {
            $("#btnUpdateProfile").attr('disabled', true);
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("FirstName", txtFirstName);
            formData.append("LastName", txtLastName);
            formData.append("MiddleName", txtMiddleName);
            formData.append("NinNo", txtNinNo);
            formData.append("DateOfBirth", txtDateOfBirth);
            formData.append("CountryName", txtCountry);
            formData.append("State", txtState);
            formData.append("City", txtCity);
            formData.append("Address", txtAddress);
            formData.append("PostalCode", txtPostalCode);
            formData.append("Gender", txtGender);
            formData.append("EmployerName", txtEmployerName);
            formData.append("EmployerId", txtEmployerId);
            formData.append("StaffId", txtStaffId);
            formData.append("BankName", txtBankName);
            formData.append("AccountNumber", txtAccountNumber);
            formData.append("BVN", txtBVN);
            formData.append("BankAccountHolderName", txtBankAccountHolderName);
            formData.append("BankCode", txtbankCode);
            // formData.append("CustomerId", txtCustomerId);
            formData.append("NonRegisterEmployerGuid", NonRegisterEmployerGuid);
            $.ajax({
                type: "POST",
                cache: false,
                url: "CompleteProfile",
                contentType: false,
                processData: false,
                data: formData,
                //type: "POST",
                //cache: false,
                //url: "Account/CompleteProfile",
                //contentType: false,
                //processData: false,
                //data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {
                        sessionStorage.setItem("ProfileImage", "/Content/img/user.jpg");
                        sessionStorage.setItem("UserName", txtFirstName + " " + txtLastName);
                        swal({
                            title: "Success",
                            text: "Your profile is updated successfully.",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            var base_url = window.location.origin;
                            window.location.href = base_url + "/Home/Index";
                            //$("#verificationmodal").modal('show');
                        })

                    }
                    else if (response == "") {
                        swal(
                            'Error!',
                            'Mobile already exists.',
                            'error'
                        ).catch(swal.noop);
                        $("#btnUpdateProfile").attr('disabled', false);
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
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                        $("#btnUpdateProfile").attr('disabled', false);
                        // window.location.href = "Account/Index";
                    } else {
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                        $("#btnUpdateProfile").attr('disabled', false);
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


    $('#ddlEmployer').change(function () {
        employerId = $('select#ddlEmployer option:selected').val();
        employerName = $('select#ddlEmployer option:selected').text();
        $('#ddlNonRegisterEmployer').empty();
        $('#nonRegisteredEmployerError').hide();
        if (employerName.toUpperCase() == "OTHER") {
            // $('#ddlNonRegisterEmployer').empty();
            isNonRegisterEmployerSelected = true;
            var d = "";
            getNonRegisterEmployerList(d);
            $("#divuserpage").hide();
            $("#btnBack").hide();
            $("#divNonRegisterEmployer").show();
            $("#btnBackFromEmployer").show();
        }
    });
    $("#btnBackFromEmployer").click(function () {
        $("#divuserpage").show();
        $("#btnBack").show();
        $("#divNonRegisterEmployer").hide();
        $("#btnBackFromEmployer").hide();
    });

    function ValidateDOB1(dateString) {


        var lblError = $("#lblError");
        var parts = dateString.split("-");
        var dtDOB = new Date(parts[2] + "/" + parts[1] + "/" + parts[0]);
        var dtCurrent = new Date();
        lblError.html("Age must be 16 years")


        var date = new Date(dateString);

        if (dtCurrent.getFullYear() - date.getFullYear() < 18) {
            IsAgeValid = false;
            alert("Age must be 16 years");
            return false;
        }

        if (dtCurrent.getFullYear() - dtDOB.getFullYear() == 16) {

            //CD: 11/06/2018 and DB: 15/07/2000. Will turned 18 on 15/07/2018.
            if (dtCurrent.getMonth() < dtDOB.getMonth()) {
                alert("Age must be 16 years");
                return false;
            }
            if (dtCurrent.getMonth() == dtDOB.getMonth()) {
                //CD: 11/06/2018 and DB: 15/06/2000. Will turned 18 on 15/06/2018.
                if (dtCurrent.getDate() < dtDOB.getDate()) {
                    alert("Age must be 16 years");
                    return false;
                }
            }
        }
        lblError.html("");
        return true;
    }
    //$('#browser').change(function () {
    //    
    //    employerId = $('select#browser option:selected').val();
    //    employerName = $('select#browser option:selected').text();
    //});

    $("#browser").on('input', function (e) {

        var selectedVal = $(this).val();
        employerId = selectedVal.split("-")[0]
        employerName = selectedVal.split("-")[1];
        //alert();
        //ddlNonRegisterEmployer
    });
    $('#ddlBank').change(function () {
        bankId = $('select#ddlBank option:selected').val();
        bankName = $('select#ddlBank option:selected').text();
    });

    $('#ddlNonRegisterEmployer').change(function () {

        employerId = $('select#ddlNonRegisterEmployer option:selected').val();
        employerName = $('select#ddlNonRegisterEmployer option:selected').attr('data-name');
        NonRegisterEmployerGuid = $('select#ddlNonRegisterEmployer option:selected').attr('data-guid');
    });

    $("#btnSaveNonRegisterEmployer").click(function () {
        if (employerName == '' || employerName == "" || employerName == null || employerName.toUpperCase() == "OTHER" || employerName == "Please select employer") {
            $("#nonRegisteredEmployerError").show();
            $("#nonRegisteredEmployerError").text("Please select employer");
        } else {
            $("#divuserpage").show();
            $("#divNonRegisterEmployer").hide();
            $("#nonRegisteredEmployerError").hide();
        }
    });

    $('#btnSaveUserProfile').click(function () {
        //  function SaveUserProfile() {
        debugger
        var txtFirstName = $('#txtFirstName').val();
        var txtLastName = $('#txtLastName').val();
        var txtMiddleName = $('#txtMiddleName').val();
        var txtNinNo = $('#txtNinNo').val();
        var txtDateOfBirth = $('#txtDateOfBirth').val();
        var txtState = state;
        var txtCity = city;
        var txtCountry = country;
        var txtAddress = $('#txtAddress').val();
        var txtPostalCode = $('#txtPostalCode').val();
        var txtGender = gender;
        var txtEmployerName = employerName;
        var txtEmployerId = employerId;
        var txtStaffId = $('#txtStaffId').val();
        var txtBankName = bankName;
        var txtbankCode = bankCode;
        var txtAccountNumber = $('#txtAccountNumber').val();
        var txtBVN = $('#txtBVN').val();
        var txtBankAccountHolderName = $('#txtBankAccountHolderName').val();
        //var txtCustomerId = $('#txtCustomerId').val();
        var txtNonRegisterdEmployerId = NonRegisterEmployerGuid;

        validate1();
        validate2();
        // validate3();
        validate4();
        // validateEmail();
        if (usernameError == true && NinNoError == true && LastNameError == true && DateOfBirthError == true &&
            GenderError == true && AddressError == true && CountryError == true && StateError == true && CityError == true
            && BankNameError == true && AccountNumberError == true && BVNError == true && customerIdError == true && BankAccountHolderNameError == true) {
            $("#btnSaveUserProfile").attr('disabled', true);
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("FirstName", txtFirstName);
            formData.append("LastName", txtLastName);
            formData.append("MiddleName", txtMiddleName);
            formData.append("NinNo", txtNinNo);
            formData.append("DateOfBirth", txtDateOfBirth);
            formData.append("CountryName", txtCountry);
            formData.append("State", txtState);
            formData.append("City", txtCity);
            formData.append("Address", txtAddress);
            formData.append("PostalCode", txtPostalCode);
            formData.append("Gender", txtGender);
            formData.append("EmployerName", txtEmployerName);
            formData.append("EmployerId", txtEmployerId);
            formData.append("StaffId", txtStaffId);
            formData.append("BankName", txtBankName);
            formData.append("AccountNumber", txtAccountNumber);
            formData.append("BVN", txtBVN);
            formData.append("BankAccountHolderName", txtBankAccountHolderName);
            formData.append("BankCode", txtbankCode);
            //formData.append("CustomerId", txtCustomerId);
            formData.append("NonRegisterEmployerGuid", txtNonRegisterdEmployerId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "CompleteProfile",
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

                    }
                    else if (response.RstKey == 8) {

                        swal(
                            'Error!',
                            'Mobile already exists.',
                            'error'
                        ).catch(swal.noop);
                        $("#btnSaveUserProfile").attr('disabled', false);
                        // window.location.href = "Account/Index";
                    }
                    else if (response.RstKey == 9) {

                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                        $("#btnSaveUserProfile").attr('disabled', false);
                        // window.location.href = "Account/Index";
                    } else if (response.RstKey == 2) {

                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                        $("#btnSaveUserProfile").attr('disabled', false);
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

    $('#txtStaffId').keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });


    function validate1() {
        debugger
        console.log("txtNinNo");
        var txtFirstName = $('#txtFirstName').val();
        var txtLastName = $('#txtLastName').val();
        var txtNinNo = $('#txtNinNo').val();
        var txtDateOfBirth = $('#txtDateOfBirth').val();
        var txtGender = gender;
        if (txtFirstName == '' || txtFirstName == "" || txtFirstName == null) {
            usernameError = false;
            $('#txtFirstNameError').show();
            $('#txtFirstNameError').text("Please enter your first name");
        }
        else {
            if (txtFirstName.length < 2) {
                $('#txtFirstNameError').show();
                $('#txtFirstNameError').text("Please enter atleast 2 characters");
                usernameError = false;
            } else {
                usernameError = true;
                $('#txtFirstNameError').hide();
            }

        }
        if (txtNinNo == '' || txtNinNo == "" || txtNinNo == null) {
            $('#txtNinNoError').show();
            $('#txtNinNoError').text("Please enter your NIN number.");
            NinNoError = false;
        } else {

            if (txtNinNo.length < 11) {
                $('#txtNinNoError').show();
                $('#txtNinNoError').text("Please enter 11 digit NIN number");
                NinNoError = false;
            }
            else if (txtNinNo == "00000000000") {
                $('#txtNinNoError').show();
                $('#txtNinNoError').text("NIN number is not valid");
                NinNoError = false;
            }
            else {
                NinNoError = true;
                $('#txtNinNoError').hide();
            }


        }
        if (txtDateOfBirth == '' || txtDateOfBirth == "" || txtDateOfBirth == null) {
            $('#txtDateOfBirthError').show();
            DateOfBirthError = false;
        } else {
            if (ValidateDOB1(txtDateOfBirth) == true) {
                DateOfBirthError = true;
                $('#txtDateOfBirthError').hide();
            }
            else {
                DateOfBirthError = false;
                $('#txtDateOfBirthError').hide();
            }
        }
        if (txtGender == '' || txtGender == "" || txtGender == null || txtGender === "Select") {
            $('#ddlGenderError').show();
            GenderError = false;
        } else {
            GenderError = true;
            $('#ddlGenderError').hide();
        }
        if (txtLastName == '' || txtLastName == "" || txtLastName == null && txtFirstName.length < 2) {
            $('#txtLastNameError').show();
            $('#txtLastNameError').text("Please enter your last name.");
            LastNameError = false;
        } else {
            if (txtLastName.length < 2) {
                $('#txtLastNameError').show();
                $('#txtLastNameError').text("Please enter atleast 2 characters");
                LastNameError = false;
            } else {
                LastNameError = true;
                $('#txtLastNameError').hide();
            }
        }
    }

    function validate2() {
        var txtState = state;
        var txtCity = city;
        var txtCountry = country;
        var txtAddress = $('#txtAddress').val();
        if (txtCountry == '' || txtCountry == "" || txtCountry == null || txtCountry === "Please select country" || countryval == "") {
            //$('#passcheck').show();
            $('#ddlcountryError').show();
            CountryError = false;
            // return false;
        } else {
            $('#ddlcountryError').hide();
            CountryError = true;
            // return true;
        }
        if (txtState == '' || txtState == "" || txtState == null || state === "Please select State" || stateval == "") {
            //$('#passcheck').show();
            $('#ddlStateError').show();
            StateError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            StateError = true;
            $('#ddlStateError').hide();
            // return true;
        }
        if (txtCity == '' || txtCity == "" || txtCity == null || txtCity === "Please select city" || cityval == "") {
            //$('#passcheck').show();
            $('#ddlCityError').show();
            CityError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            CityError = true;
            $('#ddlCityError').hide();
            // return true;
        }
        if (txtAddress == '' || txtAddress == "" || txtAddress == null) {
            $('#txtAddressError').show();
            $('#txtAddressError').text(" Please enter your addess.");
            AddressError = false;
            // return false;
        } else {

            if (txtAddress.length < 10) {
                $('#txtAddressError').show();
                $('#txtAddressError').text("Please enter atleast 10 characters");
                AddressError = false;
            } else {
                AddressError = true;
                $('#txtAddressError').hide();
            }

            // $('#passcheck').hide();
            //AddressError = true;
            //$('#txtAddressError').hide();
            // return true;
        }
    }

    function validate3() {
        var txtStaffId = $("#txtStaffId").val();
        if (employerName == '' || employerName == "" || employerName == null || employerName == "Please select employer" || employerId == "") {
            //$('#passcheck').show();
            $('#ddlEmployerError').show();
            $('#ddlEmployerError').text("Please select employer.");
            EmployerNameError = false;
            // return false;
        } else {
            $('#ddlcountryError').hide();
            EmployerNameError = true;
            // return true;
        }
        if (txtStaffId == '' || txtStaffId == "" || txtStaffId == null) {
            //$('#passcheck').show();
            $('#txtStaffIdError').show();
            $('#txtStaffIdError').text("Please enter staff id.");
            StaffIdError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            StaffIdError = true;
            $('#txtStaffIdError').hide();
            // return true;
        }
    }

    function validate4() {
        var txtAccountNumber = $("#txtAccountNumber").val();
        var txtBVN = $("#txtBVN").val();
        // var txtCustomerId = $("#txtCustomerId").val();
        var txtBankAccountHolderName = $("#txtBankAccountHolderName").val();

        if (bankName == '' || bankName == "" || bankName == null || bankName == "Please select bank") {
            //$('#passcheck').show();
            $('#ddlBankError').show();
            $('#ddlBankError').text("Please select bank.");
            BankNameError = false;
            // return false;
        } else {
            $('#ddlBankError').hide();
            BankNameError = true;
            // return true;
        }
        if (txtAccountNumber == '' || txtAccountNumber == "" || txtAccountNumber == null) {
            //$('#passcheck').show();
            $('#txtAccountNumberError').show();
            $('#txtAccountNumberError').text("Please enter account number.");
            AccountNumberError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            AccountNumberError = true;
            $('#txtAccountNumberError').hide();
            // return true;
        }
        if (txtBVN == '' || txtBVN == "" || txtBVN == null) {
            //$('#passcheck').show();
            $('#txtBVNError').show();
            $('#txtBVNError').text("Please enter BVN.");
            BVNError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            BVNError = true;
            $('#txtBVNError').hide();
            // return true;
        }
        //if (txtCustomerId == '' || txtCustomerId == "" || txtCustomerId == null) {
        //    //$('#passcheck').show();
        //    $('#txtCustomerIdError').show();
        //    $('#txtCustomerIdError').text("Please enter customer id.");
        //    customerIdError = false;
        //    // return false;
        //} else {
        //    // $('#passcheck').hide();
        //    customerIdError = true;
        //    $('#txtCustomerIdError').hide();
        //    // return true;
        //}
        if (txtBankAccountHolderName == '' || txtBankAccountHolderName == "" || txtBankAccountHolderName == null) {
            //$('#passcheck').show();
            $('#txtBankAccountHolderNameError').show();
            $('#txtBankAccountHolderNameError').text("Please enter username.");
            BankAccountHolderNameError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            BankAccountHolderNameError = true;
            $('#txtBankAccountHolderNameError').hide();
            // return true;
        }
    }

    $(function () {
        $('#browser').on('keypress', function (e) {
            $('.ddlNonRegisterEmployer').empty()
            var req = $("#browser").val()
            getNonRegisterEmployerList(req);
            // alert();
        });
    });
});