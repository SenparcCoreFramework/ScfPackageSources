using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Exceptions;
using Senparc.Scf.Core.Models;
using Senparc.Scf.XscfBase.Database;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.XscfBase
{
    /// <summary>
    /// 所有 XSCF 模块注册的基类
    /// </summary>
    public abstract class XscfRegisterBase : IXscfRegister
    {
        /// <summary>
        /// 模块名称，要求全局唯一
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// 编号，要求全局唯一
        /// </summary>
        public abstract string Uid { get; }
        /// <summary>
        /// 版本号
        /// </summary>
        public abstract string Version { get; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public abstract string MenuName { get; }
        /// <summary>
        /// Icon图标
        /// </summary>
        public abstract string Icon { get; }
        /// <summary>
        /// 说明
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// 注册方法，注册的顺序决定了界面中排列的顺序
        /// </summary>
        public abstract IList<Type> Functions { get; }
        /// <summary>
        /// 如果提供了 UI 界面，必须指定一个首页
        /// </summary>
        public virtual string HomeUrl => null;

        /// <summary>
        /// 执行 Migrate 更新数据
        /// </summary>
        /// <typeparam name="TSenparcEntities"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        protected virtual async Task MigrateDatabaseAsync<TSenparcEntities>(IServiceProvider serviceProvider)
            where TSenparcEntities : XscfDatabaseDbContext
        {
            var mySenparcEntities = serviceProvider.GetService<TSenparcEntities>();
            await mySenparcEntities.Database.MigrateAsync().ConfigureAwait(false);//更新数据库

            if (!await mySenparcEntities.Database.EnsureCreatedAsync().ConfigureAwait(false))
            {
                throw new ScfModuleException($"更新数据库失败：{typeof(TSenparcEntities).Name}");
            }
        }

        /// <summary>
        /// 安装代码
        /// </summary>
        public virtual Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
            return Task.CompletedTask;
        }
        /// <summary>
        /// 卸载代码
        /// </summary>
        public virtual async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            await unsinstallFunc().ConfigureAwait(false);
        }

        /// <summary>
        /// 获取首页Url
        /// <para>仅限实现了 IAreaRegister 接口之后的 Register，否则将返回 null</para>
        /// </summary>
        /// <returns></returns>
        public virtual string GetAreaHomeUrl()
        {
            if (this is IAreaRegister)
            {
                var homeUrl = (this as IAreaRegister).HomeUrl;
                return GetAreaUrl(homeUrl);
            }
            return null;
        }
        /// <summary>
        /// 获取指定网页的Url
        /// <para>仅限实现了 IAreaRegister 接口之后的 Register，否则将返回 null</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual string GetAreaUrl(string path)
        {
            if (this is IAreaRegister)
            {
                if (path == null)
                {
                    return "/";
                }

                path += path.Contains("?") ? "&" : "?";
                path += $"uid={Uid}";
                return path;
            }
            return null;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual IServiceCollection AddXscfModule(IServiceCollection services)
        {
            if (this is IXscfDatabase databaseRegister)
            {
                //定义 XscfSenparcEntities 实例生成
                Func<IServiceProvider, object> implementationFactory = s =>
                {
                    //DbContextOptionsBuilder
                    var dbOptionBuilderType = typeof(DbContextOptionsBuilder<>);
                    dbOptionBuilderType = dbOptionBuilderType.MakeGenericType(databaseRegister.XscfDatabaseDbContextType);
                    DbContextOptionsBuilder dbOptionBuilder = Activator.CreateInstance(dbOptionBuilderType) as DbContextOptionsBuilder;

                    dbOptionBuilder = SqlServerDbContextOptionsExtensions.UseSqlServer(dbOptionBuilder, Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString, databaseRegister.DbContextOptionsAction);

                    var xscfSenparcEntities = Activator.CreateInstance(databaseRegister.XscfDatabaseDbContextType, new object[] { dbOptionBuilder.Options });
                    return xscfSenparcEntities;
                };
                //添加 XscfSenparcEntities 依赖注入配置
                services.AddScoped(databaseRegister.XscfDatabaseDbContextType, implementationFactory);
                //
                EntitySetKeys.GetEntitySetKeys(databaseRegister.XscfDatabaseDbContextType);//注册当前数据库的对象（必须）

                //添加数据库相关
                databaseRegister.AddXscfDatabaseModule(services);
            }
            return services;
        }

        /// <summary>
        /// 获取 EF Code First MigrationHistory 数据库表名
        /// </summary>
        /// <returns></returns>
        public virtual string GetDatabaseMigrationHistoryTableName()
        {
            if (this is IXscfDatabase databaseRegiser)
            {
                return "__" + databaseRegiser.DatabaseUniquePrefix + "_EFMigrationsHistory";
            }
            return null;
        }

        public virtual void DbContextOptionsAction(IRelationalDbContextOptionsBuilderInfrastructure dbContextOptionsAction)
        {
            if (this is IXscfDatabase databaseRegiser)
            {
                if (dbContextOptionsAction is SqlServerDbContextOptionsBuilder sqlServerOptionsAction)
                {
                    var senparcEntitiesAssemblyName = databaseRegiser.XscfDatabaseDbContextType.Assembly.FullName;
                    var databaseMigrationHistoryTableName = GetDatabaseMigrationHistoryTableName();

                    sqlServerOptionsAction
                        .MigrationsAssembly(senparcEntitiesAssemblyName)
                        .MigrationsHistoryTable(databaseMigrationHistoryTableName);
                }

                //可以支持其他跟他多数据库
            }
        }
    }
}
