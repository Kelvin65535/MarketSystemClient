using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MarketSystem
{
    /// <summary>
    /// WindowConfirmOrder.xaml 的交互逻辑
    /// </summary>
    public partial class WindowConfirmOrder : Window
    {
        //局部变量
        private ShopItem[] itemList;
        double 总价;
        double 实收价格;
        double 找零价格;

        /// <summary>
        /// 默认的无参构造方法
        /// </summary>
        public WindowConfirmOrder()
        {
            InitializeComponent();

            //关闭ime输入法
            InputMethod.Current.ImeState = InputMethodState.Off;
        }

        /// <summary>
        /// 窗口加载完成后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = this.DataContext;
            itemList = (ShopItem[])list;
            Keyboard.Focus(tb实收金额);

            统计合计价格();
        }

        /// <summary>
        /// 根据商品列表统计出合计价格
        /// </summary>
        private void 统计合计价格()
        {
            总价 = 0;
            foreach (var item in itemList)
            {
                总价 += (item.ItemCount * item.ItemSellPrice);
            }

            label合计价格.Content = 总价.ToString("C");
        }

        /// <summary>
        /// 输入实收金额的输入框的键盘响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb实收金额_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                return;
            }

            switch (e.Key)
            {
                //当检测到回车输入时确认订单
                case Key.Enter:
                    显示实收价格();
                    打印订单();
                    e.Handled = true;
                    break;
                //输入q键，退出操作
                case Key.Q:
                    Close();
                    e.Handled = true;
                    break;
                //其他按钮无视操作
                default:
                    tb实收金额.Text = "";
                    e.Handled = true;
                    break;
            }
        }

        private void 打印订单()
        {
            //TODO
        }

        /// <summary>
        /// 根据输入的实收价格计算出找零价格，并将其显示到屏幕中
        /// </summary>
        private void 显示实收价格()
        {
            实收价格 = Convert.ToDouble(tb实收金额.Text);
            label实收价格.Content = 实收价格.ToString("C");
            找零价格 = 实收价格 - 总价;
            label退还价格.Content = 找零价格.ToString("C");
        }
    }
}
