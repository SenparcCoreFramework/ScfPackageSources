using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.XscfBase;
using Senparc.Xscf.DatabaseToolkit.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Xscf.DatabaseToolkit
{
    public partial class Register : IXscfDatabase
    {
        public const string DATABASE_PREFIX = "DatabaseToolkit";
        public string DatabaseUniquePrefix => DATABASE_PREFIX;

        public Type XscfDatabaseDbContextType => typeof(DatabaseToolkitEntities);

        public void AddXscfDatabaseModule(IServiceCollection services)
        {
            services.AddScoped<SetConfig>();

            //AutoMap映射
            base.AddAutoMapMapping(profile =>
            {
                profile.CreateMap<SetConfig.SetConfig_Parameters, SetConfig>();
            });
        }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            //throw new NotImplementedException();
        }
    }
}
