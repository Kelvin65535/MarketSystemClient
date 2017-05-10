using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MarketSystem.Properties
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// “登录”按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // 获取输入的用户名及密码
            string username = textboxUsername.Text;
            string password = textboxPassword.Text;

            // 验证权限
            bool result = checkAuth(ref username, ref password);

            if (result)
            {
                //验证成功，导航到MainWindow
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        /// <summary>
        /// 登录权限验证
        /// </summary>
        /// <param name="username_input"></param>
        /// <param name="password_input"></param>
        /// <returns></returns>
        private bool checkAuth(ref string username_input, ref string password_input)
        {
            // TODO 增加验证处理部分
            return true;
        }
    }
}
