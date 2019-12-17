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
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace CraftLogs.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {

        private string _title;
        private bool _isBusy = false;

        public DelayCommand NavigateToSettingsCommand => new DelayCommand(async () => await ToSettings());

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

#if DEV
        public string Version { get { return string.Format(Texts.Version, VersionTracking.CurrentVersion) + " DEV"; } }
#elif STG
        public string Version { get { return string.Format(Texts.Version, VersionTracking.CurrentVersion) + " STG"; } }
#elif PRD
        public string Version { get { return string.Format(Texts.Version, VersionTracking.CurrentVersion); } }
#endif

        protected INavigationService NavigationService { get; private set; }
        protected ILocalDataRepository DataRepository { get; private set; }
        protected IPageDialogService DialogService { get; private set; }

        public ViewModelBase(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            DataRepository = dataRepository;
            DialogService = dialogService;
        }

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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task ToSettings()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

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
    }
}
