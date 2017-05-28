using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem
{
    /// <summary>
    /// 用于包含服务端返回信息的类
    /// </summary>
    class ServerMessageArrivedEventArgs : EventArgs
    {
        /// <summary>
        /// 服务端返回信息
        /// </summary>
        private string message;
        public string Message
        {
            get { return message; }
        }

        public ServerMessageArrivedEventArgs()
        {
            message = "No Message.";
        }

        public ServerMessageArrivedEventArgs(string newMessage)
        {
            message = newMessage;
        }
    }
    
}
