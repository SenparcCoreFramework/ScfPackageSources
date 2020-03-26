using Microsoft.EntityFrameworkCore;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Xscf.DatabaseToolkit
{
    public class DatabaseToolkitEntities : XscfDatabaseDbContext
    {
        public override IXscfDatabase XscfDatabaseRegister => new Register();
        public DatabaseToolkitEntities(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<DbConfig> DbConfigs { get; set; }
    }
}
