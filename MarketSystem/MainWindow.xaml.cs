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

        /// <summary>
        /// 存放搁置订单的订单列表
        /// </summary>
        private ObservableCollection<ObservableCollection<ShopItem>> 搁置订单列表;

        public MainWindow()
        {
            InitializeComponent();
            InitData();
            InitListViewDataBindings();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            label搁置订单.Content = 0;
            搁置订单列表 = new ObservableCollection<ObservableCollection<ShopItem>>();
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
                if (num == "") //输入为空，不执行操作
                {
                    return;
                }
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
        
        /// <summary>
        /// 检测输入到商品编号中指定的热键并执行功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb商品编号_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Enter)
            {
                return;
            }

            switch (e.Key)
            {
                //当检测到s键输入时将当前订单搁置
                case Key.S:
                    新增搁置订单();
                    tb商品编号.Text = null;
                    e.Handled = true;
                    break;
                //当检测到r键输入时将当前搁置订单还原
                case Key.R:
                    还原搁置订单();
                    tb商品编号.Text = null;
                    e.Handled = true;
                    break;
                //当检测到“向下”按钮输入时跳转到商品列表进行操作
                case Key.Down:
                    tb商品编号.Text = "";
                    if (listviewShopItem.Items.Count == 0)
                    {
                        break;
                    }
                    listviewShopItem.SelectedIndex = 0;
                    Keyboard.Focus(listviewShopItem);
                    break;
                //c键输入，清除商品列表
                case Key.C:
                    tb商品编号.Text = null;
                    itemList.Clear();
                    break;
                //退格按钮
                case Key.Back:
                    break;
                //其他按钮无视操作
                default:
                    tb商品编号.Text = "";
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// 新增搁置订单到搁置列表中，并清除当前内容
        /// </summary>
        private void 新增搁置订单()
        {
            if (listviewShopItem.Items.Count == 0)
            {
                return;
            }
            int 搁置订单数 = 搁置订单列表.Count;
            //var temp = new ObservableCollection<ShopItem>(itemList.ToArray());
            搁置订单列表.Add(new ObservableCollection<ShopItem>(itemList.ToArray()));
            搁置订单数++;
            label搁置订单.Content = 搁置订单数;
            itemList.Clear();
        }

        /// <summary>
        /// 还原搁置订单到搁置列表中
        /// </summary>
        private void 还原搁置订单()
        {
            if (搁置订单列表.Count == 0)
            {
                return;
            }
            int 搁置订单数 = 搁置订单列表.Count;
            itemList.Clear();
            ObservableCollection<ShopItem> temp = 搁置订单列表.Last();
            foreach (var item in temp)
            {
                itemList.Add(item);
            }
            搁置订单列表.RemoveAt(搁置订单数 - 1);
            搁置订单数--;
            label搁置订单.Content = 搁置订单数;
        }
        

        private void listviewShopItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //当检测到按下数字键时自动切换回输入编号窗口
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                Keyboard.Focus(tb商品编号);
            }

            if (e.Key == Key.Left)
            {

            }
        }

        private void ListViewItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    MessageBox.Show("test");
                    break;
                default:
                    break;
            }
        }
    }
}
