using CraftLogs.Repositories.Local;
using Prism.Commands;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToProfileCommand { get; private set; }

        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository)
            : base(navigationService, dataRepository)
        {
            Title = "Landing Page";
            NavigateToProfileCommand = new DelegateCommand(NavigateToProfile);
            Title = DataRepository.GetLogsAsync();
        }

        public async void NavigateToProfile()
        {
            await NavigationService.NavigateAsync(Values.NavigationLinks.ProfilePage);
        }

    }
}
