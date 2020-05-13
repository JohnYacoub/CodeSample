using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests.Surveys
{
    public class SurveyInstancesUpdateRequest : SurveyInstancesAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
