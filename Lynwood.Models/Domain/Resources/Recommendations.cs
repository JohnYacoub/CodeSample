using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain.Resource
{
    public class Recommendations: CategoryType
    {
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string BusinessTypeName { get; set; }
        public string IndustryTypeName { get; set; }
        public string ImageUrl { get; set; }
        public string SiteUrl { get; set; }
        public string Phone { get; set; }
        public List<CategoryType> Categories { get; set; }
    }
}
