using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Trace;
using Senparc.Scf.Core.Exceptions;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public class Register
    {
        /// <summary>
        /// 模块和方法集合 TODO：可放置到缓存中
        /// </summary>
        public static List<IXscfRegister> RegisterList { get; set; } = new List<IXscfRegister>();

        public static string Scan(IList<CreateOrUpdate_XscfModuleDto> xscfModules,
            XscfModuleService xscfModuleService,
            Action<IXscfRegister> afterInstalled = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{SystemTime.Now}] 开始扫描 XscfModules");
            var scanTypesCount = 0;
            var types = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(a =>
                       {
                           try
                           {
                               scanTypesCount++;
                               var aTypes = a.GetTypes();
                               return aTypes.Where(t =>
                                    !t.IsAbstract &&
                                    (t.GetInterfaces().Contains(typeof(IXscfRegister)) || 
                                    t.GetInterfaces().Contains(typeof(IXscfFunction<>)/* 暂时不收录 */)
                                    ));
                           }
                           catch (Exception ex)
                           {
                               sb.Append($"[{SystemTime.Now}] 自动扫描程序集异常：" + a.FullName);
                               SenparcTrace.SendCustomLog("XscfRegister() 自动扫描程序集异常：" + a.FullName, ex.ToString());
                               return new List<Type>();//不能 return null
                           }
                       });


            if (types != null)
            {
                sb.Append($"[{SystemTime.Now}] 满足条件对象：{types.Count()}");

                //先注册
                foreach (var type in types.Where(z => z != null && z.GetInterfaces().Contains(typeof(IXscfRegister))))
                {
                    sb.Append($"[{SystemTime.Now}] 扫描到 IXscfRegister：{type.FullName}");

                    var register = type.Assembly.CreateInstance(type.FullName) as IXscfRegister;

                    if (!RegisterList.Contains(register))
                    {
                        RegisterList.Add(register);
                    }

                    var xscfModuleStoredDto = xscfModules.FirstOrDefault(z => z.Uid == register.Uid);
                    var xscfModuleAssemblyDto = new UpdateVersion_XscfModuleDto(register.Name, register.Uid, register.MenuName, register.Version, register.Description);

                    //检查更新，并安装到数据库
                    var addedOrUpdated = xscfModuleService.CheckAndUpdateVersion(xscfModuleStoredDto, xscfModuleAssemblyDto);
                    sb.Append($"[{SystemTime.Now}] 是否更新版本：{addedOrUpdated}");

                    if (addedOrUpdated)
                    {
                        //执行安装程序
                        register.Install();

                        afterInstalled?.Invoke(register);
                    }
                }

                /* 暂时不收录 */
                ////再扫描具体方法
                //foreach (var type in types.Where(z => z != null && z.GetInterfaces().Contains(typeof(IXscfFunction))))
                //{
                //    sb.Append($"[{SystemTime.Now}] 扫描到 IXscfFunction：{type.FullName}");

                //    if (!ModuleFunctionCollection.ContainsKey(type))
                //    {
                //        throw new SCFExceptionBase($"{type.FullName} 未能提供正确的注册方法！");
                //    }

                //    var function = type as IXscfFunction;
                //    ModuleFunctionCollection[type].Add(function);
                //}
            }

            sb.Append($"[{SystemTime.Now}] 扫描结束，共扫描 {scanTypesCount} 个程序集");
            return sb.ToString();
        }
    }
}
