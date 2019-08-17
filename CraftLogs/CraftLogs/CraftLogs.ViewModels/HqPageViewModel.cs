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

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class HqPageViewModel : ViewModelBase
    {
        #region Private

        Settings settings;
        HqProfile hqProfile;

        private IItemGeneratorService itemGenerator;
        private IQRService qRService;
        private DelegateCommand navigateToQRScannerPageCommand;
        private DelegateCommand giveCommand;

        #endregion

        #region Public

        public DelegateCommand NavigateToQRScannerPageCommand => navigateToQRScannerPageCommand ?? (navigateToQRScannerPageCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.QRScannerPage); }, CanSubmit).ObservesProperty(() => IsBusy));
        public DelegateCommand GiveCommand => giveCommand ?? (giveCommand = new DelegateCommand(async () => { IsBusy = true; await Give(); }, CanSubmit).ObservesProperty(() => IsBusy));

        #endregion

        #region Ctor

        public HqPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService, IQRService qrService) : base(navigationService, dataRepository, dialogService)
        {
            itemGenerator = itemGeneratorService;
            qRService = qrService;
            Title = "HQ";
        }

        #endregion

        #region Properties

        private ObservableCollection<Tuple<string, int>> teams = new ObservableCollection<Tuple<string, int>>();

        public ObservableCollection<Tuple<string, int>> Teams
        {
            get { return teams; }
            set { SetProperty(ref teams, value); }
        }

        private int exp;

        public int Exp
        {
            get { return exp; }
            set { SetProperty(ref exp, value); }
        }

        private int honor;

        public int Honor
        {
            get { return honor; }
            set { SetProperty(ref honor, value); }
        }

        private int money;

        public int Money
        {
            get { return money; }
            set { SetProperty(ref money, value); }
        }
        
        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsBusy = true;

            Init();

            IsBusy = false;
        }

        public override async Task ToSettings()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("mode", "npc");

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        #endregion

        #region Private functions

        private void Init()
        {
            settings = DataRepository.GetSettings();

            if (settings.AppMode == AppModeEnum.None)
            {
                settings.AppMode = AppModeEnum.Hq;
                DataRepository.SaveToFile(settings);
            }
            DataRepository.CreateHqProfile();

            hqProfile = DataRepository.GetHqProfile();

            Teams = new ObservableCollection<Tuple<string, int>>(hqProfile.Scores.OrderByDescending(x  => x.Item2));
        }

        private async Task Give()
        {
            HqReward hqReward = new HqReward(Exp, Honor, Money);

            var qrCode = qRService.CreateQR(hqReward);
            NavigationParameters param = new NavigationParameters();
            param.Add("code", qrCode);

            await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
        }

        #endregion

    }
}
