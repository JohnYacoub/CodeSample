using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.AppSettings
{
   public class AWSCredential
    {
        public string AccessKey { get; set; }
        public string Secret { get; set; }
        public string BucketRegion { get; set; }
        public string Domain { get; set; }
        public string BucketName { get; set; }
    }
}
