using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.XscfBase.Threads
{
    /// <summary>
    /// ThreadInfo
    /// </summary>
    public class ThreadInfo
    {
        public ThreadInfo(string name, TimeSpan intervalTime, Func<IApplicationBuilder, Task> task, Func<Exception, Task> exceptionHandler = null)
        {
            Name = name;
            IntervalTime = intervalTime;
            Task = task;
            ExceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// 用于识别 Thread，请确保单个 XSCF 模块中唯一
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 间隔时间
        /// </summary>
        public TimeSpan IntervalTime { get; set; }
        /// <summary>
        /// 执行任务
        /// </summary>
        public Func<IApplicationBuilder, Task> Task { get; set; }
        /// <summary>
        /// 发生异常时的处理
        /// </summary>
        public Func<Exception, Task> ExceptionHandler { get; set; }
    }
}
