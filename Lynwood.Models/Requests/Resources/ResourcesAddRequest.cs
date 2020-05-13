using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Resources
{
    public class ResourcesAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }
        public List<int> Categories { get; set; }
        [Required, MaxLength(255)]
        public string CompanyName { get; set; }
        [Required, MaxLength(1000)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string ContactName { get; set; }
        [MaxLength(225)]
        public string ContactEmail { get; set; }
        [Range(1, int.MaxValue), Display(Name = "Business Type Id")]
        public int BusinessTypeId { get; set; }
        [Range(1, int.MaxValue), Display(Name = "Industry Type Id")]
        public int IndustryTypeId { get; set; }
        [MaxLength(2000)]
        public string ImageUrl { get; set; }
        [MaxLength(2000)]
        public string SiteUrl { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        public int AddressId { get; set; }
    }
}