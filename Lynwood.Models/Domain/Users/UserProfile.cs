using Lynwood.Models.Domain.Users;
using System;

namespace Lynwood.Models.Domain
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Bio { get; set; }
        public string AvatarUrl { get; set; }
        public string Phone { get; set; }
        public RaceEthnicity RaceEthnicity { get; set; }
        public EducationLevel EducationLevels { get; set; }
        public int HouseIncome { get; set; }
        public int? YearsInBusiness { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }
}
