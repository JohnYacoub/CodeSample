using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain
{
    public class SurveyDisplayAnswers
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurveyName { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public int SurveyTypeId { get; set; }
        public SurveyInstance surveyInstance { get; set; }
        public List<SurveyQuestionsAnswers> SurveyQuestionsAnswers { get; set; }

    }
}
