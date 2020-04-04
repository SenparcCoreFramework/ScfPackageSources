using Senparc.Scf.Core.Config;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Senparc.Xscf.DatabaseToolkit
{
    public partial class Register : IXscfRazorRuntimeCompilation
    {
        public string LibraryPath => Path.Combine(SiteConfig.WebRootPath, "..", "..", "..", "..", "ScfPackageSources", "src", "Extensions", "Senparc.Xscf.DatabaseToolkit");
    }
}
