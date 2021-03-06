﻿/*
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
        private Settings _settings;
        private bool _isDevMode = false;

        public bool IsDevMode
        {
            get => _isDevMode;
            set => SetProperty(ref _isDevMode, value);
        }

        public MainPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IQRService qrService)
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            IsBusy = true;

            DataRepository.CreateSettings();

            _settings = DataRepository.GetSettings();

            switch (_settings.AppMode)
            {
                case AppModeEnum.None:
                    await NavigateToWithoutHistory(NavigationLinks.SelectModePage);
                    break;
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
            }

            IsBusy = false;
        }
    }
}
