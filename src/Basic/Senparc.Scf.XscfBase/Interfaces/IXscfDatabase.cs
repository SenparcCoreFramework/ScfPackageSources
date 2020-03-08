using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    /// <summary>
    /// XSCF 模块数据库配置
    /// </summary>
    public interface IXscfDatabase
    {
        /// <summary>
        /// 扩展 SenparcEntities 的程序集名称
        /// </summary>
        string SenparcEntitiesAssemblyName { get; }
        /// <summary>
        /// 全局唯一的前缀，务必避免和其他模块重复
        /// </summary>
        string DatabaseUniquePrefix { get; }
        /// <summary>
        /// 创建数据库模型
        /// </summary>
        void OnModelCreating(ModelBuilder modelBuilder);
        /// <summary>
        /// 设置数据库，主要提供给使用
        /// </summary>
        /// <param name="dbContextOptionsAction"></param>
        void DbContextOptionsAction(IRelationalDbContextOptionsBuilderInfrastructure dbContextOptionsAction);
    }
}
