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

using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class QRScannerPageViewModel : ViewModelBase
    {
        #region Public

        public DelayCommand<string> GetResultCommand => new DelayCommand<string>(async (a) => await HandleResult(a));
        
        #endregion

        #region ctor

        public QRScannerPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = "QR Scanner Page";
        }

        #endregion

        private async Task HandleResult(string text)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("res", text);
            await NavigateToWithoutHistory(NavigationLinks.QRHandlerPage, param);
        }
    }
}
