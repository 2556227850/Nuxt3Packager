using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuxt3Packager
{
    internal class Detection
    {
        public Detection()
        {
            Console.WriteLine($"检测程序:初始化...");
        }
        public bool DetectionPath(string path) {
            // 如果 node_modules 文件夹存在
            if (Directory.Exists(path))
            {
                Console.WriteLine($"检测程序:检测到文件或目录路径为{path}");
                return true;
            }

            Console.WriteLine($"检测程序:未检测到该文件或目录");
            return false;
        }
    }
}
