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
            res.Add(itemGeneratorService.GenerateBoots(1, BLL.Enums.ItemRarityEnum.Common));
            res.Add(itemGeneratorService.GenerateBoots(1, BLL.Enums.ItemRarityEnum.Uncommon));
            res.Add(itemGeneratorService.GenerateBoots(1, BLL.Enums.ItemRarityEnum.Rare));
            res.Add(itemGeneratorService.GenerateBoots(1, BLL.Enums.ItemRarityEnum.Epic));

            res.Add(itemGeneratorService.GenerateBoots(2, BLL.Enums.ItemRarityEnum.Common));
            res.Add(itemGeneratorService.GenerateBoots(2, BLL.Enums.ItemRarityEnum.Uncommon));
            res.Add(itemGeneratorService.GenerateBoots(2, BLL.Enums.ItemRarityEnum.Rare));
            res.Add(itemGeneratorService.GenerateBoots(2, BLL.Enums.ItemRarityEnum.Epic));

            res.Add(itemGeneratorService.GenerateBoots(3, BLL.Enums.ItemRarityEnum.Common));
            res.Add(itemGeneratorService.GenerateBoots(3, BLL.Enums.ItemRarityEnum.Uncommon));
            res.Add(itemGeneratorService.GenerateBoots(3, BLL.Enums.ItemRarityEnum.Rare));
            res.Add(itemGeneratorService.GenerateBoots(3, BLL.Enums.ItemRarityEnum.Epic));

            return res;
        }

        #endregion
    }
}
