using CraftLogs.Repositories.Local;
using CraftLogs.Values;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        #region Properties

        private int craftDay;

        public int CraftDay
        {
            get { return craftDay; }
            set { SetProperty(ref craftDay, value); }
        }

        private int craft1Start;

        public int Craft1Start
        {
            get { return craft1Start; }
            set { SetProperty(ref craft1Start, value); }
        }

        private int craft2Start;

        public int Craft2Start
        {
            get { return craft2Start; }
            set { SetProperty(ref craft2Start, value); }
        }

        private int craft1MinPont;

        public int Craft1MinPont
        {
            get { return craft1MinPont; }
            set { SetProperty(ref craft1MinPont, value); }
        }

        private int craft2MinPont;

        public int Craft2MinPont
        {
            get { return craft2MinPont; }
            set { SetProperty(ref craft2MinPont, value); }
        }

        private int craft1QuestCount;

        public int Craft1QuestCount
        {
            get { return craft1QuestCount; }
            set { SetProperty(ref craft1QuestCount, value); }
        }

        private int craft2QuestCount;

        public int Craft2QuestCount
        {
            get { return craft2QuestCount; }
            set { SetProperty(ref craft2QuestCount, value); }
        }

        #endregion
        public SettingsPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository) : base(navigationService, dataRepository)
        {
            Title = Texts.SettingsPage;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetUp();
        }

        private void SetUp()
        {
            CraftDay = 123;
            Craft1Start = 456;
            Craft2Start = 789;
            Craft1MinPont = 101;
            Craft2MinPont = 112;
            Craft1QuestCount = 131;
            Craft2QuestCount = 415;
        }
    }
}
