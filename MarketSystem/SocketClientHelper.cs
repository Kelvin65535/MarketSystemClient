using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem
{
    /// <summary>
    /// Socket客户端助手类
    /// </summary>
    class SocketClientHelper
    {
        private string ipAddressString; //服务器ip地址字符串
        private IPAddress ip = null;    //服务器ip地址
        private int port = 0;       //服务器端口
        private Socket socket;      //socket对象

        public NetworkStream ns;    //网络流
        public StreamReader sr;     //流读取
        public StreamWriter sw;     //流写入

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="ipAddress">服务端的IP地址字符串</param>
        /// <param name="port">服务端端口</param>
        public SocketClientHelper(string ipAddress, int port)
        {
            this.ipAddressString = ipAddress;
            this.port = port;
        }

        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Send(string data)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ip = IPAddress.Parse(ipAddressString);
            //开始连接
            try
            {
                socket.Connect(ip, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接服务器发生错误：\n" + ex.Message);
            }
            string receive = "";
            //发送数据
            try
            {
                ns = new NetworkStream(socket);
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                sw.WriteLine(data);
                sw.Flush();
                do
                {
                    receive = sr.ReadLine();
                } while (receive != "");
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接服务器发生错误：\n" + ex.Message);
                
            }

            return receive;
        }
        
    }
}
