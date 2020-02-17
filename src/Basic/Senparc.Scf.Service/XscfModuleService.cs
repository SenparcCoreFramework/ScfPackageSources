using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Service
{
    public class XscfModuleService : ServiceBase<XscfModule>
    {
        public XscfModuleService(IRepositoryBase<XscfModule> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
        }

        /// <summary>
        /// 检查并更新版本
        /// </summary>
        /// <param name="storedDto"></param>
        /// <param name="assemblyDto"></param>
        /// <returns>返回是否需要新增或更新</returns>
        public bool CheckAndUpdateVersion(CreateOrUpdate_XscfModuleDto storedDto, UpdateVersion_XscfModuleDto assemblyDto)
        {
            if (storedDto == null)
            {
                //新增模块
                var xscfModule = new XscfModule(assemblyDto.Name, assemblyDto.Uid, assemblyDto.MenuName, assemblyDto.Version, "", assemblyDto.Description, true, Core.Enums.XscfModules_State.新增待审核);
                xscfModule.Create();
                base.SaveObject(xscfModule);
                return true;
            }
            else
            {
                //检查更新
                if (storedDto.Version != assemblyDto.Version)
                {
                    var xscfModule = base.GetObject(z => z.Uid == storedDto.Uid);
                    xscfModule.UpdateVersion(assemblyDto.Version, assemblyDto.MenuName, assemblyDto.Description);
                    base.SaveObject(xscfModule);
                    return true;
                }
                return false;
            }
        }
    }
}
