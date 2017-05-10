﻿using MarketSystem.Properties;
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
            Application.Current.Properties["session"] = null; //保存用户的session

            // 启动登录窗口
            LoginWindow wd = new LoginWindow();
            wd.Show();
        }
    }
}