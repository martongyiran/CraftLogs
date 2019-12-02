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
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Plugin.VersionTracking;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        #region Private

        private string title;
        private bool isBusy = false;

        #endregion

        #region Public

        public DelayCommand NavigateToSettingsCommand => new DelayCommand(async () => await ToSettings());

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

#if DEV
        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion) + " DEV"; } }
#elif STG
        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion) + " STG"; } }
#elif PRD
        public string Version { get { return string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion); } }
#endif

        #endregion

        #region Services

        protected INavigationService NavigationService { get; private set; }
        protected ILocalDataRepository DataRepository { get; private set; }
        protected IPageDialogService DialogService { get; private set; }

        #endregion

        #region Ctor

        public ViewModelBase(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            DataRepository = dataRepository;
            DialogService = dialogService;
        }

        #endregion

        #region Virtual

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = false;
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
            IsBusy = false;
        }

        public virtual void Destroy()
        {

        }

        public virtual async Task ToSettings()
        {
        }

        #endregion

        #region Protected functions

        protected async Task NavigateTo(string navigationLink)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync(navigationLink);
        }

        protected async Task NavigateTo(string navigationLink, INavigationParameters parameters)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync(navigationLink, parameters);
        }

        protected async Task NavigateToWithoutHistory(string navigationLink)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("../" + navigationLink);
        }

        protected async Task NavigateToWithoutHistory(string navigationLink, INavigationParameters parameters)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("../" + navigationLink, parameters);
        }

        protected async Task NavigateToWithoutHistoryDouble(string navigationLink)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("../../" + navigationLink);
        }

        protected async Task NavigateToWithoutHistoryDouble(string navigationLink, INavigationParameters parameters)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("../../" + navigationLink, parameters);
        }

        protected async Task NavigateBack()
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("../");
        }

        #endregion
    }
}
