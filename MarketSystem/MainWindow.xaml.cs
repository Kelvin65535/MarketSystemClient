using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            InputMethod.Current.ImeState = InputMethodState.Off;
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
            //将合计价格显示为x.xx形式
            label合计价格.Content = 合计价格.ToString("C");
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
                    focusItemFromIndex(listviewShopItem.SelectedIndex);
                    //ListViewItem item = listviewShopItem.ItemContainerGenerator.ContainerFromIndex(listviewShopItem.SelectedIndex) as ListViewItem;
                    //item.Focus();
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
            更新合计价格();
        }

        /// <summary>
        /// 从搁置列表中还原最后一次的搁置订单
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
            更新合计价格();
        }
        
        /// <summary>
        /// 监听listview的按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listviewShopItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //当检测到按下数字键时自动切换回输入编号窗口
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                Keyboard.Focus(tb商品编号);
            }
            
            switch (e.Key)
            {
                //当按下方向键左时减少当前选定的商品数量
                case Key.Left:
                    商品数量改变("sub");
                    break;
                //当按下方向键右时增加当前选定的商品数量
                case Key.Right:
                    商品数量改变("add");
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// 通过设定“增加”或“减少”模式手动改变商品列表内当前选定的商品数量
        /// </summary>
        /// <param name="mode">当mode="add"时增加当前选定商品数量，mode="sub"减少商品数量</param>
        private void 商品数量改变(string mode)
        {
            ShopItem item = (ShopItem)listviewShopItem.SelectedItem;
            if (item == null)
            {
                return;
            }

            if (mode == "add")
            {
                item.ItemCount++;
            }
            else if (mode == "sub")
            {
                int index = listviewShopItem.SelectedIndex;
                if (item.ItemCount == 1)
                {
                    itemList.Remove(item);
                } else
                {
                    item.ItemCount--;
                }
                focusItemFromIndex(index);
            }
            else
            {
                // Do nothing.
            }

            //更新数量信息后更新价格标签显示
            更新合计价格();
        }

        /// <summary>
        /// 当商品列表的选定项发生变化时执行以下方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listviewShopItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listviewShopItem.SelectedIndex;
            //若当前未选中任何项则设置选中的项
            if (listviewShopItem.Items.Count == 0)
            {
                //当列表为空时跳转回编号输入框
                Keyboard.Focus(tb商品编号);
            } 
        }

        /// <summary>
        /// 根据指定的index，将焦点移至listview在该索引处的item
        /// </summary>
        /// <param name="index"></param>
        private void focusItemFromIndex(int index)
        {
            if (index >= 0)
            {
                ListViewItem item = listviewShopItem.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
                if (item == null)
                {
                    if (listviewShopItem.Items.Count == 0)
                    {
                        //当列表为空时跳转回编号输入框
                        Keyboard.Focus(tb商品编号);
                        return;
                    } else
                    {
                        //选中最后一个元素
                        item = listviewShopItem.ItemContainerGenerator.ContainerFromIndex(listviewShopItem.Items.Count - 1) as ListViewItem;
                    }
                }
                item.Focus();
            }
        }
    }
}
