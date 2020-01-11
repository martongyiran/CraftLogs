/*
Copyright 2018 Gyirán Márton Áron

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

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class ShopPageViewModel : ViewModelBase
    {
        private readonly IItemGeneratorService _itemGenerator;
        private readonly IQRService _qRService;

        private ShopProfile _shopProfile;
        private Settings _settings;
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private ObservableCollection<Item> _selectedItems = new ObservableCollection<Item>();
        private ObservableCollection<Item> _shoppingCart = new ObservableCollection<Item>();
        private ItemTypeEnum _selectedItemType;
        private CharacterClassEnum _selectedItemClass;
        private int _selectedItemTier;
        private string _nextRefresh;
        private bool _noItem;
        private Item _activeItem;
        private string _cartValue = string.Format(Texts.Shop_Sum, "0");

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ObservableCollection<Item> SelectedItems
        {
            get => _selectedItems;
            set => SetProperty(ref _selectedItems, value);
        }

        public ObservableCollection<Item> ShoppingCart
        {
            get => _shoppingCart;
            set => SetProperty(ref _shoppingCart, value);
        }

        public List<ItemTypeEnum> Picker1Values { get; set; } = new List<ItemTypeEnum>() { ItemTypeEnum.All, ItemTypeEnum.Armor, ItemTypeEnum.LHand, ItemTypeEnum.RHand, ItemTypeEnum.Neck, ItemTypeEnum.Ring };

        public List<CharacterClassEnum> Picker2Values { get; set; } = new List<CharacterClassEnum>() { CharacterClassEnum.Mage, CharacterClassEnum.Rogue, CharacterClassEnum.Warrior };

        public List<int> Picker3Values { get; set; } = new List<int>() { 1, 2, 3 };

        public ItemTypeEnum SelectedItemType
        {
            get => _selectedItemType;
            set
            {
                SetProperty(ref _selectedItemType, value);
                FilterList();
            }
        }

        public CharacterClassEnum SelectedItemClass
        {
            get => _selectedItemClass;
            set
            {
                SetProperty(ref _selectedItemClass, value);
                FilterList();
            }
        }

        public int SelectedItemTier
        {
            get => _selectedItemTier;
            set
            {
                SetProperty(ref _selectedItemTier, value);
                FilterList();
            }
        }

        public string NextRefresh
        {
            get => _nextRefresh;
            set => SetProperty(ref _nextRefresh, value);
        }

        public bool NoItem
        {
            get => _noItem;
            set => SetProperty(ref _noItem, value);
        }

        public Item ActiveItem
        {
            get => _activeItem;
            set => SetProperty(ref _activeItem, value);
        }

        public string CartValue
        {
            get => _cartValue;
            set => SetProperty(ref _cartValue, value);
        }

        public DelayCommand RefreshCommand => new DelayCommand(ExecuteRefreshCommand);

        public DelayCommand<object> ItemTappedCommand => new DelayCommand<object>((a) => ExecuteItemTappedCommand(a));

        public DelayCommand<object> RemoveItemCommand => new DelayCommand<object>((a) => ExecuteRemoveItemCommand(a));

        public DelayCommand BuyCommand => new DelayCommand(async () => await ExecuteBuyCommandAsync());

        public DelayCommand EmptyCommand => new DelayCommand(ExecuteEmptyCommand);

        public DelayCommand CheckOutCommand => new DelayCommand(async () => await ExecuteCheckOutCommand());

        public DelayCommand DispalyCartIsEmptyCommand => new DelayCommand(async () => await ExecuteDispalyCartIsEmptyCommandAsync());


        public ShopPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IItemGeneratorService itemGeneratorService,
            IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            _itemGenerator = itemGeneratorService;
            _qRService = qrService;

            Title = Texts.Shop_Title;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ExecuteRefreshCommand();
        }

        public override async Task ToSettings()
        {
            var param = new NavigationParameters
            {
                { "mode", "npc" }
            };

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        private void ExecuteRefreshCommand()
        {
            DataRepository.CreateShopProfile();
           _shopProfile = DataRepository.GetShopProfile();
            _settings = DataRepository.GetSettings();

            if(_settings.AppMode == AppModeEnum.None)
            {
                _settings.AppMode = AppModeEnum.Shop;
                DataRepository.SaveToFile(_settings);
            }

            SelectedItemType = ItemTypeEnum.All;
            SelectedItemClass = CharacterClassEnum.Mage;
            SelectedItemTier = 2;

            if (DateTime.Now > _shopProfile.LastRefresh.AddHours(1))
            {
                _shopProfile.ItemStock = new ObservableCollection<Item>();

                if (_settings.CraftDay == 1)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        _shopProfile.ItemStock.Add(_itemGenerator.GetRandomItem(1));
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        _shopProfile.ItemStock.Add(_itemGenerator.GetRandomItem(2));
                    }
                }
                else
                {
                    for (int i = 0; i < 40; i++)
                    {
                        _shopProfile.ItemStock.Add(_itemGenerator.GetRandomItem(2));
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        _shopProfile.ItemStock.Add(_itemGenerator.GetRandomItem(3));
                    }
                }

                _shopProfile.LastRefresh = DateTime.Now;
                DataRepository.SaveToFile(_shopProfile);
                Items = _shopProfile.ItemStock;
            }

            Items = Items.Count == 0 ? _shopProfile.ItemStock : Items;
            NextRefresh = Texts.Shop_RefreshAt + _shopProfile.LastRefresh.AddHours(1).ToShortTimeString();
            FilterList();
        }

        private void FilterList()
        {
            if (SelectedItemType == ItemTypeEnum.All)
            {
                SelectedItems = new ObservableCollection<Item>(Items);
            }
            else
            {
                SelectedItems = new ObservableCollection<Item>(Items.Where((arg) => arg.ItemType == SelectedItemType).ToList());
            }

            SelectedItems = new ObservableCollection<Item>(SelectedItems.Where((arg) => arg.UsableFor == SelectedItemClass).ToList());

            SelectedItems = new ObservableCollection<Item>(SelectedItems.Where((arg) => arg.Tier == SelectedItemTier).ToList());

            NoItem = SelectedItems.Count == 0;
        }

        private void ExecuteItemTappedCommand(object o)
        {
            ActiveItem = o as Item;
        }

        private void ExecuteRemoveItemCommand(object o)
        {
            var sitem = o as Item;

            var tempitems = ShoppingCart;
            tempitems.Remove(sitem);
            var templist = Items;
            templist.Add(sitem);

            ShoppingCart = tempitems;
            Items = templist;

            FilterList();

            int allValue = 0;

            foreach (var item in ShoppingCart)
            {
                allValue += item.Value;
            }

            CartValue = string.Format(Texts.Shop_Sum, allValue);
        }

        private async Task ExecuteBuyCommandAsync()
        {
            if(ShoppingCart.Count < 5)
            {
                var tempitems = Items;
                tempitems.Remove(ActiveItem);
                var templist = ShoppingCart;
                templist.Add(ActiveItem);

                ShoppingCart = templist;
                Items = tempitems;

                FilterList();

                int allValue = 0;

                foreach (var item in ShoppingCart)
                {
                    allValue += item.Value;
                }

                CartValue = string.Format(Texts.Shop_Sum, allValue);
            }
            else
            {
                await DialogService.DisplayAlertAsync("", Texts.Shop_ItemLimit, Texts.Ok);
            }
        }

        private void ExecuteEmptyCommand()
        {
            Items = new ObservableCollection<Item>();
            SelectedItems = new ObservableCollection<Item>();
            ShoppingCart = new ObservableCollection<Item>();
            Items = DataRepository.GetShopProfile().ItemStock;
            FilterList();
            CartValue = string.Format(Texts.Shop_Sum, "0");
        }

        private async Task ExecuteCheckOutCommand()
        {
            if(ShoppingCart.Count != 0)
            {
                var response = await DialogService.DisplayAlertAsync(Texts.Shop_Checkout, Texts.Shop_CheckoutDialog, Texts.Shop_Checkout, Texts.Cancel);
                if (response)
                {
                    int allValue = 0;
                    foreach (var item in ShoppingCart)
                    {
                        allValue += item.Value;
                    }
                    ShopResponse shopResponse = new ShopResponse(allValue, ShoppingCart);
                    var qrCode = _qRService.CreateQR(shopResponse);
                    var param = new NavigationParameters
                    {
                        { "code", qrCode }
                    };

                    _shopProfile.ItemStock = Items;
                    DataRepository.SaveToFile(_shopProfile);
                    await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.Shop_CartIsEmptyError, Texts.Ok);
            }
        }

        private async Task ExecuteDispalyCartIsEmptyCommandAsync()
        {
            await DialogService.DisplayAlertAsync(Texts.Error, Texts.Shop_CartIsEmptyError, Texts.Ok);
        }
    }
}
