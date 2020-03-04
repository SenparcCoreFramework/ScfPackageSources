using Microsoft.AspNetCore.Mvc;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Scf.AreaBase.Admin
{
    /// <summary>
    /// XSCF 模块的页面模板
    /// </summary>
    public abstract class AdminXscfModulePageModelBase : AdminPageModelBase
    {
        [BindProperty]
        public string Uid { get; set; }

        /// <summary>
        /// XscfModuleDto
        /// </summary>
        public abstract XscfModuleDto XscfModuleDto { get; set; }

        /// <summary>
        /// 当前正在操作的 XscfRegister
        /// </summary>
        public IXscfRegister XscfRegister => XscfModuleDto != null ? XscfRegisterList.FirstOrDefault(z => z.Uid == XscfModuleDto.Uid) : null;

        /// <summary>
        /// 所有 XscfRegister 列表（包括还未注册的）
        /// </summary>
        public List<IXscfRegister> XscfRegisterList => Senparc.Scf.XscfBase.Register.RegisterList;


        public AdminXscfModulePageModelBase()
        {

        }
    }
}
