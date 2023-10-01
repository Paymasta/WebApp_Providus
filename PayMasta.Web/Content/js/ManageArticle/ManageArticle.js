var UserGuid = "";
$(document).ready(function () {
    UserGuid = sessionStorage.getItem("User1");
    getArticleList();

    $(document).on('click', '#rd_button', function () {
        if ($(this).is(':checked')) {
            
            var data = $(this).val().split('-');
            var articleId = data[0];
            var correctOption = data[1];

            var radios = document.getElementsByName('options_' + articleId);
            for (var i = 0, r = radios, l = r.length; i < l; i++) {
                r[i].disabled = true;
            }

            debugger

            var formData = new FormData();
            formData.append("articleId", articleId);
            formData.append("AnsOption", correctOption);
            formData.append("UserGuid", UserGuid);


            $.ajax({
                type: "POST",
                cache: false,
                url: "Article/SubmitArticleAnswer",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                }
            });
        }
    });

});

function getArticleList() {
    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "Article/GetArticleList",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#articleList_body').empty();
            let articleList = '';

            if (response.RstKey == 1) {
                $("#noRecordFound").hide();

                $(response.Result).each(function (index, value) {

                    if (value.IsSubmitedResponse) {
                        var cls = "";
                        if (value.IsSubmitedResponseCorrect) {
                            articleList += `  <div class="panel panel-success">`
                            var cls = "label-success";
                        } else {
                            articleList += `  <div class="panel panel-danger">`
                            var cls = "label-danger";
                        }

                        articleList += `
                        <div class="panel-heading">Question ${index + 1} <span style="float:right">Price Money:₦ ${value.PriceMoney}</span></div>
                        <div class="panel-body">${value.ArticleText}</div>
                         <ul class="list-group">`

                        if (value.AnswerOption === 1) {
                            articleList += ` <li class="list-group-item ${cls}"><input disabled checked id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${1}"/><span style="margin-left:10px">${value.Option1Text}</span></li>`
                        } else {
                            articleList += ` <li class="list-group-item"><input disabled id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${1}"/><span style="margin-left:10px">${value.Option1Text}</span></li>`
                        }

                        if (value.AnswerOption === 2) {
                            articleList += ` <li class="list-group-item ${cls}"><input disabled checked id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${2}"/><span style="margin-left:10px">${value.Option2Text}</span></li>`
                        } else {
                            articleList += ` <li class="list-group-item"><input disabled id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${2}"/><span style="margin-left:10px">${value.Option2Text}</span></li>`
                        }

                        if (value.AnswerOption === 3) {
                            articleList += ` <li class="list-group-item ${cls}"><input disabled checked id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${3}"/><span style="margin-left:10px">${value.Option3Text}</span></li>`
                        } else {
                            articleList += ` <li class="list-group-item"><input disabled id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${3}"/><span style="margin-left:10px">${value.Option3Text}</span></li>`
                        }

                        if (value.AnswerOption === 4) {
                            articleList += ` <li class="list-group-item ${cls}"><input disabled checked id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${4}"/><span style="margin-left:10px">${value.Option4Text}</span></li>`
                        } else {
                            articleList += ` <li class="list-group-item"><input disabled id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${4}"/><span style="margin-left:10px">${value.Option4Text}</span></li>`
                        }
                        
                        `</ul> </div>`
                    } else {
                        articleList += `<div class="panel panel-default">
                        <div class="panel-heading">Question ${index + 1} <span style="float:right">Price Money:₦ ${value.PriceMoney}</span></div>
                        <div class="panel-body">${value.ArticleText}</div>
                         <ul class="list-group">'
                         <li class="list-group-item"><input id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${1}"/><span style="margin-left:10px">${value.Option1Text}</span></li>
                          <li class="list-group-item"><input id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${2}"/><span style="margin-left:10px">${value.Option2Text}</span></li>
                         <li class="list-group-item"><input id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${3}"/><span style="margin-left:10px">${value.Option3Text}</span></li>
                         <li class="list-group-item"><input id="rd_button" name="options_${value.ArticleId}" type="radio" value="${value.ArticleId}-${4}"/><span style="margin-left:10px">${value.Option4Text}</span></li>
                         </ul> </div>`
                    }

                });
                $('#articleList_body').append(articleList);


            } else {
                $("#noRecordFound").show();
            }
        }
    });
}