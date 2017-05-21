using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem
{
    /// <summary>
    /// Socket客户端助手类
    /// </summary>
    class SocketClientHelper
    {
        private IPAddress ip = null;
        private int port = 0;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="ipAddress">服务端的IP地址</param>
        /// <param name="port">服务端端口</param>
        public SocketClientHelper(IPAddress ipAddress, int port)
        {
            this.ip = ipAddress;
            this.port = port;
        }

        public 
    }
}
