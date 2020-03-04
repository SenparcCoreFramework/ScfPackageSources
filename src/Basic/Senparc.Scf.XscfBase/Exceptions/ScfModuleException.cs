using Senparc.Scf.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public class XscfPageException : SCFExceptionBase
    {
        public IXscfRegister XscfRegister { get; set; }

        public XscfPageException(IXscfRegister xscfRegister, string message, bool logged = false) : base(message, logged)
        {
            XscfRegister = xscfRegister;
        }

        public XscfPageException(IXscfRegister xscfRegister, string message, Exception inner, bool logged = false) : base(message, inner, logged)
        {
            XscfRegister = xscfRegister;
        }
    }
}
