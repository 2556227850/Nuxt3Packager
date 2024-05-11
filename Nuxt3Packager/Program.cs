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
        // 创建yarn --version指令
        Cmd yarnPackage = new Cmd("检测yarn环境程序") { Arguments = "/c yarn --version"  };
        // 执行yarn --version 验证是否安装yarn
        bool isYarnPackage = yarnPackage.Start();

        // yarn包是否安装
        if (!isYarnPackage)
        {
            // 未安装执行安装程序
            Console.WriteLine("检测yarn环境程序:检测到yarn包未安装,正在执行安装程序...");

            // 创建npm i yarn -g安装yarn指令
            Cmd installationYarn = new Cmd("安装yarn程序") { Arguments = "/c npm i yarn -g" };
            // 执行npm i yarn -g指令
            bool isInstallationYarn = installationYarn.Start();
            if (isInstallationYarn) {
                Console.WriteLine("安装yarn程序:yarn包安装成功.");
            }
            else
            {
                Console.WriteLine("安装yarn程序:yarn包安装失败.程序终止.");
                return;
            }
        }
        
            // 创建yarn安装包指令
        Cmd installationPackage = new Cmd("安装项目包程序") { Arguments = "/c yarn" };
        // 创建检测程序
        Detection detection = new Detection();
        // 检测项目是否安装项目包
       bool isModulesFolderPath = detection.DetectionPath(installationPackage.ModulesFolderPath);
        // 是否存在node_modules目录
        if (!isModulesFolderPath) {
             // 不存在目录下载包
            bool isInstallationPackage = installationPackage.Start();
            if (isInstallationPackage)
            {
                Console.WriteLine("安装项目包程序:项目包安装成功.");
            }
            else
            {
                Console.WriteLine("安装项目包程序:项目包安装失败.程序终止.");
                return;
            }
        }
        else
        {
            Console.WriteLine("检测程序:检测到 node_modules 文件夹已存在,无需安装再次,如果打包环节报错请务必删除node_modules文件夹重新执行该程序。");
        }
        // 检测项目是否存在打包文件夹
        bool isOutputFolderPath = detection.DetectionPath(installationPackage.OutputFolderPath);

            if(isOutputFolderPath) {
                Directory.Delete(installationPackage.OutputFolderPath, true);
                Console.WriteLine("检测程序:检测到.output文件夹已存在,已删除现有 .output 文件夹=>" + installationPackage.OutputFolderPath);
            }
            Cmd runBuild = new Cmd("打包程序") { Arguments = "/c yarn build" };

            bool isRunBuild = runBuild.Start();

            if(isRunBuild) {
                Zip zip = new Zip("压缩程序");
                bool isZip = zip.CreateZip(installationPackage.OutputFolderPath, installationPackage.ZipFilePath);
                if (isZip)
                {
                    Console.WriteLine("<=====任务成功=====>");
                }
                else
                {
                    Console.WriteLine("<=====任务失败=====>");
                }
        }

        Console.WriteLine("按任意键退出程序");

        Console.ReadLine();
    }

}