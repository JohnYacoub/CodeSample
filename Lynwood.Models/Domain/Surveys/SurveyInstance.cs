﻿using Lynwood.Models.Requests.Surveys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Domain
{
    public class SurveyInstance 
    {
        public int SurveyId { get; set; }

        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
