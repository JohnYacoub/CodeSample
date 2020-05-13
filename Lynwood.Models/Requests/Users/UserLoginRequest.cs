using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Requests.Users
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
