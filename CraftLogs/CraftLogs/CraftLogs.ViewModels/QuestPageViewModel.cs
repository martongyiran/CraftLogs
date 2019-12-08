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
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class QuestPageViewModel : ViewModelBase
    {
        private readonly IQRService _qRService;
        private readonly IItemGeneratorService _itemGeneratorService;

        private Settings settings;
        private DateTime date;

        private int minPoint;
        private int maxPoint;
        private int pointRange;

        private int _sliderScore = 50;
        private int _score;
        private bool _readyToScore = false;
        private string _from;

        public int SliderScore
        {
            get => _sliderScore;
            set
            {
                SetProperty(ref _sliderScore, value);
                Score = minPoint + (int)((SliderScore / 100.0) * pointRange);
            }
        }

        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        public bool ReadyToScore
        {
            get => _readyToScore;
            set => SetProperty(ref _readyToScore, value);
        }

        public string From
        {
            get => _from;
            set => SetProperty(ref _from, value);
        }

        public DelayCommand ScoreCommand => new DelayCommand(async () => await ExecuteScoreCommandAsync());

        public DelayCommand StartCommand => new DelayCommand(ExecuteStartCommand);

        public DelayCommand ReloadCommand => new DelayCommand( () => { ReadyToScore = false; Init(); });
        
        public QuestPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IQRService qrService, IItemGeneratorService
            itemgeneratorService)
            : base(navigationService, dataRepository, dialogService)
        {
            _qRService = qrService;
            _itemGeneratorService = itemgeneratorService;
            Title = Texts.QuestPage;
        }
        
        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            Init();
        }

        public override async Task ToSettings()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("mode", "npc");

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        private void Init()
        {
            settings = DataRepository.GetSettings();
        }

        private void ExecuteStartCommand()
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

        private async Task ExecuteScoreCommandAsync()
        {
            if (Score != 0 && !string.IsNullOrEmpty(From) && !string.IsNullOrWhiteSpace(From))
            {
                var res = await DialogService.DisplayAlertAsync(Texts.Result, string.Format(Texts.ResultDialog, Score), Texts.Yes, Texts.No);
                if (res)
                {
                    int usablePoints = Score;

                    var reward = new QuestReward
                    {
                        From = From,
                        Score = Score,
                        Honor = 1
                    };

                    usablePoints -= 10;

                    if (usablePoints >= 30)
                    {
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(3));
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(3));
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(3));
                        usablePoints -= 30;
                    }
                    else if (usablePoints >= 15)
                    {
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(2));
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(2));
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(2));
                        usablePoints -= 15;
                    }
                    else if (usablePoints >= 5)
                    {
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(1));
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(1));
                        reward.Items.Add(_itemGeneratorService.GetRandomItem(1));
                        usablePoints -= 5;
                    }

                    reward.Money = usablePoints * 10;

                    var qrCode = _qRService.CreateQR(reward);

                    var param = new NavigationParameters
                    {
                        { "res", qrCode }
                    };

                    await NavigateToWithoutHistory(NavigationLinks.QRHandlerPage, param);
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.CantScore, Texts.Ok);
            }

        }
    }
}
