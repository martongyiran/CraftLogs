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

using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;
using CraftLogs.Values;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Services;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Enums;
using System.Threading.Tasks;
using CraftLogs.BLL.Services.Interfaces;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private
        
        private Settings settings;

        private bool isDevMode = false;

        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToLogsCommand;
        private DelegateCommand navigateToProfileCommand;
        private DelegateCommand navigateToQuestCommand;
        private DelegateCommand clearModeCommand;
        //test
        private DelegateCommand navigateToQRPageCommand;
        private DelegateCommand navigateToQRScannerPageCommand;

        private AppModeEnum mode;
        private bool hqMenuVisibility = false;
        private bool teamMenuVisibility = false;
        private bool shopMenuVisibility = false;
        private bool arenaMenuVisibility = false;
        private bool isNpc = false;

        private NavigationParameters param = new NavigationParameters();

        #endregion

        #region Public

        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion); } }

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.SettingsPage)));
        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.LogsPage)));
        public DelegateCommand NavigateToProfileCommand => navigateToProfileCommand ?? (navigateToProfileCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.ProfilePage)));
        public DelegateCommand NavigateToQuestCommand => navigateToQuestCommand ?? (navigateToQuestCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.QuestPage)));
        public DelegateCommand NavigateToQRPageCommand => navigateToQRPageCommand ?? (navigateToQRPageCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.QRPage)));
        public DelegateCommand NavigateToQRScannerPageCommand => navigateToQRScannerPageCommand ?? (navigateToQRScannerPageCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.QRScannerPage)));

        public DelegateCommand ClearModeCommand => clearModeCommand ?? (clearModeCommand = new DelegateCommand(async () => await ClearMode()));

        public AppModeEnum Mode
        {
            get { return mode; }
            set { SetProperty(ref mode, value); }
        }

        public bool IsDevMode
        {
            get { return isDevMode; }
            set { SetProperty(ref isDevMode, value); }
        }

        public bool HqMenuVisibility
        {
            get { return hqMenuVisibility; }
            set { SetProperty(ref hqMenuVisibility, value); }
        }

        public bool TeamMenuVisibility
        {
            get { return teamMenuVisibility; }
            set { SetProperty(ref teamMenuVisibility, value); }
        }
        
        public bool ShopMenuVisibility
        {
            get { return shopMenuVisibility; }
            set { SetProperty(ref shopMenuVisibility, value); }
        }

        public bool ArenaMenuVisibility
        {
            get { return arenaMenuVisibility; }
            set { SetProperty(ref arenaMenuVisibility, value); }
        }

        public bool IsNpc
        {
            get { return isNpc; }
            set { SetProperty(ref isNpc, value); }
        }

        #endregion

        #region Ctor

        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
#if DEV
            Title = Texts.MainPage + " DEV";
            IsDevMode = true;
#elif STG
            Title = Texts.MainPage + " STG";
#elif PRD
            Title = Texts.MainPage;
#endif

        }

        #endregion

        #region Overrides

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            SetUpFileSystem();

            settings = DataRepository.GetSettings();

            if (settings.AppMode == AppModeEnum.None)
            {
                await NavigateToWithoutHistory(NavigationLinks.SelectModePage);
            }
            else if (settings.AppMode == AppModeEnum.Quest && !DataRepository.IsQuestProfileExist())
            {
                NavigationParameters mode = new NavigationParameters();
                mode.Add("mode", "quest");
                await NavigateToWithoutHistory(NavigationLinks.RegisterPage, mode);
            }
            else if (settings.AppMode == AppModeEnum.Quest)
            {
                await NavigateToWithoutHistory(NavigationLinks.QuestPage);
            }

            Mode = settings.AppMode;
            SetUpVisibility();
            
        }

        #endregion

        #region Private functions

        private void SetUpFileSystem()
        {
            DataRepository.CreateSettings();
            DataRepository.DeleteFile(FileNames.Logs); //I create a lot of test logs in LogsPageViewModel, so it's necessary to clear it at every start. It's only for testing.
            DataRepository.CreateLogs();
        }

        private void SetUpVisibility()
        {
            SetMenuVisibility(false);
            switch (Mode)
            {
                case AppModeEnum.None:
                    HqMenuVisibility = false;
                    break;
                case AppModeEnum.Team:
                    TeamMenuVisibility = true;
                    break;
                case AppModeEnum.Shop:
                    ShopMenuVisibility = true;
                    IsNpc = true;
                    break;
                case AppModeEnum.Arena:
                    ArenaMenuVisibility = true;
                    IsNpc = true;
                    break;
                case AppModeEnum.Hq:
                    HqMenuVisibility = true;
                    IsNpc = true;
                    break;
                default:
                    break;
            }
        }

        private void SetMenuVisibility(bool value)
        {
            HqMenuVisibility = value;
            TeamMenuVisibility = value;
            ShopMenuVisibility = value;
            ArenaMenuVisibility = value;
        }

        //for testing
        private async Task ClearMode()
        {
            settings.AppMode = AppModeEnum.None;
            DataRepository.SaveToFile(settings);
            await NavigateToWithoutHistory(NavigationLinks.SelectModePage);
        }

        #endregion
    }
}
