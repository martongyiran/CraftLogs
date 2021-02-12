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
using CraftLogs.BLL.Models.ArenaModels;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class ArenaPageViewModel : ViewModelBase
    {

        private readonly ICombatService _combatService;
        private readonly IQRService _qRService;

        private ArenaProfile _arenaProfile;
        private Settings _settings;
        private CombatUnit _challenger;
        private CombatUnit _firstUnit;
        private CombatLogDetailsModel _logs;

        public CombatUnit FirstUnit
        {
            get => _firstUnit;
            set => SetProperty(ref _firstUnit, value);
        }

        public CombatLogDetailsModel Logs
        {
            get => _logs; 
            set => SetProperty(ref _logs, value); 
        }

        public DelayCommand NavigateToQRScannerPageCommand
            => new DelayCommand(async () => await ReadQr());

        public ArenaPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService, ICombatService combatservice,
            IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            _combatService = combatservice;
            _qRService = qrService;
            Title = Texts.Arena_Title;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await Init();
        }

        public override async Task ToSettings()
        {
            NavigationParameters param = new NavigationParameters
            {
                { "mode", "npc" }
            };

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        private async Task ReadQr()
        {
            var scanResult = await _qRService.ReadQr();

            if (scanResult != null)
            {
                await NavigateToWithoutHistory(NavigationLinks.QRHandlerPage, scanResult);
            }
        }

        private async Task Init()
        {
            IsBusy = true;

            DataRepository.CreateArenaProfile();
            _arenaProfile = DataRepository.GetArenaProfile();
            _settings = DataRepository.GetSettings();

            if (_settings.AppMode == AppModeEnum.None)
            {
                _settings.AppMode = AppModeEnum.Arena;
                DataRepository.SaveToFile(_settings);
            }

            FirstUnit = _arenaProfile.Leader;
            Logs = _arenaProfile.LastLog;

            if(_arenaProfile.Attacker != null)
            {
                _challenger = _arenaProfile?.Attacker;

                if (_combatService.CanFight(FirstUnit, _challenger))
                {
                    ArenaResponse details = _combatService.Fight(FirstUnit, _challenger);

                    if (details.IsWin)
                    {
                        _arenaProfile.Leader = _challenger;
                    }

                    _arenaProfile.Attacker = null;
                    _arenaProfile.LastLog = details.CombatLog;

                    DataRepository.SaveToFile(_arenaProfile);

                    var qrCode = _qRService.CreateQR(details);
                    var param = new NavigationParameters
                    {
                        { "code", qrCode }
                    };

                    await NavigateToWithoutHistory(NavigationLinks.QRPage, param);

                    IsBusy = false;
                }
                else
                {
                    _arenaProfile.Attacker = null;

                    DataRepository.SaveToFile(_arenaProfile);
                    await DialogService.DisplayAlertAsync(Texts.Error, Texts.Arena_CantFight, Texts.Ok);
                }
            }
            
            IsBusy = false;
        }
    }
}
