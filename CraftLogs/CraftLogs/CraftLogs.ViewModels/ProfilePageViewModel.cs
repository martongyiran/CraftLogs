using CraftLogs.Repositories.Local;
using CraftLogs.Values;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        public ProfilePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository) : base(navigationService, dataRepository)
        {
            Title = Texts.ProfilePage;
        }
    }
}
