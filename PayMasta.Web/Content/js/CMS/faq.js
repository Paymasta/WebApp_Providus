$(document).ready(function () {

    getFaq();
    getFaqAfterLogin();
});


function getFaq() {
    var formData = new FormData();
    formData.append("id", 1);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/CMS/GetPayMastaFaq/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log("GetPayMastaFaq =" + JSON.stringify(response));
            var data = "";
            $(response).each(function (index, value) {
              //  var data = "";
                data += '<div class="accordion-item"  id="questionAndAnswer">';
                data += '<h2 class="accordion-header" id="headingTwo">';
                data += '<div class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo' + value.Id + '" aria-expanded="false" aria-controls="collapseTwo">' + value.QuestionText + '</div>';
                data += '</h2>';
                data += '</div>';


                $(value.FaqDetails).each(function (index, value1) {
                    data += '<div id="collapseTwo' + value1.FaqId + '" class="accordion-collapse collapse" aria-labelledby="headingTwo" data-bs-parent="#accordionExample">';
                    data += '<div class="accordion-body">' + value1.Detail;
                    data += '</div>';
                    data += ' </div>';
                });
               
            });
            $("#accordionExample").append(data);
        }
    });
}

function getFaqAfterLogin() {
    var formData = new FormData();
    formData.append("id", 1);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/CMS/GetPayMastaFaq/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log("GetPayMastaFaq =" + JSON.stringify(response));
            var data = "";
            $(response).each(function (index, value) {
               // var data = "";
                data += '<div class="question-faq-container"><div data-toggle="collapse" data-target="#collapse' + value.Id + '" class="questions">' + value.QuestionText+'</div><img src="/Content/images/Employer/dropdown.svg" class="arrow-img" /></div>';
              

                $(value.FaqDetails).each(function (index, value) {
                   
                    data += ' <div id="collapse' + value.FaqId + '" class="collapse answers-faq">' + value.Detail+'</div >';


                });
              
            });
            $("#divfaq").append(data);
        }
    });
}