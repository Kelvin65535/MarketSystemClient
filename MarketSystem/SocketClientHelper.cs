using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

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

        public event EventHandler<ServerMessageArrivedEventArgs> MessageArrived;
        public string Name { get; set; }
        public Timer pollTimer;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="ipAddress">服务端的IP地址字符串</param>
        /// <param name="port">服务端端口</param>
        public SocketClientHelper(string ipAddress, int port)
        {
            this.ipAddressString = ipAddress;
            this.port = port;
            pollTimer = new Timer(100);
            pollTimer.Elapsed += new ElapsedEventHandler(CheckServerReturnMessage);
        }

        /// <summary>
        /// 无参构造方法，从全局变量中获取IP地址和服务器端口
        /// </summary>
        public SocketClientHelper()
        {
            this.ipAddressString = (string) Application.Current.Properties["ipAddress"];
            this.port = (int) Application.Current.Properties["port"];
            pollTimer = new Timer(100);
            pollTimer.Elapsed += new ElapsedEventHandler(CheckServerReturnMessage);
        }

        /// <summary>
        /// IP地址获取器
        /// </summary>
        public string IPAddressString
        {
            get => ipAddressString;
        }

        public int Port
        {
            get => port;
        }

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ip = IPAddress.Parse(ipAddressString);
            }
            catch (Exception)
            {

                MessageBox.Show("IP地址格式错误，请检查输入", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            //开始连接
            try
            {
                socket.Connect(ip, port);
                ns = new NetworkStream(socket);
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                pollTimer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接服务器发生错误：\n" + ex.Message);
                MessageBox.Show("连接服务器发生错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
        }

        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            string receive = "";
            //发送数据
            try
            {
                sw.WriteLine(data);
                sw.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接服务器发生错误：\n" + ex.Message);
            }

        }
        
        public void Disconnect()
        {
            pollTimer.Stop();
            pollTimer.Dispose();
            sw.Close();
            sr.Close();
            socket.Close();
            
        }

        private void CheckServerReturnMessage(object source, EventArgs e)
        {
            Console.WriteLine("正在向服务端接受信息...");
            string ret = "";
            while (sr != null && ret == "")
            {
                ret = sr.ReadLine();
            };
            MessageArrived(this, new ServerMessageArrivedEventArgs(ret));
        }
    }
}
