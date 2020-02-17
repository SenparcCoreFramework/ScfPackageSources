using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    /// <summary>
    /// FunctionParammeter 信息
    /// </summary>
    public class FunctionParammeterInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }

        public FunctionParammeterInfo()
        {
        }

        public FunctionParammeterInfo(string name, string title, string description, bool isRequired)
        {
            Name = name;
            Title = title;
            Description = description;
            IsRequired = isRequired;
        }
    }
}
