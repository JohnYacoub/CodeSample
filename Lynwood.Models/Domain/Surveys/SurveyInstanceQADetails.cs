using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain.Surveys
{
    public class SurveyInstanceQADetails :SurveyInstanceDetails
    {
        public List<Answer> QuestionAnswers { get; set; }
    }
}
