/*/*$("#tblEmployees").DataTable();*/
var areaChartData;
var areaChartData1;
var emailFormateError = true;
var amountLimitError = true;
var totalRequestedAmt;
var commisionList;
$(document).ready(function () {
    var pageNumber = 1;
    var PageSize = 10;
    var totalPages = 1;
    var searchText = '';
    var week = -1;

    //window.addEventListener('load', function () {
    //    history.pushState(null, null, document.URL);
    //    alert('test1');
    //});

    //window.addEventListener('popstate', function (event) {
    //    alert('test');
    //});

    window.onbeforeunload = function () {
        alert();
    }

    //$(window).on('onpopstate', function (e) {
    //    console.log('hash changed');
    //    alert("pop!1111");
    //});


    //window.onpopstate = function () {
    //    alert("pop!");
    //}

    // history.back()
    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    getDashboardData(UserGuid);
    getDashboardTransactionData(UserGuid);
    getPieChartData(UserGuid);
    getDashboardGraphData(UserGuid);
    getCommissionData(UserGuid)
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);


    $(function () {
        $('#txtEmailInvite').on('keypress', function (e) {
            if (e.which == 32) {
                alert('Space not allowed');
                return false;
            }
        });
    });
    $('#btnInviteTofriend').click(function () {
        var txtEmail = $("#txtEmailInvite").val();
        var formData = new FormData();
        formData.append("guid", UserGuid);
        formData.append("email", txtEmail);
        var emailVerified = validateEmail(txtEmail);

        /*if (txtEmail != null || txtEmail != "" || txtEmail != '') {*/
        console.log(emailVerified);
        if (emailVerified == true) {
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Home/Invite",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.RstKey == 1) {
                        $("#inviteEmailModal").modal('hide');
                        $("#dashboardInviteModal").modal('hide');
                        swal({
                            title: "Success",
                            text: "Email sent successfully",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            window.location.reload();
                        })
                    }
                    if (response.RstKey == 2) {
                        $("#accessPayModal").modal('hide');
                        swal({
                            title: "Failed",
                            text: response.Message,
                            icon: "error",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            //$("#verificationmodal").modal('show');
                        })
                    }

                }
            });
        }
        else {
            swal({
                title: "Error",
                text: "Email format is not valid",
                icon: "error",
                buttons: true,
                dangerMode: true,
            }, function succes(isDone) {

            })
        }
    })
    // Submit button
    $('#btnAccessRequest').click(function () {

        $("#AccessAmountError").text('');
        var txtAccessAmount = $('#txtAccessAmount').val();
        if (parseInt(txtAccessAmount) >= 2000) {
            var usernameError = true;
            if (txtAccessAmount == '' || txtAccessAmount == "" || txtAccessAmount == null) {

                swal({
                    title: "Error",
                    text: "Amount can not be empty",
                    icon: "error",
                    buttons: true,
                    dangerMode: true,
                }, function succes(isDone) {

                })
                usernameError = false;
                //alert("Amount can not empty")

            }
            else {

                usernameError = true;

            }
            if (parseInt(txtAccessAmount) > 100000) {
                amountLimitError = false;
                swal(
                    'Error!',
                    'Amount can not be greater than 100000.',
                    'error'
                ).catch(swal.noop);
            }
            else {
                amountLimitError = true;
            }
            if (usernameError == true && amountLimitError == true) {

                var formData = new FormData();
                formData.append("Amount", txtAccessAmount);
                formData.append("UserGuid", UserGuid);

                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/Home/AccessAmountRequest/",
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (response) {
                        console.log(response);
                        if (response.RstKey == 1) {
                            $("#accessPayModal").modal('hide');

                            $("#accessPayModal").modal('hide');
                            $("#accessPaySuccessModal").modal('show');

                            $('#txtAccessAmount').val('');
                        }
                        else if (response.RstKey == 2) {
                            $("#accessPayModal").modal('hide');
                            swal({
                                title: "Failed",
                                text: response.Message,
                                icon: "error",
                                buttons: true,
                                dangerMode: true,
                            }, function succes(isDone) {
                                //$("#verificationmodal").modal('show');
                            })
                        }
                        else if (response.RstKey == 4) {
                            $("#accessPayModal").modal('hide');
                            swal({
                                title: "Failed",
                                text: response.Message,
                                icon: "error",
                                buttons: true,
                                dangerMode: true,
                            }, function succes(isDone) {
                                //$("#verificationmodal").modal('show');
                            })
                        }
                        else if (response.RstKey == 8 || response.RstKey == 5) {
                            $("#accessPayModal").modal('hide');
                            swal({
                                title: "Failed",
                                text: response.Message,
                                icon: "error",
                                buttons: true,
                                dangerMode: true,
                            }, function succes(isDone) {
                                //$("#verificationmodal").modal('show');
                            })
                        }
                        else if (response.RstKey == 9) {
                            $("#accessPayModal").modal('hide');
                            swal({
                                title: "Failed",
                                text: response.Message,
                                icon: "error",
                                buttons: true,
                                dangerMode: true,
                            }, function succes(isDone) {
                                //$("#verificationmodal").modal('show');
                            })
                        }
                        else if (response.RstKey == 10) {
                            $("#accessPayModal").modal('hide');
                            swal({
                                title: "Failed",
                                text: response.Message,
                                icon: "error",
                                buttons: true,
                                dangerMode: true,
                            }, function succes(isDone) {
                                //$("#verificationmodal").modal('show');
                            })
                        } else {
                            $("#accessPayModal").modal('hide');
                            swal({
                                title: "Failed",
                                text: response.Message,
                                icon: "error",
                                buttons: true,
                                dangerMode: true,
                            }, function succes(isDone) {
                                //$("#verificationmodal").modal('show');
                            })
                        }
                    }
                });

            } else {
                return false;
            }
        } else {
            swal(
                'Error!',
                'Amount can not be less than 2000.',
                'error'
            ).catch(swal.noop);
        }
    });


    $('#select-box').change(function () {

        var value = $('select#select-box option:selected').val();
        var monthName = $('select#select-box option:selected').text();


        if (value == -1) {

            getDashboardGraphData(UserGuid, value);
        }
        else if (value === "3 Week") {

            $("#myChart").empty();
            getDashboardWeeklyGraphData(UserGuid, 3)
        }
        else if (value === "3 Month") {
            $("#myChart").empty();
            getDashboardMonthLyGraphData(UserGuid, 3)
        }
        else if (value == "3 Years") {
            $("#myChart").empty();
            getDashboardYearlyGraphData(UserGuid, 3)
        }

    });

    $("#inviteMailIcon").click(function () {
        $("#dashboardInviteModal").modal('hide');
        $("#inviteEmailModal").modal('show');
    })


    //$('#txtAccessAmount').bind('keypress', function (e) {
    //    var totalAmount = $('#txtAccessAmount').val();

    //    debugger
    //    $.each(commisionList, function (index, value) {

    //        if (value.AmountFrom < totalAmount && value.AmountTo > totalAmount) {
    //            totalRequestedAmt = parseInt(totalAmount) + parseInt(value.CommisionPercent)
    //            $("#requestFee").text();
    //        }

    //    });
    //});

    $("#txtAccessAmount").keyup(function () {
        var totalAmount = $('#txtAccessAmount').val();
        if (totalAmount >= 500 && totalAmount <= 100000) {
            $.each(commisionList, function (index, value) {

                if (value.AmountFrom <= parseInt(totalAmount) && value.AmountTo >= parseInt(totalAmount)) {
                    totalRequestedAmt = parseInt(totalAmount) - parseInt(value.CommisionPercent)
                    $("#requestFee").show();
                    $("#requestFee").text(" You will be charged transaction fee of ₦" + value.CommisionPercent + ", therefore  ₦ " + totalRequestedAmt + " will be credited to your account.");
                }
            });
        } else {
            $("#requestFee").hide();
        }


    });

});

function getDashboardData(id) {



    var formData = new FormData();
    formData.append("guid", id);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetDashboardData/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {

                $("#availableAmount").text(response.AvailableAmount);
                $("#fromTo").text(response.PayCycleFrom + ' - ' + response.PayCycleTo);
                $("#fromToCycle").text(response.PayCycleFrom + ' - ' + response.PayCycleTo);
                $("#fromToCycle1").text(response.PayCycleFrom + ' - ' + response.PayCycleTo);
                $("#EarnedAmount").text(response.EarnedAmount);
                $("#AccessedAmount1").text(response.AccessedAmount);
            }
            else if (response.RstKey == 2) {
                //swal(
                //    'Error!',
                //    'No data found.',
                //    'error'
                //).catch(swal.noop);
            } else {
                //swal(
                //    'Error!',
                //    'Failed to fetch data.',
                //    'error'
                //).catch(swal.noop);
            }
        }
    });

}

function getDashboardTransactionData(id) {

    var formData = new FormData();
    formData.append("UserGuid", id);

    formData.append("PageNumber", 1);
    formData.append("PageSize", 3);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetTransactionHistory/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.RstKey == 1) {
                var item = "";
                $("#transactions").hide();
                $.each(response.getTransactionHistoryResponse, function (index, value) {
                    let createdAt = value.CreatedAt;
                    let dateFormat = moment(createdAt).format('MMMM/DD/YYYY');
                    // APPEND OR INSERT DATA TO SELECT ELEMENT.
                    item += '<div class="transaction-sub-container">'
                        + '<div class="transaction-content">'
                        + '<img src="../Content/images/mobile.svg" alt="" class="mobile-img" />'
                        + '<div>'
                        + '<div class="mob-recharge-text">' + value.CategoryName + '</div>'
                        + ' <div class="phone-text">' + value.AccountNo + '</div>'
                        + ' </div>'
                        + '</div >'
                        + '<div class="transaction-amount-text">₦' + value.TotalAmount + '</div>'
                        + '</div >'
                        + ' <div class="date-debit-container">'
                        + ' <div class="transaction-date">' + dateFormat + '</div>'
                        + '<div class="transaction-debit">Success</div>'
                        + ' </div>'
                        + ' <hr class="hr-tag">';
                });
                //$('#divBankList').append('<div value="' + value.BankCode + '">' + value.BankName + '</div>');
                $('#mainDivTransaction').append(item).show();
                // getTransactionHistoryResponse
            }
            else if (response.RstKey == 2) {
                $("#transactions").show();
            } else {
                $("#transactions").show();
            }
        }
    });

}

function getPieChartData(userGuid) {

    var formData = new FormData();
    formData.append("UserGuid", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/ManageFinance/GetPiChartData/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            //$('#tblEmployees').empty();
            if (response.RstKey == 1) {
                $("#avgSpendPerDay").text("₦" + parseInt(response.manageFinance.AvgPerDaySpend));
                $("#okSpend").text("₦" + parseInt(response.manageFinance.OkToSpend));
                $("#Travel").text(response.manageFinance.Travel + "%");
                $("#RechargeBills").text(parseInt(response.manageFinance.Bills) + "%");
                $("#ecom").text(parseInt(response.manageFinance.Ecom) + "%");

                piechart(parseInt(response.manageFinance.Travel), parseInt(response.manageFinance.Bills), parseInt(response.manageFinance.Ecom));
            }
            else if (response.RstKey == 2) {

                $("#avgSpendPerDay").text("₦" + 0);
                $("#okSpend").text("₦" + 0);
                $("#Travel").text(0 + "%");
                $("#RechargeBills").text(0 + "%");
                $("#ecom").text(0 + "%");
                piechart(0, 0, 0);
            } else {

                $("#avgSpendPerDay").text("₦" + 0);
                $("#okSpend").text("₦" + 0);
                $("#Travel").text(response.manageFinance.Travel + "%");
                $("#RechargeBills").text(0 + "%");
                $("#ecom").text(0 + "%");
                piechart(0, 0, 0);
            }
        }
    });

}

function piechart(data1, data2, data3) {

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


    new Chart("pieChart", {
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

function getDashboardGraphData(userId, week) {

    var formData = new FormData();
    formData.append("EmployeeGuid", userId);
    formData.append("LastWeak", week);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetDashboardGraphtData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.getDashboardGraphResponse != null && response.RstKey == 1) {
                $("#NoSpendingsFound").hide();
                //if (response.getDashboardGraphResponse.MondayData > 0 || response.getDashboardGraphResponse.TuesdayData > 0 || response.getDashboardGraphResponse.WednesdayData > 0 || response.getDashboardGraphResponse.ThursdayData > 0 || response.getDashboardGraphResponse.FridayData > 0 || response.getDashboardGraphResponse.SaturdayData > 0 || response.getDashboardGraphResponse.SundayData > 0) {
                STACKEDgraph(response.getDashboardGraphResponse.MondayData, response.getDashboardGraphResponse.TuesdayData, response.getDashboardGraphResponse.WednesdayData, response.getDashboardGraphResponse.ThursdayData, response.getDashboardGraphResponse.FridayData, response.getDashboardGraphResponse.SaturdayData, response.getDashboardGraphResponse.SundayData);
                //} else {
                //    $("#myChart").hide();
                //    $("#NoSpendingsFound").show();

                //}


            }
            else {
                //swal({
                //    title: "oops!",
                //    text: "Data Not Found !",
                //    icon: "error",
                //    button: "ohh no!"
                //});

            }


        }
    });
}

var chart;

function STACKEDgraph(data1, data9, data3, data4, data5, data6, data7) {
    //---------------------
    //- STACKED BAR CHART -
    //---------------------
    var resultData = [data1, data9, data3, data4, data5, data6, data7];
    var Norecord;

    const isAllZero = resultData.every(item => item === 0);
    if (isAllZero) {
        Norecord = 100;
    } else {
        Norecord = 100;
    }
    if (areaChartData1) {
        areaChartData1.destroy()
    }


    areaChartData = {
        labels: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
        datasets: [
            {
                label: 'No record found',
                backgroundColor: 'rgba(210, 214, 222, 1)',
                borderColor: 'rgba(210, 214, 222, 1)',
                pointRadius: false,
                pointColor: 'rgba(210, 214, 222, 1)',
                pointStrokeColor: '#c1c7d1',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(220,220,220,1)',
                data: [Norecord, Norecord, Norecord, Norecord, Norecord, Norecord, Norecord]
            },
            {
                label: 'Transactions',
                // backgroundColor: 'rgba(60,141,188,0.9)',
                borderColor: 'rgba(60,141,188,0.8)',
                pointRadius: false,
                pointColor: '#3b8bba',
                pointStrokeColor: 'rgba(60,141,188,1)',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                backgroundColor: "#32A4FC",
                hoverBackgroundColor: "#32A4FC",
                data: [data1, data9, data3, data4, data5, data6, data7]
            },
        ]
    }
    // var barChartCanvas = $('#barChart').get(0).getContext('2d')
    var barChartData = $.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    var temp1 = areaChartData.datasets[1]
    barChartData.datasets[0] = temp1
    barChartData.datasets[1] = temp0

    var stackedBarChartCanvas = $('#myChart').get(0).getContext('2d')
    var stackedBarChartData = $.extend(true, {}, barChartData)

    var stackedBarChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            xAxes: [{
                stacked: true,
            }],
            yAxes: [{
                stacked: true
            }]
        }
    }

    areaChartData1 = new Chart(stackedBarChartCanvas, {
        type: 'bar',
        data: stackedBarChartData,
        options: stackedBarChartOptions
    })
}

function STACKEDgraph1(data1, data9, data3, data4, data5, data6) {
    //---------------------
    //- STACKED BAR CHART -
    //---------------------
    var resultData = [data4, data5, data6];
    var Norecord;
    const isAllZero = resultData.every(item => item === 0);
    if (isAllZero) {
        Norecord = 100;
    } else {
        Norecord = 100;
    }
    if (areaChartData1) {
        areaChartData1.destroy()
    }
    //areaChartData1.destroy()

    areaChartData = {
        labels: [data1, data9, data3],
        datasets: [
            {
                label: 'No record found',
                backgroundColor: 'rgba(210, 214, 222, 1)',
                borderColor: 'rgba(210, 214, 222, 1)',
                pointRadius: false,
                pointColor: 'rgba(210, 214, 222, 1)',
                pointStrokeColor: '#c1c7d1',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(220,220,220,1)',
                data: [Norecord, Norecord, Norecord]
            },
            {
                label: 'Transactions',
                // backgroundColor: 'rgba(60,141,188,0.9)',
                borderColor: 'rgba(60,141,188,0.8)',
                pointRadius: false,
                pointColor: '#3b8bba',
                pointStrokeColor: 'rgba(60,141,188,1)',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                backgroundColor: "#32A4FC",
                hoverBackgroundColor: "#32A4FC",
                data: [data4, data5, data6]
            },
        ]
    }
    // var barChartCanvas = $('#barChart').get(0).getContext('2d')
    var barChartData = $.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    var temp1 = areaChartData.datasets[1]
    barChartData.datasets[0] = temp1
    barChartData.datasets[1] = temp0

    var stackedBarChartCanvas = $('#myChart').get(0).getContext('2d')
    var stackedBarChartData = $.extend(true, {}, barChartData)

    var stackedBarChartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            xAxes: [{
                stacked: true,
            }],
            yAxes: [{
                stacked: true
            }]
        }
    }

    areaChartData1 = new Chart(stackedBarChartCanvas, {
        type: 'bar',
        data: stackedBarChartData,
        options: stackedBarChartOptions
    })
}

function getDashboardMonthLyGraphData(userId, week) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);
    formData.append("LastWeak", week);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetDashboardMonthlyGraphData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.getDashboardMonthlyGraphResponses != null && response.RstKey == 1) {

                var DataName1 = "";
                var DataName2 = "";
                var DataName3 = "";
                var Total1 = "";
                var Total2 = "";
                var Total3 = "";

                if (response.getDashboardMonthlyGraphResponses[0] != null) {
                    DataName1 = response.getDashboardMonthlyGraphResponses[0].DataName;
                    Total1 = response.getDashboardMonthlyGraphResponses[0].Total;
                } else {
                    DataName1 = "NA";
                    Total1 = 0;
                }
                if (response.getDashboardMonthlyGraphResponses[1] != null) {
                    DataName2 = response.getDashboardMonthlyGraphResponses[1].DataName;
                    Total2 = response.getDashboardMonthlyGraphResponses[1].Total;
                } else {
                    DataName2 = "NA";
                    Total2 = 0;
                }
                if (response.getDashboardMonthlyGraphResponses[2] != null) {
                    DataName3 = response.getDashboardMonthlyGraphResponses[2].DataName;
                    Total3 = response.getDashboardMonthlyGraphResponses[2].Total;
                } else {
                    DataName3 = "NA";
                    Total3 = 0;
                }
                STACKEDgraph1(DataName1, DataName2, DataName3, Total1, Total2, Total3);

            }
            else {
                //swal({
                //    title: "oops!",
                //    text: "Data Not Found !",
                //    icon: "error",
                //    button: "ohh no!"
                //});
            }


        }
    });
}

function getDashboardWeeklyGraphData(userId, week) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);
    formData.append("LastWeak", week);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetDashboardWeeklyGraphData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.getDashboardMonthlyGraphResponses != null && response.RstKey == 1) {

                var DataName1 = "";
                var DataName2 = "";
                var DataName3 = "";
                var Total1 = "";
                var Total2 = "";
                var Total3 = "";

                if (response.getDashboardMonthlyGraphResponses[0] != null) {
                    DataName1 = response.getDashboardMonthlyGraphResponses[0].DataName;
                    Total1 = response.getDashboardMonthlyGraphResponses[0].Total;
                } else {
                    DataName1 = "NA";
                    Total1 = 0;
                }
                if (response.getDashboardMonthlyGraphResponses[1] != null) {
                    DataName2 = response.getDashboardMonthlyGraphResponses[1].DataName;
                    Total2 = response.getDashboardMonthlyGraphResponses[1].Total;
                } else {
                    DataName2 = "NA";
                    Total2 = 0;
                }
                if (response.getDashboardMonthlyGraphResponses[2] != null) {
                    DataName3 = response.getDashboardMonthlyGraphResponses[2].DataName;
                    Total3 = response.getDashboardMonthlyGraphResponses[2].Total;
                } else {
                    DataName3 = "NA";
                    Total3 = 0;
                }
                STACKEDgraph1(DataName1, DataName2, DataName3, Total1, Total2, Total3);

            }
            else {
                //swal({
                //    title: "oops!",
                //    text: "Data Not Found !",
                //    icon: "error",
                //    button: "ohh no!"
                //});
            }


        }
    });
}

function getDashboardYearlyGraphData(userId, week) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);
    formData.append("LastWeak", week);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetDashboardYearlyGraphData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.getDashboardMonthlyGraphResponses != null && response.RstKey == 1) {
                var DataName1 = "";
                var DataName2 = "";
                var DataName3 = "";
                var Total1 = "";
                var Total2 = "";
                var Total3 = "";

                if (response.getDashboardMonthlyGraphResponses[0] != null) {
                    DataName1 = response.getDashboardMonthlyGraphResponses[0].DataName;
                    Total1 = response.getDashboardMonthlyGraphResponses[0].Total;
                } else {
                    DataName1 = "NA";
                    Total1 = 0;
                }
                if (response.getDashboardMonthlyGraphResponses[1] != null) {
                    DataName2 = response.getDashboardMonthlyGraphResponses[1].DataName;
                    Total2 = response.getDashboardMonthlyGraphResponses[1].Total;
                } else {
                    DataName2 = "NA";
                    Total2 = 0;
                }
                if (response.getDashboardMonthlyGraphResponses[2] != null) {
                    DataName3 = response.getDashboardMonthlyGraphResponses[2].DataName;
                    Total3 = response.getDashboardMonthlyGraphResponses[2].Total;
                } else {
                    DataName3 = "NA";
                    Total3 = 0;
                }

                STACKEDgraph1(DataName1, DataName2, DataName3, Total1, Total2, Total3);
                //testGraph();
            }
            else {
                //swal({
                //    title: "oops!",
                //    text: "Data Not Found !",
                //    icon: "error",
                //    button: "ohh no!"
                //});
            }


        }
    });
}

function validateEmail(email) {
    var userinput = email;



    if (userinput.length != '') {

        var pattern = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;

        if (!pattern.test(userinput)) {
            emailFormateError = false;
            swal({
                title: "Email format is not valid",
                text: "Try again",
                type: "warning",
                //showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                //confirmButtonText: "Delete",
                //closeOnConfirm: false
            }, function succes(isDone) {
                emailFormateError = false;
            })
        }
        else {
            return emailFormateError = true;
        }
    }


}

function getCommissionData(id) {
    var formData = new FormData();
    formData.append("guid", id);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Home/GetCommissionList/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                commisionList = response.getCommissions;
            }
            else if (response.RstKey == 2) {
                //swal(
                //    'Error!',
                //    'No data found.',
                //    'error'
                //).catch(swal.noop);
            } else {
                //swal(
                //    'Error!',
                //    'Failed to fetch data.',
                //    'error'
                //).catch(swal.noop);
            }
        }
    });

}
