using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem
{
    class ShopItem : INotifyPropertyChanged
    {
        //商品编码
        private string itemNum;
        //商品名称
        private string itemName;
        //商品数量
        private int itemCount;
        //商品原价
        private double itemOriginalPrice;
        //商品折后价
        private double itemSellPrice;

        public event PropertyChangedEventHandler PropertyChanged;

        //封装字段
        public string ItemNum {
            get => itemNum;
            set {
                itemNum = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemNum"));
            }
        }
        public string ItemName { get => itemName; set => itemName = value; }
        public int ItemCount {
            get => itemCount;
            set {
                itemCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ItemCount"));
            }
        }
        public double ItemOriginalPrice { get => itemOriginalPrice; set => itemOriginalPrice = value; }
        public double ItemSellPrice { get => itemSellPrice; set => itemSellPrice = value; }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        //构造方法
        public ShopItem(string itemNum, string itemName, int itemCount, double itemOriginalPrice, double itemSellPrice)
        {
            this.itemNum = itemNum;
            this.itemName = itemName;
            this.itemCount = itemCount;
            this.itemOriginalPrice = itemOriginalPrice;
            this.itemSellPrice = itemSellPrice;
        }
    }
}
