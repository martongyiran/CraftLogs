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
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftLogs.ViewModels
{
    public class HqPageViewModel : ViewModelBase
    {
        #region Private

        Settings settings;

        private IItemGeneratorService itemGenerator;
        private IQRService qRService;

        #endregion

        #region Ctor

        public HqPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService, IQRService qrService) : base(navigationService, dataRepository, dialogService)
        {
            itemGenerator = itemGeneratorService;
            qRService = qrService;
            Title = "HQ";
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
        }

        #endregion

    }
}
