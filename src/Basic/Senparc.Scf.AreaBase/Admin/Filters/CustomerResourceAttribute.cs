using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Senparc.Scf.AreaBase.Admin.Filters
{
    public class CustomerResourceAttribute : Attribute
    {
        public string[] ResourceCodes { get; set; }

        public CustomerResourceAttribute(params string[] resuouceCodes)
        {
            ResourceCodes = resuouceCodes;
        }

    }
}
