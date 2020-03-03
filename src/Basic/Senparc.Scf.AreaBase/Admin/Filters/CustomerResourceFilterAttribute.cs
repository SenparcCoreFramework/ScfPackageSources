using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Senparc.Scf.AreaBase.Admin.Filters
{
    public class CustomerResourceFilterAttribute : Attribute, Microsoft.AspNetCore.Mvc.Filters.IFilterMetadata
    {
        public string ResourceCode { get; set; }

        public CustomerResourceFilterAttribute(string resuouceCode)
        {
            ResourceCode = resuouceCode;
        }

    }
}
