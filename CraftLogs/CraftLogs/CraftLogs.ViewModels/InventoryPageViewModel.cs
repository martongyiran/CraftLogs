/*
Copyright 2019 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class InventoryPageViewModel : ViewModelBase
    {

        #region Private

        private ILoggerService logger;

        private DelegateCommand<object> itemTappedCommand;

        private DelegateCommand sellTappedCommand;

        private DelegateCommand useTappedCommand;

        #endregion

        #region Public

        public DelegateCommand<object> ItemTappedCommand => itemTappedCommand ?? (itemTappedCommand = new DelegateCommand<object>((a) => ItemTapped(a)));

        public DelegateCommand SellTappedCommand => sellTappedCommand ?? (sellTappedCommand = new DelegateCommand(async () => await SellTapped()));

        public DelegateCommand UseTappedCommand => useTappedCommand ?? (useTappedCommand = new DelegateCommand(async () => await UseTapped()));

        #endregion

        #region ctor

        public InventoryPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, ILoggerService loggerService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.InventoryPage;
            logger = loggerService;
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

        public List<ItemTypeEnum> Picker1Values { get; set; } = new List<ItemTypeEnum>() { ItemTypeEnum.All, ItemTypeEnum.Armor, ItemTypeEnum.LHand, ItemTypeEnum.RHand, ItemTypeEnum.Neck, ItemTypeEnum.Ring };

        private ItemTypeEnum selectedItemType;

        public ItemTypeEnum SelectedItemType
        {
            get { return selectedItemType; }
            set { SetProperty(ref selectedItemType, value); FilterList(); }
        }

        public List<CharacterClassEnum> Picker2Values { get; set; } = new List<CharacterClassEnum>() { CharacterClassEnum.Mage, CharacterClassEnum.Rogue, CharacterClassEnum.Warrior };

        private CharacterClassEnum selectedItemClass;

        public CharacterClassEnum SelectedItemClass
        {
            get { return selectedItemClass; }
            set { SetProperty(ref selectedItemClass, value); FilterList(); }
        }

        public List<int> Picker3Values { get; set; } = new List<int>() { 1, 2, 3 };

        private int selectedItemTier;

        public int SelectedItemTier
        {
            get { return selectedItemTier; }
            set { SetProperty(ref selectedItemTier, value); FilterList(); }
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
            SelectedItemType = ItemTypeEnum.All;
            SelectedItemClass = DataRepository.GetTeamProfile().Cast;
            SelectedItemTier = 1;
        }

        #endregion

        #region Private functions

        private void Init()
        {
            AllItems = new ObservableCollection<Item>(DataRepository.GetTeamProfile().Inventory);
            FilterList();
        }

        private void FilterList()
        {
            if (SelectedItemType == ItemTypeEnum.All)
            {
                SelectedItems = new ObservableCollection<Item>(AllItems);
            }
            else
            {
                SelectedItems = new ObservableCollection<Item>(AllItems.Where((arg) => arg.ItemType == SelectedItemType).ToList());
            }

            SelectedItems = new ObservableCollection<Item>(SelectedItems.Where((arg) => arg.UsableFor == SelectedItemClass).ToList());

            SelectedItems = new ObservableCollection<Item>(SelectedItems.Where((arg) => arg.Tier == SelectedItemTier).ToList());

            NoItem = SelectedItems.Count == 0;
        }

        private void ItemTapped(object o)
        {
            ActiveItem = o as Item;
        }

        private async Task SellTapped()
        {
            var response = await DialogService.DisplayAlertAsync(Texts.Sell, Texts.DialogSell, Texts.Yes, Texts.No);
            if (response)
            {
                var profile = DataRepository.GetTeamProfile();
                foreach (var item in AllItems.ToList())
                {
                    if (item.Id == ActiveItem.Id)
                    {
                        AllItems.Remove(item);
                        profile.Money += item.Value;
                    }
                }

                profile.Inventory = AllItems;
                logger.CreateSellLog(ActiveItem);
                DataRepository.SaveToFile(profile);
                Init();
            }
        }

        private async Task UseTapped()
        {
            var profile = DataRepository.GetTeamProfile();

            if (ActiveItem.UsableFor == profile.Cast)
            {
                foreach (var item in AllItems)
                {
                    if (item.Id == ActiveItem.Id)
                    {
                        item.State = ActiveItem.State == ItemStateEnum.Backpack ? ItemStateEnum.Equipped : ItemStateEnum.Backpack;
                    }

                    if (item.ItemType == ActiveItem.ItemType && item.Id != ActiveItem.Id)
                    {
                        item.State = ItemStateEnum.Backpack;
                    }
                }

                profile.Inventory = AllItems;
                DataRepository.SaveToFile(profile);
                Init();
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.CantUse, Texts.Ok);
            }
        }

        #endregion

    }
}
