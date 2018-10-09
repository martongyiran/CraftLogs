using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;
using CraftLogs.Values;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private
        private string version;
        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToLogsCommand;
        #endregion

        #region Public
        public string Version => version ?? (version = string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.SettingsPage)));
        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.LogsPage)));
        #endregion

        #region Ctor
        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.MainPage;
        }
        #endregion

        #region Overrides

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //Temporary file cration for testing. 
            SetUpFileSystem();
        }

        #endregion

        #region Private functions

        private void SetUpFileSystem()
        {
            DataRepository.CreateSettings();
            DataRepository.DeleteFile(FileNames.Logs);
            DataRepository.CreateLogs();
        }

        #endregion
    }
}
