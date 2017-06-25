using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem
{
    class OrderData
    {
        public ShopItem[] itemList;
        public string Total { get; set; }
        public string Accept { get; set; }
        public string Refund { get; set; }
        public string date { get; set; }
    }
}
