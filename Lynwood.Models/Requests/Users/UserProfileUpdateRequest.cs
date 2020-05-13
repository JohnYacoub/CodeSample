using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Users
{
    public class UserProfileUpdateRequest : UserProfileAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
