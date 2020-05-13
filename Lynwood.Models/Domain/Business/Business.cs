using System;

namespace Lynwood.Models.Domain.Business
{
    public class Business
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public BusinessType BusinessType { get; set; }
        public IndustryType IndustryType { get; set; }
        public int AnnualBusinessIncome { get; set; }
        public int ProjectedAnnualBusinessIncome { get; set; }
        public int YearsInBusiness { get; set; }
        public string ImageUrl { get; set; }
        public Address Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
