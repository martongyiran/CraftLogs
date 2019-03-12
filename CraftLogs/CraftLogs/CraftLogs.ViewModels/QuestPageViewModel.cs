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
using CraftLogs.Values;
using Plugin.VersionTracking;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class QuestPageViewModel : ViewModelBase
    {

        #region Private

        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand scoreCommand;
        private Settings settings;
        private QuestProfile profile;
        private IQRService qRService;
        private DateTime date;
        private int minPoint;
        private int maxPoint;
        private int pointRange;

        #endregion

        #region Public

#if DEV
        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion) + " DEV"; } }
#elif STG
        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion) + " STG"; } }
#elif PRD
        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion); } }
#endif

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.SettingsPage)));

        public DelegateCommand ScoreCommand => scoreCommand ?? (scoreCommand = new DelegateCommand(async () => await ScoreAsync()));

        #endregion

        #region Ctor

        public QuestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService) : base(navigationService, dataRepository, dialogService)
        {
            qRService = qrService;
        }

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            Init();
        }

        #endregion

        #region Properties

        private int sliderScore = 50;

        public int SliderScore
        {
            get { return sliderScore; }
            set { SetProperty(ref sliderScore, value); Score = minPoint + (int)((SliderScore / 100.0) * pointRange); }
        }

        private int score;

        public int Score
        {
            get { return score; }
            set { SetProperty(ref score, value); }
        }

        #endregion

        #region Private functions

        private void Init()
        {
            profile = DataRepository.GetQuestProfile();
            Title = string.Concat(Texts.QuestPage, " - ", profile.QuestName);
            settings = DataRepository.GetSettings();
            date = DateTime.Now;
            GetPointRange();
            Score = minPoint + (int)((SliderScore / 100.0) * pointRange);
        }

        private void GetPointRange()
        {
#if DEV
            minPoint = 15;
            maxPoint = 25;
            pointRange = maxPoint - minPoint;

#else
            
            var actHour = date.Hour;
            if (settings.CraftDay == 1)
            {
                if (actHour == settings.Craft1Start)
                {
                    minPoint = settings.Craft1MinPont;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 1))
                {
                    minPoint = settings.Craft1MinPont + 2;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 2))
                {
                    minPoint = settings.Craft1MinPont + 5;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 3))
                {
                    minPoint = settings.Craft1MinPont + 7;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 4))
                {
                    minPoint = settings.Craft1MinPont + 7;
                    maxPoint = minPoint + 10;
                }
                else
                {
                    minPoint = 0;
                    maxPoint = 0;
                }

            }
            else
            {
                if (actHour == settings.Craft2Start)
                {
                    minPoint = settings.Craft2MinPont;
                    maxPoint = minPoint + 12;
                }
                else if (actHour == (settings.Craft2Start + 1))
                {
                    minPoint = settings.Craft2MinPont + 2;
                    maxPoint = minPoint + 14;
                }
                else if (actHour == (settings.Craft2Start + 2))
                {
                    minPoint = settings.Craft2MinPont + 4;
                    maxPoint = minPoint + 15;
                }
                else if (actHour == (settings.Craft2Start + 3))
                {
                    minPoint = settings.Craft2MinPont + 6;
                    maxPoint = minPoint + 18;
                }
                else if (actHour == (settings.Craft2Start + 4))
                {
                    minPoint = settings.Craft2MinPont + 6;
                    maxPoint = minPoint + 18;
                }
                else
                {
                    minPoint = 0;
                    maxPoint = 0;
                }
            }

            pointRange = maxPoint - minPoint;

#endif
        }

        private async Task ScoreAsync()
        {
            if (Score != 0)
            {
                var res = await DialogService.DisplayAlertAsync(Texts.Result, string.Format(Texts.ResultDialog, Score), Texts.Yes, Texts.No);
                if (res)
                {
                    int usablePoints = Score;
                    QuestReward reward = new QuestReward();
                    reward.From = profile.QuestName;
                    reward.Score = Score;
                    reward.Honor = 1;

                    usablePoints -= 10;

                    if (usablePoints >= 30)
                    {
                        reward.Items.Add(RandomItem(3));
                        reward.Items.Add(RandomItem(3));
                        reward.Items.Add(RandomItem(3));
                        usablePoints -= 30;
                    }
                    else if (usablePoints >= 15)
                    {
                        reward.Items.Add(RandomItem(2));
                        reward.Items.Add(RandomItem(2));
                        reward.Items.Add(RandomItem(2));
                        usablePoints -= 15;
                    }
                    else if (usablePoints >= 5)
                    {
                        reward.Items.Add(RandomItem(1));
                        reward.Items.Add(RandomItem(1));
                        reward.Items.Add(RandomItem(1));
                        usablePoints -= 5;
                    }

                    reward.Money = usablePoints * 10;

                    var qr = qRService.CreateQR(reward);

                    NavigationParameters param = new NavigationParameters();
                    param.Add("code", qr);
                    await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.CantScore, Texts.Ok);
            }

        }

        //TEMP: waiting for item generator service
        private Item RandomItem(int tier)
        {
            switch (tier)
            {
                case 1:
                    return new Item(1, ItemRarityEnum.Common, ItemTypeEnum.Armor, CharacterClassEnum.Mage, "1 1 1 1 1");
                case 2:
                    return new Item(2, ItemRarityEnum.Common, ItemTypeEnum.Armor, CharacterClassEnum.Mage, "1 1 1 1 1");
                case 3:
                    return new Item(3, ItemRarityEnum.Common, ItemTypeEnum.Armor, CharacterClassEnum.Mage, "1 1 1 1 1");
                default:
                    return null;
            }

        }

        #endregion
    }
}
