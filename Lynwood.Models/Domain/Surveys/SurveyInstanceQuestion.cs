using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain.Surveys
{
    public class SurveyInstanceQuestion
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }

        public List<SurveyInstanceAnswer> Answers { get;set;}
    }
}
