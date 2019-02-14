using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;
using CraftLogs.Values;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Services;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Enums;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private

        private Settings settings;
        private string version;
        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToLogsCommand;
        private DelegateCommand clearModeCommand; //for testing
        private AppModeEnum mode;
        private bool hqMenuVisibility = false;
        private bool teamMenuVisibility = false;
        private bool questMenuVisibility = false;
        private bool shopMenuVisibility = false;
        private bool arenaMenuVisibility = false;

        #endregion

        #region Public

        public string Version => version ?? (version = string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.SettingsPage)));
        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(async () => await NavigateTo(NavigationLinks.LogsPage)));
        public DelegateCommand ClearModeCommand => clearModeCommand ?? (clearModeCommand = new DelegateCommand(async () => await ClearMode()));

        public AppModeEnum Mode
        {
            get { return mode; }
            set { SetProperty(ref mode, value); }
        }

        public bool HqMenuVisibility
        {
            get { return hqMenuVisibility; }
            set { SetProperty(ref hqMenuVisibility, value); }
        }

        public bool TeamMenuVisibility
        {
            get { return teamMenuVisibility; }
            set { SetProperty(ref teamMenuVisibility, value); }
        }

        public bool QuestMenuVisibility
        {
            get { return questMenuVisibility; }
            set { SetProperty(ref questMenuVisibility, value); }
        }

        public bool ShopMenuVisibility
        {
            get { return shopMenuVisibility; }
            set { SetProperty(ref shopMenuVisibility, value); }
        }

        public bool ArenaMenuVisibility
        {
            get { return arenaMenuVisibility; }
            set { SetProperty(ref arenaMenuVisibility, value); }
        }

        #endregion

        #region Ctor

        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.MainPage;
        }

        #endregion

        #region Overrides

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            SetUpFileSystem();

            settings = DataRepository.GetSettings();

            if (settings.AppMode == AppModeEnum.None)
                await NavigateTo(NavigationLinks.SelectModePage);

            Mode = settings.AppMode;
            SetUpVisibility();
        }

        #endregion

        #region Private functions

        private void SetUpFileSystem()
        {
            DataRepository.CreateSettings();
            DataRepository.DeleteFile(FileNames.Logs); //I create a lot of test logs in LogsPageViewModel, so it's necessary to clear it at every start. It's only for testing.
            DataRepository.CreateLogs();
        }

        private void SetUpVisibility()
        {
            ResetVisibility();
            switch (Mode)
            {
                case AppModeEnum.None:
                    HqMenuVisibility = false;
                    break;
                case AppModeEnum.Team:
                    TeamMenuVisibility = true;
                    break;
                case AppModeEnum.Quest:
                    QuestMenuVisibility = true;
                    break;
                case AppModeEnum.Shop:
                    ShopMenuVisibility = true;
                    break;
                case AppModeEnum.Arena:
                    ArenaMenuVisibility = true;
                    break;
                case AppModeEnum.Hq:
                    HqMenuVisibility = true;
                    break;
                default:
                    break;
            }
        }

        private void ResetVisibility()
        {
            HqMenuVisibility = false;
            TeamMenuVisibility = false;
            QuestMenuVisibility = false;
            ShopMenuVisibility = false;
            ArenaMenuVisibility = false;
    }

        //for testing
        private async Task ClearMode()
        {
            settings.AppMode = AppModeEnum.None;
            DataRepository.SaveToFile(settings);
            await NavigateTo(NavigationLinks.SelectModePage);
        }

        #endregion
    }
}
