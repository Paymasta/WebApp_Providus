var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var ServiceCategoryId = 0;
var Month = 0;
var totalPages = 1;
var searchText = '';
let TotalRowCount = "";

$(document).ready(function () {
    $("#transactionNoRecordFound").show();
    UserGuid = sessionStorage.getItem("User1");
    getTransactionHistoryList(UserGuid, pageNumber, PageSize, ServiceCategoryId, Month);
    getCategoryList(UserGuid);

    $('#ddlCategory').change(function () {

        ServiceCategoryId = $('select#ddlCategory option:selected').val();
        var value = $('select#ddlCategory option:selected').text();
        getTransactionHistoryList(UserGuid, pageNumber, PageSize, ServiceCategoryId, Month);

    });

    $('#ddlMonth').change(function () {

        Month = $('select#ddlMonth option:selected').val();
        var value = $('select#ddlMonth option:selected').text();
        getTransactionHistoryList(UserGuid, pageNumber, PageSize, ServiceCategoryId, Month);

    });
});


function getTransactionHistoryList(userGuid, pageNumber, PageSize, ServiceCategoryId, Month) {
    $('#tblEmployees tbody').empty();
    var formData = new FormData();
    formData.append("UserGuid", userGuid);
    formData.append("ServiceCategoryId", ServiceCategoryId);
    formData.append("Month", Month);
    formData.append("PageSize", PageSize);
    formData.append("PageNumber", pageNumber);
    $.ajax({
        type: "POST",
        cache: false,
        url: "Transactions/TransactionHistory",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#tblTransaction').empty();
            if (response.RstKey == 1) {
                $("#transactionNoRecordFound").hide();
                console.log(response);
                var transactions = '';

                //totalPages = Math.floor(response.employeesListViewModel[0].TotalCount / PageSize);

                // page(totalPages)

                $.each(response.getTransactionHistoryResponse, function (key, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    //employees += '<tbody>';
                    transactions += '<tr data-toggle="modal" data-target="#">';
                    transactions += '<td>' + value.RowNumber + '</td>';
                    transactions += '<td>' + dateFormat + '</td>';
                    transactions += '<td>' + value.Description + '</td>';
                    if (value.TransactionStatus == "Success") {
                        transactions += '<td style="color:#0CAF38">' + value.TransactionStatus + '</td>';
                    }
                    else if (value.TransactionStatus == "Pending") {
                        transactions += '<td style="color: #32A4FC">' + value.TransactionStatus + '</td>';
                    }
                    else if (value.TransactionStatus == "Rejected") {
                        transactions += '<td style="color: #EB2E0C">' + value.TransactionStatus + '</td>';
                    }
                    else if (value.TransactionStatus == "Failed") {
                        transactions += '<td style="color:red">' + value.TransactionStatus + '</td>';
                    }

                    transactions += '<td>₦' + value.TotalAmount + '</td>';
                    if (value.TransactionType.toUpperCase() === "DEBIT".toUpperCase()) {
                        transactions += '<td style="color:red">' + value.TransactionType + '</td>';
                    } else {
                        transactions += '<td style="color:green">' + value.TransactionType + '</td>';
                    }
                  
                    transactions += '</tr>';
                });
                $('#tblTransaction').append(transactions);

                page_number(TotalRowCount, pageNumber);
            }
            else if (response.RstKey == 2) {
                /* swal(
                     'Error!',
                     'No data foundsss.',
                     'error'
                 ).catch(swal.noop);*/

                $("#transactionNoRecordFound").show();
                page_number(0, pageNumber);
            } else {
                swal(
                    'Error!',
                    'Failed to fetch data.',
                    'error'
                ).catch(swal.noop);
                page_number(0, pageNumber);
            }
        }
    });

}

function page_number(count, currentPage) {
    $('#user_pagination').pagination({
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
                getTransactionHistoryList(UserGuid, options.current, PageSize, ServiceCategoryId, Month);
            }
            else if (searchText.length == 0) {
                getTransactionHistoryList(UserGuid, options.current, PageSize, ServiceCategoryId, Month);
            }
        }
    });
}

function getCategoryList(userGuid) {

    var formData = new FormData();
    formData.append("userGuid", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "Transactions/GetCategories",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                let ddOptions = '';
                $.each(response.categoryResponse, function (index, value) {
                    ddOptions += '<option value="' + value.Id + '">' + value.CategoryName + '</option>';
                });
                $('#ddlCategory').append(ddOptions);
            }
            else {
                //swal(
                //    'Error!',
                //    'Failed to fetch data.',
                //    'error'
                //).catch(swal.noop);
            }
        }
    });

}