using System.ComponentModel.DataAnnotations;

namespace Lynwood.Models.Users
{
    public class AddressAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int AddressTypeId { get; set; }
        [MaxLength (225)]
        public string LineOne { get; set; }
        [MaxLength (225)]
        public string LineTwo { get; set; }
        [MaxLength(225)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Zip { get; set; }
        [Range(1, int.MaxValue)]
        public int StateId { get; set; }
        [Range(double.MinValue, double.MaxValue)]
        public double Latitude { get; set; }
        [Range(double.MinValue, double.MaxValue)]
        public double Longitude { get; set; }

    }
}
