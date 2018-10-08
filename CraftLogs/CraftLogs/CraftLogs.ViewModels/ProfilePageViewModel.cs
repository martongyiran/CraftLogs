using CraftLogs.Repositories.Local;
using CraftLogs.Services;
using CraftLogs.Values;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        public ProfilePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.ProfilePage;
        }
    }
}
