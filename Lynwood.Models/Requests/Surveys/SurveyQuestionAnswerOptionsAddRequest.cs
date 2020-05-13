using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests.SurveyQuestionAnswerOptions
{
    public class SurveyQuestionAnswerOptionsAddRequest
    {
        [Range(1, int.MaxValue), Display(Name = "Question Id")]
        public int QuestionId { get; set; }

        [Required, MaxLength(225), Display(Name = "Text")]
        public string Text { get; set; }

        [Required, MaxLength(100), Display(Name = "Values")]
        public string Values { get; set; }

        [Required, MaxLength(200), Display(Name = "Additional Info")]
        public string AdditionalInfo { get; set; }
    }
}
