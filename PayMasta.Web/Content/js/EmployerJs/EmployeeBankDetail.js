var UserGuid = "";
var EmployerGuid = "";
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
    EmployerGuid = sessionStorage.getItem("User1");
    getUrlVars();
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        UserGuid = hash[1];
        getEmployeesBankList(EmployerGuid, UserGuid, pageNumber, PageSize, searchText, status, FromDate, ToDate);
        return vars;
    }
})
function salaryAccount(g, bankId, userId) {
    if ($(g).is(":checked")) {
        swal({
            title: "Are you sure you want to set this account as salary account?",
            // text: "Warnning",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("UserId", userId);
            formData.append("BankDetailId", bankId);
            formData.append("EmployerGuid", EmployerGuid);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/UserBankDetail/SetSalaryAccount/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        window.location.reload();
                        swal({
                            title: "Good job!",
                            text: "Successfully!",
                            icon: "success",
                            button: "Aww yiss!"
                        });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Error!",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        })
    } else {
        swal({
            title: "oops!",
            text: "This account is already set as salary account!",
            icon: "error",
            button: "Aww yiss!"
        });
    }
    
}
function getEmployeesBankList(employerGuid, UserGuid, pageNumber, PageSize, searchText, status, fromDate, toDate) {
    $('#tblBankEmployees tbody').empty();
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    formData.append("EmployerGuid", employerGuid);
    formData.append("SearchTest", searchText);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);
    formData.append("FromDate", fromDate);
    formData.append("ToDate", toDate);
    formData.append("Status", status);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/UserBankDetail/GetEmployeesBankListByEmployerId/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#divEmployeeBanks').empty();
            if (response.RstKey == 1) {
                $("#user-no-record1").hide();
                var employees = '';

                $.each(response.employeesListViewModel, function (key, value) {

                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    employees += '<div class="employee-banks-sub-container">'
                    employees += '<div class="row">'
                    employees += '<div class="col-lg-6 col-md-6 col-sm-6 col-xs-12" >'
                    employees += '<div class="row left-column" >'
                    employees += '<p class="para bank-name-wrapper">'
                    if (value.ImageUrl != null) {

                        employees += '<img src="' + value.ImageUrl + '" width="22" height="22" /><span class=" bank-name-padding">' + value.BankName + '</span>'
                    } else {
                        employees += '<img src="/Content/images/add-account-img.png" width="22" height="22" /><span class=" bank-name-padding">' + value.BankName + '</span>'
                    }

                    employees += ' </p>'
                    employees += ' <div class="col-md-6 col-sm-6 col-xs-6  name-data">'
                    employees += '<p class="para">Bank Account Holder Name</p>'
                    employees += '<p class="para">Customer Id</p>'
                   

                    employees += '</div>'
                    employees += '<div class="col-md-6 col-sm-6 col-xs-6  name-data1">'
                    employees += '<p class="para1" id="firstName">' + value.BankAccountHolderName + '</p>'
                    employees += '<p class="para1" id="lastName">' + value.CustomerId + '</p>'
                    employees += '</div>'
                    employees += ' </div >'
                    employees += '</div >'
                    employees += '<div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">'
                    employees += '<div class="row left-column">'
                    employees += '<div class="col-md-6 col-sm-6 col-xs-6  name-data">'
                    employees += '<p class="para">Account Number</p>'
                    employees += '<p class="para">BVN Number</p>'
                    employees += '<p class="para">Bank Code</p>'
                    employees += '</div>'
                    employees += '<div class="col-md-6 col-sm-6 col-xs-6 name-data1">'
                    employees += '<p class="para1" id="middlename">' + value.AccountNumber + '</p>'
                    employees += '<p class="para1" id="email">' + value.BVN + '</p>'
                    employees += '<p class="para1" id="dob">' + value.BankCode + '</p>'
                    employees += '</div>'
                    employees += '</div>'
                    employees += '</div>'
                    employees += '</div>'
                    if (value.IsSalaryAccount) {
                        employees += '<p class="is-salary-account-container><span><input onclick="salaryAccount(this,' + value.Id + ',' + value.UserId + ')" type="checkbox" id="' + value.Id + ',' + value.UserId + '" data-val=' + value.Id + ',' + value.UserId + ' class="setSalaryAccount1" checked/></span><label class="bank-name-padding">Is  Salary Account</label></p>'

                    } else {
                        employees += '<p class="is-salary-account-container"><span><input onclick="salaryAccount(this,' + value.Id + ',' + value.UserId + ')" type="checkbox" id="' + value.Id + ',' + value.UserId + '"  data-val=' + value.Id + ',' + value.UserId + ' class="setSalaryAccount1"/></span><label class="bank-name-padding">Is  Salary Account</label></p>'
                        // $("#chkIsSalaryAccount").prop("checked", false);
                    }
                    employees += '</div>'
                  


                });
                $('#divEmployeeBanks').append(employees);
                //$("#chkIsSalaryAccount").prop("checked", true);
                page_number(TotalRowCount, pageNumber);
            }
            else if (response.RstKey == 2) {
                $("#user-no-record1").show();
            } else {
                //$("#userBankNoRecord").show();
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