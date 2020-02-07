using Senparc.Scf.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.Models.DataBaseModel
{
    /// <summary>
    /// 扩展模块信息
    /// </summary>
    public class XscfModule : EntityBase<int>
    {
        public string Name { get; set; }
        public string Uid { get; set; }
        public string MenuName { get; set; }
        public bool AllowRemove { get; set; }
        public XscfModules_State State { get; set; }
    }
}
