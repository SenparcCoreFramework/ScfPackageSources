using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Senparc.Scf.AreaBase.Admin.Filters
{
    public class CustomerResourceFilterAttribute : Attribute, Microsoft.AspNetCore.Mvc.Filters.IFilterMetadata
    {
        public string[] ResourceCodes { get; set; }

        public CustomerResourceFilterAttribute(string[] resuouceCodes)
        {
            ResourceCodes = resuouceCodes;
        }

    }
}
