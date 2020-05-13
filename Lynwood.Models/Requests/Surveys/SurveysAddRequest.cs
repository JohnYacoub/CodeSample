using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests
{
    public class SurveysAddRequest
    {
        [Required, MaxLength(100), Display(Name = "Name")]
        public string Name { get; set; }

        [Required, MaxLength(4000), Display(Name = "Description")]
        public string Description { get; set; }

        [Range(1, int.MaxValue), Display(Name = "Status Id")]
        public int StatusId { get; set; }

        [Range(1, int.MaxValue), Display(Name = "Survey Type Id")]
        public int SurveyTypeId { get; set; }
        public string ImageUrl { get; set; }
    }
}
