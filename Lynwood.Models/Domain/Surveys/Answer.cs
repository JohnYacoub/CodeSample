using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain.Surveys
{
   public class Answer
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public List<SurveyInstanceQuestion> SurveyQuestions { get; set; }
    }
}
