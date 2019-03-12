using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
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

        public DelegateCommand RateCommand => rateCommand ?? (rateCommand = new DelegateCommand(async () => await RateAsync()));

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

        public List<int> Numbers { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private int rating = 4;

        public int Rating
        {
            get { return rating; }
            set { SetProperty(ref rating, value); }
        }

        #endregion

        #region Private functions

        private async Task RateAsync()
        {
            profile.AvgScore.Add(Rating + 1);
            DataRepository.SaveToFile(profile);
            NavigationParameters param = new NavigationParameters();
            param.Add("code", qrCode);
            await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
        }

        #endregion
    }
}
