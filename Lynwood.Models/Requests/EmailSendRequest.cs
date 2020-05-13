using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lynwood.Models.Requests
{
    public class EmailSendRequest
    {
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }
        [Required]
        public string Subject { get; set; }
        [DataType(DataType.EmailAddress)]
        public string To { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
        public string Recipient { get; set; }
        public string Sender { get; set; }
    }
}
