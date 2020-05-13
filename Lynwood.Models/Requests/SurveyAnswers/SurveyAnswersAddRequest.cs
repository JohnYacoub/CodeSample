using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests.SurveyAnswers
{
    public class SurveyAnswersAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int InstanceId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int QuestionId { get; set; }

        [Range(1, int.MaxValue)]
        public int? AnswerOptionId { get; set; }

        [MaxLength(500)]
        public string Answer { get; set; }

        [Range(1, int.MaxValue)]
        public int? AnswerNumber { get; set; }
    }
}
