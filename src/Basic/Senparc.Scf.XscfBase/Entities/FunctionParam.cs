using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public class FunctionParam
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TypeCode TypeCode { get; set; }

        public FunctionParam()
        {
        }

        public FunctionParam(string name, string description, TypeCode typeCode)
        {
            Name = name;
            Description = description;
            TypeCode = typeCode;
        }


    }
}
