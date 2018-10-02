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
        #endregion

        #region Ctor
        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository)
            : base(navigationService, dataRepository)
        {
            NavigateToProfileCommand = new DelegateCommand(NavigateToProfile);
            Title = DataRepository.GetLogsAsync();
            Version = string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion);
        }
        #endregion

        #region Public functions
        public async void NavigateToProfile()
        {
            await NavigationService.NavigateAsync(NavigationLinks.ProfilePage);
        }
        #endregion
    }
}
