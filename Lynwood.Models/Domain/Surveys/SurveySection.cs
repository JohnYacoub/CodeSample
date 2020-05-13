using System;
using System.Collections.Generic;

namespace Lynwood.Models.Domain
{
    public class SurveySection
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
