using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Resources
{
    public class ResourcesUpdateRequest : ResourcesAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
