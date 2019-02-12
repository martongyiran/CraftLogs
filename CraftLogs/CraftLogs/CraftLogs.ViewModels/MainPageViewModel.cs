using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;
using CraftLogs.Values;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Services;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Enums;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private

        private Settings settings;
        private string version;
        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToLogsCommand;
        private DelegateCommand clearModeCommand; //for testing
        private AppModeEnum mode;
        private bool devMenuVisibility = false;

        #endregion

        #region Public

        public string Version => version ?? (version = string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.SettingsPage)));
        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.LogsPage)));
        public DelegateCommand ClearModeCommand => clearModeCommand ?? (clearModeCommand = new DelegateCommand(() => ClearMode()));

        public AppModeEnum Mode
        {
            get { return mode; }
            set { SetProperty(ref mode, value); }
        }

        public bool DevMenuVisibility
        {
            get { return devMenuVisibility; }
            set { SetProperty(ref devMenuVisibility, value); }
        }

        #endregion

        #region Ctor

        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.MainPage;
        }

        #endregion

        #region Overrides

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //Temporary file cration for testing. 
            SetUpFileSystem();

            settings = DataRepository.GetSettings();

            if (settings.AppMode == AppModeEnum.None)
                await NavigateTo(NavigationLinks.SelectModePage);

            Mode = settings.AppMode;
            SetUpVisibility();
        }

        #endregion

        #region Private functions

        private void SetUpFileSystem()
        {
            DataRepository.CreateSettings();
            DataRepository.DeleteFile(FileNames.Logs); //I create a lot of test logs in LogsPageViewModel, so it's necessary to clear it at every start. It's only for testing.
            DataRepository.CreateLogs();
        }

        private void SetUpVisibility()
        {
            DevMenuVisibility = Mode == AppModeEnum.None ? false : true;
        }

        //for testing
        private void ClearMode()
        {
            settings.AppMode = AppModeEnum.None;
            DataRepository.SaveToFile(settings);
            DialogService.DisplayAlertAsync("Figyelem", "Kérlek indítsd újra az alkalmazást!", "OK");
        }
        #endregion
    }
}
