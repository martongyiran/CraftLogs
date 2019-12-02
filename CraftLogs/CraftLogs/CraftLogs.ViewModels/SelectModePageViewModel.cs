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

using System.Threading.Tasks;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class SelectModePageViewModel : ViewModelBase
    {
        #region Private

        private Settings settings;

        #endregion

        #region Public

        public DelayCommand SetModeToTeamCommand => new DelayCommand(async () => await SetMode(AppModeEnum.Team));
        public DelayCommand SetModeToShopCommand => new DelayCommand(async () => await SetMode(AppModeEnum.Shop));
        public DelayCommand SetModeToArenaCommand => new DelayCommand(async () => await SetMode(AppModeEnum.Arena));
        public DelayCommand SetModeToHqCommand => new DelayCommand(async () => await SetMode(AppModeEnum.Hq));

        #endregion

        #region Ctor

        public SelectModePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
#if STG
            IsDev = false;
#elif PRD
            IsDev = false;
#endif
        }

        #endregion

        #region Overrides

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            settings = DataRepository.GetSettings();

            if (settings.AppMode != AppModeEnum.None)
                await NavigateToWithoutHistory(NavigationLinks.MainPage);
        }

        #endregion

        #region Properties

        private bool isDev = true;

        public bool IsDev
        {
            get { return isDev; }
            set { SetProperty(ref isDev, value); }
        }

        #endregion

        #region Private functions

        private async Task SetMode(AppModeEnum appMode)
        {
            IsBusy = true;

            switch (appMode)
            {
                case AppModeEnum.None:
                    break;
                case AppModeEnum.Team:
                    await NavigateToWithoutHistory(NavigationLinks.RegisterPage);
                    break;
                case AppModeEnum.Shop:
                    await NavigateToWithoutHistory(NavigationLinks.ShopPage);
                    break;
                case AppModeEnum.Arena:
                    await NavigateToWithoutHistory(NavigationLinks.ArenaPage);
                    break;
                case AppModeEnum.Hq:
                    await NavigateToWithoutHistory(NavigationLinks.HqPage);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
