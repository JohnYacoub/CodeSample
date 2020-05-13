using Lynwood.Models.Domain.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain.Resources
{
   public class ResourcesTypes
    {
        public List<CategoryType> Categories { get; set; }
        public List<BusinessType> BusinessTypes { get; set; }
        public List<IndustryType> IndustryTypes { get; set; }

    }
}
