using Lynwood.Models.Requests.Files;
using System;

namespace Lynwood.Models.Domain
{
    public class FileUpload : FileUpdateRequest
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}