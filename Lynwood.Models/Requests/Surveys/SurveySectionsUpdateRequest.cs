using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests
{
   public class SurveySectionsUpdateRequest : SurveySectionsAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
