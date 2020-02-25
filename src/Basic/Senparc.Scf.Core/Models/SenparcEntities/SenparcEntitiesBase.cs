using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Senparc.Scf.Core.Models
{
    /// <summary>
    /// SenparcEntities 的基类
    /// </summary>
    public abstract class SenparcEntitiesBase : DbContext, ISenparcEntities
    {
        private static readonly bool[] _migrated = { true };

        public SenparcEntitiesBase(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// 执行 EF Core 的合并操作（等价于 update-database）
        /// <para>出于安全考虑，每次执行 Migrate() 方法之前，必须先执行 ResetMigrate() 开启允许 Migrate 执行的状态。</para>
        /// </summary>
        public virtual void Migrate()
        {
            if (!_migrated[0])
            {
                lock (_migrated)
                {
                    if (!_migrated[0])
                    {
                        Database.Migrate(); // apply all migrations
                        _migrated[0] = true;
                    }
                }
            }
        }

        /// <summary>
        /// 重置合并状态
        /// </summary>
        public virtual void ResetMigrate()
        {
            _migrated[0] = false;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 不可修改系统表

            modelBuilder.ApplyConfiguration(new XscfModuleAccountConfigurationMapping());

            #endregion
            var types = modelBuilder.Model.GetEntityTypes().Where(e => typeof(EntityBase).IsAssignableFrom(e.ClrType));
            foreach (var entityType in types)
            {
                SetGlobalQueryMethodInfo
                        .MakeGenericMethod(entityType.ClrType)
                        .Invoke(this, new object[] { modelBuilder });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly MethodInfo SetGlobalQueryMethodInfo = typeof(SenparcEntitiesBase)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        /// <summary>
        /// 全局查询，附带软删除状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        public virtual void SetGlobalQuery<T>(ModelBuilder builder) where T : EntityBase
        {
            builder.Entity<T>().HasQueryFilter(z => !z.Flag);
        }
    }
}
