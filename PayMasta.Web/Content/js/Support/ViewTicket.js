
var searchParam = '';
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
$(document).ready(function () {

    var UserGuid = "";

    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);

    getSupportTicket(UserGuid, pageNumber, PageSize);


});

function getSupportTicket(UserGuid, pagenumber, pageSize) {

    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    formData.append("pageNumber", pagenumber);
    formData.append("PageSize", pageSize);
    $.ajax({
        type: "POST",
        cache: false,
        url: "GetSupportDetailList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            var item = "";
            var count = 1;
            if (response.RstKey == 1) {
                $("#user-no-record").hide();
                $('#viewSupportTicket').empty();
                $.each(response.supportMasterTicketResponse, function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    //item += '<tbody>';
                    item += '<tr>';
                    item += '<td>' + value.RowNumber + '</td>';
                    item += '<td>' + value.Title + '</td>';
                    item += '<td>' + value.DescriptionText + '</td>';
                    item += '<td>' + value.TicketNumber + '</td>';
                    item += '<td>' + value.CreatedAt + '</td>';
                    if (value.StatusId == 0) {
                        item += '<td><span class="coloureddata" style="color : #32A4FF">' + value.Status + '</span></td>';
                    } else if (value.StatusId == 2) {
                        item += '<td><span class="coloureddata" style="color:#32A4FF">' + value.Status + '</span></td>';
                    }
                    else if (value.StatusId == 3) {
                        item += '<td><span class="coloureddata" style="color:forestgreen">' + value.Status + '</span></td>';
                    }
                    else if (value.StatusId == 4) {
                        item += '<td><span class="coloureddata" style="color:orange">' + value.Status + '</span></td>';
                    }
                    else if (value.StatusId == 5) {
                        item += '<td><span class="coloureddata" style="color:red">' + value.Status + '</span></td>';
                    }
                    //item += '</tbody>';
                    item += '</tr>';
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    /*item += '<div class="ticketdata upperspace"><p class="contentViewTickect">' + value.Title + "-" + value.DescriptionText + '</p>' +
                        '<p class="datedata" >' + value.CreatedAt + '</p>' +
                        '<p class="status1">Ticket Number:<span class="coloureddata1">' + value.TicketNumber + '</span></p>';
                    if (value.StatusId == 0) {
                        item += '<p class="status">Status: <span class="coloureddata">' + value.Status + '</span></p>';
                    } else if (value.StatusId == 2) {
                        item += '<p class="status">Status: <span class="coloureddata" style="color:blue">' + value.Status + '</span></p>';
                    }
                    else if (value.StatusId == 3) {
                        item += '<p class="status">Status: <span class="coloureddata" style="color:greeb">' + value.Status + '</span></p>';
                    }
                    else if (value.StatusId == 4) {
                        item += '<p class="status">Status: <span class="coloureddata" style="color:orange">' + value.Status + '</span></p> ';
                    }
                    else if (value.StatusId == 5) {
                        item += '<p class="status">Status: <span class="coloureddata" style="color:red">' + value.Status + '</span></p>';
                    }
                    item += ' </div>';*/
                   // count++;
                });
                //$('#divBankList').append('<div value="' + value.BankCode + '">' + value.BankName + '</div>');
                $('#viewSupportTicket').append(item).show();
                page_number(TotalRowCount, pageNumber);
            } else {
                $("#user-no-record").show();
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
            pageNumber = options.current;
            if (searchParam.length >= 3) {
                getSupportTicket(UserGuid, options.current, PageSize);
            }
            else if (searchParam.length == 0) {
                getSupportTicket(UserGuid, options.current, PageSize);
            }
        }
    });
}