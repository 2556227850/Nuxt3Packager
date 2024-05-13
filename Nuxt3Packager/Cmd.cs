using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuxt3Packager
{
    internal class Cmd :Log
    {

        string FileName { get; set; } = "cmd.exe"; // 执行的命令程序
        string WorkingDirectory { get; set; } = @"D:\code\dome"; // 设置工作目录路径
        public string? Message { get; set; } // 提示
        public string? Arguments { get; set; } // 执行命令
        public string ModulesFolderPath { get; set; } = @".\node_modules"; // 包目录
        public string OutputFolderPath { get; set; } = @".\.output"; // 打包文件目录
        public string ZipFilePath { get; set; } = @".\.output.zip"; // 打包文件目录

        public bool Start () {
            Console.WriteLine($"{Message}:初始化...");
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

                Console.WriteLine($"{Message}:创建进程...");
                // 创建一个新的进程
                Process process = new Process();
                process.StartInfo = startInfo;

                process.OutputDataReceived += Process_OutputDataReceived; // 添加输出数据接收事件处理程序
                process.ErrorDataReceived += Process_ErrorDataReceived; // 添加错误数据接收事件处理程序
                                                                        // 启动进程
                Console.WriteLine($"{Message}:开始执行=>执行指令为 {Arguments}");
                process.Start();

                process.BeginOutputReadLine(); // 异步读取输出流
                process.BeginErrorReadLine(); // 异步读取错误流

                // 等待命令执行完成
                process.WaitForExit();
                // 进程执行完毕
                // 检查命令的退出代码
                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"{Message}:执行成功");
                    return true;
                }
                else
                {
                    Console.WriteLine($"{Message}:执行失败");
                    return false;
                }

            }
            catch (Exception ex)
            {
                // 捕获异常
                Console.WriteLine($"{Message}:执行出错=>: {ex.Message}");
                return false;
            }
        }

    }
}
