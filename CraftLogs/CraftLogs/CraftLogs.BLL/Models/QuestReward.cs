using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Models
{
    public class QuestReward
    {
        public string From { get; set; }
        public int Score { get; set; }
        public int Money { get; set; }
        public int Honor { get; set; }
        public ObservableCollection<Item> Items = new ObservableCollection<Item>();

        public QuestReward(string from, int score, int money, int honor, ObservableCollection<Item> items)
        {
            From = from;
            Score = score;
            Money = money;
            Honor = honor;
            Items = items;
        }
    }
}
