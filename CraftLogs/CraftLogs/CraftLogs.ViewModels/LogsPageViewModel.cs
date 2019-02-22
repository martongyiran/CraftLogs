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
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class LogsPageViewModel : ViewModelBase
    {
        #region Private

        private ObservableCollection<Log> logs;
        private int numberOfVisibleLogs = 20;
        private ObservableCollection<Log> filteredLogsList;
        private bool footerIsVisible;
        private bool headerIsVisible;
        private DelegateCommand loadLogsCommand;
        private readonly ILoggerService loggerService;

        #endregion

        #region Public

        public ObservableCollection<Log> FilteredLogsList
        {
            get { return filteredLogsList; }
            set { SetProperty(ref filteredLogsList, value); }
        }

        public bool FooterIsVisible
        {
            get { return footerIsVisible; }
            set { SetProperty(ref footerIsVisible, value); }
        }

        public bool HeaderIsVisible
        {
            get { return headerIsVisible; }
            set { SetProperty(ref headerIsVisible, value); }
        }

        public DelegateCommand LoadLogsCommand => loadLogsCommand ?? (loadLogsCommand = new DelegateCommand(LoadLogs));

        #endregion

        #region Ctor

        public LogsPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, ILoggerService loggerService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.LogsPage;
            this.loggerService = loggerService;
        }

        #endregion

        #region Overrides

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await CreateMock(); //for testing
            SetUp();
        }

        #endregion

        #region Private functions

        private void SetUp()
        {
            logs = DataRepository.GetLogs();
            LoadLogs();
        }

        private void LoadLogs()
        {
            IsBusy = true;

            HeaderIsVisible = logs.Count == 0;
            if (logs.Count > numberOfVisibleLogs)
            {
                FilteredLogsList = new ObservableCollection<Log>();
                for (int i = 0; i < numberOfVisibleLogs; i++)
                {
                    FilteredLogsList.Add(logs[i]);
                }
                numberOfVisibleLogs += 20;
                FooterIsVisible = true;
            }
            else
            {
                FooterIsVisible = false;
                FilteredLogsList = logs;
            }

            IsBusy = false;
        }

        private async Task CreateMock()
        {
            IsBusy = true;
            ObservableCollection<Item> items = new ObservableCollection<Item>();
            items.Add(new Item(1, ItemRarityEnum.Common, ItemTypeEnum.Armor, CharacterClassEnum.Warrior, "0 4 2 0 0"));
            items.Add(new Item(2, ItemRarityEnum.Rare, ItemTypeEnum.TwoHand, CharacterClassEnum.Mage, "5 0 0 3 0"));
            items.Add(new Item(3, ItemRarityEnum.Legendary, ItemTypeEnum.Trinket, CharacterClassEnum.Rogue, "5 5 5 5 5"));

            for (int i = 0; i < 5; i++)
            {
                loggerService.CreateArenaLog("első log");
                await Task.Delay(500);
                loggerService.CreateBuyLog(7, new System.Collections.Generic.List<Item>());
                await Task.Delay(500);
                loggerService.CreateQueustLog(6, new QuestReward("Mocsári veszedelem", 40, 100, 2, items));
                await Task.Delay(500);
                loggerService.CreateSellLog(new Item());
                await Task.Delay(500);
                loggerService.CreateSystemLog("fasz");
                await Task.Delay(500);
                loggerService.CreateTradeLog("picspat");
                await Task.Delay(500);
            }
            IsBusy = false;
        }

        #endregion
    }
}
