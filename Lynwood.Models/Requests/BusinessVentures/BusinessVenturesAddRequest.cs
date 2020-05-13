using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.BusinessVentures
{
    public class BusinessVenturesAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int StatusId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int BusinessTypeId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int IndustryTypeId { get; set; }

        [Range(1,int.MaxValue)]
        public int? AnnualBusinessIncome { get; set; }

        [Range(1, int.MaxValue)]
        public int? ProjectedAnnualBusinessIncome { get; set; }

        [Range(1, int.MaxValue)]
        public int? YearsInBusiness { get; set; }

        public string ImageUrl { get; set; }

        public int AddressId { get; set; }
    }
}
