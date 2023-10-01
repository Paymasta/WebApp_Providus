﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel
{
    public class ArticleViewModel
    {
        public long ArticleId { get; set; }
        public string ArticleText { get; set; }
        public double PriceMoney { get; set; }
        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public string Option4Text { get; set; }
        public int CorrectOption { get; set; }
    }

    public class ArticleResponseVM
    {
        public long ArticleId { get; set; }
        public string ArticleText { get; set; }
        public double PriceMoney { get; set; }
        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public string Option4Text { get; set; }
        public bool IsSubmitedResponse { get; set; }
        public int AnswerOption { get; set; }
        public bool IsSubmitedResponseCorrect { get; set; }
    }

    public class ArticleListResponse
    {
        public ArticleListResponse()
        {
            Result = new List<ArticleResponseVM>();
        }
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public List<ArticleResponseVM> Result { get; set;}
    }

    public class ApiResponseVM<T>
    {
        public bool IsSuccess { get; set; }
        public int RstKey { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public class ArticleAnsRequest
    {
        public Guid UserGuid { get; set; }
        public long ArticleId { get; set; }
        public int AnsOption { get; set; }
    }
}
