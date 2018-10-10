using Prism.Commands;
using Prism.Navigation;
using Plugin.VersionTracking;
using CraftLogs.Values;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Services;
using CraftLogs.BLL.Services.Interfaces;
using System.Collections.Generic;
using CraftLogs.BLL.Models;

namespace CraftLogs.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private
        private string version;
        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToLogsCommand;
        private readonly IItemGeneratorService itemGeneratorService;
        #endregion

        #region Public
        public string Version => version ?? (version = string.Format(Texts.Version, CrossVersionTracking.Current.CurrentVersion));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.SettingsPage)));
        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(() => NavigateTo(NavigationLinks.LogsPage)));
        #endregion

        #region Ctor
        public MainPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.MainPage;
            this.itemGeneratorService = itemGeneratorService;
        }
        #endregion

        #region Overrides

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //Temporary file cration for testing. 
            SetUpFileSystem();
            var items = GenerateTestItems();
        }

        #endregion

        #region Private functions

        private void SetUpFileSystem()
        {
            DataRepository.CreateSettings();
            DataRepository.DeleteFile(FileNames.Logs);
            DataRepository.CreateLogs();
        }

        private List<Item> GenerateTestItems()
        {
            List<Item> res = new List<Item>();
            for (int i = 0; i < 21; i++)
            {
                res.Add(itemGeneratorService.GenerateRandom());
            }

            return res;
        }

        #endregion
    }
}
