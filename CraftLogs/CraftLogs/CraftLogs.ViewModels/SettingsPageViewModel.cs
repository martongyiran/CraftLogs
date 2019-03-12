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
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        #region Private

        private IQRService qRService;
        private Settings settings;
        private DelegateCommand saveSettingsCommand;
        private DelegateCommand resetSettingsCommand;
        private DelegateCommand deleteProfileCommand;
        private DelegateCommand getAvgCommand;

        #endregion

        #region Public

        public DelegateCommand SaveSettingsCommand => saveSettingsCommand ?? (saveSettingsCommand = new DelegateCommand(SaveSettings));
        public DelegateCommand ResetSettingsCommand => resetSettingsCommand ?? (resetSettingsCommand = new DelegateCommand(async () => await ResetSettingsAsync()));
        public DelegateCommand DeleteProfileCommand => deleteProfileCommand ?? (deleteProfileCommand = new DelegateCommand(async () => await DeleteProfileAsync()));
        public DelegateCommand GetAvgCommand => getAvgCommand ?? (getAvgCommand = new DelegateCommand(async () => await GetAvgAsync()));

        #endregion



        #region Ctor

        public SettingsPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            qRService = qrService;
            Title = Texts.SettingsPage;
        }

        #endregion

        #region Properties

        private int craftDay;

        public int CraftDay
        {
            get { return craftDay; }
            set { SetProperty(ref craftDay, value); }
        }

        private int craft1Start;

        public int Craft1Start
        {
            get { return craft1Start; }
            set { SetProperty(ref craft1Start, value); }
        }

        private int craft2Start;

        public int Craft2Start
        {
            get { return craft2Start; }
            set { SetProperty(ref craft2Start, value); }
        }

        private int craft1MinPont;

        public int Craft1MinPont
        {
            get { return craft1MinPont; }
            set { SetProperty(ref craft1MinPont, value); }
        }

        private int craft2MinPont;

        public int Craft2MinPont
        {
            get { return craft2MinPont; }
            set { SetProperty(ref craft2MinPont, value); }
        }

        private bool avgVisibility;

        public bool AvgVisibility
        {
            get { return avgVisibility; }
            set { SetProperty(ref avgVisibility, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            SetUp();
        }

        #endregion

        #region Private functions

        private void SetUp()
        {
            settings = DataRepository.GetSettings();

            AvgVisibility = settings.AppMode == BLL.Enums.AppModeEnum.Quest;

            CraftDay = settings.CraftDay;
            Craft1Start = settings.Craft1Start;
            Craft2Start = settings.Craft2Start;
            Craft1MinPont = settings.Craft1MinPont;
            Craft2MinPont = settings.Craft2MinPont;
        }

        private void SaveSettings()
        {
            settings.CraftDay = CraftDay;
            settings.Craft1Start = Craft1Start;
            settings.Craft2Start = Craft2Start;
            settings.Craft1MinPont = Craft1MinPont;
            settings.Craft2MinPont = Craft2MinPont;

            DataRepository.SaveToFile(settings);
            DialogService.DisplayAlertAsync("", Texts.SuccessfulSaving, Texts.Ok);
        }

        private async Task ResetSettingsAsync()
        {
            var res = await DialogService.DisplayAlertAsync("", Texts.ResetData, Texts.Yes, Texts.No);
            if (res)
            {
                var mode = settings.AppMode;
                DataRepository.ResetSettings();
                SetUp();
                settings.AppMode = mode;
                DataRepository.SaveToFile(settings);
            }
        }

        private async Task DeleteProfileAsync()
        {
            var res = await DialogService.DisplayAlertAsync("", Texts.DeleteProfileQuestion, Texts.Yes, Texts.No);
            if (res)
            {
                DataRepository.ResetSettings();
                SetUp();
                DataRepository.DeleteQuestProfile();
                await NavigateToWithoutHistoryDouble(NavigationLinks.SelectModePage);
            }
        }

        private async Task GetAvgAsync()
        {
            var qrCode = qRService.CreateQR(new QuestProfileQR(DataRepository.GetQuestProfile()));

            NavigationParameters param = new NavigationParameters();
            param.Add("code", qrCode);
            await NavigateToWithoutHistoryDouble(NavigationLinks.QRPage, param);
        }

        #endregion


    }
}
