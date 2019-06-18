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
        private DelegateCommand startCommand;
        private DelegateCommand reloadCommand;

        private Settings settings;
        private QuestProfile profile;
        private IQRService qRService;
        private IItemGeneratorService itemGeneratorService;
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

        public DelegateCommand StartCommand => startCommand ?? (startCommand = new DelegateCommand(Start));

        public DelegateCommand ReloadCommand => reloadCommand ?? (reloadCommand = new DelegateCommand( () => { ReadyToScore = false; Init(); }));
        
        #endregion

        #region Ctor

        public QuestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService, IItemGeneratorService itemgeneratorService) : base(navigationService, dataRepository, dialogService)
        {
            qRService = qrService;
            itemGeneratorService = itemgeneratorService;
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

        private bool readyToScore = false;

        public bool ReadyToScore
        {
            get { return readyToScore; }
            set { SetProperty(ref readyToScore, value); }
        }

        #endregion

        #region Private functions

        private void Init()
        {
            profile = DataRepository.GetQuestProfile();
            Title = string.Concat(Texts.QuestPage, " - ", profile.QuestName);
            settings = DataRepository.GetSettings();
        }

        private void Start()
        {
            date = DateTime.Now;
            GetPointRange();
            Score = minPoint + (int)((SliderScore / 100.0) * pointRange);
            ReadyToScore = true;
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
                        reward.Items.Add(itemGeneratorService.GetRandomItem(3));
                        reward.Items.Add(itemGeneratorService.GetRandomItem(3));
                        reward.Items.Add(itemGeneratorService.GetRandomItem(3));
                        usablePoints -= 30;
                    }
                    else if (usablePoints >= 15)
                    {
                        reward.Items.Add(itemGeneratorService.GetRandomItem(2));
                        reward.Items.Add(itemGeneratorService.GetRandomItem(2));
                        reward.Items.Add(itemGeneratorService.GetRandomItem(2));
                        usablePoints -= 15;
                    }
                    else if (usablePoints >= 5)
                    {
                        reward.Items.Add(itemGeneratorService.GetRandomItem(1));
                        reward.Items.Add(itemGeneratorService.GetRandomItem(1));
                        reward.Items.Add(itemGeneratorService.GetRandomItem(1));
                        usablePoints -= 5;
                    }

                    reward.Money = usablePoints * 10;

                    var qrCode = qRService.CreateQR(reward);

                    NavigationParameters param = new NavigationParameters();
                    param.Add("code", qrCode);
                    await NavigateToWithoutHistory(NavigationLinks.RatingPage, param);
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.CantScore, Texts.Ok);
            }

        }
        
        #endregion
    }
}
