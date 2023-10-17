var UserGuid = "";
$(document).ready(function () {
    let ipAddress = "";
    $("#viewBalanceDetail").hide();
    $("#dataNotFound").hide();
    $("#QrCodeImage").hide();
    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
        console.log(e.ip);
    });


    UserGuid = sessionStorage.getItem("User1");
    getWalletCurrentBalance(UserGuid);

    $('body').on('click', "span#linkAccount", function (e) {

        e.preventDefault();
        var data = $(this).data('val');
        console.log("data", data);
        console.log("User Id" + UserGuid)
        console.log("test function");
        var formData = new FormData();
        formData.append("guid", data);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/Home/GetWidgetLink/",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.RstKey == 1) {
                    window.location.href = response.UrlLink;
                }
                else {
                    //error massage
                }
            }
        });
    })


});


function getLinkedOrUnlinkedAccount(userId) {

    var formData = new FormData();
    formData.append("UserGuid", userId);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetLinkedOrUnLinkedBank/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            var item = "";
            $('#loader1').hide();


            if (response.RstKey == 1) {
                $.each(response.linkedOrUnlinkedBanks, function (index, value) {
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class="bank-name-container" style="margin-bottom: 10px; display: flex; align-items: center; justify-content: space-between; background: #FFFFFF; padding: 20px; box-shadow: 0px 1px 1px black;border-radius: 4px"><div style="display: flex; align-items: center; grid-column-gap: 10px" class="bank-name-sub-container">';
                    if (value.ImageUrl == null || value.ImageUrl == "") {
                        item += '<img src = "../Content/images/add-account-img.png" alt = "" width = "36px" height = "36px">' +
                            '<div class="bank-content">' + value.BankName + '</div></div>'
                    } else {
                        item += '<img src = "' + value.ImageUrl + '" alt = "" width = "36px" height = "36px">' +
                            '<div class="bank-content">' + value.BankName + '</div></div>'
                    }

                    if (value.IsLinked == true) {
                        item += '<span class="view-bank-balance" data-val=' + value.BankCodeOrBankId + ' style="color: #FFFFFF; background: #32A4FC; padding: 8px 18px;border-radius: 4px; cursor: pointer"">View Account Balance</span></div>';//'<span class="delete-bank" data-val=' + value.BankCodeOrBankId + '><img src="../Content/images/delete-icon.png" alt="" style="cursor:pointer;"></span></div>';
                    } else {
                        /*item += '<h1>Hello</h1></div>'*/
                        item += '<span id="linkAccount" class="delete-bank" data-val=' + UserGuid + ' style="color: #FFFFFF; background: #32A4FC; padding: 8px 18px;border-radius: 4px; cursor: pointer"">Link</span></div>';
                    }

                });
                $('#divBankList').append(item).show();
            } else {
                $("#dataNotFound").show();
            }
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

$('body').on('click', 'span.view-bank-balance', function (e) {
    e.preventDefault;

    var data = $(this).data('val')
    var formData = new FormData();
    formData.append("bankId", data);
    formData.append("guid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetAccountBalance/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.status == true) {
                $("#divBankList").hide();
                $("#viewBalanceDetail").show();

                //if (response != null || response.ImageUrl != "") {
                //    $('#bankImage').attr('src', "../Content/images/add-account-img.png");
                //} else {
                //    $('#bankImage').attr('src', response.ImageUrl);
                //}
                $("#QrCodeImage").attr("src", "data:image/png;base64," + response.QrCode); // Make sure to change the data type (e.g., image/png) based on your image format
                //$('#QrCodeImage').attr('src', response.QrCode);
                $('#bankImage').attr('src', "../Content/images/add-account-img.png");
                //alert();
                $("#balance").text('₦' + response.wallet.availableBalance);
                $("#spnbankName").text(response.wallet.bankName);
                $("#spnAccountNumber").text(response.wallet.accountNumber);
            } else {

            }
        }
    });

});

function getWalletCurrentBalance(id) {
    var formData = new FormData();
    formData.append("bankId", 1);
    formData.append("guid", id);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetAccountBalance/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            debugger
            if (response.status == true) {
                $("#QrCodeImage").show();
                $("#divBankList").hide();
                $("#viewBalanceDetail").show();

                //if (response.ImageUrl == null || response.ImageUrl == "") {
                //    $('#bankImage').attr('src', "../Content/images/wallet.png");
                //} else {
                //    $('#bankImage').attr('src', response.ImageUrl);
                //}

                ////alert();
                //$("#balance").text('₦' + response.Balance);
                //$("#spnbankName").text(response.BankName);
                //$("#spnAccountNumber").text(response.AccountNumber);
                //$("#spnNubanAccountNumber").text(response.NubanAccountNumber);
                $("#QrCodeImage").attr("src", "data:image/png;base64," + response.QrCode);
                $('#bankImage').attr('src', "../Content/images/add-account-img.png");
                //alert();
                $("#balance").text('₦' + response.wallet.availableBalance);
                $("#spnbankName").text(response.wallet.bankName);
                $("#spnAccountNumber").text(response.wallet.accountNumber);
            } else {

            }
        }
    });
}