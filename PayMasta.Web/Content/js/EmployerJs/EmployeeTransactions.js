var searchParam = "";
var UserGuid = "";
let TotalRowCount = "";
var pageNumber = 1;
var pageSize = 10;
var FromDate = null;
var ToDate = null;
var month = 0;
$("#trans-no-record").hide();
$(document).ready(function () {
    // var id = sessionStorage.getItem("User1");
    UserGuid = sessionStorage.getItem("User1");
    getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, month);

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

            //getEmployeeEWAList(UserGuid, pageNumber, pageSize, searchParam, fromDateSelected, toDateSelected);
            getTransactionList(UserGuid, searchParam, pageNumber, pageSize, fromDateSelected, toDateSelected, status);
        }
    });

    $("#btnReset").click(function () {
        if (FromDate != "") {
            window.location.reload();
        }

    });

    $(document).on('keyup', '#employeeSearch', function () {
        searchParam = $("#employeeSearch").val();
        if (searchParam.length >= 3) {
            getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, month);
        }
        else if (searchParam.length == 0) {
            getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, month);
        }

    });

    $('#ddlMonth').change(function () {
        month = $('select#ddlMonth option:selected').val();
        var monthName = $('select#ddlMonth option:selected').text();
        getTransactionList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate, month);
        // getEmployeeEWAList(userId, null, null, monthNumber)
    });

    $('body').on('click', 'span.view-employeeProfile', function (e) {
        e.preventDefault;

        var data = $(this).data('val')

        window.location.href = "/EmployeesTransactions/GetEmployeDetailByGuid/id=" + data;
    });
   

    $("#btnExportToCSV").click(function () {
        var fromDateSelected = $("#txtFromDate").val();
        var toDateSelected = $("#txtEnddate").val();
       

        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("DateFrom", fromDateSelected);
        formData.append("DateTo", toDateSelected);
        formData.append("Month", month);
        formData.append("PageNumber", 1);
        formData.append("PageSize", 10);
        $.ajax({
            type: "POST",
            cache: false,
            url: "EmployeesTransactions/ExportCsvReport",
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
                    link.download = "Transactions.xlsx";
                    link.click();
                }
            }
        });
    });
    $("#btnPaycycle").click(function () {
      
        var formData = new FormData();
        formData.append("userGuid", UserGuid);
      
        $.ajax({
            type: "POST",
            cache: false,
            url: "EmployeesTransactions/ExportCsvReportForPayCycle",
            contentType: false,
            processData: false,
            data: formData,
            //beforeSend: function () {
            //    alert();
            //},
            success: function (response) {
                if (response != 1) {
                    var bytes = new Uint8Array(response.FileContents);
                    var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = "PayCycleData.xlsx";
                    link.click();
                }
                else {

                }
            }
        });
    });
});


function getTransactionList(id, searchText, pageNumber, pageSize, FromDate, ToDate, month) {


    var formData = new FormData();
    formData.append("userGuid", id);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", pageSize);
    formData.append("FromDate", FromDate);
    formData.append("ToDate", ToDate);
    formData.append("month", month);
    $.ajax({
        type: "POST",
        cache: false,
        url: "EmployeesTransactions/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        //beforeSend: function () {
        //    alert();
        //},
        success: function (response) {
            $('#userList').empty();

            let employerList = '';
            if (response.RstKey == 1) {
                $("#trans-no-record").hide();
                $(response.employeesListForTransactions).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + " " + value.LastName + '</td>';
                    employerList += '<td>' + value.StaffId + '</td>';
                    employerList += '<td>' + value.CountryCode + " " + value.PhoneNumber + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td title=' + dateFormat + '>' + dateFormat + '</td>';
                    employerList += '<td><span id="view-employeeProfile" class="view-employeeProfile" data-val=' + value.UserGuid + '><img class="img-eye" src="Content/images/Employer/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    employerList += '</tr>';

                });
                $('#userList').append(employerList);
                page_number(TotalRowCount, pageNumber);
               
            } else {
                $("#btnExportToCSV").attr('disabled', true);
                $("#trans-no-record").show();
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
                getTransactionList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate, month);
            }
            else if (searchParam.length == 0) {
                getTransactionList(UserGuid, searchParam, options.current, pageSize, FromDate, ToDate, month);
            }
        }
    });
}

