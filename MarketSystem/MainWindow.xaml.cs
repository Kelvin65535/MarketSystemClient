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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarketSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        //局部变量
        /// <summary>
        /// ListView数据源，存放商品item的集合
        /// </summary>
        private ObservableCollection<ShopItem> itemList;

        public MainWindow()
        {
            InitializeComponent();
            InitListViewDataBindings();
        }

        /// <summary>
        /// 为ListView绑定数据源
        /// </summary>
        private void InitListViewDataBindings()
        {
            //为ListView绑定数据源
            itemList = new ObservableCollection<ShopItem>();
            listviewShopItem.ItemsSource = itemList;
            
        }

        private void tb商品编号_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //获取输入的商品编号
                string num = tb商品编号.Text;
                //判断商品是否存在list中，若存在则数量+1
                foreach (var item in itemList)
                {
                    if (item.ItemNum == num)
                    {
                        item.ItemCount++;
                        更新合计价格();
                        //清除输入
                        tb商品编号.Text = null;
                        return;
                    }
                }
                //为itemList添加新的商品item
                //TODO 使用ajax获取商品信息
                itemList.Add(new ShopItem(num, "测试商品", 1, 1.00, 5.00));
                更新合计价格();
                //清除输入
                tb商品编号.Text = null;
            }
        }

        /// <summary>
        /// 主窗口加载完成后执行以下方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //将键盘焦点聚焦到tb商品编号文本框
            Keyboard.Focus(tb商品编号);
        }

        /// <summary>
        /// 更新主界面的“合计价格”标签
        /// </summary>
        private void 更新合计价格()
        {
            double 合计价格 = 0.00;
            foreach (var item in itemList)
            {
                合计价格 += (item.ItemCount * item.ItemSellPrice);
            }
            label合计价格.Content = 合计价格.ToString();
        }
    }
}
