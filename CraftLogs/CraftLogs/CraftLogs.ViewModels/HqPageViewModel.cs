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

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class HqPageViewModel : ViewModelBase
    {
        private readonly IItemGeneratorService _itemGenerator;
        private readonly IQRService _qRService;

        private Settings _settings;
        private HqProfile _hqProfile;
        private ObservableCollection<ProfileQr> _teams = new ObservableCollection<ProfileQr>();
        private HqReward _reward;
        private LegendaryEnum _lego = LegendaryEnum.None;

        public ObservableCollection<ProfileQr> Teams
        {
            get => _teams;
            set => SetProperty(ref _teams, value);
        }


        public HqReward Reward
        {
            get => _reward;
            set => SetProperty(ref _reward, value);
        }

        public List<LegendaryEnum> Legendaries { get; set; } = new List<LegendaryEnum>() { LegendaryEnum.None, LegendaryEnum.Baetylus, LegendaryEnum.Brisingamen, LegendaryEnum.Mjolnir };


        public LegendaryEnum Lego
        {
            get => _lego;
            set => SetProperty(ref _lego, value);
        }

        public DelayCommand NavigateToQRScannerPageCommand => new DelayCommand(async () => await NavigateTo(NavigationLinks.QRScannerPage));
        public DelayCommand GiveCommand => new DelayCommand(async () => await ExecuteGiveCommandAsync());

        public HqPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IItemGeneratorService itemGeneratorService,
            IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            _itemGenerator = itemGeneratorService;
            _qRService = qrService;
            Title = Texts.SelectMode_Hq;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Init();
        }

        public override async Task ToSettings()
        {
            var param = new NavigationParameters
            {
                { "mode", "npc" }
            };

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        private void Init()
        {
            _settings = DataRepository.GetSettings();

            if (_settings.AppMode == AppModeEnum.None)
            {
                _settings.AppMode = AppModeEnum.Hq;
                DataRepository.SaveToFile(_settings);
            }

            DataRepository.CreateHqProfile();

            _hqProfile = DataRepository.GetHqProfile();

            Reward = new HqReward();

            Teams = new ObservableCollection<ProfileQr>(_hqProfile.Scores.OrderByDescending(x => x.Score));
        }

        private async Task ExecuteGiveCommandAsync()
        {
            if (Lego != LegendaryEnum.None)
            {
                Reward.RewardItems.Add(_itemGenerator.GetLegendary(Lego));
            }

            var qrCode = _qRService.CreateQR(Reward);
            var param = new NavigationParameters
            {
                { "code", qrCode }
            };

            await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
        }
    }
}
