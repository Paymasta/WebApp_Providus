var UserGuid = "";
var categoryId = 0;
var ServiceName = "";
let amtError = false;
let categoryError = false;

let airtime = 0;
let data = 0;
let tv = 0;
let power = 0;
let internet = 0;

let BudgetAmountairtime = 0;
let BudgetAmountdata = 0;
let BudgetAmounttv = 0;
let BudgetAmountpower = 0;
let BudgetAmountinternet = 0;
$.ajaxSetup({ async: false });
$(document).ready(function () {
    $("#Budgetloader1").hide();
    UserGuid = sessionStorage.getItem("User1");

    getBudgetServices(UserGuid);
    getExpenses(UserGuid);
    getDailyExpense(UserGuid);
    $("#btnCreateBudget").click(function () {
        $("#budgetModal").modal('show');
    })


    $('#budgetServices').change(function () {
        categoryId = $('select#budgetServices option:selected').val();
        ServiceName = $('select#budgetServices option:selected').text();

    });

    $("#btnSaveBudget").click(function () {
        var txtAmount = $("#txtBudgetAmount").val();
        var formData = new FormData();
        formData.append("Amount", txtAmount);
        formData.append("UserGuid", UserGuid);
        formData.append("CategoryId", categoryId);

        if (txtAmount == "" || txtAmount == 0 || txtAmount == '') {
            amtError = false;
            //$("#BudgetCategoryError").show();
            //$("#BudgetCategoryError").text("Please enter amount");
            //swal({
            //    title: "Error!",
            //    text: "Please enter amount",
            //    icon: "warning",
            //    button: "Aww yiss!",
            //}, function succes(isDone) { });

            swal({
                title: "Oh no!",
                text: "Please enter amount",
                icon: "error",
                button: "Retry",
            });
        }
        else {
            amtError = true;
            $("#BudgetCategoryError").hide();
        }
        if (categoryId == "" || categoryId == '') {
            categoryError = false;
            swal({
                title: "Oh no!",
                text: "Please select category",
                icon: "error",
                button: "Retry",
            });
        } else {
            categoryError = true;
            $("#txtBudgetAmountError").hide();
        }
        if (amtError == true && categoryError == true) {
            $('#Budgetloader1').show();
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Budget/CreateBudget/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('#Budgetloader1').hide();
                    //$(".modal").modal('hide');
                    if (response.RstKey == 1) {
                        $("#budgetModal").modal('hide');
                        swal(
                            'Success!',
                            response.Message,
                            'success'
                        ).catch(swal.noop);
                        //swal({
                        //    title: "Success!",
                        //    text: response.Message,
                        //    icon: "success",
                        //    button: "Aww yiss!",
                        //}, function succes(isDone) { });
                    }
                    else if (response.RstKey == 2) {
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 3) {
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                    }
                    else if (response.RstKey == 4) {
                        swal(
                            'Error!',
                            response.Message,
                            'error'
                        ).catch(swal.noop);
                    }
                }
            });
        }


    })


    // barChart();
    //lineChart();
})

function chart(data1, data2, data3, data4, data5) {

    if (data1 == 0 && data2 == 0 && data3 == 0 && data4 && data5) {
        var xValues = [];
        var yValues = [100, 0, 0, 0, 0];
        var barColors = [
            "#e3e3e3",

        ];

    } else {
        var xValues = [];
        var yValues = [data1, data2, data3, data4, data5];
        var barColors = [
            "#F66847",
            "#3EA4FF",
            "#53D17A",
            "#151B54",
            "#01F9C6"
        ];

    }

    new Chart("myChart", {
        type: "pie",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues,
                borderWidth: 1
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

function getBudgetServices(userGuid) {
    $('#Budgetloader1').show();
    var formData = new FormData();
    formData.append("request", userGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "Budget/GetServiceList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            let ddOptions = "";//'<option value="">Please select country</option>'
            $.each(response.serviceCategories, function (index, value) {
                // APPEND OR INSERT DATA TO SELECT ELEMENT.
                ddOptions += '<option value="' + value.Id + '">' + value.CategoryName + '</option>';
            });
            $('#budgetServices').append(ddOptions);
            $('#Budgetloader1').hide();
        }
    });

}

function getExpenses(userGuid) {

    $('#Budgetloader1').show();
    var formData = new FormData();
    formData.append("request", userGuid);
    const serviceData = [];
    $.ajax({
        type: "POST",
        cache: false,
        url: "Budget/GetExpenses",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            debugger
            getBudget(UserGuid);
            if (response.RstKey == 1) {

                if (response.expenseTrack.AIRTIME != null) {
                    airtime = response.expenseTrack.AIRTIME;
                    $("#txtAIRTIME").text(airtime);
                } else {
                    $("#txtAIRTIME").text(0);
                }
                if (response.expenseTrack.INTERNET != null) {
                    internet = response.expenseTrack.INTERNET;
                    $("#txtINTERNET").text(internet);
                } else {
                    $("#txtINTERNET").text(0);
                }
                if (response.expenseTrack.CABLE != null) {
                    tv = esponse.expenseTrack.CABLE;
                    $("#txtTV").text(tv);
                } else {
                    $("#txtTV").text(0);
                }
                if (response.expenseTrack.ELECTICITY != null) {
                    power = response.expenseTrack.ELECTICITY;
                    $("#txtPOWER").text(power);
                } else {
                    $("#txtPOWER").text(0);
                }
                if (response.expenseTrack.DATABUNDLE != null) {
                    data = response.expenseTrack.DATABUNDLE;
                    $("#txtDATA").text(data);
                } else {
                    $("#txtDATA").text(0);
                }
                //chart(10, 10, 10, 10, 10);
                chart(airtime, internet, tv, power, data);
            } else {
                chart(0, 0, 0, 0, 0);
            }


            //$('#budgetServices').append(ddOptions);
            $('#Budgetloader1').hide();
        }
    });

}

function getBudget(userGuid) {

    $('#Budgetloader1').show();
    var formData = new FormData();
    formData.append("request", userGuid);
    const serviceData = [];
    $.ajax({
        type: "POST",
        cache: false,
        url: "Budget/GetBudget",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            debugger

            if (response.RstKey == 1) {
                if (response.expenseTrack.AIRTIME != null) {
                    debugger
                    BudgetAmountairtime = response.expenseTrack.AIRTIME;
                    $("#txtAIRTIMEBudget").text(response.expenseTrack.AIRTIME)
                    var airAmt = parseFloat(response.expenseTrack.AIRTIME) - parseFloat(airtime);
                    $("#txtAIRTIMEUsedBudget").text(airtime + "=" + airAmt);
                } else {
                    $("#txtAIRTIMEBudget").text(0)
                    $("#txtAIRTIMEUsedBudget").text(0);
                    BudgetAmountairtime = 0;
                }
                if (response.expenseTrack.CABLE != null) {
                    BudgetAmounttv = response.expenseTrack.CABLE;
                    $("#txtTVBudget").text(response.expenseTrack.CABLE)

                    var cableAmt = parseFloat(response.expenseTrack.CABLE) - parseFloat(tv);
                    $("#txtTVUsedBudget").text(tv + "=" + cableAmt);

                } else {
                    $("#txtTVBudget").text(0)
                    $("#txtTVUsedBudget").text(0);
                    BudgetAmounttv = 0;
                }
                if (response.expenseTrack.DATABUNDLE != null) {
                    BudgetAmountdata = response.expenseTrack.DATABUNDLE;
                    $("#txtDATABudget").text(response.expenseTrack.DATABUNDLE)

                    var dataAmt = parseFloat(response.expenseTrack.DATABUNDLE) - parseFloat(data);
                    $("#txtDATAUsedBudget").text(data + "=" + dataAmt);
                } else {
                    $("#txtDATABudget").text(0)
                    $("#txtDATAUsedBudget").text(0);
                    BudgetAmountdata = 0;
                }
                if (response.expenseTrack.INTERNET != null) {
                    BudgetAmountinternet = response.expenseTrack.INTERNET
                    $("#txtINTERNETBudget").text(response.expenseTrack.INTERNET)
                    var internetAmt = parseFloat(response.expenseTrack.INTERNET) - parseFloat(internet);
                    $("#txtINTERNETUsedBudget").text(internet + "=" + internetAmt);
                } else {
                    $("#txtINTERNETBudget").text(0)
                    $("#txtINTERNETUsedBudget").text(0);
                    BudgetAmountinternet = 0;
                }
                if (response.expenseTrack.ELECTICITY != null) {
                    BudgetAmountpower = response.expenseTrack.ELECTICITY;
                    $("#txtPOWERBudget").text(response.expenseTrack.ELECTICITY)

                    var electricityAmt = parseFloat(response.expenseTrack.ELECTICITY) - parseFloat(power);
                    $("#txtPOWERUsedBudget").text(power + "=" + electricityAmt);
                } else {
                    $("#txtPOWERBudget").text(0)
                    $("#txtPOWERUsedBudget").text(0);
                    BudgetAmountpower = 0;
                }
            } else {
                // chart(0, 0, 0, 0, 0);
            }


            //$('#budgetServices').append(ddOptions);
            $('#Budgetloader1').hide();
        }
    });

}

function barChart() {
    var ctxB = document.getElementById("barChart").getContext('2d');
    var myBarChart = new Chart(ctxB, {
        type: 'bar',
        data: {
            labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
            datasets: [{
                label: '# of Votes',
                data: [12, 19, 3, 5, 2, 3],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}

function lineChart(data) {


    var chart = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Daily Expense Chart"
        },
        data: [{
            type: "line",
            indexLabelFontSize: 100,
            dataPoints: data
            //dataPoints: [
            //    { y: 450 },
            //    { y: 414 },
            //    { y: 520, indexLabel: "\u2191 highest", markerColor: "red", markerType: "triangle" },
            //    { y: 460 },
            //    { y: 450 },
            //    { y: 500 },
            //    { y: 480 },
            //    { y: 480 },
            //    { y: 410, indexLabel: "\u2193 lowest", markerColor: "DarkSlateGrey", markerType: "cross" },
            //    { y: 500 },
            //    { y: 480 },
            //    { y: 510 },
            //    { y: 800 }
            //]
        }]
    });
    chart.render();

}

function getDailyExpense(userGuid) {

    $('#Budgetloader1').show();
    var formData = new FormData();
    formData.append("request", userGuid);
    const serviceData = [];
    $.ajax({
        type: "POST",
        cache: false,
        url: "Budget/GetDailyExpense",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            debugger

            if (response.RstKey == 1) {
                var amtData = [];
                $.each(response.dailyExpenseAmounts, function (index, value) {
                    var data = {
                        "y": parseInt(value.TotalAmount)
                    };
                    amtData.push(data);
                });
                lineChart(amtData);
                console.log("Amt res============================================>" + JSON.stringify(amtData));
            } else {
                // chart(0, 0, 0, 0, 0);
            }


            //$('#budgetServices').append(ddOptions);
            // $('#Budgetloader1').hide();
        }
    });

}