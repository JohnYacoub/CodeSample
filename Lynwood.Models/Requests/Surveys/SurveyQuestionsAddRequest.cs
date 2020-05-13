using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests.SurveyQuestions
{
    public class SurveyQuestionsAddRequest
    {
        [Required, MaxLength(500), Display(Name = "Question")]
        public string Question { get; set; }

        [Required, MaxLength(255), Display(Name = "Help Text")]
        public string HelpText { get; set; }

        [Required, Display(Name = "Is Required")]
        public bool? IsRequired { get; set; }

        [Required, Display(Name = "Is Multiple Allowed")]
        public bool? IsMultipleAllowed { get; set; }

        [Range(1, int.MaxValue), Display(Name = "Question Type Id")]
        public int QuestionTypeId { get; set; }

        [Range(1, int.MaxValue), Display(Name = "Section Id")]
        public int SectionId { get; set; }

        [Range(1, int.MaxValue), Display(Name = "Status Id")]
        public int StatusId { get; set; }

        [Range(1, int.MaxValue), Display(Name = "Sort Order")]
        public int SortOrder { get; set; }     
    }
}
