using Lynwood.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Domain.Users
{
    public class AddressUpdateRequest : AddressAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
