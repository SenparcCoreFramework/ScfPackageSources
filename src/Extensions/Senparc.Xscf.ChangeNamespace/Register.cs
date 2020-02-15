using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Xscf.ChangeNamespace
{
    [XscfRegister]
    public class Register : IRegister
    {
        public Register()
        { }

        #region IRegister 接口

        public string Name => "Senparc.Xscf.ChangeNamespace";
        public string MenuName => "修改命名空间";
        public string Description => "此功能提供给开发者在安装完 SCF、发布产品之前，全局修改命名空间，请在生产环境中谨慎使用，此操作不可逆！必须做好提前备份！不建议在已经部署至生产环境并开始运行后使用此功能！";

        public void Install()
        {
        }

        public void Uninstall()
        {
        }

        #endregion


        //[XscfMethod("Change")]
        //public void Change(

        //    [Description("命名空间根，必须以.结尾，用于替换[Senparc.Scf.]")]string newNamespace,
        //    [Description("本地物理路径，如：E:\\Senparc\\Scf\\")]string path)
        //{

        //}
    }
}
