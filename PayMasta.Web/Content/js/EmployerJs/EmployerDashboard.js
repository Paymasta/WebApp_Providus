var areaChartData;
var areaChartData1;
var employerName;
$(document).ready(function () {
    $("#noEmployerBarChart").hide();
    var UserGuid = "";
    var pageNumber = 1;
    var PageSize = 10;
    var totalPages = 1;
    var searchText = '';
    var week = -1;
    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    employerName = sessionStorage.getItem("UserName");
    getDashboardData(UserGuid);
    getEWAData(UserGuid);
    getDashboardGraphData(UserGuid)
    getDashboardEmployeeListData(UserGuid);
    getDashboardPayCycleData(UserGuid);

    $("#spnEmployerName").text("Welcome :" + employerName);

    $('body').on('click', 'span.view-employeeProfile', function (e) {
        e.preventDefault;
        var data = $(this).data('val')
        var url = window.location.origin;
        window.location.href = url + "/EmployeesTransactions/GetEmployeDetailByGuid/id=" + data;
    });

    $('#month').change(function () {

        var value = $('select#month option:selected').val();
        var monthName = $('select#month option:selected').text();
        if (value == -1) {

            getDashboardGraphData(UserGuid, value);
        }
        else if (value === "3 Week") {

            $("#employerBarChart").empty();
            getDashboardWeeklyGraphData(UserGuid, 3)
        }
        else if (value === "3 Month") {
            $("#employerBarChart").empty();
            getDashboardMonthLyGraphData(UserGuid, 3)
        }
        else if (value == "3 Years") {
            $("#employerBarChart").empty();
            getDashboardYearlyGraphData(UserGuid, 3)
        }
    });

});

function getDashboardData(userId) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/EmployerDashbord/GetDashboardData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.dashboard != null && response.RstKey == 1) {
                let value = response.dashboard;
                $("#total_employees").text(value.TotalEmployees)
                $("#totalActiveEmplopees").text(value.TotalActiveEmployees);
                $("#totalWithdrawalValue").text("₦" + value.TotalWithdrawlValue);
                $("#totalTransaction").text(value.TotalTransactions);
                $("#total_withdraw_req").text(value.TotalWithdrawRequest);
                $("#withdrawalrequestispending").text(value.TotalPendingWithdrawRequest);

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

function getEWAData(userId) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/EmployerDashbord/GetDashboardEWARequestData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            if (response.pendingEWRRequestResponse != null && response.RstKey == 1) {
                let employerList = '';
                $("#noWithdrawlRequest").hide();
                $(response.pendingEWRRequestResponse).each(function (index, value) {
                    var name = value.FirstName + ' ' + value.LastName;
                    let createdAt = value.CreatedAt;
                    let dateFormat = moment(createdAt).format('MMMM/DD/YYYY');
                    name = name.substr(0, 20);
                    employerList += '<div class="text-section2"><div><img src="/Content/images/Employer/Icon.png" /></div><div class="cherry-text"><span class="deo-color" >' + name + '</span> request you to withdraw <span class="value-num">₦' + value.AccessAmount + '</span> amount.</div></div><span class="date-text">' + dateFormat + '</span><hr>';

                });
                $('#maiDivEwa').append(employerList);

            }
            else {
                $("#noWithdrawlRequest").show();

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

function getDashboardGraphData(userId, week) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);
    formData.append("LastWeak", week);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/EmployerDashbord/GetDashboardGraphtData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.getDashboardGraphResponse != null && response.RstKey == 1) {
                STACKEDgraph(response.getDashboardGraphResponse.MondayData, response.getDashboardGraphResponse.TuesdayData, response.getDashboardGraphResponse.WednesdayData, response.getDashboardGraphResponse.ThursdayData, response.getDashboardGraphResponse.FridayData, response.getDashboardGraphResponse.SaturdayData, response.getDashboardGraphResponse.SundayData);;
                //  grapg(response.getDashboardGraphResponse.MondayData, response.getDashboardGraphResponse.TuesdayData, response.getDashboardGraphResponse.WednesdayData, response.getDashboardGraphResponse.ThursdayData, response.getDashboardGraphResponse.FridayData, response.getDashboardGraphResponse.SaturdayData, response.getDashboardGraphResponse.SundayData);

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

function getDashboardEmployeeListData(userId) {

    var formData = new FormData();
    formData.append("userGuid", userId);
    formData.append("pageNumber", 1);
    formData.append("PageSize", 4);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/EmployerDashbord/GetEmployeesList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#userList').empty();
            let employerList = "";
            if (response.employeesListForTransactions != null && response.RstKey == 1) {
                $("#transnorecord").hide();
                $(response.employeesListForTransactions).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dd = created_date.getDate();
                    let mm = created_date.getMonth() + 1;
                    let yyyy = created_date.getFullYear();
                    let dateFormat = dd + '/' + mm + '/' + yyyy;
                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + value.FirstName + " " + value.LastName + '</td>';
                    employerList += '<td>' + value.Email + '</td>';
                    employerList += '<td>' + value.CountryCode + " " + value.PhoneNumber + '</td>';
                    employerList += '<td>' + value.StaffId + '</td>';
                    if (value.StatusId == 1) {
                        employerList += '<td style="color:#0CAF38">' + value.Status + '</td>';
                    } else if (value.StatusId == 0) {
                        employerList += '<td style="color:#EB2E0C">' + value.Status + '</td>';
                    }

                    employerList += '<td><span id="view-employeeProfile" class="view-employeeProfile" data-val=' + value.UserGuid + '><img class="img-eye" src="../Content/images/Employer/view_eye_icon.svg" alt="" data-toggle="tooltip" title="View Profile"></span>';
                    employerList += '</tr>';

                });
                $('#userList').append(employerList);
            }
            else {
                $("#transnorecord").show();
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

function getDashboardPayCycleData(userId) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/EmployerDashbord/GetDashboardPayCycleData",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.getDashboardPayCycleResponse != null && response.RstKey == 1) {

                $("#PayCycle").text(response.getDashboardPayCycleResponse.PayCycleFrom + ' - ' + response.getDashboardPayCycleResponse.PayCycleTo);
            }
            else {

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
        url: "/EmployerDashbord/GetDashboardWeeklyGraphData",
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
        url: "/EmployerDashbord/GetDashboardYearlyGraphData",
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

function getDashboardMonthLyGraphData(userId, week) {

    var formData = new FormData();
    formData.append("EmployerGuid", userId);
    formData.append("LastWeak", week);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/EmployerDashbord/GetDashboardMonthlyGraphData",
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

    var stackedBarChartCanvas = $('#employerBarChart').get(0).getContext('2d')
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

    var stackedBarChartCanvas = $('#employerBarChart').get(0).getContext('2d')
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