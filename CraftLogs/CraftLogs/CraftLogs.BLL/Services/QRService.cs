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
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ZXing.Mobile;

namespace CraftLogs.BLL.Services
{
    public class QRService : IQRService
    {
        public string CreateQR<T>(T data)
        {
            if (typeof(QuestReward) == data.GetType())
            {
                QRResponse<QuestReward> response = new QRResponse<QuestReward>(QRTypeEnum.Reward, JsonConvert.SerializeObject(data));

                return JsonConvert.SerializeObject(response);
            }
            else if (typeof(ShopResponse) == data.GetType())
            {
                QRResponse<ShopResponse> response = new QRResponse<ShopResponse>(QRTypeEnum.ShopList, JsonConvert.SerializeObject(data));

                return JsonConvert.SerializeObject(response);
            }
            else if (typeof(CombatUnit) == data.GetType())
            {
                QRResponse<CombatUnit> response = new QRResponse<CombatUnit>(QRTypeEnum.ProfileForArena, JsonConvert.SerializeObject(data));

                return JsonConvert.SerializeObject(response);
            }
            else if (typeof(ArenaResponse) == data.GetType())
            {
                QRResponse<ArenaResponse> response = new QRResponse<ArenaResponse>(QRTypeEnum.ArenaResult, JsonConvert.SerializeObject(data));

                return JsonConvert.SerializeObject(response);
            }
            else if (typeof(TradeModel) == data.GetType())
            {
                QRResponse<TradeModel> response = new QRResponse<TradeModel>(QRTypeEnum.TradeResult, JsonConvert.SerializeObject(data));

                return JsonConvert.SerializeObject(response);
            }
            else if (typeof(ProfileQr) == data.GetType())
            {
                QRResponse<ProfileQr> response = new QRResponse<ProfileQr>(QRTypeEnum.ProfileForHq, JsonConvert.SerializeObject(data));

                return JsonConvert.SerializeObject(response);
            }
            else if (typeof(HqReward) == data.GetType())
            {
                QRResponse<HqReward> response = new QRResponse<HqReward>(QRTypeEnum.HqReward, JsonConvert.SerializeObject(data));

                return JsonConvert.SerializeObject(response);
            }

            return "";
        }
      
        public QRResponse HandleQR(string scanResult)
        {
            var response = JsonConvert.DeserializeObject<QRResponse>(scanResult);

            return response;
        }

        public async Task<INavigationParameters> ReadQr()
        {
            var scanner = new MobileBarcodeScanner();
            var scanResult = await scanner.Scan(MobileBarcodeScanningOptions.Default);

            if (scanResult != null && scanResult?.Text != null)
            {
                return new NavigationParameters
                {
                    { "res", scanResult.Text }
                };
            }

            return null;
        }
    }
}
