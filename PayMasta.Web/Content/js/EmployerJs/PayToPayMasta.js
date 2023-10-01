var searchParam = "";
var UserGuid = "";
let TotalRowCount = "";
var pageNumber = 1;
var pageSize = 10;
var FromDate = null;
var ToDate = null;
var status = -1;
let accounterror = true
let downloadInvoice = false
let accountnameerror = true
let bvnerror = true
var payAmt = 0;
$(document).ready(function () {
    // var id = sessionStorage.getItem("User1");
    $("#noRecordFound1").hide();
    UserGuid = sessionStorage.getItem("User1");
    getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
    getEmployeePayableAmount(UserGuid);


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
            getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, fromDateSelected, toDateSelected, status);
            // getEmployeeEWAList(UserGuid, pageNumber, PageSize, searchText, status, fromDateSelected, toDateSelected);
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
        getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, userStatus);

    });



    $(document).on('keyup', '#employeeSearch', function () {
        searchParam = $("#employeeSearch").val();
        if (searchParam.length >= 3) {
            getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
        }
        else if (searchParam.length == 0) {
            getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
        }

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
            url: "/PayToPayMasta/ExportCsvReport/",
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
                    link.download = "PayToPaymastaList.xlsx";
                    link.click();
                }
            }
        });
    });



    $("#btnPayToPayMasta").click(function () {
        var AccountHolderName = $("#txtAccountHolderName").val();
        var AccountNumber = $("#txtAccountNumber").val();
        var BvnNumber = $("#txtBvnNumber").val();
        if (AccountHolderName == '' || AccountHolderName == "" || AccountHolderName == undefined) {

            accountnameerror = false;
            swal(
                'Error!',
                'Please enter account holder name.',
                'error'
            ).catch(swal.noop);

        }
        else {
            accountnameerror = true;
        }
        if (AccountNumber == '' || AccountNumber == "" || AccountNumber == undefined) {
            accounterror = false;
            swal(
                'Error!',
                'Please enter account number.',
                'error'
            ).catch(swal.noop);
        } else {
            accounterror = true;
        }
        if (BvnNumber == '' || BvnNumber == "" || BvnNumber == undefined) {

            bvnerror = false;
            swal(
                'Error!',
                'Please enter bvn number.',
                'error'
            ).catch(swal.noop);
        } else {
            bvnerror = true;
        }
        if (bvnerror == true && accounterror == true && accountnameerror == true) {
            swal({
                title: "Are you sure you want to transfer the amount?",
                // text: "Warnning",
                icon: "warnning",
                buttons: false,
                showConfirmButton: true,
                showCancelButton: true,
                dangerMode: true,
            }, function succes(isDone) {
                var formData = new FormData();
                formData.append("UserGuid", UserGuid);
                formData.append("DebitAccount", AccountNumber);
                formData.append("AccountHolderName", AccountHolderName);
                formData.append("BVN", BvnNumber);
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/PayToPayMasta/PayToPaymasta/",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        getEmployeeEWAList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, status);
                        if (response.RstKey == 1) {
                            $("#employerPaymentDetails").modal('hide');
                            swal({
                                title: "Good job!",
                                text: "Your Payment to PayMasta has been successfully done.",
                                icon: "success",
                                button: "Aww yiss!"
                            });
                        } else if (response.RstKey == 2) {
                            swal({
                                title: "oops!",
                                text: "Error!",
                                icon: "error",
                                button: "Aww yiss!"
                            });
                        }
                        else if (response.RstKey == 3) {
                            swal({
                                title: "oops!",
                                text: response.Message,
                                icon: "error",
                                button: "Aww yiss!"
                            });
                        }
                    }
                });
            })
        } else {

        }

    })

    $("#btnShowPayModal").click(function () {
        if (payAmt > 0) {
            $("#employerPaymentDetails").modal('show');
        } else {
            swal(
                'Success!',
                'Already paid',
                'success'
            ).catch(swal.noop);
        }

    });

    $("#btnDownloadInvoice").click(function () {
        if (downloadInvoice === true) {
            var data = window.location.origin;
            window.location.href = data + "/PayToPayMasta/InvoiceGenerate?request=" + UserGuid
        }
        else {
            swal(
                'Error!',
                'You can download this invoice after pay cycle end.',
                'error'
            ).catch(swal.noop);
        }
    });
});


function getEmployeeEWAList(id, searchText, pageNumber, pageSize, FromDate, ToDate, status) {


    var formData = new FormData();
    formData.append("userGuid", id);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);
    formData.append("Status", status);

    $.ajax({
        type: "POST",
        cache: false,
        url: "PayToPayMasta/GetEmployeesEWARequestList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#userList').empty();
            let employerList = '';
            if (response.RstKey == 1) {

                downloadInvoice = response.isInvoiceDownload;
                $("#noRecordFound1").hide();
                $(response.accessAmountViewModels).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    var created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    employerList += '<tr>';
                    // employerList += '<td> <input type="checkbox" class="pay-to-paymasta-check" id="checkAll"></td>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + ' ' + value.LastName + '</td>';
                    employerList += '<td>' + value.StaffId + '</td>';
                    employerList += '<td>' + value.CountryCode + '-' + value.PhoneNumber + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + '₦' + value.AccessAmount + '</td>';
                    employerList += '<td>' + '₦' + value.CommissionCharge + '</td>';
                    employerList += '<td>' + '₦' + value.TotalAmountWithCommission + '</td>';
                    employerList += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    if (value.IsPaidToPayMastaId == true) {
                        employerList += '<td style="color:green">' + value.IsPaidToPayMasta + '</td>';
                    }
                    else if (value.IsPaidToPayMastaId == false) {
                        employerList += '<td style="color:red">' + value.IsPaidToPayMasta + '</td>';
                    }
                });
                $('#userList').append(employerList);
                page_number(TotalRowCount, pageNumber);


            } else {
                $("#btnExportToCSV").attr('disabled', true);
                page_number('', pageNumber);
                $("#noRecordFound1").show();
                //$("#emplyr_pagination").hide();

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
                getEmployeeEWAList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate, status);
            }
            else if (searchParam.length == 0) {
                getEmployeeEWAList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate, status);
            }
        }
    });
}

function getEmployeePayableAmount(id) {


    var formData = new FormData();
    formData.append("request", id);

    $.ajax({
        type: "POST",
        cache: false,
        url: "PayToPayMasta/GetPayAbleAmount",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            payAmt = parseInt(response.payableAmount.TotalAmount);
            if (response.RstKey == 1) {
                $("#divTotalUser").text(response.payableAmount.TotalUser);
                $("#divPayableAmt").text("₦" + response.payableAmount.TotalAmount);
            } else {

            }
        }
    });

}