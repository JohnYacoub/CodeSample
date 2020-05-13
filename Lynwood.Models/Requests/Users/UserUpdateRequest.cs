using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Users
{
    public class UserUpdateRequest : UserAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
        public int Status { get; set; }
        public bool IsConfirmed { get; set; }
    }
}