using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Xscf.DatabaseToolkit
{
	public partial class Register : IXscfRazorRuntimeCompilation
	{
		public string LibraryPath => Path.Combine(SiteConfig.WebRootPath, "..", "..", "..", "ScfPackageSources", "src", "Extensions", "Senparc.Xscf.DatabaseToolkit");
	}
}
