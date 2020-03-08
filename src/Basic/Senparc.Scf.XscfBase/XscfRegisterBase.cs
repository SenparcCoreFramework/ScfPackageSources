using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.Enums;
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
            //TODO：自动搜索符合条件的DB
            if (this is IXscfDatabase databaseRegiser)
            {
                Func<IServiceProvider, object> implementationFactory = s =>
                {
                    //DbContextOptionsBuilder
                    var dbOptionBuilderType = typeof(DbContextOptionsBuilder<>);
                    dbOptionBuilderType = dbOptionBuilderType.MakeGenericType(databaseRegiser.XscfDatabaseDbContextType);
                    object dbOptionBuilder = Activator.CreateInstance(dbOptionBuilderType);

                    Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = b => databaseRegiser.DbContextOptionsAction(b);


                    var builder = dbOptionBuilderType.InvokeMember("UseSqlServer", BindingFlags.Default | BindingFlags.InvokeMethod,
                                                         null, dbOptionBuilder,
                                                         new object[] {
                                                        Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString,
                                                        sqlServerOptionsAction
                                                         });
                    builder = dbOptionBuilderType.InvokeMember("Options", BindingFlags.Default | BindingFlags.Public, null, dbOptionBuilder, null);

                    var xscfSenparcEntities = Activator.CreateInstance(databaseRegiser.XscfDatabaseDbContextType, new object[] { builder });
                    return xscfSenparcEntities;
                };

                services.AddScoped(databaseRegiser.XscfDatabaseDbContextType, implementationFactory);

                EntitySetKeys.GetEntitySetKeys(databaseRegiser.XscfDatabaseDbContextType);//注册当前数据库的对象（必须）

                databaseRegiser.AddXscfDatabaseModule(services);
            }
            return services;
        }

        public virtual void DbContextOptionsAction(IRelationalDbContextOptionsBuilderInfrastructure dbContextOptionsAction)
        {
            if (this is IXscfDatabase databaseRegiser)
            {
                if (dbContextOptionsAction is SqlServerDbContextOptionsBuilder sqlServerOptionsAction)
                {
                    sqlServerOptionsAction.MigrationsAssembly(databaseRegiser.SenparcEntitiesAssemblyName)
                  .MigrationsHistoryTable("__" + databaseRegiser.DatabaseUniquePrefix + "_EFMigrationsHistory");
                }

                //可以支持其他跟他多数据库
            }
        }
    }
}
