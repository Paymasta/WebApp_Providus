let TotalRowCount = "";
var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var searchParam = '';
var status;
var FromDate = null;
var ToDate = null;
var monthName = '';
let isButtonShow = false;
$(document).ready(function () {
    $("#btnApprove").hide();
    $("#btnReject").hide();
    $("#btnHold").hide();
    $("#adminStatusth").hide()
    $("#no-record-found").hide();

    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);
    getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, FromDate, ToDate);

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });


    $(document).on('keyup', '#txtSearch', function () {
        searchParam = $("#txtSearch").val();
        if (searchParam.length >= 3) {
            getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, FromDate, ToDate);
        }
        else if (searchParam.length == 0) {
            getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, FromDate, ToDate);
        }

    });

    $('#ddlStatus').change(function () {
        if (isButtonShow == true) {
            $("#btnApprove").show();
            $("#btnReject").show();
            $("#btnHold").show();
        }

        status = $('select#ddlStatus option:selected').val();

        if (isButtonShow == true && status == -1) {
            $("#btnApprove").show();
            $("#btnReject").show();
            $("#btnHold").show();
        }
        else if (isButtonShow == true && status == 0) {
            $("#btnApprove").show();
            $("#btnReject").show();
            $("#btnHold").show();
        }
        else if (isButtonShow == true && status == 1) {
            $("#btnApprove").hide();
            $("#btnReject").hide();
            $("#btnHold").hide();

        }
        else if (isButtonShow == true && status == 3) {
            $("#btnApprove").hide();
            $("#btnReject").hide();
            $("#btnHold").hide();
        }
        else if (isButtonShow == true && status == 6) {
            $("#btnApprove").show();
            $("#btnReject").show();
            $("#btnHold").hide();
        }

        var monthName = $('select#ddlStatus option:selected').text();
        getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, FromDate, ToDate);
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

            //getEmployeeEWAList(UserGuid, pageNumber, pageSize, searchParam, fromDateSelected, toDateSelected);
            //getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, fromDateSelected, toDateSelected, status);
            getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, fromDateSelected, toDateSelected);
        }
    });



    $("#checkAll").click(function () {

        $("input[name='withdrawRequest']").attr("checked", this.checked);

    });

    $("#btnApprove").click(function () {

        var arrItem = [];
        var commaSeparetedId = '';
        $("#tblEmployees td input[type=checkbox]").each(function (index, val) {

            var checkId = $(val).attr("Id");
            var arr = checkId.split('_');
            var currentCheckboxId = arr[1];
            var IsChecked = $("#" + checkId).is(":checked", true);
            if (IsChecked) {
                arrItem.push(currentCheckboxId);
            }
            //if (arrItem.length != 0) {

            //}

        });
        if (arrItem.length != 0) {
            swal({
                title: "Are you sure you want to approve?",
                text: '',
                icon: "success",
                button: "Aww yiss!",
            }, function succes(isDone) {
                commaSeparetedId = arrItem.toString();
                var formData = new FormData();
                formData.append("UserGuid", UserGuid);
                formData.append("AccessAmountIds", commaSeparetedId);
                formData.append("Status", 1);
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "WithdrawRequest/UpdateAccessAmountRequestById",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, FromDate, ToDate);
                        if (response.RstKey == 1) {
                            swal({
                                title: "Good job!",
                                text: response.Message,
                                icon: "success",
                                button: "Aww yiss!"
                            });
                            window.location.reload();
                        }
                    }
                });
            });
        }

    });
    $("#btnReject").click(function () {

        var arrItem = [];
        var commaSeparetedId = '';
        $("#tblEmployees td input[type=checkbox]").each(function (index, val) {

            var checkId = $(val).attr("Id");
            var arr = checkId.split('_');
            var currentCheckboxId = arr[1];
            var IsChecked = $("#" + checkId).is(":checked", true);
            if (IsChecked) {
                arrItem.push(currentCheckboxId);
            }
            //if (arrItem.length != 0) {

            //}

        });
        if (arrItem.length != 0) {
            swal({
                title: "Are you sure you want to reject?",
                text: '',
                icon: "success",
                button: "Aww yiss!",
            }, function succes(isDone) {

                commaSeparetedId = arrItem.toString();
                var formData = new FormData();
                formData.append("UserGuid", UserGuid);
                formData.append("AccessAmountIds", commaSeparetedId);
                formData.append("Status", 3);
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "WithdrawRequest/UpdateAccessAmountRequestById",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, FromDate, ToDate);
                        if (response.RstKey == 1) {
                            swal({
                                title: "Good job!",
                                text: response.Message,
                                icon: "success",
                                button: "Aww yiss!"
                            });
                            window.location.reload();
                        }



                    }
                });
            });
        }
    });
    $("#btnHold").click(function () {

        var arrItem = [];
        var commaSeparetedId = '';
        $("#tblEmployees td input[type=checkbox]").each(function (index, val) {

            var checkId = $(val).attr("Id");
            var arr = checkId.split('_');
            var currentCheckboxId = arr[1];
            var IsChecked = $("#" + checkId).is(":checked", true);
            if (IsChecked) {
                arrItem.push(currentCheckboxId);
            }
            //if (arrItem.length != 0) {

            //}

        });
        if (arrItem.length != 0) {
            swal({
                title: "Are you sure you want to hold?",
                text: '',
                icon: "success",
                button: "Aww yiss!",
            }, function succes(isDone) {

                commaSeparetedId = arrItem.toString();
                var formData = new FormData();
                formData.append("UserGuid", UserGuid);
                formData.append("AccessAmountIds", commaSeparetedId);
                formData.append("Status", 6);
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "WithdrawRequest/UpdateAccessAmountRequestById",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        getEmployeesWithdrawsRequestList(UserGuid, pageNumber, PageSize, searchParam, status, FromDate, ToDate);
                        if (response.RstKey == 1) {
                            swal({
                                title: "Good job!",
                                text: response.Message,
                                icon: "success",
                                button: "Aww yiss!"
                            });
                            window.location.reload();
                        }
                    }
                });
            });
        }
    });
    $(document).on('click', '#fromDateIcon', function () {
        $('#txtFromDate').focus();
    });
    $(document).on('click', '#toDateIcon', function () {
        $('#txtEnddate').focus();
    });


    $("#btnExportToCSV").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();

        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("FromDate", fromDateSelected);
        formData.append("ToDate", toDateSelected);
        formData.append("Status", status);
        $.ajax({
            type: "POST",
            cache: false,
            url: "WithdrawRequest/ExportCsvReport",
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
                    link.download = "WithdrawlsList.xlsx";
                    link.click();
                }
            }
        });
    });

});

function getEmployeesWithdrawsRequestList(userGuid, pageNumber, PageSize, searchText, status, FromDate, ToDate) {
    $("#lower_button").show();
    $('#tblEmployees tbody').empty();
    $("#no-record-found").hide();

    var formData = new FormData();
    formData.append("userGuid", userGuid);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);
    formData.append("Status", status);
    $.ajax({
        type: "POST",
        cache: false,
        url: "WithdrawRequest/GetEmployeesWithdrawsRequestList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#tblEmployees').empty();
            if (response.RstKey == 1) {
                $("#no-record-found").hide();
                console.log(response);
                var employees = '';

                if (response.IsEwaApprovalAccess == true) {
                    $("#btnApprove").show()
                    $("#btnReject").show()
                    $("#btnHold").show()
                    $("#adminStatusth").show()
                    isButtonShow = true;
                }


                //page_number(TotalRowCount, pageNumber);
                $.each(response.employeesListViewModel, function (key, value) {
                    // employees += '<tbody>';
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    employees += '<tr>';
                    if (value.StatusId == 1 || value.StatusId == 3) {
                        employees += '<td><input type="checkbox" name="" class="withdraw-request-check" id="check_' + value.AccessAmountId + '"' + ' disabled></td>';
                    }
                    else if (response.IsEwaApprovalAccess == true) {
                        employees += '<td><input type="checkbox" name="withdrawRequest" class="withdraw-request-check" id="check_' + value.AccessAmountId + '"' + '></td>';
                    }
                    else {
                        employees += '<td><input type="checkbox" name="withdrawRequest" class="withdraw-request-check" disabled></td>';
                    }
                    //employees += '<td><input type="checkbox" name="withdrawRequest" class="check" id="check_' + value.AccessAmountId + '"' + '></td>';
                    employees += ' <td>' + value.RowNumber + '</td>';
                    employees += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employees += '<td>' + value.StaffId + '</td>';
                    employees += '<td>' + value.CountryCode + ' ' + value.PhoneNumber + '</td>';
                    employees += '<td>' + value.Email + '</td>';
                    employees += '<td>' + '₦' + value.AccessAmount + '</td>';
                    employees += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    if (response.IsEwaApprovalAccess == true) {
                        if (value.StatusId == 0 || value.StatusId == 2) {
                            employees += '<td style="color:#32A4FC">' + value.Status + '</td>';
                        }
                        //} else if (value.StatusId == 1) {
                        //    employerList += '<td style="color:yellow">' + value.Status + '</td>';
                        //}
                        else if (value.StatusId == 1) {
                            employees += '<td style="color:#0CAF38">' + value.Status + '</td>'; //approved

                        } else if (value.StatusId == 3) {
                            employees += '<td style="color:#EB2E0C">' + value.Status + '</td>'; //rejected
                        }
                        else if (value.StatusId == 6) {
                            employees += '<td style="color:orange">' + value.Status + '</td>';
                        }

                        if (value.AdminStatusId == 0 || value.AdminStatusId == 2) {
                            employees += '<td style="color:#32A4FC">' + value.AdminStatus + '</td>';
                        }
                        //} else if (value.StatusId == 1) {
                        //    employerList += '<td style="color:yellow">' + value.Status + '</td>';
                        //}
                        else if (value.AdminStatusId == 1) {
                            employees += '<td style="color:#0CAF38">' + value.AdminStatus + '</td>'; //approved

                        } else if (value.AdminStatusId == 3) {
                            employees += '<td style="color:#EB2E0C">' + value.AdminStatus + '</td>'; //rejected
                        }
                        else if (value.AdminStatusId == 6) {
                            employees += '<td style="color:orange">' + value.AdminStatus + '</td>';
                        }
                    }
                    if (response.IsEwaApprovalAccess == false) {
                        if (value.AdminStatusId == 0 || value.AdminStatusId == 2) {
                            employees += '<td style="color:#32A4FC">' + value.AdminStatus + '</td>';
                        }
                        //} else if (value.StatusId == 1) {
                        //    employerList += '<td style="color:yellow">' + value.Status + '</td>';
                        //}
                        else if (value.AdminStatusId == 1) {
                            employees += '<td style="color:#0CAF38">' + value.AdminStatus + '</td>'; //approved

                        } else if (value.AdminStatusId == 3) {
                            employees += '<td style="color:#EB2E0C">' + value.AdminStatus + '</td>'; //rejected
                        }
                        else if (value.AdminStatusId == 6) {
                            employees += '<td style="color:orange">' + value.AdminStatus + '</td>';
                        }
                    }
                    
                    //  employees += '</tbody>';
                    employees += '</tr>';
                });
                $('#tblEmployees').append(employees);
                page_number(TotalRowCount, pageNumber);
                $("#no-record-found").hide();

            }
            else if (response.RstKey == 2) {
                $("#btnExportToCSV").attr('disabled', true);
                $("#no-record-found").show();
                $("#lower_button").hide();
                page_number(0, pageNumber);
            } else {
                $("#btnExportToCSV").attr('disabled', true);
                $("#no-record-found").show();
                $("#lower_button").hide();
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
            if (searchParam.length >= 3) {
                getEmployeesWithdrawsRequestList(UserGuid, options.current, PageSize, searchParam, status, FromDate, ToDate);
            }
            else if (searchParam.length == 0) {

                getEmployeesWithdrawsRequestList(UserGuid, options.current, PageSize, searchParam, status, FromDate, ToDate);
            }
        }
    });
}