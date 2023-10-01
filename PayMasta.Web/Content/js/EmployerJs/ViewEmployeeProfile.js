/*/*$("#tblEmployees").DataTable();*/
var userGuid;
$(document).ready(function () {

    var UserGuid = "";
    var pageNumber = 1;
    var PageSize = 10;
    var totalPages = 1;
    var searchText = '';
    UserGuid = sessionStorage.getItem("User1");
    $("#btnApproveProfile").hide();
    getUrlVars();
    // Read a page's GET URL variables and return them as an associative array.
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        var data = hash[1];
        userGuid = data;
        getEmployeeProfile(data)
        return vars;
    }

    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");


    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);

    $("#btnSaveNetPays").click(function () {
        var txtGrosspay = $("#grosspay").val();
        var txtNetGross = $("#netPay").val();
        var txtUserGuid = $("#userGuidHiddenField").val();
        var netPay = txtNetGross.substring(1).trim();
        var grossPay = txtGrosspay.substring(1).trim();

        if (netPay == "" || netPay == null || netPay == "0") {
            swal({
                title: "oops!",
                text: "Net pay should be greater then Zero",
                icon: "error",
                button: "ohh no!"
            });
            return false;
        }
        if (grossPay == "" || grossPay == null || grossPay == "0") {
            swal({
                title: "oops!",
                text: "Gross pay should be greater then Zero",
                icon: "error",
                button: "ohh no!"
            });
            return false;
        }

        var formData = new FormData();
        formData.append("EmployeeUserGuid", txtUserGuid);
        formData.append("EmployeerUserGuid", UserGuid);
        formData.append("NetPay", netPay);
        formData.append("GrossPay", grossPay);


        $.ajax({
            type: "POST",
            cache: false,
            url: "/Employees/UpdateNetAndGrossPay",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.RstKey == 1) {
                    swal({
                        title: "Success",
                        text: response.Message,
                        icon: "success",
                        button: "ohh Yes!"
                    });
                } else {
                    swal({
                        title: "oops!",
                        text: response.Message,
                        icon: "error",
                        button: "ohh no!"
                    });
                }
            }
        });
    });

    //$('body').on('click', 'span.view-employeeProfile', function (e) {
    //    e.preventDefault;

    //    var data = $(this).data('val')

    //    window.location.href = "/EmployeesTransactions/GetEmployeDetailByGuid/id=" + data;
    //});

    $("#btnViewTransactions").click(function () {
        var txtUserGuid = $("#userGuidHiddenField").val();
        window.location.href = "/EmployeesTransactions/GetEmployeDetailByGuid/id=" + txtUserGuid;

    })

    $("#btnApproveProfile").click(function () {
        swal({
            title: "Are you sure you want to verify this user?",
            // text: "Warnning",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            // $("#loader1").show();
            var formData = new FormData();
            formData.append("UserGuid", userGuid);
            formData.append("EmployerGuid", UserGuid);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Employees/ApproveUserProfile",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {

                    // $("#loader1").hide();
                    if (response.RstKey == 1) {
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                        //window.location.href = "Employees/Index";
                        window.location.reload();
                    } else {
                        swal({
                            title: "oops",
                            text: response.Message,
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        })
    });

});


function getEmployeeProfile(guid) {



    var formData = new FormData();
    formData.append("EmployeeUserGuid", guid);
    // formData.append("EmployeerUserGuid", UserGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Employees/ViewEmployeeProfile",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.ProfileImage == "") {
                $('.profile-pic').attr('src', '/Content/images/avatar.jpg');

            } else {
                $('.profile-pic').attr('src', response.ProfileImage);
                sessionStorage.setItem("ProfileImage", response.ProfileImage);
            }


            if (response.RstKey == 1) {
                $("#firstName").text(response.FirstName);
                $("#lastName").text(response.LastName);
                $("#Mobile").text(response.CountryCode + '-' + response.PhoneNumber);
                $("#gender").text(response.Gender);
                $("#netPay").val('₦ ' + response.NetPay);
                $("#middlename").text(response.MiddleName);
                $("#email").text(response.Email);
                $("#dob").text(response.DateOfBirth);
                $("#staffid").text(response.StaffId);
                $("#grosspay").val('₦ ' + response.GrossPay);
                $("#userGuidHiddenField").val(response.UserGuid);
                if (response.IsUserVerified == true) {
                    $("#btnApproveProfile").hide();
                }
                else {
                   // $("#btnApproveProfile").hide();
                    $("#btnApproveProfile").show();
                }
            } else {
                swal({
                    title: "oops!",
                    text: response.Message,
                    icon: "error",
                    button: "ohh no!"
                });
            }
        }
    });

}
