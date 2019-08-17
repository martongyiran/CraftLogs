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

using CraftLogs.BLL.Models;
using System.Collections.Generic;

namespace CraftLogs.BLL.Services.Interfaces
{
    public interface ILoggerService
    {
        /// <summary> 
        /// Create a QuestLog type log entry.
        /// </summary>
        void CreateQueustLog(QuestReward reward);

        /// <summary> 
        /// Create a BuyLog type log entry.
        /// </summary>
        void CreateBuyLog(ShopResponse shopList);

        /// <summary> 
        /// Create a SellLog type log entry.
        /// </summary>
        void CreateSellLog(Item item);

        /// <summary> 
        /// Create a TradeLog type log entry.
        /// </summary>
        void CreateTradeLog(TeamProfile profile);

        /// <summary> 
        /// Create a ArenaLog type log entry.
        /// </summary>
        void CreateArenaLog(ArenaResponse enemyTeam);

        /// <summary> 
        /// Create a SystemLog type log entry.
        /// </summary>
        void CreateSystemLog(HqReward logText);
    }
}
