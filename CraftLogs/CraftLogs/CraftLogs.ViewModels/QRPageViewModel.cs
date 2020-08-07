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

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class QRPageViewModel : ViewModelBase
    {
        private string _qrCode;
        private Settings _settings;
        private string isSettings;

        public string QrCode
        {
            get => _qrCode;
            set => SetProperty(ref _qrCode, value);
        }

        public DelayCommand NavigateToHomeCommand => new DelayCommand(async () => await ExecuteNavigateToHomeCommandAsync());

        public QRPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _settings = DataRepository.GetSettings();

            QrCode = parameters["code"] as string;

            isSettings = parameters["type"] as string;
#if DEV
            System.Diagnostics.Debug.WriteLine(QrCode);
#endif
        }

        private async Task ExecuteNavigateToHomeCommandAsync()
        {
            if(isSettings != null)
            {
                await NavigateToWithoutHistoryDouble(NavigationLinks.ProfilePage);
            }
            else
            {
                switch (_settings.AppMode)
                {
                    case AppModeEnum.Team:
                        await NavigateToWithoutHistory(NavigationLinks.ProfilePage);
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
                        await NavigateToWithoutHistory(NavigationLinks.MainPage);
                        break;
                }
            }
        }
    }
}
