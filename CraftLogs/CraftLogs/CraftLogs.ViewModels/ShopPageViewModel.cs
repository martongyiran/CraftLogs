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

        #region Private

        ShopProfile shopProfile;
        Settings settings;

        private IItemGeneratorService itemGenerator;
        private IQRService qRService;

        #endregion

        #region Public

        public DelayCommand RefreshCommand => new DelayCommand(Refresh);

        public DelayCommand<object> ItemTappedCommand => new DelayCommand<object>((a) => ItemTapped(a));

        public DelayCommand<object> RemoveItemTappedCommand => new DelayCommand<object>((a) => RemoveItemTapped(a));

        public DelayCommand BuyTappedCommand => new DelayCommand(async () => await Buy());

        public DelayCommand EmptyTappedCommand => new DelayCommand(Empty);

        public DelayCommand CheckOutTappedCommand => new DelayCommand(async () => await CheckOut());

        public DelayCommand DispalyCartIsEmptyCommand => new DelayCommand(async () => await DispalyCartIsEmpty());

        #endregion

        #region Ctor

        public ShopPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService, IQRService qrService) : base(navigationService, dataRepository, dialogService)
        {
            itemGenerator = itemGeneratorService;
            qRService = qrService;
            Title = Texts.ShopPageTitle;
        }

        #endregion

        #region Properties

        private ObservableCollection<Item> items = new ObservableCollection<Item>();

        public ObservableCollection<Item> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        private ObservableCollection<Item> selectedItems = new ObservableCollection<Item>();

        public ObservableCollection<Item> SelectedItems
        {
            get { return selectedItems; }
            set { SetProperty(ref selectedItems, value); }
        }

        private ObservableCollection<Item> shoppingCart = new ObservableCollection<Item>();

        public ObservableCollection<Item> ShoppingCart
        {
            get { return shoppingCart; }
            set { SetProperty(ref shoppingCart, value); }
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

        private string nextRefresh;

        public string NextRefresh
        {
            get { return nextRefresh; }
            set { SetProperty(ref nextRefresh, value); }
        }

        private bool noItem;

        public bool NoItem
        {
            get { return noItem; }
            set { SetProperty(ref noItem, value); }
        }

        private Item activeItem;

        public Item ActiveItem
        {
            get { return activeItem; }
            set { SetProperty(ref activeItem, value); }
        }

        private string cartValue = "Összesen: 0$";

        public string CartValue
        {
            get { return cartValue; }
            set { SetProperty(ref cartValue, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsBusy = true;
           
            Refresh();

            IsBusy = false;
        }

        public override async Task ToSettings()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("mode", "npc");

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        #endregion

        #region Private functions

        private void Refresh()
        {
            DataRepository.CreateShopProfile();
            shopProfile = DataRepository.GetShopProfile();
            settings = DataRepository.GetSettings();

            if(settings.AppMode == AppModeEnum.None)
            {
                settings.AppMode = AppModeEnum.Shop;
                DataRepository.SaveToFile(settings);
            }

            SelectedItemType = ItemTypeEnum.All;
            SelectedItemClass = CharacterClassEnum.Mage;
            SelectedItemTier = 2;

            if (DateTime.Now > shopProfile.LastRefresh.AddHours(1))
            {
                shopProfile.ItemStock = new ObservableCollection<Item>();

                if (settings.CraftDay == 1)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        shopProfile.ItemStock.Add(itemGenerator.GetRandomItem(1));
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        shopProfile.ItemStock.Add(itemGenerator.GetRandomItem(2));
                    }
                }
                else
                {
                    for (int i = 0; i < 40; i++)
                    {
                        shopProfile.ItemStock.Add(itemGenerator.GetRandomItem(2));
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        shopProfile.ItemStock.Add(itemGenerator.GetRandomItem(3));
                    }
                }

                shopProfile.LastRefresh = DateTime.Now;
                DataRepository.SaveToFile(shopProfile);
                Items = shopProfile.ItemStock;
            }

            Items = Items.Count == 0 ? shopProfile.ItemStock : Items;
            NextRefresh = "Frissül: " + shopProfile.LastRefresh.AddHours(1).ToShortTimeString();
            FilterList();
            IsBusy = false;
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
            IsBusy = false;
        }

        private void ItemTapped(object o)
        {
            ActiveItem = o as Item;
            IsBusy = false;
        }

        private void RemoveItemTapped(object o)
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

            CartValue = "Összesen: " + allValue + "$";

            IsBusy = false;
        }

        private async Task Buy()
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

                CartValue = "Összesen: " + allValue + "$";
            }
            else
            {
                IsBusy = false;
                await DialogService.DisplayAlertAsync("", Texts.ItemLimit, Texts.Ok);
            }
            

            IsBusy = false;
        }

        private void Empty()
        {
            Items = new ObservableCollection<Item>();
            SelectedItems = new ObservableCollection<Item>();
            ShoppingCart = new ObservableCollection<Item>();
            Items = DataRepository.GetShopProfile().ItemStock;
            FilterList();
            CartValue = "Összesen: 0$";
            IsBusy = false;
        }

        private async Task CheckOut()
        {
            if(ShoppingCart.Count != 0)
            {
                var response = await DialogService.DisplayAlertAsync(Texts.Checkout, Texts.CheckoutQuestion, Texts.Checkout, Texts.Cancel);
                if (response)
                {
                    int allValue = 0;
                    foreach (var item in ShoppingCart)
                    {
                        allValue += item.Value;
                    }
                    ShopResponse shopResponse = new ShopResponse(allValue, ShoppingCart);
                    var qrCode = qRService.CreateQR(shopResponse);
                    NavigationParameters param = new NavigationParameters();
                    param.Add("code", qrCode);

                    shopProfile.ItemStock = Items;
                    DataRepository.SaveToFile(shopProfile);
                    await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    IsBusy = false;
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.YourCartIsEmpty, Texts.Ok);
            }

            
            IsBusy = false;
        }

        private async Task DispalyCartIsEmpty()
        {
            await DialogService.DisplayAlertAsync(Texts.Error, Texts.YourCartIsEmpty, Texts.Ok);

            IsBusy = false;
        }

        #endregion
    }
}
