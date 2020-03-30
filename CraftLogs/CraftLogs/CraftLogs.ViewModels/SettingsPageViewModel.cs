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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace CraftLogs.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IQRService _qRService;

        private Settings _userSettings;
        private bool _isNpc;
        private string _pw;
        private string _param;

        public Settings UserSettings
        {
            get => _userSettings;
            set => SetProperty(ref _userSettings, value);
        }

        public bool IsNpc
        {
            get => _isNpc;
            set => SetProperty(ref _isNpc, value);
        }

        public string Pw
        {
            get => _pw;
            set => SetProperty(ref _pw, value);
        }

        public List<int> Days { get; set; } = Enumerable.Range(1, 2).ToList();

        public List<int> C1Starts { get; set; } = Enumerable.Range(8, 12).ToList();

        public List<int> C2Starts { get; set; } = Enumerable.Range(8, 12).ToList();

        public List<int> C1PointRange { get; set; } = Enumerable.Range(10, 40).ToList();

        public List<int> C2PointRange { get; set; } = Enumerable.Range(10, 40).ToList();

        public DelayCommand SaveSettingsCommand => new DelayCommand(async () => await SaveSettings());
        public DelayCommand ResetSettingsCommand => new DelayCommand(async () => await ResetSettingsAsync());
        public DelayCommand DeleteProfileCommand => new DelayCommand(async () => await DeleteProfileAsync());
        public DelayCommand ToQuestCommand => new DelayCommand(async () => await ToQuest());
        public DelayCommand SupportCommand => new DelayCommand(async () => await Launcher.OpenAsync(new Uri("https://paypal.me/CHlGGA")));
        public DelayCommand MyProfileQrCommand => new DelayCommand(async () => await ExecuteMyProfileQrCommand());

        public SettingsPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            _qRService = qrService;
            Title = Texts.Settings_Title;
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            _param = parameters["mode"] as string;

            SetUp();
        }

        private void SetUp()
        {
            UserSettings = DataRepository.GetSettings();
            IsNpc = _param == "npc";
        }

        private async Task SaveSettings()
        {
            DataRepository.SaveToFile(UserSettings);

            if (IsNpc)
            {
                await DialogService.DisplayAlertAsync("", Texts.Settings_SuccessfulSave, Texts.Ok);
            }
            else
            {
                await NavigateToWithoutHistory(NavigationLinks.MainPage);
            }

        }

        private async Task ResetSettingsAsync()
        {
            var res = await DialogService.DisplayAlertAsync("", Texts.Settings_ResetData, Texts.Yes, Texts.No);
            if (res)
            {
                var mode = UserSettings.AppMode;
                DataRepository.ResetSettings();
                SetUp();
                UserSettings.AppMode = mode;
                DataRepository.SaveToFile(UserSettings);
            }
        }

        private async Task DeleteProfileAsync()
        {
            var res = await DialogService.DisplayAlertAsync(Texts.Settings_DeleteProfile, Texts.Settings_DeleteProfileDialog, Texts.Yes, Texts.No);
            if (res)
            {
                IsBusy = true;
                DataRepository.ResetSettings();
                SetUp();
                DataRepository.DeleteTeamProfile();
                DataRepository.DeleteShopProfile();
                DataRepository.DeleteArenaProfile();
                DataRepository.DeleteHqProfile();
                DataRepository.DeleteLogs();

                await NavigateToWithoutHistoryDouble(NavigationLinks.SelectModePage);
            }
        }

        private async Task ExecuteMyProfileQrCommand()
        {
            var teamProfile = DataRepository.GetTeamProfile();
            var profileQr = new ProfileQr(teamProfile);
            var qrCode = _qRService.CreateQR(profileQr);
            var param = new NavigationParameters
            {
                { "code", qrCode },
                {"type", "settings"}
            };

            await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
        }

        private async Task ToQuest()
        {
#if DEV
            await NavigateToWithoutHistoryDouble(NavigationLinks.QuestPage);
#else
        if(Pw == "123456")
            {
                await NavigateToWithoutHistoryDouble(NavigationLinks.QuestPage);
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.Settings_WrongPasswor, Texts.Ok);
            }
#endif
        }
    }
}
