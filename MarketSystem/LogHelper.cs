using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem
{
    /// <summary>
    /// Log助手类
    /// </summary>
    class LogHelper
    {
        /// <summary>
        /// 用于将提供的内容记录到C:\log.txt文件中
        /// </summary>
        /// <param name="content">要记录的文本内容</param>
        public static void WriteLog(ref string content)
        {
            //打开文件
            FileStream fs = new FileStream("C:\\log.txt", FileMode.Create);
            //获得字节流
            byte[] data = System.Text.Encoding.Default.GetBytes(content);
            //开始写入
            fs.Write(data, 0, data.Length);
            //关闭文件
            fs.Flush();
            fs.Close();
        }
    }
}
