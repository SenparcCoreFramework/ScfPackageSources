using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Repository
{
    public class XscfModuleRepository : ClientRepositoryBase<XscfModule>
    {
        public XscfModuleRepository(ISqlBaseFinanceData db) : base(db)
        {
        }
    }
}
