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

    //$(document).ready(function () {
    //    $('[data-toggle="tooltip"]').tooltip();
    //});









    $("#txtSearch").keyup(function () {
        $('#tblEmployees tbody').empty();
        var text = $("#txtSearch").val();
        getEmployeesList(UserGuid, pageNumber, PageSize, text, status, FromDate, ToDate);
        //getEmployeesList(UserGuid, pageNumber, PageSize, text);
    });


    $('body').on('click', 'span.view-employeeProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val')
        window.location.href = "UserBankDetail/UserBankDetails/id=" + data;
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
                    link.download = "EmployeeBankDetails.xlsx";
                    link.click();
                }
            }
        });
    });

    //$('#checkbox1').change(function () {
    //    if (this.checked) {
    //        var returnVal = confirm("Are you sure?");
    //        $(this).prop("checked", returnVal);
    //    }
    //    $('#textbox1').val(this.checked);
    //});
});

function getEmployeesList(userGuid, pageNumber, PageSize, searchText, status, fromDate, toDate) {
    $('#userBankDetails tbody').empty();
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
        url: "UserBankDetail/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#userBankDetails').empty();

            if (response.RstKey == 1) {
                $("#userBankNoRecord").hide();
                var employees = '';
                let isBlock = "";
                //totalPages = Math.floor(response.employeesListViewModel[0].TotalCount / PageSize);

                // page(totalPages)

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
                    employees += '<td>' + dateFormat + '</td>';
                    employees += '<td><span class="view-employeeProfile" data-val=' + value.UserGuid + '><img class="img-eye" src="../Content/images/Employer/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>' +'</td > ';
                    //employees += '</tbody>';
                    employees += '</tr>';
                });
                $('#userBankDetails').append(employees);

                page_number(TotalRowCount, pageNumber);
            }
            else if (response.RstKey == 2) {
                $("#userBankNoRecord").show();
                $("#btnExportToCSV").attr('disabled', true);
            } else {
                $("#userBankNoRecord").show();
                $("#btnExportToCSV").attr('disabled', true);
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

