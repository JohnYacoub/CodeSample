using System;
using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Users
{
    public class UserProfileAddRequest
    {
        [Required, MaxLength(255)]
        public string FirstName { get; set; }
        [Required, MaxLength(255)]
        public string LastName { get; set; }
        [MaxLength(4000)]
        public string Bio { get; set; }
        [MaxLength(400)]
        public string AvatarUrl { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Range(0, int.MaxValue)]
        public int? RaceEthnicityId { get; set; }
        [Range(0, int.MaxValue)]
        public int? EducationLevelId { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int HouseIncome { get; set; }
        [Range(0, int.MaxValue)]
        public int? YearsInBusiness { get; set; }
        [Required, DataType(DataType.PostalCode)]
        public string Zip { get; set; }
    }
}
