using MarketSystem.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MarketSystem
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 定义全局变量
            Application.Current.Properties["userid"] = null; //保存用户用于查询的userid
            Application.Current.Properties["id"] = 0; //保存用户的查询id
            //全局存储服务器IP地址和端口
            Application.Current.Properties["ipAddress"] = String.Empty; //保存IP地址
            Application.Current.Properties["port"] = 0; //保存端口

            // 启动登录窗口
            LoginWindow wd = new LoginWindow();
            //MainWindow wd = new MainWindow();
            wd.Show();
        }
    }
}
