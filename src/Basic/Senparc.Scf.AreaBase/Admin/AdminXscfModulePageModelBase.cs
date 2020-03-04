using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
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
        public virtual XscfModuleDto XscfModuleDto { get; set; }

        /// <summary>
        /// 当前正在操作的 XscfRegister
        /// </summary>
        public virtual IXscfRegister XscfRegister => XscfModuleDto != null ? XscfRegisterList.FirstOrDefault(z => z.Uid == XscfModuleDto.Uid) : null;

        /// <summary>
        /// 所有 XscfRegister 列表（包括还未注册的）
        /// </summary>
        public virtual List<IXscfRegister> XscfRegisterList => Senparc.Scf.XscfBase.Register.RegisterList;

        protected readonly XscfModuleService _xscfModuleService;

        protected AdminXscfModulePageModelBase(XscfModuleService xscfModuleService)
        {
            _xscfModuleService = xscfModuleService;

            SetXscfModuleDto();
        }

        public virtual void SetXscfModuleDto()
        {
            if (Uid.IsNullOrEmpty())
            {
                throw new XscfPageException(null, "页面未提供UID！");
            }

            var xscfModule = _xscfModuleService.GetObject(z => z.Uid == Uid);
            if (xscfModule == null)
            {
                throw new XscfPageException(null, "尚未注册 XSCF 模块，UID：" + Uid);
            }

            XscfModuleDto = _xscfModuleService.Mapper.Map<XscfModuleDto>(xscfModule);
        }
    }
}
