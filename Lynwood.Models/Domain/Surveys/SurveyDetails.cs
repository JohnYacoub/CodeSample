using Lynwood.Models.Domain.Surveys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain
{
    public class SurveyDetails :Survey
    {
        public List<SurveySection> SurveySections { get; set; }
        public List<SurveyQDetails> SurveyQDetails { get; set; }
    }
}
