﻿using System.Windows;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;
using System.Threading;

namespace MarketSystem.Properties
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {

        SocketClientHelper helper;

        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 记录当前登陆状态是否验证成功，若登陆成功，将此项设为true
        /// </summary>
        bool canLogin = false;
        

        /// <summary>
        /// “登录”按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // 获取输入的用户名及密码
            string username = textboxUsername.Text;
            string password = textboxPassword.Password;

            // 获取ip地址及端口
            string ip = tbIPAddress.Text;
            string port = tbPort.Text;
            
            // 验证权限
            checkAuth(ref username, ref password, ref ip, ref port);

            // 查看验证结果
            int count = 2;//倒计时
            for (int i = 0; i < count; i++)
            {
                // 查询验证结果是否返回成功
                if (canLogin)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                    return;
                }

                Thread.Sleep(1000);//暂停1s
            }
            return;
        }

        /// <summary>
        /// 登录权限验证
        /// </summary>
        /// <param name="username_input"></param>
        /// <param name="password_input"></param>
        /// <returns></returns>
        private void checkAuth(ref string username_input, ref string password_input, ref string ip, ref string port)
        {
            // TODO 增加验证处理部分

            //构造json查询字符串
            int id = (int)Application.Current.Properties["id"];
            JObject obj = new JObject();
            JObject queryObj = new JObject();
            queryObj.Add("password", password_input);
            queryObj.Add("username", username_input);
            obj.Add("id", id);
            obj.Add("query", queryObj);
            obj.Add("quest", "login");
            obj.Add("userid", "");
            string json = JsonConvert.SerializeObject(obj);

            //发送数据
            //TODO 增加ip地址的修改功能
            int portnum = 0;
            try
            {
                portnum = Convert.ToInt32(port);
            }
            catch (Exception)
            {
                MessageBox.Show("端口格式错误，请检查输入", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            helper = new SocketClientHelper(ip, portnum);
            helper.MessageArrived += DisplayServerReturnMessage;
            helper.Connect();
            helper.Send(json);
        }

        /// <summary>
        /// 用于处理服务端返回的json字符串并分析返回结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayServerReturnMessage(object sender, ServerMessageArrivedEventArgs e)
        {
            string ret = e.Message;
            var obj = JsonConvert.DeserializeAnonymousType(ret, new { id = 0, status = String.Empty , result = String.Empty, userid = 0});
            if (obj == null)
            {
                MessageBox.Show("权限验证失败，请检查输入是否正确", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Console.WriteLine(string.Format("id:{0} status:{1} result={2} userid={3}", obj.id, obj.status, obj.result, obj.userid));
            helper.Disconnect();

            //分析返回结果
            if (obj.result == "YES" && obj.status == "OK")
            {
                Console.WriteLine("权限验证成功");
                //全局存储userid键值
                Application.Current.Properties["userid"] = obj.userid; //保存用户用于查询的userid
                //全局存储服务器IP地址和端口
                Application.Current.Properties["ipAddress"] = helper.IPAddressString; //保存IP地址
                Application.Current.Properties["port"] = helper.Port; //保存端口

                //验证成功，导航到MainWindow
                canLogin = true;
                helper.Disconnect();
                return;
            }
            else
            {
                MessageBox.Show("权限验证失败，请检查输入是否正确", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                helper.Disconnect();
                return;
            }
        }
       
    }
}
