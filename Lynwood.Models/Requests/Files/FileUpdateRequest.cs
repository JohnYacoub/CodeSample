using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Files
{
    public class FileUpdateRequest : FileAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}