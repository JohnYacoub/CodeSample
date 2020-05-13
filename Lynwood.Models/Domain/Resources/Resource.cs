using Lynwood.Models.Domain.Business;
using System;
using System.Collections.Generic;

namespace Lynwood.Models.Domain.Resources
{
    public class Resource
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public BusinessType BusinessType { get; set; }
        public IndustryType IndustryType { get; set; }
        public string ImageUrl { get; set; }
        public string SiteUrl { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<CategoryType> Categories { get; set; }
    }
}