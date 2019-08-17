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
using System.Threading.Tasks;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class QRHandlerViewModel : ViewModelBase
    {
        #region Private

        private IQRService qRService;
        private ILoggerService loggerService;
        private Settings settings;

        #endregion

        #region Properties

        private string response;

        public string Response
        {
            get { return response; }
            set { SetProperty(ref response, value); }
        }

        private bool rewardIsVisible = false;

        public bool RewardIsVisible
        {
            get { return rewardIsVisible; }
            set { SetProperty(ref rewardIsVisible, value); }
        }

        private string rewardText;

        public string RewardText
        {
            get { return rewardText; }
            set { SetProperty(ref rewardText, value); }
        }

        private ObservableCollection<Item> rewards = new ObservableCollection<Item>();

        public ObservableCollection<Item> Rewards
        {
            get { return rewards; }
            set { SetProperty(ref rewards, value); }
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            settings = DataRepository.GetSettings();

            var lul = parameters["res"] as string;
            Response = lul ?? "none";
            if(Response != "none")
            {
                HandleQR(lul);
            }
        }

        #endregion

        #region Functions

        private async Task HandleQR(string rspns)
        {
            try
            {
                var data = qRService.HandleQR(rspns);

                IsBusy = true;

                if (data.Type == QRTypeEnum.Reward)
                {
                    Title = Texts.QuestRewardTitle;
                    QuestReward processedData = JsonConvert.DeserializeObject<QuestReward>(data.AdditionalData);
                    //TODO UI
                    var profile = DataRepository.GetTeamProfile();
                    profile.AllExp += 1;
                    profile.Honor += processedData.Honor;
                    profile.Money += processedData.Money;
                    profile.Score += processedData.Score;

                    RewardText = "+1 EXP \n+" + processedData.Honor + " Honor \n+" + processedData.Money + " pénz";

                    List<Item> temp = new List<Item>();

                    foreach (var item in processedData?.Items)
                    {
                        profile.Inventory.Add(new Item(item.Tier,item.Rarity,item.ItemType,item.UsableFor,item.StatsFromQR, item.Name, item.Image));
                        temp.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Name, item.Image));
                    }

                    processedData.Items = new ObservableCollection<Item>(temp);
                    Rewards = new ObservableCollection<Item>(temp);
                    DataRepository.SaveToFile(profile);
                    loggerService.CreateQueustLog(processedData);
                }
                else if(data.Type == QRTypeEnum.ShopList)
                {
                    Title = Texts.ShopListOkTitle;
                    ShopResponse processedData = JsonConvert.DeserializeObject<ShopResponse>(data.AdditionalData);
                    var profile = DataRepository.GetTeamProfile();
                    if(profile.Money >= processedData.Money)
                    {
                        profile.Money -= processedData.Money;

                        RewardText = "-" + processedData.Money + "$";

                        List<Item> temp = new List<Item>();

                        foreach (var item in processedData?.Items)
                        {
                            profile.Inventory.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Name, item.Image));
                            temp.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Name, item.Image));
                        }

                        processedData.Items = new ObservableCollection<Item>(temp);
                        Rewards = new ObservableCollection<Item>(temp);
                        DataRepository.SaveToFile(profile);
                        loggerService.CreateBuyLog(processedData);
                    }
                    else
                    {
                        Title = Texts.ShopListNotOkTitle;
                        RewardText = Texts.NotEnoughMoney;
                    }
                }
                else if (data.Type == QRTypeEnum.ProfileForArena && settings.AppMode == AppModeEnum.Arena)
                {
                    Title = Texts.ArenaTeamDetails;
                    CombatUnit processedData = JsonConvert.DeserializeObject<CombatUnit>(data.AdditionalData);
                    var profile = DataRepository.GetArenaProfile();

                    profile.Attacker = processedData;
                    DataRepository.SaveToFile(profile);

                    RewardText = Texts.ArenaScanned;

                    await NavigateBack();
                }
                else if (data.Type == QRTypeEnum.ArenaResult)
                {
                    ArenaResponse processedData = JsonConvert.DeserializeObject<ArenaResponse>(data.AdditionalData);
                    var profile = DataRepository.GetTeamProfile();

                    profile.AllExp += 1;
                    profile.Honor += 1;
                    profile.Money += processedData.Money;
                    DataRepository.SaveToFile(profile);

                    if (processedData.IsWin)
                    {
                        Title = Texts.ArenaWin;
                    }
                    else
                    {
                        Title = Texts.ArenaLose;
                    }
                    RewardText = "+1 EXP \n+1 Honor \n+" + processedData.Money + " pénz";
                    loggerService.CreateArenaLog(processedData);
                }
                else if (data.Type == QRTypeEnum.TradeGive)
                {
                    var profile = DataRepository.GetTeamProfile();
                    TradeGive processedData = JsonConvert.DeserializeObject<TradeGive>(data.AdditionalData);

                    Title = Texts.TradePage;

                    if (profile.TradeStatus == TradeStatusEnum.Finished)
                    {
                        profile.TradeStatus = TradeStatusEnum.TradeGetAndGive;
                        profile.TradeNumber = processedData.TradeNumber;
                        profile.TradeIn = processedData.Reward;

                        DataRepository.SaveToFile(profile);
                        RewardText = Texts.TradeHandlerIP;
                        await NavigateToWithoutHistory(NavigationLinks.TradePage);
                    }
                    else
                    {
                        RewardText = Texts.TradeIP;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.TradeIP, Texts.Ok);
                    }
                }
                else if (data.Type == QRTypeEnum.TradeGetAndGive)
                {
                    var profile = DataRepository.GetTeamProfile();
                    TradeGetAndGive processedData = JsonConvert.DeserializeObject<TradeGetAndGive>(data.AdditionalData);

                    Title = Texts.TradePage;

                    if (profile.TradeStatus == TradeStatusEnum.TradeGive && profile.TradeNumber == processedData.TradeNumber)
                    {
                        profile.TradeStatus = TradeStatusEnum.TradeFirstOk;
                        profile.TradeIn = processedData.Reward;
                        
                        DataRepository.SaveToFile(profile);
                        RewardText = Texts.TradeHandlerIP;

                        TradeFirstOk tradeResponse = new TradeFirstOk(profile.TradeNumber);

                        var qrCode = qRService.CreateQR(tradeResponse);
                        NavigationParameters param = new NavigationParameters();
                        param.Add("code", qrCode);

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                    else
                    {
                        RewardText = Texts.TradeIP;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.TradeIP, Texts.Ok);
                    }
                }
                else if (data.Type == QRTypeEnum.TradeFirstOk)
                {
                    var profile = DataRepository.GetTeamProfile();
                    TradeFirstOk processedData = JsonConvert.DeserializeObject<TradeFirstOk>(data.AdditionalData);

                    Title = Texts.TradePage;

                    if (profile.TradeStatus == TradeStatusEnum.TradeGiveAndGet && profile.TradeNumber == processedData.TradeNumber)
                    {
                        profile.TradeStatus = TradeStatusEnum.Finished;

                        profile.Money += profile.TradeIn.Money;

                        foreach(var item in profile.TradeIn.ItemsToTrade)
                        {
                            profile.Inventory.Add(item);
                        }

                        loggerService.CreateTradeLog("asd");
                        DataRepository.SaveToFile(profile);
                        RewardText = Texts.TradeHandlerIP;

                        TradeSecondOk tradeResponse = new TradeSecondOk(profile.TradeNumber);

                        var qrCode = qRService.CreateQR(tradeResponse);
                        NavigationParameters param = new NavigationParameters();
                        param.Add("code", qrCode);

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                    else
                    {
                        RewardText = Texts.TradeIP;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.TradeIP, Texts.Ok);
                    }
                }
                else if (data.Type == QRTypeEnum.TradeSecondOk)
                {
                    var profile = DataRepository.GetTeamProfile();
                    TradeSecondOk processedData = JsonConvert.DeserializeObject<TradeSecondOk>(data.AdditionalData);

                    Title = Texts.TradePage;

                    if (profile.TradeStatus == TradeStatusEnum.TradeFirstOk && profile.TradeNumber == processedData.TradeNumber)
                    {
                        profile.TradeStatus = TradeStatusEnum.Finished;

                        profile.Money += profile.TradeIn.Money;

                        foreach (var item in profile.TradeIn.ItemsToTrade)
                        {
                            profile.Inventory.Add(item);
                        }

                        DataRepository.SaveToFile(profile);
                        RewardText = "Sikeres csere!";

                        loggerService.CreateTradeLog("asd");
                    }
                    else
                    {
                        RewardText = Texts.TradeIP;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.TradeIP, Texts.Ok);
                    }
                }
                else
                {
                    Title = Texts.HandlerErrorTitle;
                    RewardText = Texts.HandlerErrorText;
                }
                RewardIsVisible = true;
                IsBusy = false;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("------------" + e.ToString());
            }

        }

        #endregion

    }
}
