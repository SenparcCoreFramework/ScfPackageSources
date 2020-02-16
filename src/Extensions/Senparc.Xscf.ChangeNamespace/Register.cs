using Senparc.Scf.XscfBase;

namespace Senparc.Xscf.ChangeNamespace
{
    [XscfRegister]
    public class Register : IRegister
    {
        public Register()
        { }

        #region IRegister 接口

        public string Name => "Senparc.Xscf.ChangeNamespace";
        public string Uid => "476A8F12-860D-4B18-B703-393BBDEFBD85";
        public string MenuName => "修改命名空间";
        public string Description => "此功能提供给开发者在安装完 SCF、发布产品之前，全局修改命名空间，请在生产环境中谨慎使用，此操作不可逆！必须做好提前备份！不建议在已经部署至生产环境并开始运行后使用此功能！";

        public void Install()
        {
        }

        public void Uninstall()
        {
        }

        #endregion
    }
}
