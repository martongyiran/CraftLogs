using CraftLogs.Repositories.Local;
using CraftLogs.Values;
using Prism.Navigation;

namespace CraftLogs.ViewModels
{
    public class QuestPageViewModel : ViewModelBase
    {
        public QuestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository) : base(navigationService, dataRepository)
        {
            Title = Texts.QuestPage;
        }
    }
}
