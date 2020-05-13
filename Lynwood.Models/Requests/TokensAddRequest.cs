using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Requests
{
    public class TokensAddRequest
    {
        public Guid Token { get; set; }
        public int TokenType { get; set; }
        public int UserId { get; set; }
    }
}
