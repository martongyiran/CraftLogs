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
        private readonly IQRService _qRService;
        private readonly ILoggerService _loggerService;
        private Settings _settings;
        private string _response;
        private string _rewardText;
        private bool _rewardIsVisible = false;
        private bool _isQuestReward = false;
        private ObservableCollection<Item> _rewards = new ObservableCollection<Item>();

        public string Response
        {
            get => _response;
            set => SetProperty(ref _response, value);
        }

        public bool RewardIsVisible
        {
            get => _rewardIsVisible;
            set => SetProperty(ref _rewardIsVisible, value);
        }

        public string RewardText
        {
            get => _rewardText;
            set => SetProperty(ref _rewardText, value);
        }

        public ObservableCollection<Item> Rewards
        {
            get => _rewards;
            set => SetProperty(ref _rewards, value);
        }

        public DelayCommand NavigateToProfilePageCommand
            => new DelayCommand(async () => await NavigateToWithoutHistory(NavigationLinks.ProfilePage));

        public QRHandlerViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IQRService qrService,
            ILoggerService loggerservice)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = "QR Handler Page";
            _qRService = qrService;
            _loggerService = loggerservice;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _settings = DataRepository.GetSettings();

            var par = parameters["res"] as string;
            Response = par ?? "none";
            if (Response != "none")
            {
                await HandleQR(par);
#if DEV
                System.Diagnostics.Debug.WriteLine(par);
#endif
            }
        }

        private async Task HandleQR(string rspns)
        {
            try
            {
                var data = _qRService.HandleQR(rspns);

                IsBusy = true;

                if (data.Type == QRTypeEnum.Reward)
                {
                    Title = Texts.Handler_QuestRewardTitle;
                    var processedData = JsonConvert.DeserializeObject<QuestReward>(data.D);

                    var profile = DataRepository.GetTeamProfile();

                    profile.AllExp += 1;
                    profile.Honor += processedData.Honor;
                    profile.Money += processedData.Money;
                    profile.Score += processedData.Score;

                    RewardText = "+1 EXP \n+" + processedData.Honor + " hírnév \n+" + processedData.Money + " $";

                    List<Item> temp = new List<Item>();

                    foreach (var item in processedData?.Items)
                    {
                        profile.Inventory.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Ad));
                        temp.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Ad));
                    }

                    processedData.Items = new ObservableCollection<Item>(temp);
                    Rewards = new ObservableCollection<Item>(temp);
                    DataRepository.SaveToFile(profile);
                    _loggerService.CreateQueustLog(processedData);
                }
                else if (data.Type == QRTypeEnum.ShopList)
                {
                    Title = Texts.Handler_ShopListOkTitle;

                    var processedData = JsonConvert.DeserializeObject<ShopResponse>(data.D);
                    var profile = DataRepository.GetTeamProfile();

                    if (profile.Money >= processedData.Money)
                    {
                        profile.Money -= processedData.Money;

                        RewardText = "-" + processedData.Money + "$";

                        List<Item> temp = new List<Item>();

                        foreach (var item in processedData?.Items)
                        {
                            profile.Inventory.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Ad));
                            temp.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Ad));
                        }

                        processedData.Items = new ObservableCollection<Item>(temp);
                        Rewards = new ObservableCollection<Item>(temp);
                        DataRepository.SaveToFile(profile);
                        _loggerService.CreateBuyLog(processedData);
                    }
                    else
                    {
                        Title = Texts.Handler_ShopListNotOk;
                        RewardText = Texts.Handler_NotEnoughMoney;
                    }
                }
                else if (data.Type == QRTypeEnum.ProfileForArena && _settings.AppMode == AppModeEnum.Arena)
                {
                    Title = Texts.Arena_Details;

                    var processedData = JsonConvert.DeserializeObject<CombatUnit>(data.D);
                    var profile = DataRepository.GetArenaProfile();

                    profile.Attacker = processedData;
                    DataRepository.SaveToFile(profile);

                    RewardText = Texts.Arena_Scanned;

                    await NavigateBack();
                }
                else if (data.Type == QRTypeEnum.ArenaResult)
                {
                    var processedData = JsonConvert.DeserializeObject<ArenaResponse>(data.D);
                    var profile = DataRepository.GetTeamProfile();

                    profile.AllExp += 1;
                    profile.Honor += 1;
                    profile.Money += processedData.Money;
                    profile.Score += (int)(processedData.Money / 10);
                    DataRepository.SaveToFile(profile);

                    if (processedData.IsWin)
                    {
                        Title = Texts.Handler_ArenaWin;
                    }
                    else
                    {
                        Title = Texts.Handler_ArenaLose;
                    }
                    RewardText = "+1 EXP \n+1 hírnév \n+" + processedData.Money + " $";
                    _loggerService.CreateArenaLog(processedData);
                }
                else if (data.Type == QRTypeEnum.TradeGive)
                {
                    var profile = DataRepository.GetTeamProfile();
                    var processedData = JsonConvert.DeserializeObject<TradeGive>(data.D);

                    Title = Texts.Trade_Title;

                    if (profile.TradeStatus == TradeStatusEnum.Finished)
                    {
                        profile.TradeStatus = TradeStatusEnum.TradeGetAndGive;
                        profile.TradeNumber = processedData.TradeNumber;
                        profile.TradeIn = processedData.Reward;
                        profile.TradeWith = processedData.Name;

                        DataRepository.SaveToFile(profile);
                        RewardText = Texts.Handler_TradeInProgress;
                        await NavigateToWithoutHistory(NavigationLinks.TradePage);
                    }
                    else
                    {
                        RewardText = Texts.Profile_TradeInProgress;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.Profile_TradeInProgress + "Velük: " + profile.TradeWith, Texts.Ok);
                    }
                }
                else if (data.Type == QRTypeEnum.TradeGetAndGive)
                {
                    var profile = DataRepository.GetTeamProfile();
                    var processedData = JsonConvert.DeserializeObject<TradeGetAndGive>(data.D);

                    Title = Texts.Trade_Title;

                    if (profile.TradeStatus == TradeStatusEnum.TradeGive && profile.TradeNumber == processedData.TradeNumber)
                    {
                        profile.TradeStatus = TradeStatusEnum.TradeFirstOk;
                        profile.TradeIn = processedData.Reward;
                        profile.TradeWith = processedData.Name;

                        RewardText = Texts.Handler_TradeInProgress;

                        var tradeResponse = new TradeFirstOk(profile.TradeNumber);

                        var qrCode = _qRService.CreateQR(tradeResponse);
                        var param = new NavigationParameters
                        {
                            { "code", qrCode }
                        };

                        profile.TradeLastQR = qrCode;

                        DataRepository.SaveToFile(profile);

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                    else
                    {
                        RewardText = Texts.Profile_TradeInProgress;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.Profile_TradeInProgress + "Velük: " + profile.TradeWith, Texts.Ok);
                    }
                }
                else if (data.Type == QRTypeEnum.TradeFirstOk)
                {
                    var profile = DataRepository.GetTeamProfile();
                    var processedData = JsonConvert.DeserializeObject<TradeFirstOk>(data.D);

                    Title = Texts.Trade_Title;

                    if (profile.TradeStatus == TradeStatusEnum.TradeGiveAndGet && profile.TradeNumber == processedData.TradeNumber)
                    {
                        profile.TradeStatus = TradeStatusEnum.Finished;

                        profile.Money += profile.TradeIn.Money;

                        foreach (var item in profile.TradeIn.ItemsToTrade)
                        {
                            profile.Inventory.Add(item);
                        }

                        RewardText = Texts.Handler_TradeInProgress;

                        TradeSecondOk tradeResponse = new TradeSecondOk(profile.TradeNumber);

                        var qrCode = _qRService.CreateQR(tradeResponse);
                        var param = new NavigationParameters
                        {
                            { "code", qrCode }
                        };

                        profile.TradeLastQR = qrCode;

                        DataRepository.SaveToFile(profile);
                        _loggerService.CreateTradeLog(profile);

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                    else
                    {
                        RewardText = Texts.Profile_TradeInProgress;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.Profile_TradeInProgress + "Velük: " + profile.TradeWith, Texts.Ok);
                    }
                }
                else if (data.Type == QRTypeEnum.TradeSecondOk)
                {
                    var profile = DataRepository.GetTeamProfile();
                    var processedData = JsonConvert.DeserializeObject<TradeSecondOk>(data.D);

                    Title = Texts.Trade_Title;

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

                        _loggerService.CreateTradeLog(profile);
                    }
                    else
                    {
                        RewardText = Texts.Profile_TradeInProgress;
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.Profile_TradeInProgress + "Velük: " + profile.TradeWith, Texts.Ok);
                    }
                }
                else if (data.Type == QRTypeEnum.ProfileForHq && _settings.AppMode == AppModeEnum.Hq)
                {
                    Title = Texts.Arena_Details;
                    var processedData = JsonConvert.DeserializeObject<ProfileQr>(data.D);

                    var profile = DataRepository.GetHqProfile();
                    profile.Scores.Add(processedData);
                    DataRepository.SaveToFile(profile);
                    RewardText = Texts.Arena_Scanned;
                }
                else if (data.Type == QRTypeEnum.HqReward)
                {
                    Title = Texts.Arena_Details;
                    var processedData = JsonConvert.DeserializeObject<HqReward>(data.D);

                    var profile = DataRepository.GetTeamProfile();
                    profile.AllExp += processedData.Exp;
                    profile.Honor += processedData.Honor;
                    profile.Money += processedData.Money;
                    var temp = new List<Item>();

                    foreach (var item in processedData?.RewardItems)
                    {
                        profile.Inventory.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Ad));
                        temp.Add(new Item(item.Tier, item.Rarity, item.ItemType, item.UsableFor, item.StatsFromQR, item.Ad));
                    }

                    DataRepository.SaveToFile(profile);
                    RewardText = processedData.Exp + " EXP \n" + processedData.Honor + " hírnév \n" + processedData.Money + " $";

                    processedData.RewardItems = new ObservableCollection<Item>(temp);
                    Rewards = new ObservableCollection<Item>(temp);
                    _loggerService.CreateSystemLog(processedData);

                }
                else
                {
                    Title = Texts.Handler_ErrorTitle;
                    RewardText = Texts.Handler_Error;
                }
                RewardIsVisible = true;
                IsBusy = false;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("------------" + e.ToString());
            }

        }
    }
}