using System.Threading.Tasks;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class SelectModePageViewModel : ViewModelBase
    {
        #region Private
        private DelegateCommand setModeToTeamCommand;
        private DelegateCommand setModeToQuestCommand;
        private DelegateCommand setModeToShopCommand;
        private DelegateCommand setModeToArenaCommand;
        private Settings settings;
        #endregion

        #region Public

        public DelegateCommand SetModeToTeamCommand => setModeToTeamCommand ?? (setModeToTeamCommand = new DelegateCommand(async () => await SetMode(AppModeEnum.Team)));
        public DelegateCommand SetModeToQuestCommand => setModeToQuestCommand ?? (setModeToQuestCommand = new DelegateCommand(async () => await SetMode(AppModeEnum.Quest)));
        public DelegateCommand SetModeToShopCommand => setModeToShopCommand ?? (setModeToShopCommand = new DelegateCommand(async () => await SetMode(AppModeEnum.Shop)));
        public DelegateCommand SetModeToArenaCommand => setModeToArenaCommand ?? (setModeToArenaCommand = new DelegateCommand(async () => await SetMode(AppModeEnum.Arena)));

        #endregion

        #region Ctor

        public SelectModePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
        }

        #endregion

        #region Overrides

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            settings = DataRepository.GetSettings();

            if (settings.AppMode != AppModeEnum.None)
                await NavigationService.GoBackToRootAsync();
        }

        #endregion

        #region Private functions

        private async Task SetMode(AppModeEnum appMode)
        {
            settings.AppMode = appMode;
            DataRepository.SaveToFile(settings);
            //Only for testing. I should change it to s switch and navigate to the specific page based on the enum.
            await NavigationService.GoBackToRootAsync();
        }

        #endregion
    }
}
