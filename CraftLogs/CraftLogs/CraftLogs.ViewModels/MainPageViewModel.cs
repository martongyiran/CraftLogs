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
using CraftLogs.Values;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Services;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Services.Interfaces;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private
        
        private Settings settings;

        private bool isDevMode = false;
        
        #endregion

        #region Public

        public bool IsDevMode
        {
            get { return isDevMode; }
            set { SetProperty(ref isDevMode, value); }
        }

        #endregion

        #region Ctor

        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            IsBusy = true;
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
            IsBusy = true;
            SetUpFileSystem();
            settings = DataRepository.GetSettings();
#if SPC
            settings.AppMode = AppModeEnum.Spectator;
            DataRepository.SaveToFile(settings);
#endif

            if (settings.AppMode == AppModeEnum.None)
            {
                await NavigateToWithoutHistory(NavigationLinks.SelectModePage);
            }
            else if (settings.AppMode == AppModeEnum.Team)
            {
                await NavigateToWithoutHistory(NavigationLinks.ProfilePage);
            }
            else if (settings.AppMode == AppModeEnum.Shop)
            {
                await NavigateToWithoutHistory(NavigationLinks.ShopPage);
            }
            else if (settings.AppMode == AppModeEnum.Arena)
            {
                await NavigateToWithoutHistory(NavigationLinks.ArenaPage);
            }
            else if (settings.AppMode == AppModeEnum.Hq)
            {
                await NavigateToWithoutHistory(NavigationLinks.HqPage);
            }

            IsBusy = false;
        }

        #endregion

        #region Private functions

        private void SetUpFileSystem()
        {
            DataRepository.CreateSettings();
        }
        #endregion
    }
}
