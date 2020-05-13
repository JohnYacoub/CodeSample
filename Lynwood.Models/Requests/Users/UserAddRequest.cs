using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Users
{
    public class UserAddRequest 
    {
        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required, MaxLength(128), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z])(?=.*[!@#$%^&*()_+=\[{\]};:<>|./?,-]).{8,}$", 
            ErrorMessage = "must be at least 8 character long and contain one of the following 'UpperCase/Lowercase letter, one number, one Special character'")]
        public string PasswordHash { get; set; }

        [Required, Compare("PasswordHash", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public int  UserTypeId { get; set; }
    }
}
