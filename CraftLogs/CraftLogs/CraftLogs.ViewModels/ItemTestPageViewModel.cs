using System;
using System.Collections.ObjectModel;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class ItemTestPageViewModel : ViewModelBase
    {
        private readonly IItemGeneratorService ItemGeneratorService;

        private ObservableCollection<Item> itemsList;

        public ObservableCollection<Item> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value); }
        }

        public ItemTestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService) : base(navigationService, dataRepository, dialogService)
        {
            ItemGeneratorService = itemGeneratorService;
            Title = "Item Generator Test Page";
            ItemsList = new ObservableCollection<Item>();

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            ItemsList = GenerateTestItems();

        }

        private ObservableCollection<Item> GenerateTestItems()
        {
            ObservableCollection<Item> res = new ObservableCollection<Item>();
            for (int i = 1; i < 20; i++)
            {
                res.Add(ItemGeneratorService.GenerateRandom());
            }

            return res;
        }

    }
}