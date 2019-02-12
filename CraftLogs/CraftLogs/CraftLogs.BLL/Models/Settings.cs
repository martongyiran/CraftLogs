using CraftLogs.BLL.Enums;

namespace CraftLogs.BLL.Models
{
    public class Settings
    {
        public int CraftDay { get; set; }
        public int Craft1Start { get; set; }
        public int Craft2Start { get; set; }
        public int Craft1MinPont { get; set; }
        public int Craft2MinPont { get; set; }
        public int Craft1QuestCount { get; set; }
        public int Craft2QuestCount { get; set; }
        public AppModeEnum AppMode { get; set; }
    }
}
