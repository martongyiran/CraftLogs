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
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class QRHandlerViewModel : ViewModelBase
    {
        #region Private

        private string response;

        #endregion

        #region Public

        public string Response
        {
            get { return response; }
            set { SetProperty(ref response, value); }
        }

        #endregion


        #region Ctor

        public QRHandlerViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = "QR Handler Page";
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var lul = parameters["res"] as string;
            Response = lul != null ? lul : "none";
        }

        #endregion

    }
}
