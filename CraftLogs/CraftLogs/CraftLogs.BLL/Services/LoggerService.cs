/*
Copyright 2018 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;

namespace CraftLogs.BLL.Services
{
    public class LoggerService : ILoggerService
    {

        private readonly ILocalDataRepository _dataRepository;

        public LoggerService(ILocalDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void CreateArenaLog(ArenaResponse enemyTeam)
        {
            var logs = _dataRepository.GetLogs();
            var logText = enemyTeam.GetResoult();
            var newLog = new Log(LogTypeEnum.Arena, logText);
            logs.Insert(0, newLog);
            _dataRepository.SaveToFile(logs);
        }

        public void CreateBuyLog(ShopResponse shopList)
        {
            var logs = _dataRepository.GetLogs();
            var logText = shopList.ToString();
            var newLog = new Log(LogTypeEnum.Buy, logText);
            logs.Insert(0, newLog);
            _dataRepository.SaveToFile(logs);
        }

        public void CreateQueustLog(QuestReward reward)
        {
            var logs = _dataRepository.GetLogs();
            var logText = reward.ToString();
            var newLog = new Log(LogTypeEnum.Quest, logText);
            logs.Insert(0, newLog);
            _dataRepository.SaveToFile(logs);
        }

        public void CreateSellLog(Item item)
        {
            var logs = _dataRepository.GetLogs();
            var logText = "+" + item.Value + " $\n\n" + item.ToString();
            var newLog = new Log(LogTypeEnum.Sell, logText);
            logs.Insert(0, newLog);
            _dataRepository.SaveToFile(logs);
        }

        public void CreateSystemLog(HqReward logText)
        {
            var logs = _dataRepository.GetLogs();
            var newLogText = logText.ToString();
            var newLog = new Log(LogTypeEnum.System, newLogText);
            logs.Insert(0, newLog);
            _dataRepository.SaveToFile(logs);
        }

        public void CreateTradeLog(TeamProfile profile)
        {
            var logs = _dataRepository.GetLogs();
            var logText = "Velük: " + profile.TradeWith +"\n\n Adtál: " + profile.TradeOut.Money + " pénzt\n\n";
            foreach(var item in profile.TradeOut.ItemsToTrade)
            {
                logText += item.ToString();
            }
            logText += "\n\nKaptál:"  + profile.TradeIn.Money + " pénzt\n\n";
            foreach (var item in profile.TradeIn.ItemsToTrade)
            {
                logText += item.ToString();
            }
            var newLog = new Log(LogTypeEnum.Trade, logText);
            logs.Insert(0, newLog);
            _dataRepository.SaveToFile(logs);
        }
    }
}
