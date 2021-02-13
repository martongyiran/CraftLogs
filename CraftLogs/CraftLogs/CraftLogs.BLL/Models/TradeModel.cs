using System;
using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Models
{
    public class TradeModel
    {
        public string Target { get; set; }

        public string TradeId { get; set; }

        public int Money { get; set; }

        public ObservableCollection<Item> TradeItems = new ObservableCollection<Item>();

        public TradeModel()
        {
        }

        public string GetRecepie()
        {
            return $"{Target}{TradeId}{Money}";
        }

        public void SetTradeId()
        {
            TradeId = Guid.NewGuid().ToString().Substring(0, 5);
        }
    }
}
