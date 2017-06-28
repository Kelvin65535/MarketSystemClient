using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

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

        /// <summary>
        /// Socket查询助手类
        /// </summary>
        SocketClientHelper helper;

        bool canAddItem = false;
        ShopItem tempShopItem = null;

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
            label合计价格.Content = (0.00).ToString("C");
            //关闭ime输入法
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
                if (num.Length > 9)
                {
                    tb商品编号.Text = "";
                    return;
                }
                //判断商品是否存在list中，若存在则数量+1
                foreach (var item in itemList)
                {
                    if (item.ItemNum == num)
                    {
                        item.ItemCount++;
                        //从后台将显示焦点移动到刚修改的元素
                        listviewShopItem.SelectedItem = item;
                        focusItemInBackground(item);
                        更新合计价格();
                        //清除输入
                        tb商品编号.Text = null;
                        return;
                    }
                }
                //使用ajax获取对应商品num的详细信息，随后为itemList添加新的商品item
                ajaxGetItemInfo(Convert.ToInt32(num));

                int count = 30; //设定发起ajax请求多少个0.1s内判断是否正确接收服务端返回数据
                for (int i = 0; i < count; i++)
                {
                    if (canAddItem)
                    {
                        itemList.Add(tempShopItem);

                        //调用委托，从后台将显示焦点移动到最后一个元素
                        focusItemInBackground((ShopItem)listviewShopItem.Items[listviewShopItem.Items.Count - 1]);
                        更新合计价格();
                        //清除输入
                        tb商品编号.Text = null;
                        //清除临时变量
                        tempShopItem = null;
                        canAddItem = false;
                        return;
                    }

                    Thread.Sleep(100);
                }

                MessageBox.Show("查询商品信息失败，请重试", "错误", MessageBoxButton.OK, MessageBoxImage.Error);

                //清除输入
                tb商品编号.Text = null;
                //清除临时变量
                tempShopItem = null;
                canAddItem = false;
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
        /// 监听商品编号输入框的按键事件，检测输入到商品编号中指定的热键并执行功能
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
                    break;
                //c键输入，清除商品列表
                case Key.C:
                    tb商品编号.Text = null;
                    itemList.Clear();
                    break;
                //退格按钮
                case Key.Back:
                    break;
                //空格按钮，确认订单
                case Key.Space:
                    confirmOrder();
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
                //当按下方向键左时减少当前选定的商品数量
                case Key.Left:
                    商品数量改变("sub");
                    break;
                //当按下方向键右时增加当前选定的商品数量
                case Key.Right:
                    商品数量改变("add");
                    break;
                //空格按钮，确认订单
                case Key.Space:
                    confirmOrder();
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
            listviewShopItem.ScrollIntoView(listviewShopItem.Items.IndexOf(index));
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

        /// <summary>
        /// 新建线程将当前焦点移动到listview与指定的item相匹配的item
        /// </summary>
        /// <param name="item"></param>
        private void focusItemInBackground(ShopItem item)
        {
            FocusSelectedItemDelegate focusDelegate = new FocusSelectedItemDelegate(focusSelectedItemInBackground);
            listviewShopItem.Dispatcher.BeginInvoke(focusDelegate, DispatcherPriority.Background, item);
        }
        delegate void FocusSelectedItemDelegate(ShopItem item);
        private void focusSelectedItemInBackground(ShopItem item)
        {
            listviewShopItem.SelectedItem = item;
            listviewShopItem.ScrollIntoView(item);
        }

        /// <summary>
        /// 使用异步方式发起商品详细信息查询
        /// </summary>
        /// <param name="itemNum"></param>
        private void ajaxGetItemInfo(int itemNum)
        {
            //构造json查询字符串
            int id = (int)Application.Current.Properties["id"];//查询ID
            int userid = (int)Application.Current.Properties["userid"];//用户ID
            JObject obj = new JObject();
            obj.Add("id", id);
            obj.Add("query", itemNum); //TODO 将商品ID放到查询字符串中
            obj.Add("quest", "queryByID");
            obj.Add("userid", userid);
            string json = JsonConvert.SerializeObject(obj);//序列化

            //发送数据
            helper = new SocketClientHelper();
            //设置消息响应的回调事件
            helper.MessageArrived += DisplayServerReturnMessage;
            helper.Connect();
            helper.Send(json);
        }

        /// <summary>
        /// 用于处理服务端返回的json字符串并分析返回结果
        /// TODO 未经测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayServerReturnMessage(object sender, ServerMessageArrivedEventArgs e)
        {
            string ret = e.Message;
            if (ret.Contains("null"))
            {
                return;
            }
            var obj = JsonConvert.DeserializeObject<ResultFromServer>(ret);

            var result = obj.result;
            
            helper.Disconnect();

            //分析返回结果
            if (obj.status == "OK")
            {
                tempShopItem = new ShopItem(result.itemNum, result.itemName, 1, result.itemOriginalPrice, result.itemSellPrice);
                canAddItem = true;
                return;
            }
            else
            {
                MessageBox.Show("服务器连接错误，清稍后再试", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        private void confirmOrder()
        {
            WindowConfirmOrder window = new WindowConfirmOrder();
            window.DataContext = itemList.ToArray();
            window.ShowDialog();
        }
    }
}
