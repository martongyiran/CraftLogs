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
    public class QRPageViewModel : ViewModelBase
    {
        #region Private

        private DelegateCommand navigateToHomeCommand;

        private string qrCode;
        private Settings settings;

        #endregion

        #region Public

        public DelegateCommand NavigateToHomeCommand => navigateToHomeCommand ?? (navigateToHomeCommand = new DelegateCommand(async () => { IsBusy = true; await SmartNavigation(); }, CanSubmit).ObservesProperty(()=>IsBusy));

        public string QrCode
        {
            get { return qrCode; }
            set { SetProperty(ref qrCode, value); }
        }

        #endregion

        #region ctor

        public QRPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
        }

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            settings = DataRepository.GetSettings();

            QrCode = parameters["code"] as string;
            System.Diagnostics.Debug.WriteLine(QrCode);
        }

        #endregion

        #region Private functions

        private async Task SmartNavigation()
        {
            if(settings.AppMode == BLL.Enums.AppModeEnum.Quest)
            {
                await NavigateToWithoutHistory(NavigationLinks.QuestPage);
            } 
            else if (settings.AppMode == BLL.Enums.AppModeEnum.Team)
            {
                await NavigateToWithoutHistory(NavigationLinks.ProfilePage);
            }
            else if (settings.AppMode == BLL.Enums.AppModeEnum.Shop)
            {
                await NavigateToWithoutHistory(NavigationLinks.ShopPage);
            }
            else if (settings.AppMode == BLL.Enums.AppModeEnum.Arena)
            {
                await NavigateToWithoutHistory(NavigationLinks.ArenaPage);
            }
            await NavigateToWithoutHistory(NavigationLinks.MainPage);
        }

        #endregion
    }
}
