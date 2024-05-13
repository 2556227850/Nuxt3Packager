using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuxt3Packager
{
    internal class Zip
    {
        public string? Message { get; set; } // 提示

        // 打包完项目创建压缩包
        public bool CreateZip(string sourceDirectory, string zipFilePath)
        {

            Console.WriteLine($"{Message}:初始化...");

            try
            {
                Console.WriteLine($"{Message}:准备中...");
                // 检查源文件夹是否存在
                if (!Directory.Exists(sourceDirectory))
                {
                    Console.WriteLine($"{Message}:源文件夹不存在。压缩路文件夹为{sourceDirectory}，请检查！");
                    return false;
                }
                // 如果压缩包存在，先删除
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                    Console.WriteLine($"{Message}:已删除现有压缩包=>{zipFilePath}" );
                }

                // 创建一个压缩包
                ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath);

                Console.WriteLine($"{Message}:压缩包已创建=>{zipFilePath}");

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Message}:创建压缩包时出错：=>{ex.Message}");
                return false;
            }
        }
    }
}
