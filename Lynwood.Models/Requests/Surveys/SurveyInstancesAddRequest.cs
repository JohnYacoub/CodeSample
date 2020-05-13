using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Surveys
{
    public class SurveyInstancesAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int SurveyId { get; set; }
    }
}
