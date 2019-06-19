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
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {

        #region Private

        private TeamProfile teamProfile;

        private DelegateCommand navigateToLogsCommand;
        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToQRScannerPageCommand;
        private DelegateCommand getProfileQRCommand;

        #endregion

        #region Public

        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.LogsPage)));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.SettingsPage)));

        public DelegateCommand NavigateToQRScannerPageCommand => navigateToQRScannerPageCommand ?? (navigateToQRScannerPageCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.QRScannerPage)));

        public DelegateCommand GetProfileQRCommand => getProfileQRCommand ?? (getProfileQRCommand = new DelegateCommand(async () => await GetProfileQRAsync()));

        #endregion

        #region Ctor

        public ProfilePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.ProfilePage;
        }

        #endregion

        #region Properties

        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        private string lvl;

        public string Lvl
        {
            get { return lvl; }
            set { SetProperty(ref lvl, value); }
        }

        private string hp;

        public string Hp
        {
            get { return hp; }
            set { SetProperty(ref hp, value); }
        }

        private string exp;

        public string Exp
        {
            get { return exp; }
            set { SetProperty(ref exp, value); }
        }

        private string atk;

        public string Atk
        {
            get { return atk; }
            set { SetProperty(ref atk, value); }
        }

        private string def;

        public string Def
        {
            get { return def; }
            set { SetProperty(ref def, value); }
        }

        private string stamina;

        public string Stamina
        {
            get { return stamina; }
            set { SetProperty(ref stamina, value); }
        }

        private string critR;

        public string CritR
        {
            get { return critR; }
            set { SetProperty(ref critR, value); }
        }

        private string dodge;

        public string Dodge
        {
            get { return dodge; }
            set { SetProperty(ref dodge, value); }
        }

        private string points;

        public string Points
        {
            get { return points; }
            set { SetProperty(ref points, value); }
        }

        private string honor;

        public string Honor
        {
            get { return honor; }
            set { SetProperty(ref honor, value); }
        }

        private bool pointIsVisible;

        public bool PointIsVisible
        {
            get { return pointIsVisible; }
            set { SetProperty(ref pointIsVisible, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Init();
        }

        #endregion

        #region Private functions

        private void Init()
        {
            teamProfile = DataRepository.GetTeamProfile();

            Name = teamProfile.Name;

            Image = teamProfile.Image;
            Lvl = "Lvl." + teamProfile.Level + " " + teamProfile.Cast;
            Exp = "EXP: " + teamProfile.AllExp + "/" + teamProfile.XpForNextLevel;
            Honor = "Honor: " + teamProfile.Honor;

            Points = "Elosztható pontok: " + teamProfile.StatPoint;
            PointIsVisible = teamProfile.StatPoint != 0;

            Atk = "ATK: " + teamProfile.Atk;
            Def = "DEF: " + teamProfile.Def;
            Stamina = "STM: " + teamProfile.Stamina;

            Hp = "HP: " + teamProfile.Hp;
            CritR = "CritR: " + teamProfile.CritR + "%";
            Dodge = "Dodge: " + teamProfile.Dodge + "%";
        }

        private async Task GetProfileQRAsync()
        {
            await DialogService.DisplayAlertAsync("ok", "ok", "ok");
        }

        #endregion
    }
}
