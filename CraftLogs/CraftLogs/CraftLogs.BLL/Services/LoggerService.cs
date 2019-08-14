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
using System;
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

        public void CreateArenaLog(ArenaResponse enemyTeam)
        {
            var logs = dataRepository.GetLogs();
            string logText = enemyTeam.GetResoult();
            Log newLog = new Log(LogTypeEnum.Arena, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateBuyLog(ShopResponse shopList)
        {
            var logs = dataRepository.GetLogs();
            string logText = shopList.ToString();
            Log newLog = new Log(LogTypeEnum.Buy, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateQueustLog(QuestReward reward)
        {
            var logs = dataRepository.GetLogs();
            string logText = reward.ToString();
            Log newLog = new Log(LogTypeEnum.Quest, logText);
            logs.Insert(0, newLog);
            dataRepository.SaveToFile(logs);
        }

        public void CreateSellLog(Item item)
        {
            var logs = dataRepository.GetLogs();
            string logText = "+" + item.Value + " $\n\n" + item.ToString();
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
