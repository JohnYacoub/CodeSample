using System.Collections.Generic;

namespace Lynwood.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public int Status { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}