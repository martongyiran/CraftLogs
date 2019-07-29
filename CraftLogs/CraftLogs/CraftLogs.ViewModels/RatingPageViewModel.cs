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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class RatingPageViewModel : ViewModelBase
    {

        #region Private

        private QuestProfile profile;
        private string qrCode;

        private DelegateCommand rateCommand;

        #endregion

        #region Public

        public DelegateCommand RateCommand => rateCommand ?? (rateCommand = new DelegateCommand(async () => await RateAsync(), CanSubmit).ObservesProperty(()=>IsBusy));

        #endregion

        #region Ctor

        public RatingPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.RatingTitle;
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            profile = DataRepository.GetQuestProfile();

            qrCode = parameters["code"] as string;
        }

        #endregion

        #region Properties

        public List<int> Numbers { get; set; } = Enumerable.Range(1, 10).ToList();

        private int rating = 5;

        public int Rating
        {
            get { return rating; }
            set { SetProperty(ref rating, value); }
        }

        #endregion

        #region Private functions

        private async Task RateAsync()
        {
            IsBusy = true;
            profile.AvgScore.Add(Rating);
            DataRepository.SaveToFile(profile);
            NavigationParameters param = new NavigationParameters();
            param.Add("code", qrCode);
            await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
        }

        #endregion
    }
}
