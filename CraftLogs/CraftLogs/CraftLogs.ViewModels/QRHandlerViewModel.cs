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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class QRHandlerViewModel : ViewModelBase
    {
        #region Private

        private string response;
        private IQRService qRService;
        private ILoggerService loggerService;

        #endregion

        #region Public

        public string Response
        {
            get { return response; }
            set { SetProperty(ref response, value); }
        }

        #endregion


        #region Ctor

        public QRHandlerViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService, ILoggerService loggerservice)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = "QR Handler Page";
            qRService = qrService;
            loggerService = loggerservice;
        }

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            var lul = parameters["res"] as string;
            Response = lul ?? "none";
            HandleQR(lul);
        }

        #endregion

        #region Functions

        private void HandleQR(string response)
        {
            try
            {
                var data = qRService.HandleQR(response);

                if (data.Type == BLL.Enums.QRTypeEnum.Reward)
                {
                    QuestReward processedData = JsonConvert.DeserializeObject<QuestReward>(data.AdditionalData);
                    //TODO UI
                    var profile = DataRepository.GetTeamProfile();
                    profile.AllExp += 1;
                    profile.Honor += processedData.Honor;
                    profile.Money += processedData.Money;
                    profile.Score += processedData.Score;

                    List<Item> temp = new List<Item>();

                    foreach (var item in processedData?.Items)
                    {
                        profile.Inventory.Add(new Item(item.Tier,item.Rarity,item.ItemType,item.UsableFor,item.StatsFromQR));
                        temp.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR));
                    }

                    processedData.Items = new ObservableCollection<Item>(temp);
                    DataRepository.SaveToFile(profile);
                    loggerService.CreateQueustLog(processedData);
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("------------" + e.ToString());
            }

        }

        #endregion

    }
}
