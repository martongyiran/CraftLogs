using System;
using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Models
{
    public class TradeModel
    {
        public string Target { get; set; }

        public string TradeId { get; set; }

        public string From { get; set; }

        public int Money { get; set; }

        public ObservableCollection<Item> TradeItems = new ObservableCollection<Item>();

        public TradeModel()
        {
        }

        public string GetRecepie()
        {
            return $"{From}{Target}{TradeId}{Money}";
        }

        public void SetTradeId()
        {
            TradeId = Guid.NewGuid().ToString().Substring(0, 5);
        }

        public string ToStringGetItems()
        {
            string outp = $"Cseréltél a {From} csapattal. Kaptál {Money}$-t";

            if (TradeItems.Count > 0)
            {
                outp += ", és kaptál:\n";
            }
            else
            {
                outp += ".";
            }

            foreach (var item in TradeItems)
            {
                outp += item.ToString() + "\n\n";
            }

            return outp;
        }

        public string ToStringGiveItems()
        {
            string outp = $"Cseréltél a {Target} csapattal. Adtál {Money}$-t";

            if (TradeItems.Count > 0)
            {
                outp += ", és adtál:\n";
            }
            else
            {
                outp += ".";
            }

            foreach (var item in TradeItems)
            {
                outp += item.ToString() + "\n\n";
            }

            return outp;
        }
    }
}
