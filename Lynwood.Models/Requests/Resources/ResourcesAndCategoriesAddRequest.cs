using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Requests.Resources
{
    public class ResourcesAndCategoriesAddRequest
    {
        public int ResourceId { get; set; }
        public int ResourceCategoryId { get; set; }

    }
}
