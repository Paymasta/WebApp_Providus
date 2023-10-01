
var companySize = "";

let errorfirstName = true;
let errortxtRequestDemoLastName = true
let errortxtRequestDemoEmail = true
let errortxtRequestDemoPhone = true
let errortxtRequestDemoJobTitle = true
let errortxtRequestDemoCompany = true
let errortxtRequestDemoDetail = true

$(document).ready(function () {



    $('#ddlCompanySize').change(function () {

        companySize = $('select#ddlCompanySize option:selected').val();
        var value = $('select#ddlCompanySize option:selected').text();

    });

    // getFaq();

    $("#btnSendRequestDemo").click(function () {
        var firstName = $("#txtRequestDemoFirstName").val();
        var txtRequestDemoLastName = $("#txtRequestDemoLastName").val();
        var txtRequestDemoEmail = $("#txtRequestDemoEmail").val();
        var txtRequestDemoPhone = $("#txtRequestDemoPhone").val();
        var txtRequestDemoJobTitle = $("#txtRequestDemoJobTitle").val();
        var txtRequestDemoCompany = $("#txtRequestDemoCompany").val();
        var txtRequestDemoDetail = $("#txtRequestDemoDetail").val();
        debugger
        if (firstName == '' || firstName == "" || firstName == null && firstName.length < 2) {
            errorfirstName = false;//
            $("#errortxtRequestDemoFirstName").text("Please enter first name.");
            $("#errortxtRequestDemoFirstName").show();
        }
        else {
            errorfirstName = true;
            $("#errortxtRequestDemoFirstName").hide();
        }
        if (txtRequestDemoLastName == '' || txtRequestDemoLastName == "" || txtRequestDemoLastName == null && txtRequestDemoLastName.length < 2) {
            errortxtRequestDemoLastName = false;//
            $("#errortxtRequestDemoLastName").text("Please enter last name.");
            $("#errortxtRequestDemoLastName").show();
        }
        else {
            errortxtRequestDemoLastName = true;
            $("#errortxtRequestDemoLastName").hide();
        }
        if (txtRequestDemoEmail == '' || txtRequestDemoEmail == "" || txtRequestDemoEmail == null && txtRequestDemoEmail.length < 2) {
            errortxtRequestDemoEmail = false;//
            $("#errortxtRequestDemoEmail").text("Please enter email.");
            $("#errortxtRequestDemoEmail").show();
        }
        else {
            errortxtRequestDemoEmail = true;
            $("#errortxtRequestDemoEmail").hide();
        }
        if (txtRequestDemoPhone == '' || txtRequestDemoPhone == "" || txtRequestDemoPhone == null && txtRequestDemoPhone.length < 2) {
            errortxtRequestDemoPhone = false;//
            $("#errortxtRequestDemoPhone").text("Please enter mobile number.");
            $("#errortxtRequestDemoPhone").show();
        }
        else {
            $("#errortxtRequestDemoPhone").hide();
            errortxtRequestDemoPhone = true;
        }
        if (txtRequestDemoJobTitle == '' || txtRequestDemoJobTitle == "" || txtRequestDemoJobTitle == null && txtRequestDemoJobTitle.length < 2) {
            errortxtRequestDemoJobTitle = false;//
            $("#errortxtRequestDemoJobTitle").text("Please enter job title.");
            $("#errortxtRequestDemoJobTitle").show();
        }
        else {
            errortxtRequestDemoJobTitle = true;
            $("#errortxtRequestDemoJobTitle").hide();
        }
        if (txtRequestDemoCompany == '' || txtRequestDemoCompany == "" || txtRequestDemoCompany == null && txtRequestDemoCompany.length < 2) {
            errortxtRequestDemoCompany = false;//
            $("#errortxtRequestDemoCompany").text("Please enter company name.");
            $("#errortxtRequestDemoCompany").show();
        }
        else {
            errortxtRequestDemoCompany = true;
            $("#errortxtRequestDemoCompany").hide();
        }
        if (txtRequestDemoDetail == '' || txtRequestDemoDetail == "" || txtRequestDemoDetail == null && txtRequestDemoDetail.length < 2) {
            errortxtRequestDemoDetail = false;//
            $("#errortxtRequestDemoDetail").text("Please enter description.");
            $("#errortxtRequestDemoDetail").show();
        }
        else {
            errortxtRequestDemoDetail = true;
            $("#errortxtRequestDemoDetail").hide();
        }
        if (companySize == '' || companySize == "" || companySize == null || companySize == "0") {
            errortxtRequestDemoDetail = false;//
            $("#errorddlCompanySize").text("Please enter company size.");
            $("#errorddlCompanySize").show();
        }
        else {
            errortxtRequestDemoDetail = true;
            $("#errorddlCompanySize").hide();
        }

        if (errorfirstName && errortxtRequestDemoLastName && errortxtRequestDemoEmail && errortxtRequestDemoPhone && errortxtRequestDemoJobTitle && errortxtRequestDemoCompany && errortxtRequestDemoDetail) {
            var formData = new FormData();
            formData.append("FirrstName", firstName);
            formData.append("LastName", txtRequestDemoLastName);
            formData.append("Email", txtRequestDemoEmail);
            formData.append("PhoneNumber", txtRequestDemoPhone);
            formData.append("JobTitle", txtRequestDemoJobTitle);
            formData.append("CompanyName", txtRequestDemoCompany);
            formData.append("CompanySize", companySize);
            formData.append("Detail", txtRequestDemoDetail);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/CMS/RequestDemo",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {

                        var firstName = $("#txtRequestDemoFirstName").val('');
                        var txtRequestDemoLastName = $("#txtRequestDemoLastName").val('');
                        var txtRequestDemoEmail = $("#txtRequestDemoEmail").val('');
                        var txtRequestDemoPhone = $("#txtRequestDemoPhone").val();
                        var txtRequestDemoJobTitle = $("#txtRequestDemoJobTitle").val('');
                        var txtRequestDemoCompany = $("#txtRequestDemoCompany").val('');
                        var txtRequestDemoDetail = $("#txtRequestDemoDetail").val('');
                        $('#ddlCompanySize').empty();


                        window.location.reload();
                        swal(
                            'Success!',
                            'Request sent to the admin.',
                            'success'
                        ).catch(swal.noop);
                    }
                    else if (response == "") {
                        swal(
                            'Error!',
                            'Mobile already exists.',
                            'error'
                        ).catch(swal.noop);
                    }

                }
            });
        } else {

        }
    });

});
