using Microsoft.EntityFrameworkCore;
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
        /// 全局唯一的前缀，务必避免和其他模块重复
        /// </summary>
        string UniquePrefix { get; }
        /// <summary>
        /// 创建数据库模型
        /// </summary>
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}
