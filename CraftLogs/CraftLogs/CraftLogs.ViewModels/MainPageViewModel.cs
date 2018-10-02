using CraftLogs.Repositories.Local;
using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string version;
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }
        public DelegateCommand NavigateToProfileCommand { get; private set; }

        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository)
            : base(navigationService, dataRepository)
        {
            Title = "Landing Page";
            NavigateToProfileCommand = new DelegateCommand(NavigateToProfile);
            Title = DataRepository.GetLogsAsync();
            Version = string.Format("Version: {0}", CrossVersionTracking.Current.CurrentVersion);
        }

        public async void NavigateToProfile()
        {
            await NavigationService.NavigateAsync(Values.NavigationLinks.ProfilePage);
        }

    }
}
