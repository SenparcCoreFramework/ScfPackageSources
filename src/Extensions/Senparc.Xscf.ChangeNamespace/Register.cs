using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
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

        public void Install()
        {
        }

        public void Uninstall()
        {
        }

        #endregion

        [XscfMethod("Change")]
        public void Change(string newNamespace)
        {
            //TODO：修改命名空间
        }
    }
}
