using Nuxt3Packager;
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
        // 创建指令
        Cmd YarnPackage = new Cmd { Message = "检测yarn环境程序", Arguments = "/c yarn --version" , };
        Cmd InstallationYarn = new Cmd() { Message = "安装yarn程序", Arguments = "/c npm i yarn -g" };
        Cmd InstallationPackage = new Cmd() { Message = "安装项目包程序", Arguments = "/c yarn" };
        Cmd RunBuild = new Cmd() { Message = "打包程序", Arguments = "/c yarn build" };

        // 创建压缩包
        Zip Zip = new Zip() { Message = "压缩程序", };

        // 检测程序
        Detection Detection = new Detection() { Message = "检测程序", };

        // 验证是否安装yarn
        bool isYarnPackage = YarnPackage.Start();
        // 未安装yarn执行安装程序
        if (!isYarnPackage)
        {
            Console.WriteLine($"{YarnPackage.Message}:检测到yarn包未安装,准备执行安装程序...");
            // 创建npm i yarn -g安装yarn指令
            bool isInstallationYarn = InstallationYarn.Start();
            // 未安装成功
            if (!isInstallationYarn)
            {
                Console.WriteLine($"{InstallationYarn.Message}:yarn包安装失败.程序终止.");
                Console.WriteLine("<=====任务失败=====>");
                Console.WriteLine("按任意键退出程序");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"{InstallationYarn.Message}:yarn包安装成功,准备执行安装项目包程序...");
        }
        else
        {
            Console.WriteLine($"{YarnPackage.Message}:检测到yarn包已安装，准备执行安装项目包程序...");
        }
        // 检测项目是否安装项目包
        bool isModulesFolderPath = Detection.DetectionPath(InstallationPackage.ModulesFolderPath);
        // 如果不存在node_modules目录
        if (!isModulesFolderPath) {
             // 不存在目录开始下载包
            bool isInstallationPackage = InstallationPackage.Start();
            // 未下载成功
            if (!isInstallationPackage)
            {
                Console.WriteLine($"{InstallationPackage.Message}:项目包安装失败.程序终止.");
                Console.WriteLine("<=====任务失败=====>");
                Console.WriteLine("按任意键退出程序");
                Console.ReadLine();
                return;
            }
            Console.WriteLine($"{InstallationPackage.Message}:项目包安装成功,准备执行打包程序...");
        }
        else
        {
            Console.WriteLine($"{Detection.Message}:检测到node_modules文件夹已存在,无需再次安装,如果打包环节报错请务必删除node_modules文件夹重新执行该程序。");
        }
        // 检测项目是否存在打包文件夹
        bool isOutputFolderPath = Detection.DetectionPath(InstallationPackage.OutputFolderPath);
            // 如果存在打包文件夹
            if(isOutputFolderPath) {
            // 则先删除原打包文件夹
                Directory.Delete(InstallationPackage.OutputFolderPath, true);
                Console.WriteLine($"{Detection.Message}:检测到{InstallationPackage.OutputFolderPath}文件夹已存在,已删除现有{InstallationPackage.OutputFolderPath}文件夹");
            }
            else
            {
                Console.WriteLine($"{Detection.Message}:检测到{InstallationPackage.OutputFolderPath}文件夹已存在,准备执行打包程序...");
            }
        // 执行打包程序
        bool isRunBuild = RunBuild.Start();
            // 如果打包没成功
            if(!isRunBuild) {
                Console.WriteLine($"{RunBuild.Message}:项目打包失败,程序终止！");
                Console.WriteLine("<=====任务失败=====>");
                Console.WriteLine("按任意键退出程序");
                Console.ReadLine();
            return;
        }
        // 执行压缩程序

        bool isZip = Zip.CreateZip(InstallationPackage.OutputFolderPath, InstallationPackage.ZipFilePath);
        // 如果压缩没成功
            if (!isZip)
            {
                Console.WriteLine($"{Zip.Message}:项目压缩失败,程序终止！");
                Console.WriteLine("<=====任务失败=====>");
                Console.WriteLine("按任意键退出程序");
                Console.ReadLine();
            return;
        }

        Console.WriteLine("<=====任务成功=====>");
        Console.WriteLine("按任意键退出程序");
        Console.ReadLine();
    }

}