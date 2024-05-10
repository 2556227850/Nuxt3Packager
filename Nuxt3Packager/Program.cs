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
        bool isBuild = RunBuild();
        if (isBuild)
        {

            CreateZip();
            Console.WriteLine("<=====任务成功=====>");
        }
        else
        {
            Console.WriteLine("<=====任务失败=====>");
        }
        Console.WriteLine("按任意键退出程序");

        Console.ReadLine();
    }
    static public bool RunBuild()
    {
        Console.WriteLine("打包:打包程序准备中...");

        string folderPath = @".\.output"; // 指定要打包的文件夹路径
        string workingDirectory = @".\"; // 设置工作目录路径
        string arguments = "/c npm run build"; // 执行命令
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

            Console.WriteLine($"打包:开始执行打包程序,执行指令为 {arguments},打包时间较久请等待...");

            process.OutputDataReceived += Process_OutputDataReceived; // 添加输出数据接收事件处理程序
            process.ErrorDataReceived += Process_ErrorDataReceived; // 添加错误数据接收事件处理程序
            // 启动进程
            process.Start();

            process.BeginOutputReadLine(); // 异步读取输出流
            process.BeginErrorReadLine(); // 异步读取错误流

            // 等待命令执行完成
            process.WaitForExit();
            // 进程执行完毕
            Console.WriteLine($"打包:打包程序执行完毕,打包成功 路径为{folderPath}");
            return true;
        }
        catch (Exception ex)
        {
            // 捕获异常
            Console.WriteLine("打包:打包出错=>: " + ex.Message);
            return false;
        }
    }
    static public void CreateZip()
    {
        Console.WriteLine("压缩:压缩程序准备中...");

        string sourceDirectory = @".\.output"; // 指定要打包的文件夹路径
        string zipFilePath = @".\.output.zip"; // 指定压缩包的保存路径

        try
        {
            // 检查源文件夹是否存在
            if (!Directory.Exists(sourceDirectory))
            {
                Console.WriteLine($"压缩:源文件夹不存在。压缩路文件夹为{sourceDirectory}，请检查！");
                return;
            }
            // 如果压缩包存在，先删除
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
                Console.WriteLine("压缩:已删除现有压缩包=>" + zipFilePath);
            }

            // 创建一个压缩包
            ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath);

            Console.WriteLine("压缩:压缩包已创建=>" + zipFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("压缩:创建压缩包时出错：=>" + ex.Message);
        }
    }
    // 输出数据接收事件处理程序
    private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Console.WriteLine("打包输出: " + e.Data);
        }
    }
    // 错误数据接收事件处理程序
    private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Console.WriteLine("打包警告: " + e.Data);
        }
    }
}