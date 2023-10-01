var UserGuid = '';
var WalletTransactionId = '';
$(document).ready(function () {
    UserGuid = sessionStorage.getItem("User1");
    getUpComingBills(UserGuid);
    getCurrentBalance(UserGuid);
    $(document).on('click', '#backBtn', function () {
        window.href = window.history.back();
    });
    $(document).on('click', '#removeConfirmPop', function () {
        $('#managefinanceBillModal84').modal('show');
        WalletTransactionId = $(this).attr('data-transaction-id');
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
});

function getUpComingBills(userGuid) {

    var formData = new FormData();
    formData.append("UserGuid", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageFinance/GetUpcomingBillsHistory",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $("#upcomingBillsDiv").empty();
            if (response.RstKey == 1) {
                $("#UpcomingBillsTotal").text('₦ ' + response.PayableAmount);
                var item = "";
                $.each(response.upComingBills, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class="box-main-container">'
                        + '<div class="flex-container">'
                        + '<img src="../Content/images/mobile.svg" class="image" />'
                        + '<div class="upcoming-bill-content">' + value.ServiceName +'</div>'
                        + ' </div>'
                        + '<div class="flex-container">'
                        + '<div class="spend-amount">₦' + value.TotalAmount +'</div>'
                        + '<img src="../Content/images/close.png" class="close-btn" id="removeConfirmPop" data-transaction-id="'+ value.WalletTransactionId + '"/>'
                        + '</div>'
                        + '</div>';
                });

                $('#upcomingBillsDiv').append(item)
            }
            else {
                $("#upcomingBillsDiv").empty();
            }
        }
    });

}

function getCurrentBalance(userGuid) {

    var formData = new FormData();
    formData.append("userGuid", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageFinance/GetVirtualAccountBalance",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
           
            if (response.RstKey == 1) {
                $("#divCurrentBalance").text("₦ "+response.AvailableBalance);
            }
            else {
                $("#divCurrentBalance").text("₦ " + response.AvailableBalance);
            }
        }
    });

}