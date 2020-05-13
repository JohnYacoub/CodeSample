using Lynwood.Models.Domain.Surveys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain
{
    public class SurveyQDetails
    {
        public int SectionId { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string HelpText { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMultipleAllowed { get; set; }
        public int QuestionTypeId { get; set; }
        public int SortOrder { get; set; }
        public List<AnswerOption> AnswerOptions { get; set; }
    }
}

