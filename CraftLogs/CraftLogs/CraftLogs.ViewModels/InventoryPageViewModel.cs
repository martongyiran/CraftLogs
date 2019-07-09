using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class InventoryPageViewModel : ViewModelBase
    {

        #region Private

        private DelegateCommand<object> itemTappedCommand;

        private DelegateCommand sellTappedCommand;

        #endregion

        #region Public

        public DelegateCommand<object> ItemTappedCommand => itemTappedCommand ?? (itemTappedCommand = new DelegateCommand<object>((a) => ItemTapped(a)));

        public DelegateCommand SellTappedCommand => sellTappedCommand ?? (sellTappedCommand = new DelegateCommand(SellTapped));

        #endregion

        #region ctor

        public InventoryPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.InventoryPage;
        }

        #endregion

        #region Properties

        private ObservableCollection<Item> allItems = new ObservableCollection<Item>();

        public ObservableCollection<Item> AllItems
        {
            get { return allItems; }
            set { SetProperty(ref allItems, value); }
        }

        private ObservableCollection<Item> selectedItems = new ObservableCollection<Item>();

        public ObservableCollection<Item> SelectedItems
        {
            get { return selectedItems; }
            set { SetProperty(ref selectedItems, value); }
        }

        public List<ItemTypeEnum> PickerValues { get; set; } = new List<ItemTypeEnum>() { ItemTypeEnum.All, ItemTypeEnum.Armor, ItemTypeEnum.OneHand, ItemTypeEnum.TwoHand, ItemTypeEnum.Trinket };

        private ItemTypeEnum selectedItem;

        public ItemTypeEnum SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); FilterList(); }
        }

        private Item activeItem;

        public Item ActiveItem
        {
            get { return activeItem; }
            set { SetProperty(ref activeItem, value); }
        }

        private bool noItem;

        public bool NoItem
        {
            get { return noItem; }
            set { SetProperty(ref noItem, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Init();
        }

        #endregion

        #region Private functions

        private void Init()
        {
            AllItems = DataRepository.GetTeamProfile().Inventory;
            SelectedItem = ItemTypeEnum.All;
        }

        private void FilterList()
        {
            if(SelectedItem == ItemTypeEnum.All)
            {
                SelectedItems = new ObservableCollection<Item>(AllItems);
            }
            else
            {
                SelectedItems = new ObservableCollection<Item>(AllItems.Where((arg) => arg.ItemType == SelectedItem).ToList());
            }

            NoItem = SelectedItems.Count == 0;
        }

        private void ItemTapped(object o)
        {
            ActiveItem = o as Item;
        }

        private void SellTapped()
        {
            var profile = DataRepository.GetTeamProfile();
            foreach(var item in AllItems.ToList())
            {
                if(item.Id == ActiveItem.Id)
                {
                    AllItems.Remove(item);
                    profile.Money += item.Value;
                }
            }

            profile.Inventory = AllItems;
            DataRepository.SaveToFile(profile);
            Init();
        }


        #endregion

    }
}
