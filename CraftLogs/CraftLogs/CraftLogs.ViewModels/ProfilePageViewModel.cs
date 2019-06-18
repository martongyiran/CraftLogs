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

namespace CraftLogs.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {

        #region Private

        private TeamProfile teamProfile;

        private DelegateCommand navigateToLogsCommand;
        private DelegateCommand navigateToSettingsCommand;

        #endregion

        #region Public

        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.LogsPage)));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.SettingsPage)));

        #endregion

        #region Ctor

        public ProfilePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
            Title = null;
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            teamProfile = DataRepository.GetTeamProfile();

            if (string.IsNullOrEmpty(Title))
            {
                Title = Texts.ProfilePage + " - " + teamProfile.Name;
            }
        }

        #endregion
    }
}
