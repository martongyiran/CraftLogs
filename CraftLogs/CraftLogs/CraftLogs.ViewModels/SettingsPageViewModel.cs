using CraftLogs.Repositories.Local;
using CraftLogs.Values;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository) : base(navigationService, dataRepository)
        {
            Title = Texts.SettingsPage;
        }
    }
}
