using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.XscfBase;
using Senparc.Xscf.DatabaseToolkit.Functions;
using Senparc.Xscf.DatabaseToolkit.Models;
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
            services.AddScoped<SetConfig.SetConfig_Parameters>();

            //AutoMap映射
            base.AddAutoMapMapping(profile =>
            {
                profile.CreateMap<SetConfig.SetConfig_Parameters, SetConfig>();
                profile.CreateMap<SetConfig.SetConfig_Parameters, DbConfig>();
            });
        }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DbConfig_WeixinUserConfigurationMapping());
        }
    }
}
