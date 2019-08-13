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
using System.Linq;
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

        private ObservableCollection<CombatUnit> units = new ObservableCollection<CombatUnit>();

        public ObservableCollection<CombatUnit> Units
        {
            get { return units; }
            set { SetProperty(ref units, value); }
        }

        private CombatUnit firstUnit;

        public CombatUnit FirstUnit
        {
            get { return firstUnit; }
            set { SetProperty(ref firstUnit, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            Init();

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
            var a = new CombatUnit("Zugzug1", 6, 5, 5, 5, 165);
            a.CombatScore = 500;
            arenaProfile.CombatUnits.Add(a);

            var b = new CombatUnit("Zugzug2", 6, 5, 5, 5, 165);
            b.CombatScore = 250;
            arenaProfile.CombatUnits.Add(b);

            Units = new ObservableCollection<CombatUnit>(arenaProfile.CombatUnits.OrderByDescending(x => x.CombatScore).ToList());

            for (int i = 0; i < Units.Count; i++)
            {
                Units[i].Placement = i + 1;
            }

            FirstUnit = Units[0];
            Units.Remove(FirstUnit);

            var zeroScore = Units.Where(x => x.CombatScore == 0).ToList();

            if(zeroScore.Count != 0)
            {
                player2 = zeroScore[0];
                Units.Remove(player2);

                if(combatService.CanFight(FirstUnit, player2))
                {
                    ArenaResponse details = combatService.Fight(FirstUnit, player2);
                    arenaProfile.CombatUnits.Add(details.UpdatedProfile);
                    DataRepository.SaveToFile(arenaProfile);

                    var qrCode = qRService.CreateQR(details);
                    NavigationParameters param = new NavigationParameters();
                    param.Add("code", qrCode);

                    IsBusy = false;

                    await NavigateTo(NavigationLinks.QRPage, param);
                }
            }
            IsBusy = false;
        }

        #endregion

    }
}
