using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuxt3Packager
{
    internal class Cmd
    {

        string FileName { get; set; } = "cmd.exe"; // 执行的命令程序
        string WorkingDirectory { get; set; } = @"D:\code\dome"; // 设置工作目录路径
        public string? Arguments { get; set; } // 执行命令
        public string ModulesFolderPath { get; set; } = @"D:\code\dome\node_modules"; // 包目录
        public string OutputFolderPath { get; set; } = @"D:\code\dome\.output"; // 打包文件目录
        public string ZipFilePath { get; set; } = @"D:\code\dome\.output.zip"; // 打包文件目录

        public Cmd(string message)
        {
            Console.WriteLine($"指令程序:{message}初始化...");
        }
        public Cmd()
        {
            Console.WriteLine($"指令程序:初始化...");
        }
        public bool Start () {
                Console.WriteLine($"指令程序:创建进程...");
            try
            {
                // 设置进程信息
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = FileName, // 执行 cmd.exe
                    Arguments = Arguments, // 执行 yarn --version 命令
                    WorkingDirectory = WorkingDirectory, // 设置工作目录
                    RedirectStandardOutput = true, // 重定向标准输出
                    RedirectStandardError = true, // 重定向标准错误输出
                    UseShellExecute = false, // 不使用操作系统 shell
                    CreateNoWindow = true, // 不创建新窗口
                };

                // 创建一个新的进程
                Process process = new Process();
                process.StartInfo = startInfo;

                process.OutputDataReceived += Process_OutputDataReceived; // 添加输出数据接收事件处理程序
                process.ErrorDataReceived += Process_ErrorDataReceived; // 添加错误数据接收事件处理程序
                                                                        // 启动进程
                Console.WriteLine($"指令程序:开始执行=>执行指令为 {Arguments}");
                process.Start();

                process.BeginOutputReadLine(); // 异步读取输出流
                process.BeginErrorReadLine(); // 异步读取错误流

                // 等待命令执行完成
                process.WaitForExit();
                // 进程执行完毕
                // 检查命令的退出代码
                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"指令程序:执行成功");
                    return true;
                }
                else
                {
                    Console.WriteLine($"指令程序:执行失败");
                    return false;
                }

            }
            catch (Exception ex)
            {
                // 捕获异常
                Console.WriteLine("指令程序:执行出错=>: " + ex.Message);
                return false;
            }
        }
        // 输出数据接收事件处理程序
        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine("命令执行日志: " + e.Data);
            }
        }
        // 错误数据接收事件处理程序
        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine("命令执行日志: " + e.Data);
            }
        }
    }
}
