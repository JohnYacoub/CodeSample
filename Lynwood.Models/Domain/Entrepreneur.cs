namespace Lynwood.Models.Domain
{
    public class Entrepreneur
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }  
        public IndustryType IndustryType { get; set; }
        public CompanyStatus CompanyStatus { get; set; }
        public bool HasSecurityClearance { get; set; }
        public bool HasInsurance { get; set; }
        public bool HasBonds { get; set; }
        public string ImageUrl { get; set; }
        public string SpecializedEquipment { get; set; }
    }
}
