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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
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
        
        #endregion
    }
}
