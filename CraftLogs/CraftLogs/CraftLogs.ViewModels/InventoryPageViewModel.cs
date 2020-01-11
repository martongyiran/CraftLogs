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
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class InventoryPageViewModel : ViewModelBase
    {

        private readonly ILoggerService _logger;

        private ObservableCollection<Item> _allItems = new ObservableCollection<Item>();
        private ObservableCollection<Item> _selectedItems = new ObservableCollection<Item>();
        private ItemTypeEnum _selectedItemType;
        private CharacterClassEnum _selectedItemClass;
        private int _selectedItemTier;
        private Item _activeItem;
        private bool _noItem;
        private bool _isPopupVisible;

        public ObservableCollection<Item> AllItems
        {
            get => _allItems;
            set => SetProperty(ref _allItems, value);
        }

        public ObservableCollection<Item> SelectedItems
        {
            get =>  _selectedItems; 
            set => SetProperty(ref _selectedItems, value); 
        }

        public List<ItemTypeEnum> Picker1Values { get; set; } = new List<ItemTypeEnum>() { ItemTypeEnum.All, ItemTypeEnum.Armor, ItemTypeEnum.LHand, ItemTypeEnum.RHand, ItemTypeEnum.Neck, ItemTypeEnum.Ring };

        public ItemTypeEnum SelectedItemType
        {
            get => _selectedItemType;
            set
            {
                SetProperty(ref _selectedItemType, value);
                FilterList();
            }
        }

        public List<CharacterClassEnum> Picker2Values { get; set; } = new List<CharacterClassEnum>() { CharacterClassEnum.Mage, CharacterClassEnum.Rogue, CharacterClassEnum.Warrior };

        public CharacterClassEnum SelectedItemClass
        {
            get => _selectedItemClass;
            set
            {
                SetProperty(ref _selectedItemClass, value);
                FilterList();
            }
        }

        public List<int> Picker3Values { get; set; } = new List<int>() { 1, 2, 3 };

        public int SelectedItemTier
        {
            get => _selectedItemTier;
            set
            {
                SetProperty(ref _selectedItemTier, value);
                FilterList();
            }
        }

        public Item ActiveItem
        {
            get => _activeItem;
            set => SetProperty(ref _activeItem, value);
        }

        public bool NoItem
        {
            get => _noItem;
            set => SetProperty(ref _noItem, value);
        }

        public bool IsPopupVisible
        {
            get => _isPopupVisible;
            set => SetProperty(ref _isPopupVisible, value);
        }

        public DelayCommand<object> ItemTappedCommand => new DelayCommand<object>((a) => ExecuteItemTapped(a));

        public DelayCommand SellCommand => new DelayCommand(async () => await ExecuteSellCommandAsync());

        public DelayCommand UseCommand => new DelayCommand(async () => await ExecuteUseCommandAsync());

        public InventoryPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            ILoggerService loggerService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.Inventory_Title;
            _logger = loggerService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Init();

            SelectedItemType = ItemTypeEnum.All;
            SelectedItemClass = DataRepository.GetTeamProfile().Cast;
            SelectedItemTier = 1;
            IsPopupVisible = false;
        }

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

        private void ExecuteItemTapped(object o)
        {
            ActiveItem = o as Item;
        }

        private async Task ExecuteSellCommandAsync()
        {
            var response = await DialogService.DisplayAlertAsync(Texts.Inventory_Sell, Texts.Inventory_SellDialog, Texts.Yes, Texts.No);
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
                IsPopupVisible = false;

                _logger.CreateSellLog(ActiveItem);
                DataRepository.SaveToFile(profile);

                Init();
            }
        }

        private async Task ExecuteUseCommandAsync()
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
                IsPopupVisible = false;

                DataRepository.SaveToFile(profile);
                Init();
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.Inventory_CantUse, Texts.Ok);
            }
        }
    }
}
