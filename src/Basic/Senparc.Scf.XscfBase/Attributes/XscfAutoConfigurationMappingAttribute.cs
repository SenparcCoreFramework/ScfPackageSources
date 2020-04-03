using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase.Attributes
{
    /// <summary>
    /// 自动配置 ConfigurationMapping 特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class XscfAutoConfigurationMappingAttribute : Attribute
    {
    }
}
