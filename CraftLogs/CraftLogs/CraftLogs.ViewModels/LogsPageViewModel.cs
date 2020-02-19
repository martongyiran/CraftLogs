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
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;

namespace CraftLogs.ViewModels
{
    public class LogsPageViewModel : ViewModelBase
    {
        private int _numberOfVisibleLogs = 20;
        private bool _footerIsVisible;
        private ObservableCollection<Log> _logs;
        private ObservableCollection<Log> _filteredLogsList;

        public ObservableCollection<Log> FilteredLogsList
        {
            get => _filteredLogsList;
            set => SetProperty(ref _filteredLogsList, value);
        }

        public bool FooterIsVisible
        {
            get => _footerIsVisible;
            set => SetProperty(ref _footerIsVisible, value);
        }

        public DelayCommand LoadLogsCommand => new DelayCommand(LoadLogs);

        public LogsPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.Logs_Title;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetUp();
        }

        private void SetUp()
        {
            _logs = DataRepository.GetLogs();
            LoadLogs();
        }

        private void LoadLogs()
        {
            if (_logs.Count > _numberOfVisibleLogs)
            {
                FilteredLogsList = new ObservableCollection<Log>();
                for (int i = 0; i < _numberOfVisibleLogs; i++)
                {
                    FilteredLogsList.Add(_logs[i]);
                }
                _numberOfVisibleLogs += 20;
                FooterIsVisible = true;
            }
            else
            {
                FooterIsVisible = false;
                FilteredLogsList = _logs.Count != 0 ? new ObservableCollection<Log>(_logs) : null;
            }
        }
    }
}
