using CraftLogs.Repositories.Local;
using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;
using CraftLogs.Values;
using CraftLogs.Services;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private
        private string version;
        private DelegateCommand navigateToSettingsCommand;
        #endregion

        #region Public
        public string Version => version ?? (version = string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.SettingsPage)));
        #endregion

        #region Ctor
        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
        }
        #endregion

        #region Overrides

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Title = Texts.MainPage;
            //Temporary file cration for testing. 
            DataRepository.CreateSettings();
        }

        #endregion
    }
}
