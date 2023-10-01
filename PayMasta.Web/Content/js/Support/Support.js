$(document).ready(function () {

    var UserGuid = "";
    getInvoiceNumber();
    let Titleerror = true
    let Descerror = true
    let Invoiceerror = true
    var title = "";
    var titleText = "";
    var spaceValidationError = true
    //$("#btnSaveQuery").attr('disabled', true);
    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);


    //$('#btnViewTickets').click(function () {
    //    window.location.href = "Support/ViewTickets";
    //});

    $('#txtTitle').change(function () {

        title = $('select#txtTitle option:selected').val();
        titleText = $('select#txtTitle option:selected').text();

    });
    function spaceValidation(param) {
        var userInput = param;
        var incorrectFormat = /\s{2}/.test(userInput)

        if (incorrectFormat) {
            alert("Double spaces are not allowed");
            return false;
        } else {
            return true;
        }
    }
    $('#txtDesc').on('keyup',function () {
        var description = $('#txtDesc').val();
        var validDescription = spaceValidation(description);
        console.log(validDescription);
        //if (validDescription == true) { $("#btnSaveQuery").attr('disabled', false); } else { $("#btnSaveQuery").attr('disabled', true); }
    });

    function getInvoiceNumber() {
        var formData = new FormData();
        formData.append("Size", 6);
        $.ajax({
            type: "POST",
            cache: false,
            url: "Support/GeticketNumber",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {

                $('#txtInvoice').attr("value", response.InvoiceNumber);
            }
        });
    }

    $('#btnSaveQuery').click(function () {
        var txtInvoice = $('#txtInvoice').val();
        var txtTitle = titleText;
        var txtDesc = $('#txtDesc').val();

        if (txtInvoice == '' || txtInvoice == "" || txtInvoice == null) {
            // $('#usercheck').show();
            Invoiceerror = false;
            swal(
                'Error!',
                'Invoice can not be empty',
                'error'
            ).catch(swal.noop);
        }
        else {
            // $('#usercheck').hide();
            Invoiceerror = true;
            // return true;
        }
        if (txtTitle == '' || txtTitle == "" || txtTitle == null || txtTitle == "Select") {
            // $('#usercheck').show();
            Titleerror = false;
            swal(
                'Error!',
                'Please select title',
                'error'
            ).catch(swal.noop);
            // return false;
        }
        else {
            // $('#usercheck').hide();
            Titleerror = true;
            // return true;
        } if (txtDesc == '' || txtDesc == "" || txtDesc == null) {
            // $('#usercheck').show();
            Descerror = false;
            swal(
                'Error!',
                'Description can not be empty',
                'error'
            ).catch(swal.noop);
            // return false;
        }
        else {
            if (txtDesc.length < 10) {
                swal(
                    'Error!',
                    'Please enter atleast 10 character',
                    'error'
                ).catch(swal.noop);
            }
            else {
                Descerror = true;
            }
            // $('#usercheck').hide();

            // return true;
        }


        // validateEmail();
        if (Invoiceerror == true && Titleerror == true && Descerror == true) {
            var formData = new FormData();
            formData.append("UserGuid", UserGuid);
            formData.append("TicketNumber", txtInvoice);
            formData.append("Title", txtTitle);
            formData.append("DescriptionText", txtDesc);
            $.ajax({
                type: "POST",
                cache: false,
                url: "Support/InsertSupportTicket",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {
                        var url = window.location.origin;
                       

                        //swal(
                        //    'Success!',
                        //    'Your support query is submitted',
                        //    'success'
                        //).catch(swal.noop);

                        window.location.href = url + "/Support/ViewTickets";
                        //swal({
                        //    title: "Success",
                        //    text: "Your support query is submitted",
                        //    icon: "Success",
                        //    buttons: true,
                        //    dangerMode: true,
                        //}, function succes(isDone) {
                        //    var url = window.location.origin;
                        //    window.location.href = url + "/Support/ViewTickets";
                        //})

                    }
                    else if (response.RstKey == 2) {

                        swal(
                            'Error!',
                            'Your support query is not submitted.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    } else if (response.RstKey == 2) {

                        swal(
                            'Error!',
                            'Your support query is not submitted.',
                            'error'
                        ).catch(swal.noop);
                    }

                }
            });
        } else {
            //swal(
            //    'Error!',
            //    'Failed',
            //    'error'
            //).catch(swal.noop);
            return false;
        }
    });
    


});

