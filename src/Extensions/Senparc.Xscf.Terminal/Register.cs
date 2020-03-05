using Senparc.Scf.Core.Enums;
using Senparc.Scf.XscfBase;
using Senparc.Xscf.Terminal.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Xscf.Terminal
{
    [XscfRegister]
    public class Register : XscfRegisterBase, IXscfRegister
    {
        public Register()
        { }

        #region IRegister 接口

        public override string Name => "Senparc.Xscf.Terminal";
        public override string Uid => "600C608A-F99A-4B1B-A18E-8CE69BE8DA92";//必须确保全局唯一，生成后必须固定
        public override string Version => "0.1.5";//必须填写版本号

        public override string MenuName => "终端模块";
        public override string Icon => "fa fa-terminal";
        public override string Description => $"此模块提供给开发者一个可以直接使用终端命令控制系统的模块！" +
                                      $"目前可以使用的命令如下:" +
                                      $"=================================" +
                                      $"'CD',             //Displays the name of or changes the current directory." +
                                      $"'CHDIR',          //Displays the name of or changes the current directory." +
                                      $"'CHKDSK',         //Checks a disk and displays a status report." +
                                      $"'CLS',            //Clears the screen." +
                                      $"'CMD',            //Starts a new instance of the Windows command interpreter." +
                                      $"'COLOR',          //Sets the default console foreground and background colors." +
                                      $"'COMP',           //Compares the contents of two files or sets of files." +
                                      $"'COPY',           //Copies one or more files to another location." +
                                      $"'DATE',           //Displays or sets the date." +
                                      $"'DIR',            //Displays a list of files and subdirectories in a directory." +
                                      $"'DISKPART',       //Displays or configures Disk Partition properties." +
                                      $"'ECHO',           //Displays messages, or turns command echoing on or off." +
                                      $"'EXIT',           //Quits the CMD.EXE program (command interpreter)." +
                                      $"'FC',             //Compares two files or sets of files, and displays the differences between them." +
                                      $"'FIND',           //Searches for a text string in a file or files." +
                                      $"'FINDSTR',        //Searches for strings in files." +
                                      $"'GPRESULT',       //Displays Group Policy information for machine or user." +
                                      $"'GRAFTABL',       //Enables Windows to display an extended character set in graphics mode." +
                                      $"'HELP',           //Provides Help information for Windows commands." +
                                      $"'ICACLS',         //Display, modify, backup, or restore ACLs for files and directories." +
                                      $"'MD',             //Creates a directory." +
                                      $"'MKDIR',          //Creates a directory." +
                                      $"'MKLINK',         //Creates Symbolic Links and Hard Links" +
                                      $"'MORE',           //Displays output one screen at a time." +
                                      $"'MOVE',           //Moves one or more files from one directory to another directory." +
                                      $"'PAUSE',          //Suspends processing of a batch file and displays a message." +
                                      $"'PRINT',          //Prints a text file." +
                                      $"'PROMPT',         //Changes the Windows command prompt." +
                                      $"'RECOVER',        //Recovers readable information from a bad or defective disk." +
                                      $"'REM',            //Records comments(remarks) in batch files or CONFIG.SYS." +
                                      $"'REN',            //Renames a file or files." +
                                      $"'RENAME',         //Renames a file or files." +
                                      $"'REPLACE',        //Replaces files." +
                                      $"'ROBOCOPY',       //Advanced utility to copy files and directory trees" +
                                      $"'START',          //Starts a separate window to run a specified program or command." +
                                      $"'SYSTEMINFO',     //Displays machine specific properties and configuration." +
                                      $"'TASKLIST',       //Displays all currently running tasks including services." +
                                      $"'TIME',           //Displays or sets the system time." +
                                      $"'TITLE',          //Sets the window title for a CMD.EXE session." +
                                      $"'TREE',           //Graphically displays the directory structure of a drive or path." +
                                      $"'TYPE',           //Displays the contents of a text file." +
                                      $"'VER',            //Displays the Windows version." +
                                      $"'VOL',            //Displays a disk volume label and serial number." +
                                      $"'XCOPY',          //Copies files and directory trees." +
                                      $"'WMIC'            //Displays WMI information inside interactive command shell.";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public override IList<Type> Functions => new[] { 
            typeof(Functions.Terminal),
        };

        public override Task InstallOrUpdateAsync(InstallOrUpdate installOrUpdate)
        {
            return Task.CompletedTask;
        }

        public override async Task UninstallAsync(Func<Task> unsinstallFunc)
        {
            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion
    }
}
