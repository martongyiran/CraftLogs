using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using System.Collections.Generic;

namespace CraftLogs.BLL.Services
{
    public class LoggerService : ILoggerService
    {
        #region Ctor

        private readonly ILocalDataRepository dataRepository;
        public LoggerService(ILocalDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        #endregion

        #region Public

        public void CreateArenaLog(string enemyTeam)
        {
            var logs = dataRepository.GetLogs();
            string logText = enemyTeam;
            Log newLog = new Log(LogTypeEnum.Arena, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateBuyLog(int value, List<Item> items)
        {
            var logs = dataRepository.GetLogs();
            string logText = value +"\n";
            foreach(var item in items)
            {
                logText += item.ToString() + "\n";
            }
            Log newLog = new Log(LogTypeEnum.Buy, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateQueustLog(int questId, QuestReward reward)
        {
            var logs = dataRepository.GetLogs();
            string logText = questId + "\n" + reward.ToString();
            Log newLog = new Log(LogTypeEnum.Quest, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateSellLog(Item item)
        {
            var logs = dataRepository.GetLogs();
            string logText = item.ToString();
            Log newLog = new Log(LogTypeEnum.Sell, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateSystemLog(string logText)
        {
            var logs = dataRepository.GetLogs();
            string newLogText = logText;
            Log newLog = new Log(LogTypeEnum.System, newLogText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateTradeLog(string teamName)
        {
            var logs = dataRepository.GetLogs();
            string logText = teamName;
            Log newLog = new Log(LogTypeEnum.Trade, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        #endregion
    }
}
