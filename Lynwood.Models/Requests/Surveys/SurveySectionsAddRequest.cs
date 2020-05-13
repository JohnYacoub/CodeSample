using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests
{
    public class SurveySectionsAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int SurveyId { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int SortOrder { get; set; }
    }
}
