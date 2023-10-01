var UserGuid = "";
var value1 = 50;
var value2 = 50;
var value3 = 50;
var WalletTransactionId = '';
var WalletTransId = '';
var accountNumber = '';
var ServiceType = 0;
var meterNumber = 0;
$(document).ready(function () {
    UserGuid = sessionStorage.getItem("User1");
    getPiChartData(UserGuid);
    getTodaysTransaction(UserGuid);
    getUpComingBills(UserGuid);
    //chart(value1, value2, value3);
    //$("openRechargeModal").click(function () {
    //    $("#rechargeSuccessModal").modal("show");
    //})

    //$("#financePayBtn").click(function () {
    //    $("#rechargeSuccessModal").modal('hide');
    //    $("#financeContratsModal").modal('show');
    //})

    $("#btnPay").click(function () {
        $("#rechargeSuccessModal").modal('hide');
    })
    $("#btnPayBills").click(function () {
        debugger

        if (ServiceType == 4) {
            var url = window.location.origin;
            window.location.href = url + "/BillAndPayment/Index" + "?Id=" + WalletTransId + "&AccountNumber=" + accountNumber + "&Type=" + ServiceType + "&meterNumber=" + meterNumber
        }
        else if (ServiceType == 5) {
            var url = window.location.origin;
            window.location.href = url + "/BillAndPayment/Index" + "?Id=" + WalletTransId + "&AccountNumber=" + accountNumber + "&Type=" + ServiceType;
        }
        else if (ServiceType == 3) {
            var url = window.location.origin;
            window.location.href = url + "/BillAndPayment/Index" + "?Id=" + WalletTransId + "&AccountNumber=" + accountNumber + "&Type=" + ServiceType;
        }
        else if (ServiceType == 2) {
            var url = window.location.origin;
            window.location.href = url + "/BillAndPayment/Index" + "?Id=" + WalletTransId + "&AccountNumber=" + accountNumber + "&Type=" + ServiceType;
        }
        return false;
    })

    $("#congratsCloseBtn22").click(function () {
        $("#financeContratsModal").modal('hide');

    })
    $("#btnPayOkay").click(function () {

        $("#financeContratsModal").modal('hide');
        window.location.reload();
    })

    $(document).on('click', '.delete-bill', function (e) {
        e.stopPropagation();
        WalletTransactionId = $(this).attr('data-val');
        $('#managefinanceBillModal84').modal('show');
    })

    $(document).on('click', 'div.get-billinfo', function (e) {
        e.stopPropagation();
        e.preventDefault;

        var data = $(this).data('val')
        var UserGuid = sessionStorage.getItem("User1");
        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append("WalletTransactionId", data);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/ManageFinance/GetTransactionByWalletTransactionId",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                debugger
                if (response.RstKey == 1) {

                    var value = response.getTransactionResponse;
                    $("#transaction_id").val(value.TransactionId);
                    $("#reference_id").val(value.InvoiceNo);
                    $("#debited_from").val(response.DebitFrom);
                    $("#amount").val(value.TotalAmount);
                    $("#rechargeSuccessModal").modal("show");

                    $("#paymentAmt").text("Payment of ₦" + value.TotalAmount + " has been done.");
                    if (value.AccountNo != null && value.SubCategoryId != null) {
                        accountNumber = value.AccountNo;
                        ServiceType = value.SubCategoryId;
                        WalletTransId = value.WalletTransactionId;

                    }
                    if (value.VoucherCode != null && value.VoucherCode != "") {
                        meterNumber = value.VoucherCode;
                    }

                }
                else {

                }
            }
        });
        //var formData = new FormData();
        //formData.append("UserGuid", UserGuid);
        //formData.append("BankId", data);
        //$.ajax({
        //    type: "POST",
        //    cache: false,
        //    url: "DeleteBankByBankDetailId",
        //    contentType: false,
        //    processData: false,
        //    data: formData,
        //    success: function (response) {
        //        if (response.RstKey == 1) {

        //        } else {

        //        }
        //    }
        //});

    });

    $(document).on('click', '#removeBtn', function () {
        var IsRemove = true;
        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append("WalletTransactionId", WalletTransactionId);
        formData.append("IsRemove", IsRemove);
        $.ajax({
            type: "POST",
            cache: false,
            url: "/ManageFinance/RemoveBillfromUpcomingBilsList",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                $("#upcomingBillsDiv").empty();
                if (response.RstKey == 1) {
                    getUpComingBills(UserGuid);
                }
                else {
                    $("#upcomingBillsDiv").empty();
                }
            }
        });

    });



})


function getPiChartData(userGuid) {

    var formData = new FormData();
    formData.append("UserGuid", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "ManageFinance/GetPiChartData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#tblEmployees').empty();
            if (response.RstKey == 1) {
                $("#avgSpendPerDay").text("₦" + parseInt(response.manageFinance.AvgPerDaySpend));
                if (parseInt(response.manageFinance.OkToSpend) < 0) {
                    $("#okSpend").text("Please request for EWA " +"₦" + parseInt(response.manageFinance.OkToSpend));
                   
                } else {
                    $("#okSpend").text("₦" + parseInt(response.manageFinance.OkToSpend));
                }
              
                $("#Travel").text(response.manageFinance.Travel + "%");
                $("#RechargeBills").text(parseInt(response.manageFinance.Bills) + "%");
                $("#ecom").text(parseInt(response.manageFinance.Ecom) + "%");

                chart(parseInt(response.manageFinance.Travel), parseInt(response.manageFinance.Bills), parseInt(response.manageFinance.Ecom));
            }
            else if (response.RstKey == 2) {

                $("#avgSpendPerDay").text("₦" + 0);
                $("#okSpend").text("₦" + parseInt(response.manageFinance.OkToSpend));
                $("#Travel").text(0 + "%");
                $("#RechargeBills").text(0 + "%");
                $("#ecom").text(0 + "%");
                chart(0, 0, 0);
            } else {

                $("#avgSpendPerDay").text("₦" + 0);
                $("#okSpend").text("₦" + 0);
                $("#Travel").text(response.manageFinance.Travel + "%");
                $("#RechargeBills").text(0 + "%");
                $("#ecom").text(0 + "%");
                chart(0, 0, 0);
            }
        }
    });

}

function chart(data1, data2, data3) {

    if (data1 == 0 && data2 == 0 && data3 == 0) {
        var xValues = [];
        var yValues = [100, 0, 0];
        var barColors = [
            "#e3e3e3",

        ];

    } else {
        var xValues = [];
        var yValues = [data1, data2, data3];
        var barColors = [
            "#F66847",
            "#3EA4FF",
            "#53D17A",
        ];

    }

    new Chart("myChart", {
        type: "pie",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues,
                borderWidth: 0
            }]
        },
        options: {
            tooltips: { enabled: false },
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: "top",
                    align: "middle"
                }
            },

            title: {
                display: true,
                //   text: "World Wide Wine Production 2018"
            }
        }
    });
}


function getTodaysTransaction(userGuid) {

    var formData = new FormData();
    formData.append("UserGuid", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "ManageFinance/GetTodaysTransactionHistory",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                $('#noTransactionsForDay').hide();
                var item = "";

                $.each(response.getTodayTransactionHistoryResponses, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class=" line" >' +
                        ' <div class="recharge_text" >' +
                        value.CategoryName
                        + ' </div >'
                        + ' <div class="recharge_num">₦' + value.TotalAmount + '</div>' +
                        ' </div >';
                });

                $('#TodaysTransaction').append(item).show();
            }
          /*  else if (response.RstKey == 2) {

            } */else {
                $('#noTransactionsForDay').show();
            }
        }
    });

}

function getUpComingBills(userGuid) {

    var formData = new FormData();
    formData.append("UserGuid", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "ManageFinance/GetUpcomingBillsHistory",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                $('#upComingbillsList').empty();
                $('#upcomingBillNoRecordFound').hide();
                var item = "";
                $("#UpcomingBillsTotal").text('₦' + response.PayableAmount);
                $.each(response.upComingBills, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class="card_section get-billinfo" data-val=' + value.WalletTransactionId + ' data-tag = "div">'
                        + '<div id = "cardCloseBtn" class="card-close-icon">'
                        + '<img class="close_img delete-bill" src="../Content/images/close.svg" alt="" data-val=' + value.WalletTransactionId + ' id="removeUpcomingBills" data-tag="img">'
                        + ' </div>'
                        + ' <div class="mob_recharge">'
                        + '<div class="text_center">'
                        + '<img class="phone_img" src="../Content/images/phone.svg"/>'
                        + '<p class="ph_text">' + value.ServiceName + ' <br> Recharge</p>'
                        + '<p class="ph_num">₦' + value.TotalAmount + '</p>'
                        + '</div>'
                        + '</div>'
                        + '</div>';
                });

                $('#upComingbillsList').append(item).show();
                $("#UpcomingBillsTotal").show();
                $("#noUpcomingBillFound").show();

            }
           /* else if (response.RstKey == 2) {

            } */else {
                $("#UpcomingBillsTotal").hide();
                $('#upcomingBillNoRecordFound').show();
                $("#noUpcomingBillFound").hide();
            }
        }
    });

}