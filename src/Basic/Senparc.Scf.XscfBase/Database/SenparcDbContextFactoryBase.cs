using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Scf.Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Senparc.Scf.XscfBase.Database
{
    /// <summary>
    /// 提供给数据库 Migration 使用的 DesignTimeDbContextFactory
    /// </summary>
    /// <typeparam name="TSenparcEntities"></typeparam>
    public abstract class SenparcDbContextFactoryBase<TSenparcEntities> : IDesignTimeDbContextFactory<TSenparcEntities>
         where TSenparcEntities : DbContext
    {
        public virtual string RootDictionaryPath => Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\");

        public virtual SenparcSetting SenparcSetting => new SenparcSetting();

        public virtual string SqlConnectionStr => SenparcDatabaseConfigs.ClientConnectionString ?? "Server=.\\;Database=SCF;Trusted_Connection=True;integrated security=True;";

        public abstract string AssemblyName { get; }

        public abstract TSenparcEntities GetInstance(DbContextOptions<TSenparcEntities> dbContextOptions);

        public TSenparcEntities CreateDbContext(string[] args)
        {
            //修复 https://github.com/SenparcCoreFramework/SCF/issues/13 发现的问题（在非Web环境下无法得到网站根目录路径）
            IRegisterService register = RegisterService.Start(SenparcSetting);
            CO2NET.Config.RootDictionaryPath = RootDictionaryPath;

            var builder = new DbContextOptionsBuilder<TSenparcEntities>();

            builder.UseSqlServer(SqlConnectionStr, b => b.MigrationsAssembly(AssemblyName));

            return GetInstance(builder.Options);
        }
    }
}
