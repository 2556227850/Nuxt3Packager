using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

class Program
{
    static void Main()
    {
        Console.WriteLine("<=====创建Nuxt3项目打包任务=====>");
        Console.WriteLine("<=====开始任务=====>");
      
       bool isYarnPackage= YarnPackage();
        // yarn包是否安装成功
        if (isYarnPackage)
        {
            bool isInstallation = InstallationPackage();
        // 项目包是否安装成功
            if (isInstallation)
            {
                bool isBuild = RunBuild();
        // 项目是否打包成功
                if (isBuild)
                {

                // 创建压缩包文件
                    CreateZip();
                    Console.WriteLine("<=====任务成功=====>");
                }
                else
                {
                    Console.WriteLine("<=====任务失败=====>");
                }
            }
        }


        Console.WriteLine("按任意键退出程序");

        Console.ReadLine();
    }
    // 全局安装yarn
    static public bool InstallationYarn()
    {
        Console.WriteLine("安装yarn:检测是否安装yarn");

        string workingDirectory = @".\"; // 设置工作目录路径
        string arguments = "/c npm i yarn -g"; // 执行命令
        string fileName = "cmd.exe"; // 执行的命令程序

        try
        {
            // 创建一个新的进程
            Process process = new Process();

            // 设置进程信息
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName; // 执行 cmd.exe
            startInfo.Arguments = arguments; // 执行 npm i yarn -g 命令
            startInfo.WorkingDirectory = workingDirectory; // 设置工作目录
            startInfo.RedirectStandardOutput = true; // 重定向标准输出
            startInfo.RedirectStandardError = true; // 重定向标准错误输出
            startInfo.UseShellExecute = false; // 不使用操作系统 shell
            startInfo.CreateNoWindow = true; // 不创建新窗口

            process.StartInfo = startInfo;

            Console.WriteLine($"安装yarn:开始下载安装yarn,执行指令为 {arguments}");

            process.OutputDataReceived += Process_OutputDataReceived; // 添加输出数据接收事件处理程序
            process.ErrorDataReceived += Process_ErrorDataReceived; // 添加错误数据接收事件处理程序
            // 启动进程
            process.Start();

            process.BeginOutputReadLine(); // 异步读取输出流
            process.BeginErrorReadLine(); // 异步读取错误流

            // 等待命令执行完成
            process.WaitForExit();
            // 进程执行完毕

            // 检查命令的退出代码
            if (process.ExitCode == 0)
            {
                Console.WriteLine($"安装yarn:安装yarn成功");
                return true;
            }
            else
            {
                Console.WriteLine($"安装yarn:安装yarn失败");
                return false;
            }

        }
        catch (Exception ex)
        {
            // 捕获异常
            Console.WriteLine("安装下载工具:安装下载出错=>: " + ex.Message);
            return false;
        }
    }
    // 检测yarn是否安装
    static public bool YarnPackage()
    {
        Console.WriteLine("检测必备环境:检测yarn工具是否安装...");

        string workingDirectory = @".\"; // 设置工作目录路径
        string arguments = "/c yarn --version"; // 执行命令
        string fileName = "cmd.exe"; // 执行的命令程序

        try
        {
            // 创建一个新的进程
            Process process = new Process();

            // 设置进程信息
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName; // 执行 cmd.exe
            startInfo.Arguments = arguments; // 执行 yarn --version 命令
            startInfo.WorkingDirectory = workingDirectory; // 设置工作目录
            startInfo.RedirectStandardOutput = true; // 重定向标准输出
            startInfo.RedirectStandardError = true; // 重定向标准错误输出
            startInfo.UseShellExecute = false; // 不使用操作系统 shell
            startInfo.CreateNoWindow = true; // 不创建新窗口

            process.StartInfo = startInfo;

            Console.WriteLine($"检测必备环境:验证yarn是否安装,执行指令为 {arguments}");

            process.OutputDataReceived += Process_OutputDataReceived; // 添加输出数据接收事件处理程序
            process.ErrorDataReceived += Process_ErrorDataReceived; // 添加错误数据接收事件处理程序
            // 启动进程
            process.Start();

            process.BeginOutputReadLine(); // 异步读取输出流
            process.BeginErrorReadLine(); // 异步读取错误流

            // 等待命令执行完成
            process.WaitForExit();
            // 进程执行完毕
            // 检查命令的退出代码
            if (process.ExitCode == 0)
            {
                Console.WriteLine($"检测必备环境:yarn已经安装");
                return true;
            }
            else
            {
                Console.WriteLine($"检测必备环境:未检测到yarn,正在执行安装yarn程序");

                bool isInstallationYarn = InstallationYarn();
                if (isInstallationYarn)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        catch (Exception ex)
        {
            // 捕获异常
            Console.WriteLine("安装下载工具:安装下载出错=>: " + ex.Message);
            return false;
        }
    }
    // 用yarn安装项目包
    static public bool InstallationPackage()
    {
        Console.WriteLine("下载项目包:准备下载安装打包必备环境...");

        string folderPath = @".\node_modules"; // 指下载包文件夹路径
        string workingDirectory = @".\"; // 设置工作目录路径
        string arguments = "/c yarn"; // 执行命令
        string fileName = "cmd.exe"; // 执行的命令程序

        // 如果 node_modules 文件夹存在
        if (Directory.Exists(folderPath))
        {
            Console.WriteLine("下载项目包:检测到 node_modules 文件夹已存在,无需安装再次,如果打包环节报错请务必删除node_modules文件夹重新执行该程序。");

            return true;
        }

        try
        {
            // 创建一个新的进程
            Process process = new Process();

            // 设置进程信息
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName; // 执行 cmd.exe
            startInfo.Arguments = arguments; // 执行 npm i 命令
            startInfo.WorkingDirectory = workingDirectory; // 设置工作目录
            startInfo.RedirectStandardOutput = true; // 重定向标准输出
            startInfo.RedirectStandardError = true; // 重定向标准错误输出
            startInfo.UseShellExecute = false; // 不使用操作系统 shell
            startInfo.CreateNoWindow = true; // 不创建新窗口

            process.StartInfo = startInfo;

            Console.WriteLine($"下载项目包:开始下载,执行指令为 {arguments},下载时间较久请等待...");

            process.OutputDataReceived += Process_OutputDataReceived; // 添加输出数据接收事件处理程序
            process.ErrorDataReceived += Process_ErrorDataReceived; // 添加错误数据接收事件处理程序
            // 启动进程
            process.Start();

            process.BeginOutputReadLine(); // 异步读取输出流
            process.BeginErrorReadLine(); // 异步读取错误流

            // 等待命令执行完成
            process.WaitForExit();
            // 进程执行完毕
            // 检查命令的退出代码
            if (process.ExitCode == 0)
            {
                Console.WriteLine($"下载项目包:下载程序执行完毕,下载成功 下载包文件夹为{folderPath}");

                return true;
            }
            else
            {
                Console.WriteLine($"下载项目包:下载程序执行失败。");

                return false;
            }

        }
        catch (Exception ex)
        {
            // 捕获异常
            Console.WriteLine("下载项目包:下载出错=>: " + ex.Message);
            return false;
        }
    }
    // 用yarn打包项目
    static public bool RunBuild()
    {
        Console.WriteLine("项目打包:打包程序准备中...");

        string folderPath = @".\.output"; // 指定要打包的文件夹路径
        string workingDirectory = @".\"; // 设置工作目录路径
        string arguments = "/c yarn build"; // 执行命令
        string fileName = "cmd.exe"; // 执行的命令程序

        // 如果 .output 文件夹存在，先删除
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
            Console.WriteLine("打包:检测到.output文件夹已存在,已删除现有 .output 文件夹=>" + folderPath);
        }

        try
        {
            // 创建一个新的进程
            Process process = new Process();

            // 设置进程信息
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName; // 执行 cmd.exe
            startInfo.Arguments = arguments; // 执行 npm run build 命令
            startInfo.WorkingDirectory = workingDirectory; // 设置工作目录
            startInfo.RedirectStandardOutput = true; // 重定向标准输出
            startInfo.RedirectStandardError = true; // 重定向标准错误输出
            startInfo.UseShellExecute = false; // 不使用操作系统 shell
            startInfo.CreateNoWindow = true; // 不创建新窗口

            process.StartInfo = startInfo;

            Console.WriteLine($"项目打包:开始执行打包程序,执行指令为 {arguments},打包时间较久请等待...");

            process.OutputDataReceived += Process_OutputDataReceived; // 添加输出数据接收事件处理程序
            process.ErrorDataReceived += Process_ErrorDataReceived; // 添加错误数据接收事件处理程序
            // 启动进程
            process.Start();

            process.BeginOutputReadLine(); // 异步读取输出流
            process.BeginErrorReadLine(); // 异步读取错误流

            // 等待命令执行完成
            process.WaitForExit();
            // 进程执行完毕
            // 检查命令的退出代码
            if (process.ExitCode == 0)
            {
                Console.WriteLine($"项目打包:打包程序执行完毕,打包成功 路径为{folderPath}");
                return true;
            }
            else
            {
                Console.WriteLine($"项目打包:打包程序执行失败。");

                return false;
            }

        }
        catch (Exception ex)
        {
            // 捕获异常
            Console.WriteLine("项目打包:打包出错=>: " + ex.Message);
            return false;
        }
    }
    // 打包完项目创建压缩包
    static public void CreateZip()
    {
        Console.WriteLine("压缩打包文件夹:压缩程序准备中...");

        string sourceDirectory = @".\.output"; // 指定要打包的文件夹路径
        string zipFilePath = @".\.output.zip"; // 指定压缩包的保存路径

        try
        {
            // 检查源文件夹是否存在
            if (!Directory.Exists(sourceDirectory))
            {
                Console.WriteLine($"压缩打包文件夹:源文件夹不存在。压缩路文件夹为{sourceDirectory}，请检查！");
                return;
            }
            // 如果压缩包存在，先删除
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
                Console.WriteLine("压缩打包文件夹:已删除现有压缩包=>" + zipFilePath);
            }

            // 创建一个压缩包
            ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath);

            Console.WriteLine("压缩打包文件夹:压缩包已创建=>" + zipFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("压缩打包文件夹:创建压缩包时出错：=>" + ex.Message);
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