namespace Lynwood.Models.Domain
{
    public class UserAuth : User
    {
        public string PasswordHash { get; set; }
    }
}
