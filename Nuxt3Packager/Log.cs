using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuxt3Packager
{
    internal class Log
    {
        // 输出数据接收事件处理程序
        protected static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine("命令执行日志: " + e.Data);
            }
        }
        // 错误数据接收事件处理程序
        protected static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine("命令执行日志: " + e.Data);
            }
        }
    }
}
