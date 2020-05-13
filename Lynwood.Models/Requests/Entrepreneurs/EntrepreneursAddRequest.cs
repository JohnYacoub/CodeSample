using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Entrepreneurs
{
    public class EntrepreneursAddRequest
    {
        [Range(1, int.MaxValue), Display(Name = "Industry Type Id")]
        public int IndustryTypeId { get; set; }

        [Range(1, int.MaxValue), Display(Name = "Company Status Id")]
        public int CompanyStatusId { get; set; }

        [Display(Name = "Is Mentored")]
        public bool IsMentored { get; set; }

        [Display(Name = "Has Security Clearance")]
        public bool HasSecurityClearance { get; set; }
         
        [Display(Name = "Has Insurance")]
        public bool HasInsurance { get; set; }

        [Display(Name = "Has Bonds")]
        public bool HasBonds { get; set; }

        [Required, MaxLength(4000), Display(Name = "Specialized Equipment")]
        public string SpecializedEquipment { get; set; }
        public string ImageUrl { get; set; }
    }
}
