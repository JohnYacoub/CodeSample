namespace Lynwood.Models.Domain
{
    public class Address
    {
        public int Id { get; set; }
        public int AddressTypeId { get; set; }
        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
