using CraftLogs.BLL.Models;
using System.Collections.Generic;

namespace CraftLogs.BLL.Services.Interfaces
{
    public interface ILoggerService
    {
        /// <summary> 
        /// Create a QuestLog type log entry.
        /// </summary>
        void CreateQueustLog(int questId, QuestReward reward);
        /// <summary> 
        /// Create a BuyLog type log entry.
        /// </summary>
        void CreateBuyLog(int value, List<Item> items);
        /// <summary> 
        /// Create a SellLog type log entry.
        /// </summary>
        void CreateSellLog(Item item);
        /// <summary> 
        /// Create a TradeLog type log entry.
        /// </summary>
        void CreateTradeLog(string teamName);
        /// <summary> 
        /// Create a ArenaLog type log entry.
        /// </summary>
        void CreateArenaLog(string enemyTeam);
        /// <summary> 
        /// Create a SystemLog type log entry.
        /// </summary>
        void CreateSystemLog(string logText);
    }
}
