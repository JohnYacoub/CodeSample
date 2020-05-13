using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests.Addresses
{
    public class AddressVerifyAddRequest
    {
        [MaxLength(225)]
        public string LineOne { get; set; }
        [MaxLength(225)]
        public string LineTwo { get; set; }
        [MaxLength(225)]
        public string City { get; set; }
        [MaxLength(225)]
        public string StateCode { get; set; }
   
    }
}
