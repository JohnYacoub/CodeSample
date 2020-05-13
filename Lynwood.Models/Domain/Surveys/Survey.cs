using Lynwood.Models.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Domain
{
    public class Survey
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public SurveyStatus Status { get; set; }

        public SurveyType SurveyType { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string ImageUrl { get; set; }
    }
}
