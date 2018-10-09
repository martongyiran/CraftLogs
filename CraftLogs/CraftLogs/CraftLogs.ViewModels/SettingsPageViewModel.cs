using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        #region Private

        private Settings settings;
        private int craftDay;        
        private int craft1Start;        
        private int craft2Start;        
        private int craft1MinPont;        
        private int craft2MinPont;        
        private int craft1QuestCount;       
        private int craft2QuestCount;
        private DelegateCommand saveSettingsCommand;
        private DelegateCommand resetSettingsCommand;

        #endregion

        #region Public

        public int CraftDay
        {
            get { return craftDay; }
            set { SetProperty(ref craftDay, value); }
        }

        public int Craft1Start
        {
            get { return craft1Start; }
            set { SetProperty(ref craft1Start, value); }
        }

        public int Craft2Start
        {
            get { return craft2Start; }
            set { SetProperty(ref craft2Start, value); }
        }

        public int Craft1MinPont
        {
            get { return craft1MinPont; }
            set { SetProperty(ref craft1MinPont, value); }
        }

        public int Craft2MinPont
        {
            get { return craft2MinPont; }
            set { SetProperty(ref craft2MinPont, value); }
        }

        public int Craft1QuestCount
        {
            get { return craft1QuestCount; }
            set { SetProperty(ref craft1QuestCount, value); }
        }

        public int Craft2QuestCount
        {
            get { return craft2QuestCount; }
            set { SetProperty(ref craft2QuestCount, value); }
        }

        public DelegateCommand SaveSettingsCommand => saveSettingsCommand ?? (saveSettingsCommand = new DelegateCommand(SaveSettings));
        public DelegateCommand ResetSettingsCommand => resetSettingsCommand ?? (resetSettingsCommand = new DelegateCommand(ResetSettingsAsync));
        #endregion

        #region Ctor
        public SettingsPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.SettingsPage;
        }
        #endregion

        #region Overrides
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetUp();
        }

        #endregion

        #region Private functions

        private void SetUp()
        {
            settings = DataRepository.GetSettings();

            CraftDay = settings.CraftDay;
            Craft1Start = settings.Craft1Start;
            Craft2Start = settings.Craft2Start;
            Craft1MinPont = settings.Craft1MinPont;
            Craft2MinPont = settings.Craft2MinPont;
            Craft1QuestCount = settings.Craft1QuestCount;
            Craft2QuestCount = settings.Craft2QuestCount;
        }

        private void SaveSettings()
        {
            settings.CraftDay = CraftDay;
            settings.Craft1Start = Craft1Start;
            settings.Craft2Start = Craft2Start;
            settings.Craft1MinPont = Craft1MinPont;
            settings.Craft2MinPont = Craft2MinPont;
            settings.Craft1QuestCount = Craft1QuestCount;
            settings.Craft2QuestCount = Craft2QuestCount;

            DataRepository.SaveToFile(settings);
            DialogService.DisplayAlertAsync("", Texts.SuccessfulSaving, Texts.Ok);
        }

        private async void ResetSettingsAsync()
        {
            var res = await DialogService.DisplayAlertAsync("", Texts.ResetData, Texts.Yes, Texts.No);
            if (res)
            {
                DataRepository.ResetSettings();
                SetUp();
            }
        }

        private void DeleteProfile()
        {
            //TODO
        }

        private void FinalScore()
        {
            //TODO
        }

        #endregion


    }
}
