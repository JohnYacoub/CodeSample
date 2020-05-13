using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Requests.Entrepreneurs
{
    public class EntrepreneursUpdateRequest : EntrepreneursAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
