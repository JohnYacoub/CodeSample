using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain
{
    public class SurveyQuestionsAnswers
    {
        public int SortOrder { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int InstanceId { get; set; }
    }
}
