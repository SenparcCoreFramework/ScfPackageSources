using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Scf.XscfBase.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Senparc.Xscf.DatabaseToolkit
{
    /// <summary>
    /// 设计时 DbContext 创建（仅在开发时创建 Code-First 的数据库 Migration 使用，在生产环境不会执行）
    /// </summary>
    public class SenparcDbContextFactory : SenparcDesignTimeDbContextFactoryBase<DatabaseToolkitEntities, Register>
    {
        /// <summary>
        /// 用于寻找 App_Data 文件夹，从而找到数据库连接字符串配置信息
        /// </summary>
        public override string RootDictionaryPath
        {
            get
            {


                Console.WriteLine("dir:" + AppContext.BaseDirectory);
                var finalPath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"/*项目根目录*/);
                Console.WriteLine("finalPath:" + finalPath);
                return finalPath;
            }
        }
    }
}
