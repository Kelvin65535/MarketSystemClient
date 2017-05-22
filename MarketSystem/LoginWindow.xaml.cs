using System.Windows;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

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
            string ret = "";

            //发送数据
            //TODO 增加ip地址的修改功能
            SocketClientHelper helper = new SocketClientHelper("192.168.1.1", 9527);
            ret = helper.Send(json);

            MessageBox.Show(ret);

            return true;
        }
    }
}
