var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
let TotalRowCount = "";
var status = -1;
var FromDate = null;
var ToDate = null;
var monthName = '';
var searchParam = '';
var userStatus = -1;
let isvalidFileFormat = true;
$(document).ready(function () {

    $("#loaderEmployees").hide();
    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);
    getEmployeesList(UserGuid, pageNumber, PageSize, searchText, status, FromDate, ToDate);
    getEmployeesListError(UserGuid, pageNumber, PageSize, searchText, status, FromDate, ToDate);
    //$(document).ready(function () {
    //    $('[data-toggle="tooltip"]').tooltip();
    //});
    getQoreIdBankList();








    $("#txtSearch").keyup(function () {
        $("#loaderEmployees").show();
        $('#tblEmployees tbody').empty();
        var text = $("#txtSearch").val();
        getEmployeesList(UserGuid, pageNumber, PageSize, text, status, FromDate, ToDate);
        //getEmployeesList(UserGuid, pageNumber, PageSize, text);
    });


    $('body').on('click', 'span.block-employee', function (e) {
        e.preventDefault;


        var data = $(this).data('val')
        var UserGuid = sessionStorage.getItem("User1");
        var msg = $(this).attr('data-msg');
        swal({
            title: "Are you sure you want to " + msg + " employee?",
            text: "",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", data);
            formData.append("EmployeerUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 1);
            $("#loaderEmployees").show();
            $.ajax({
                type: "POST",
                cache: false,
                url: "Employees/BlockUnBlockEmployees",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $("#loaderEmployees").hide();
                    if (response.RstKey == 1) {
                        getEmployeesList(UserGuid, pageNumber, PageSize, searchText)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Employee not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $('body').on('click', 'span.delete-employee', function (e) {
        e.preventDefault;

        var data = $(this).data('val')
        var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you sure you want to delete employee?",
            text: "",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", data);
            formData.append("EmployeerUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 2);

            $.ajax({
                type: "POST",
                cache: false,
                url: "Employees/BlockUnBlockEmployees",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        getEmployeesList(UserGuid, pageNumber, PageSize, searchText)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Employee not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

    $('body').on('click', 'span.view-employeeProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val')
        window.location.href = "Employees/ViewEmployeeProfile/id=" + data;
    });


    //------------------date filter
    var startDate = new Date('01/01/2012');
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    // ToEndDate.setDate(ToEndDate.getDate() + 365);
    ToEndDate.setDate(ToEndDate.getDate());

    $('#txtFromDate').datepicker({
        weekStart: 1,
        startDate: '01/01/2012',
        endDate: FromEndDate,
        autoclose: true,
        format: "MM/dd/yyyy"
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        // fromDate = selected.date.valueOf();
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('#txtEnddate').datepicker('setStartDate', startDate);
    });
    $('#calender_from').on("click", function () {
        $('#txtFromDate').focus();
    });

    $('#txtEnddate').datepicker({
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true,
        format: "MM/dd/yyyy"
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        //toDate = FromEndDate.parse('dd-mm-yy') ;
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('#txtFromDate').datepicker('setEndDate', FromEndDate);
    });
    $('#calender_to').on("click", function () {
        $('#txtEnddate').focus();
    });

    $("#btnFilter").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();
        var isFromdateTrue = true;
        var isTodateTrue = true;
        if (fromDateSelected == '' || fromDateSelected == "" || fromDateSelected == null) {
            isFromdateTrue = false
            $("#fromdateError").show();
            $("#fromdateError").text("Please select from date");
        }
        else {
            isFromdateTrue = true
            $("#fromdateError").hide();
        }
        if (toDateSelected == '' || toDateSelected == "" || toDateSelected == null) {
            isTodateTrue = false;
            $("#TodateError").show();
            $("#TodateError").text("Please select to date");
        }
        else {
            isTodateTrue = true;
            $("#TodateError").hide();
        }
        if (isFromdateTrue == true && isTodateTrue == true) {

            getEmployeesList(UserGuid, pageNumber, PageSize, searchText, status, fromDateSelected, toDateSelected);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $('#ddlStatus').change(function () {
        userStatus = $('select#ddlStatus option:selected').val();
        var value = $('select#ddlStatus option:selected').text();
        getEmployeesList(UserGuid, pageNumber, PageSize, searchText, userStatus, FromDate, ToDate);

    });

    $("#btnExportToCSV").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();
        var isFromdateTrue = true;
        var isTodateTrue = true;


        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("FromDate", fromDateSelected);
        formData.append("ToDate", toDateSelected);
        formData.append("Status", status);
        formData.append("pageNumber", 1);
        formData.append("PageSize", 10);
        $.ajax({
            type: "POST",
            cache: false,
            url: "Employees/ExportCsvReport",
            contentType: false,
            processData: false,
            data: formData,
            //beforeSend: function () {
            //    alert();
            //},
            success: function (response) {
                if (response != "") {
                    var bytes = new Uint8Array(response.FileContents);
                    var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = "EmployeesList.xlsx";
                    link.click();
                }
            }
        });
    });

    $("#uploadMmodalFileBrowse").on('change', function () {
        var ext = $('#uploadMmodalFileBrowse').val().split('.').pop().toLowerCase();
        var fimeName = $('#uploadMmodalFileBrowse')[0].files[0];
        if ($.inArray(ext, ['xlsx']) == -1) {
            alert('invalid extension!,You can upload image in formats(xlsx)');
            isvalidFileFormat = false;
            return false;
        } else {
            $("#lblfilename").text(fimeName.name);
        }


    });

    $("#uploadMmodalFileBrowse").on('click', function () {

        $("#uploadMmodalFileBrowse").click();


    });
    $("#btnDownload").on('click', function () {
        window.location.href = "https://paymasta.co/HtmlTemplates/BulkUploadUser.xlsx"
    });
    $("#btnClose").click(function () {
        window.location.reload();
    });


    $("#btnUploadCsv").click(function () {
        $("#loaderEmployees").show();
        $('#btnUploadCsv').prop('disabled', true);
        var fi = $('#uploadMmodalFileBrowse')[0].files[0];
        var formData = new FormData();
        formData.append('file', $('#uploadMmodalFileBrowse')[0].files[0]);
        formData.append("guid", UserGuid);
        if (isvalidFileFormat == true) {
            $.ajax({
                type: "POST",
                cache: false,
                url: "Employees/UploadUserCsv",
                contentType: false,
                processData: false,
                data: formData,
                //beforeSend: function () {
                //    alert();
                //},
                success: function (response) {
                    if (response.RstKey == 1) {
                        $("#loaderEmployees").hide();
                        $("#uploadUserListModal").modal('hide');

                        swal({
                            title: "Good job!",
                            text: "Data uploaded successfully" + " Success Record=" + response.Sucess + " Failed=" + response.Error,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else if (response.RstKey == 3) {
                        $("#loaderEmployees").hide();

                        swal({
                            title: "Oops!",
                            text: "column not matched",
                            icon: "error",
                            button: "oh no!"
                        });

                        $('#btnUploadCsv').prop('disabled', false);
                    }
                    else if (response.RstKey == 10) {
                        $("#loaderEmployees").hide();
                        $('#btnUploadCsv').prop('disabled', false);

                        swal({
                            title: "Oops!",
                            text: "Duplicate data exists in the uploaded file.",
                            icon: "error",
                            button: "oh no!"
                        });
                    } else if (response.RstKey == 11) {
                        $('#btnUploadCsv').prop('disabled', false);

                        swal({
                            title: "Oops!",
                            text: "Duplicate data exists in the nin column.",
                            icon: "error",
                            button: "oh no!"
                        });
                    }
                    else if (response.RstKey == 12) {
                        $('#btnUploadCsv').prop('disabled', false);

                        swal({
                            title: "Oops!",
                            text: "Nin number is aleady exists.",
                            icon: "error",
                            button: "oh no!"
                        });
                    }
                    else {
                        swal({
                            title: "Oops!",
                            text: "Contains empty cells ",
                            icon: "error",
                            button: "oh no!"
                        });
                    }
                }
            });
        } else {
            swal({
                title: "Oops!",
                text: "File extension does not support",
                icon: "error",
                button: "oh no!"
            });
        }
    });


    $('body').on('click', 'span.delete-employeeError', function (e) {
        e.preventDefault;
        debugger
        var data = $(this).data('val2')
        var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you sure you want to delete employee?",
            text: "",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", data);
            formData.append("EmployeerUserGuid", UserGuid);
            formData.append("DeleteOrBlock", 2);

            $.ajax({
                type: "POST",
                cache: false,
                url: "/Employees/BlockUnBlockEmployeesError/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        getEmployeesListError(UserGuid, pageNumber, PageSize, searchText)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Employee not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

});

function getEmployeesList(userGuid, pageNumber, PageSize, searchText, status, fromDate, toDate) {
    $('#tblEmployees tbody').empty();
    var formData = new FormData();
    formData.append("userGuid", userGuid);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);
    formData.append("FromDate", fromDate);
    formData.append("ToDate", toDate);
    formData.append("Status", status);
    $.ajax({
        type: "POST",
        cache: false,
        url: "Employees/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#tblEmployees').empty();

            if (response.RstKey == 1) {
                $("#loaderEmployees").hide();
                $("#user-no-record").hide();
                var employees = '';
                let isBlock = "";
                //totalPages = Math.floor(response.employeesListViewModel[0].TotalCount / PageSize);


                $.each(response.employeesListViewModel, function (key, value) {

                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    //employees += '<tbody>';
                    employees += '<tr>';
                    employees += '<td>' + value.RowNumber + '</td>';
                    employees += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employees += '<td>' + value.StaffId + '</td>';
                    employees += '<td>' + value.CountryCode + ' ' + value.PhoneNumber + '</td>';
                    employees += '<td>' + value.Email + '</td>';
                    if (value.Status == 'Active') {
                        employees += '<td style="color:forestgreen">' + value.Status + '</td>'; isBlock = "block";
                    } else {
                        employees += '<td style="color:red">' + value.Status + '</td>'; isBlock = "unblock";
                    }
                    if (value.IsverifiedByEmployer == true) {
                        employees += '<td style="color:forestgreen">Approved</td>'; isBlock = "block";
                    } else {
                        employees += '<td style="color:red">Not Approved</td>'; isBlock = "unblock";
                    }
                    employees += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    employees += '<td><span class="view-employeeProfile" data-val=' + value.UserGuid + '><img class="img-eye" src="../Content/images/Employer/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>' +
                        '<span class="delete-employee" data-val=' + value.UserGuid + '><img class="img-eye" src ="../Content/images/Employer/delete.png" alt = "" data-toggle="tooltip" title="Delete"></span>' +
                        '<span class="block-employee" data-val=' + value.UserGuid + ' data-msg=' + isBlock + '><img class="img-eye" src ="../Content/images/Employer/close.png"  alt = "" data-toggle="tooltip" title="Block"></span> </td > ';
                    //employees += '</tbody>';
                    employees += '</tr>';
                });
                $('#tblEmployees').append(employees);

                page_number(TotalRowCount, pageNumber);
            }
            else if (response.RstKey == 2) {
                $("#btnExportToCSV").attr('disabled', true);
                $("#user-no-record").show();
                page_number(0, pageNumber);
            } else {
                $("#user-no-record").show();
                $("#btnExportToCSV").attr('disabled', true);
                page_number(0, pageNumber);
            }
        }
    });

}

function page_number(count, currentPage) {
    $('#emplyr_pagination').pagination({
        total: count,
        current: currentPage,
        length: 10,
        size: 2,
        /**
         * [click description]
         * @param  {[object]} options = {
         *      current: options.current,
         *      length: options.length,
         *      total: options.total
         *  }
         * @param  {[object]} $target [description]
         * @return {[type]}         [description]
         */
        click: function (options, $target) {
            console.log(options);
            pageNumber = options.current;
            if (searchText.length >= 3) {
                getEmployeesList(UserGuid, options.current, PageSize, searchText);
            }
            else if (searchText.length == 0) {
                getEmployeesList(UserGuid, options.current, PageSize, searchText);
            }
        }
    });
}

function getQoreIdBankList() {
    var formData = new FormData();
    formData.append("guid", 0);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/RegisterWithNuban/BankListForRegister/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            debugger
            let ddlCountryOptions = "";// '<option value="">Please select city</option>';
            // $('#ddlCity').append(ddlCountryOptions);
            $.each(response.bankListResponse.data, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddlCountryOptions += '<option value="' + value.QoreIdBankCode + '">' + value.bankName + '(Bank code-' + value.QoreIdBankCode + ')' + '</option>';
            });
            $('#dllBankList').append(ddlCountryOptions);
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

function getEmployeesListError(userGuid, pageNumber, PageSize, searchText, status, fromDate, toDate) {
    $('#tblEmployeesError tbody').empty();
    var formData = new FormData();
    formData.append("userGuid", userGuid);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);
    formData.append("FromDate", fromDate);
    formData.append("ToDate", toDate);
    formData.append("Status", status);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Employees/GetEmployeesListError/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#tblEmployeesError').empty();

            if (response.RstKey == 1) {
                $("#loaderEmployees").hide();
                $("#user-no-record_Uploaded").hide();
                var employees = '';
                let isBlock = "";
                //totalPages = Math.floor(response.employeesListViewModel[0].TotalCount / PageSize);


                $.each(response.employeesListViewModel, function (key, value) {

                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    //employees += '<tbody>';
                    employees += '<tr>';
                    employees += '<td>' + value.RowNumber + '</td>';
                    employees += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employees += '<td>' + value.StaffId + '</td>';
                    employees += '<td>' + value.CountryCode + ' ' + value.PhoneNumber + '</td>';
                    employees += '<td>' + value.Email + '</td>';
                    //if (value.Status == 'Active') {
                    //    employees += '<td style="color:forestgreen">' + value.Status + '</td>'; isBlock = "block";
                    //} else {
                    //    employees += '<td style="color:red">' + value.Status + '</td>'; isBlock = "unblock";
                    //}
                    employees += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    employees += '<td>' +
                        '<span class="delete-employeeError" data-val2=' + value.UserGuid + '><img class="img-eye" src ="../Content/images/Employer/delete.png" alt = "" data-toggle="tooltip" title="Delete"></span>' +
                        '</td > ';
                    //employees += '</tbody>';
                    employees += '</tr>';
                });
                $('#tblEmployeesError').append(employees);

                page_number2(TotalRowCount, pageNumber);
            }
            else if (response.RstKey == 2) {
                $("#btnExportToCSV").attr('disabled', true);
                $("#user-no-record_Uploaded").show();
                page_number2(0, pageNumber);
            } else {
                $("#user-no-record_Uploaded").show();
                $("#btnExportToCSV").attr('disabled', true);
                page_number2(0, pageNumber);
            }
        }
    });

}

function page_number2(count, currentPage) {
    $('#emplyr_pagination').pagination({
        total: count,
        current: currentPage,
        length: 10,
        size: 2,
        /**
         * [click description]
         * @param  {[object]} options = {
         *      current: options.current,
         *      length: options.length,
         *      total: options.total
         *  }
         * @param  {[object]} $target [description]
         * @return {[type]}         [description]
         */
        click: function (options, $target) {
            console.log(options);
            pageNumber = options.current;
            if (searchText.length >= 3) {
                getEmployeesListError(UserGuid, options.current, PageSize, searchText);
            }
            else if (searchText.length == 0) {
                getEmployeesListError(UserGuid, options.current, PageSize, searchText);
            }
        }
    });
}