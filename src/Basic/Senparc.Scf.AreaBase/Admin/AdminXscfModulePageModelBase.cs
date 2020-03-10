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
        private XscfModuleDto _xscfModuleDto;
        /// <summary>
        /// XscfModuleDto
        /// </summary>
        public XscfModuleDto XscfModuleDto
        {
            get
            {
                if (_xscfModuleDto == null)
                {
                    SetXscfModuleDto();
                }
                return _xscfModuleDto;
            }
        }
        /// <summary>
        /// XscfModuleDto.Uid
        /// </summary>
        public string XscfModuleUid => XscfModuleDto?.Uid;

        /// <summary>
        /// 当前正在操作的 XscfRegister
        /// </summary>
        public virtual IXscfRegister XscfRegister => XscfModuleDto != null ? XscfRegisterList.FirstOrDefault(z => z.Uid == XscfModuleDto.Uid) : null;

        protected readonly Lazy<XscfModuleService> _xscfModuleService;

        protected AdminXscfModulePageModelBase(Lazy<XscfModuleService> xscfModuleService)
        {
            _xscfModuleService = xscfModuleService;
        }

        public virtual void SetXscfModuleDto()
        {
            if (Uid.IsNullOrEmpty())
            {
                throw new XscfPageException(null, "页面未提供UID！");
            }

            var xscfModule = _xscfModuleService.Value.GetObject(z => z.Uid == Uid);
            if (xscfModule == null)
            {
                throw new XscfPageException(null, "尚未注册 XSCF 模块，UID：" + Uid);
            }

            _xscfModuleDto = _xscfModuleService.Value.Mapper.Map<XscfModuleDto>(xscfModule);
        }
    }
}
