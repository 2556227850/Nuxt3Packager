using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuxt3Packager
{
    internal class Detection
    {
        public string? Message { get; set; } // 提示

        public bool DetectionPath(string path) {
            Console.WriteLine($"{Message}:初始化...");
            // 如果 node_modules 文件夹存在
            if (Directory.Exists(path))
            {
                Console.WriteLine($"{Message}:检测到文件或文件夹路径为{path}");
                return true;
            }

            Console.WriteLine($"{Message}:未检测到{path}文件或文件夹");
            return false;
        }
    }
}
