﻿/*
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
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class ArenaPageViewModel : ViewModelBase
    {
        #region Private

        private ICombatService combatService;
        private IQRService qRService;

        private ArenaProfile arenaProfile;
        private Settings settings;
        private CombatUnit challenger;

        #endregion

        #region Public

        public DelayCommand NavigateToQRScannerPageCommand => new DelayCommand(async () => await NavigateTo(NavigationLinks.QRScannerPage));

        #endregion

        #region Ctor

        public ArenaPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, ICombatService combatservice, IQRService qrService) : base(navigationService, dataRepository, dialogService)
        {
            combatService = combatservice;
            qRService = qrService;
            Title = Texts.ArenaTitle;
        }

        #endregion

        #region Properties

        private CombatUnit firstUnit;

        public CombatUnit FirstUnit
        {
            get { return firstUnit; }
            set { SetProperty(ref firstUnit, value); }
        }

        private string logs;

        public string Logs
        {
            get { return logs; }
            set { SetProperty(ref logs, value); }
        }

        #endregion

        #region Overrides

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await Init();

            IsBusy = false;
        }

        public override async Task ToSettings()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("mode", "npc");

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        #endregion

        #region Private functions

        private async Task Init()
        {
            IsBusy = true;
            DataRepository.CreateArenaProfile();
            arenaProfile = DataRepository.GetArenaProfile();
            settings = DataRepository.GetSettings();

            if (settings.AppMode == AppModeEnum.None)
            {
                settings.AppMode = AppModeEnum.Arena;
                DataRepository.SaveToFile(settings);
            }

            FirstUnit = arenaProfile.Leader;
            Logs = arenaProfile.LastLog;
            if(arenaProfile.Attacker != null)
            {
                challenger = arenaProfile?.Attacker;

                if (combatService.CanFight(FirstUnit, challenger))
                {
                    ArenaResponse details = combatService.Fight(FirstUnit, challenger);

                    if (details.IsWin)
                    {
                        arenaProfile.Leader = challenger;
                    }

                    arenaProfile.Attacker = null;
                    arenaProfile.LastLog = details.CombatLog;

                    DataRepository.SaveToFile(arenaProfile);

                    var qrCode = qRService.CreateQR(details);
                    NavigationParameters param = new NavigationParameters();
                    param.Add("code", qrCode);

                    IsBusy = false;

                    await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                }
                else
                {
                    arenaProfile.Attacker = null;

                    DataRepository.SaveToFile(arenaProfile);
                    await DialogService.DisplayAlertAsync(Texts.Oupsie, Texts.CantFight, Texts.Sadface);
                }
            }
            
            IsBusy = false;
        }

        #endregion

    }
}
