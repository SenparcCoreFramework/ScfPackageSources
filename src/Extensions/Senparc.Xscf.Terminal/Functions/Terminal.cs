using Senparc.CO2NET.Extensions;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Senparc.Xscf.Terminal.Functions
{
    public class Terminal_Parameters : IFunctionParameter
    {
        [MaxLength(300)]
        [Description("命令行，如：dir /?")]
        public string CommandLine { get; set; }
    }

    public class Terminal : FunctionBase
    {
        //注意：Name 必须在单个 Xscf 模块中唯一！
        public override string Name => "命令提示符";

        public override string Description => "输入Windows命令提示符中的命令,即可返回相应的结果。请注意：命令将在服务器系统中执行！";

        public override Type FunctionParameterType => typeof(Terminal_Parameters);

        public Terminal(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override FunctionResult Run(IFunctionParameter param)
        {
            var typeParam = param as Terminal_Parameters;

            FunctionResult result = new FunctionResult()
            {
                Success = true
            };

            StringBuilder sb = new StringBuilder();
            base.RecordLog(sb, "开始运行 Terminal");

            //TODO:需要限制一下执行的命令

            string strExecRes = string.Empty;
            if (!string.IsNullOrEmpty(typeParam.CommandLine))
            {
                strExecRes = ExeCommand($"{typeParam.CommandLine}");
            }
            else
            {
                strExecRes = ExeCommand($"dir");
            }
            sb.AppendLine(strExecRes);
            result.Log = sb.ToString();
            result.Message = "操作成功！";

            if (!string.IsNullOrEmpty(strExecRes))
            {
                result.Message += Environment.CommandLine + strExecRes;
            }

            return result;
        }

        /// <summary>
        /// 执行cmd.exe命令
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <returns>命令输出文本</returns>
        private string ExeCommand(string commandText)
        {
            return ExeCommand(new string[] { commandText });
        }
        /// <summary>
        /// 执行多条cmd.exe命令
        /// </summary>
        /// <param name="commandTexts">命令文本数组</param>
        /// <returns>命令输出文本</returns>
        private string ExeCommand(string[] commandTexts)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strOutput = null;
            try
            {
                p.Start();
                foreach (string item in commandTexts)
                {
                    p.StandardInput.WriteLine(item);
                }
                p.StandardInput.WriteLine("exit");
                strOutput = p.StandardOutput.ReadToEnd();
                //strOutput = Encoding.UTF8.GetString(Encoding.Default.GetBytes(strOutput));
                p.WaitForExit();
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }
    }
}
