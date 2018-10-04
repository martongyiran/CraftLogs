using CraftLogs.Repositories.Local;
using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;
using CraftLogs.Values;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private
        private string version;
        #endregion

        #region Public
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }

        public DelegateCommand NavigateToProfileCommand { get; private set; }
        public DelegateCommand NavigateToSettingsCommand { get; private set; }
        public DelegateCommand NavigateToQuestCommand { get; private set; }
        #endregion

        #region Ctor
        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository)
            : base(navigationService, dataRepository)
        {
            NavigateToProfileCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.ProfilePage));
            NavigateToSettingsCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.SettingsPage));
            NavigateToQuestCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.QuestPage));
            Title = DataRepository.GetLogsAsync();
            Version = string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion);
        }
        #endregion

        #region Private functions
        private async void NavigateTo(string navigationLink)
        {
            await NavigationService.NavigateAsync(navigationLink);
        }
        #endregion
    }
}
