using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.SurveyAnswers
{
    public class UpdateSurveyAnswers : SurveyAnswersAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }

}
