using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public abstract class FunctionBase : IXscfFunction
    {
        public abstract IList<FunctionParam> FunctionParams { get; }
        public virtual IServiceProvider ServiceProvider { get; set; }


        public FunctionBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract string Run(params object[] param);
    }
}
