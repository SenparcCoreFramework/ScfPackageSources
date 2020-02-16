using System;

namespace Senparc.Scf.XscfBase
{
    /// <summary>
    /// Xscf 模块特性 - 扩展方法
    /// </summary>
    public class XscfMethodAttribute : Attribute
    {
        public string Name { get; set; }

        public XscfMethodAttribute(string name)
        {
            Name = name;
        }
    }
}
