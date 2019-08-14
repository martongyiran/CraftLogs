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

using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Plugin.VersionTracking;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
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
        private CombatUnit player2;

        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToQRScannerPageCommand;

        #endregion

        #region Public

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.SettingsPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand NavigateToQRScannerPageCommand => navigateToQRScannerPageCommand ?? (navigateToQRScannerPageCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.QRScannerPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion); } }

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

        private ObservableCollection<string> logs = new ObservableCollection<string>();

        public ObservableCollection<string> Logs
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

        #endregion

        #region Private functions

        private async Task Init()
        {
            IsBusy = true;
            DataRepository.CreateArenaProfile();
            arenaProfile = DataRepository.GetArenaProfile();
            settings = DataRepository.GetSettings();

            FirstUnit = arenaProfile.Leader;
            Logs = new ObservableCollection<string>(arenaProfile.LastLog);
            if(arenaProfile.Attacker != null)
            {
                player2 = arenaProfile?.Attacker;

                if (combatService.CanFight(FirstUnit, player2))
                {
                    ArenaResponse details = combatService.Fight(FirstUnit, player2);

                    if (details.IsWin)
                    {
                        arenaProfile.Leader = player2;
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
