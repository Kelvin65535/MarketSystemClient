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
    /// PageSuspendOrder.xaml 的交互逻辑
    /// </summary>
    public partial class PageSuspendOrder : Page
    {

        //局部变量
        private ObservableCollection<ObservableCollection<ShopItem>> 搁置订单列表;

        public PageSuspendOrder()
        {
            InitializeComponent();
        }


    }
}
