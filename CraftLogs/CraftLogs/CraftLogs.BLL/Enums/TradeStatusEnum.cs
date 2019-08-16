/*
Copyright 2019 Gyirán Márton Áron

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

namespace CraftLogs.BLL.Enums
{
    /// <summary>
    /// Status of the trade.
    /// 1a. Give: if you open the Trede menu. Pick what you want to give. 
    /// 1b. GetAndGive: if you read a TradeQR. You can see what the other team want's to give you and you can pick what you want to give in return.
    /// 2. GiveAndGet: When you read the QR after YOU started a trade(1a). You can see what you give and get.
    /// 3. FirstOk: You confirm the trade, get/lose items. You create a QR for the other team.
    /// 4. SecondOk: You confirm it's finished - by reading QR, then creat a new one for the other team. After this status changes to Finished.
    /// 5. Finished: no trade in progress.
    /// </summary>
    public enum TradeStatusEnum
    {
        TradeGive, 
        TradeGetAndGive,
        TradeGiveAndGet,
        TradeFirstOk,
        TradeSecondOk,
        Finished
    }
}
