using Prism.Commands;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToProfileCommand { get; private set; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Landing Page";
            NavigateToProfileCommand = new DelegateCommand(NavigateToProfile);
        }

        public async void NavigateToProfile()
        {
            await NavigationService.NavigateAsync(Values.NavigationLinks.ProfilePage);
        }

    }
}
