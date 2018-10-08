using CraftLogs.Models;
using CraftLogs.Repositories.Local;
using CraftLogs.Services;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using System;

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
        public DelegateCommand ResetSettingsCommand => resetSettingsCommand ?? (resetSettingsCommand = new DelegateCommand(ResetSettings));
        #endregion

        #region Ctor
        public SettingsPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
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

            CraftDay = settings.craftDay;
            Craft1Start = settings.craft1Start;
            Craft2Start = settings.craft2Start;
            Craft1MinPont = settings.craft1MinPont;
            Craft2MinPont = settings.craft2MinPont;
            Craft1QuestCount = settings.craft1QuestCount;
            Craft2QuestCount = settings.craft2QuestCount;

            Title = Texts.SettingsPage;
        }

        private void SaveSettings()
        {
            settings.craftDay = CraftDay;
            settings.craft1Start = Craft1Start;
            settings.craft2Start = Craft2Start;
            settings.craft1MinPont = Craft1MinPont;
            settings.craft2MinPont = Craft2MinPont;
            settings.craft1QuestCount = Craft1QuestCount;
            settings.craft2QuestCount = Craft2QuestCount;

            DataRepository.SaveSettingsToFile(settings);
            DialogService.DisplayAlert("", Texts.SuccessfulSaving, Texts.Ok);
        }

        private async void ResetSettings()
        {
            var res = await DialogService.DisplayAlert("", Texts.ResetData, Texts.Yes, Texts.No);
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
