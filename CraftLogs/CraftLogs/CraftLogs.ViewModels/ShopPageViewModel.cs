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
        private ObservableCollection<Item> _shoppingCart = new ObservableCollection<Item>();
        private string _nextRefresh;
        private Item _activeItem;
        private string _cartValue = string.Format(Texts.Shop_Sum, "0");
        private bool _isPopupVisible;
        private bool _isCartVisible;

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ObservableCollection<Item> ShoppingCart
        {
            get => _shoppingCart;
            set => SetProperty(ref _shoppingCart, value);
        }

        public string NextRefresh
        {
            get => _nextRefresh;
            set => SetProperty(ref _nextRefresh, value);
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

        public bool IsPopupVisible
        {
            get => _isPopupVisible;
            set => SetProperty(ref _isPopupVisible, value);
        }

        public bool IsCartVisible
        {
            get => _isCartVisible;
            set => SetProperty(ref _isCartVisible, value);
        }

        public int CartSize => ShoppingCart.Count;

        public bool CanEmpty => ShoppingCart.Count > 0;

        public bool CanBuy => ShoppingCart.Count < 5;
        public DelayCommand RefreshCommand => new DelayCommand(ExecuteRefreshCommand);

        public DelayCommand<object> ItemTappedCommand => new DelayCommand<object>((a) => ExecuteItemTappedCommand(a));

        public DelayCommand BuyCommand => new DelayCommand(ExecuteBuyCommand);

        public DelayCommand EmptyCommand => new DelayCommand(ExecuteEmptyCommand);

        public DelayCommand CheckOutCommand => new DelayCommand(async () => await ExecuteCheckOutCommand());

        public DelayCommand CloseCartCommand => new DelayCommand(ExecuteCloseCartCommand);

        public DelayCommand CheckCartCommand => new DelayCommand(ExecuteCheckCartCommand);

        public DelayCommand<object> RemoveItemCommand => new DelayCommand<object>((a) => ExecuteRemoveItemCommand(a));

        public ShopPageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IItemGeneratorService itemGeneratorService,
            IQRService qRService)
            : base(navigationService, dataRepository, dialogService)
        {
            _itemGenerator = itemGeneratorService;
            _qRService = qRService;

            Title = Texts.Shop_Title;
            IsBusy = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Task.Run(() =>
            {
                ExecuteRefreshCommand();
                IsBusy = false;
            });
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
            IsBusy = true;

            IsPopupVisible = false;

            DataRepository.CreateShopProfile();
            _shopProfile = DataRepository.GetShopProfile();
            _settings = DataRepository.GetSettings();

            if (_settings.AppMode == AppModeEnum.None)
            {
                _settings.AppMode = AppModeEnum.Shop;
                DataRepository.SaveToFile(_settings);
            }

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
                Items = new ObservableCollection<Item>(_shopProfile.ItemStock.OrderBy(y => y.UsableFor));
            }

            Items = Items.Count == 0 ? new ObservableCollection<Item>(_shopProfile.ItemStock.OrderBy(y => y.UsableFor)) : Items;
            NextRefresh = string.Format("{0} {1:HH:mm}", Texts.Shop_RefreshAt, _shopProfile.LastRefresh.AddHours(1));

            RaisePropertyChanged(nameof(CartSize));
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanBuy));

            IsCartVisible = false;
            IsPopupVisible = false;

            IsBusy = false;
        }

        private void ExecuteItemTappedCommand(object o)
        {
            ActiveItem = o as Item;
        }

        private void ExecuteCloseCartCommand()
        {
            IsCartVisible = false;
        }

        private void  ExecuteBuyCommand()
        {
            var tempitems = Items;
            tempitems.Remove(ActiveItem);
            var templist = ShoppingCart;
            templist.Add(ActiveItem);

            ShoppingCart = templist;
            Items = tempitems;

            int allValue = 0;

            foreach (var item in ShoppingCart)
            {
                allValue += item.Value;
            }

            CartValue = string.Format(Texts.Shop_Sum, allValue);

            RaisePropertyChanged(nameof(CartSize));
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanBuy));
            IsPopupVisible = false;
        }

        private void ExecuteEmptyCommand()
        {
            if (ShoppingCart.Count == 0)
            {
                return;
            }
            else
            {
                Items = new ObservableCollection<Item>();
                ShoppingCart = new ObservableCollection<Item>();
                Items = new ObservableCollection<Item>(DataRepository.GetShopProfile().ItemStock.OrderBy(y => y.UsableFor));
                CartValue = string.Format(Texts.Shop_Sum, "0");
            }

            RaisePropertyChanged(nameof(CartSize));
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanBuy));
            IsPopupVisible = false;
        }

        private void ExecuteRemoveItemCommand(object o)
        {
            IsBusy = true;

            var sitem = o as Item;

            var tempitems = ShoppingCart;
            tempitems.Remove(sitem);
            var templist = Items;
            templist.Add(sitem);

            ShoppingCart = tempitems;
            Items = new ObservableCollection<Item>(templist.OrderBy(y => y.UsableFor));

            int allValue = 0;

            foreach (var item in ShoppingCart)
            {
                allValue += item.Value;
            }

            CartValue = string.Format(Texts.Shop_Sum, allValue);

            if(CartSize == 0)
            {
                IsCartVisible = false;
            }

            RaisePropertyChanged(nameof(CartSize));
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanBuy));

            IsBusy = false;
        }

        private void ExecuteCheckCartCommand()
        {
            IsCartVisible = true;
        }

        private async Task ExecuteCheckOutCommand()
        {
            if (ShoppingCart.Count != 0)
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

            IsPopupVisible = false;
        }
    }
}
