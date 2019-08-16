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

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class TradePageViewModel : ViewModelBase
    {

        #region Private

        TeamProfile teamProfile;

        private IQRService qRService;

        private DelegateCommand<object> itemTappedCommand;
        private DelegateCommand<object> removeItemTappedCommand;
        private DelegateCommand emptyTappedCommand;
        private DelegateCommand checkOutTappedCommand;

        #endregion

        #region Public

        public DelegateCommand<object> ItemTappedCommand => itemTappedCommand ?? (itemTappedCommand = new DelegateCommand<object>(async (a) => await ItemTapped(a)));

        public DelegateCommand<object> RemoveItemTappedCommand => removeItemTappedCommand ?? (removeItemTappedCommand = new DelegateCommand<object>((a) => RemoveItemTapped(a)));

        public DelegateCommand EmptyTappedCommand => emptyTappedCommand ?? (emptyTappedCommand = new DelegateCommand(() => { IsBusy = true; Empty(); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand CheckOutTappedCommand => checkOutTappedCommand ?? (checkOutTappedCommand = new DelegateCommand(async () => { IsBusy = true; await CheckOut(); }, CanSubmit).ObservesProperty(() => IsBusy));

        #endregion

        #region Ctor

        public TradePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            qRService = qrService;
            Title = Texts.TradePage;
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

        private ObservableCollection<Item> tradeGiveCart = new ObservableCollection<Item>();

        public ObservableCollection<Item> TradeGiveCart
        {
            get { return tradeGiveCart; }
            set { SetProperty(ref tradeGiveCart, value); }
        }

        private ObservableCollection<Item> tradeGetCart = new ObservableCollection<Item>();

        public ObservableCollection<Item> TradeGetCart
        {
            get { return tradeGetCart; }
            set { SetProperty(ref tradeGetCart, value); }
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

        private bool getListIsVisible;

        public bool GetListIsVisible
        {
            get { return getListIsVisible; }
            set { SetProperty(ref getListIsVisible, value); }
        }

        private int money;

        public int Money
        {
            get { return money; }
            set { SetProperty(ref money, value); }
        }

        private string incMoney;

        public string IncMoney
        {
            get { return incMoney; }
            set { SetProperty(ref incMoney, value); }
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
            teamProfile = DataRepository.GetTeamProfile();

            SelectedItemType = ItemTypeEnum.All;
            SelectedItemClass = CharacterClassEnum.Mage;
            SelectedItemTier = 2;

            if (teamProfile.TradeStatus == TradeStatusEnum.Finished)
            {
                GetListIsVisible = false;
                Items = teamProfile.Inventory;
            }
            else if (teamProfile.TradeStatus == TradeStatusEnum.TradeGetAndGive)
            {
                TradeGetCart = teamProfile.TradeIn.ItemsToTrade;
                Items = teamProfile.Inventory;
                IncMoney = teamProfile.TradeIn.Money + " pénz";
                GetListIsVisible = true;
            }
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

            IsBusy = false;
        }

        private async Task ItemTapped(object o)
        {
            ActiveItem = o as Item;

            if (TradeGiveCart.Count < 5)
            {
                var tempitems = Items;
                tempitems.Remove(ActiveItem);
                var templist = TradeGiveCart;
                templist.Add(ActiveItem);

                TradeGiveCart = templist;
                Items = tempitems;

                FilterList();
            }
            else
            {
                IsBusy = false;
                await DialogService.DisplayAlertAsync("", "Maximum 5 tárgyat adhatsz egyszerre!", Texts.Ok);
            }

            IsBusy = false;
        }

        private void RemoveItemTapped(object o)
        {
            var sitem = o as Item;

            var tempitems = TradeGiveCart;
            tempitems.Remove(sitem);
            var templist = Items;
            templist.Add(sitem);

            TradeGiveCart = tempitems;
            Items = templist;

            FilterList();

            IsBusy = false;
        }

        private void Empty()
        {
            Items = new ObservableCollection<Item>();
            SelectedItems = new ObservableCollection<Item>();
            TradeGiveCart = new ObservableCollection<Item>();
            Items = DataRepository.GetTeamProfile().Inventory;
            FilterList();

            IsBusy = false;
        }

        private async Task CheckOut()
        {
            var response = await DialogService.DisplayAlertAsync(Texts.Checkout, "Biztos ezeket a tárgyakat akarod elcserélni?", Texts.TradePage, Texts.Cancel);
            if (response)
            {
                if (teamProfile.TradeStatus == TradeStatusEnum.Finished)
                {
                    if(Money > teamProfile.Money)
                    {
                        await DialogService.DisplayAlertAsync(Texts.Error, "Nincs ennyi pénzed!", Texts.Sadface);
                    }
                    else
                    {
                        TradeGive tradeResponse = new TradeGive();
                        tradeResponse.Reward.ItemsToTrade = TradeGiveCart;
                        tradeResponse.Reward.Money = Money;

                        teamProfile.TradeStatus = TradeStatusEnum.TradeGive;
                        teamProfile.Inventory = Items;
                        teamProfile.TradeOut.ItemsToTrade = TradeGiveCart;
                        teamProfile.TradeOut.Money = Money;
                        teamProfile.TradeNumber = tradeResponse.TradeNumber;
                        teamProfile.Money -= Money;

                        var qrCode = qRService.CreateQR(tradeResponse);
                        NavigationParameters param = new NavigationParameters();
                        param.Add("code", qrCode);

                        DataRepository.SaveToFile(teamProfile);
                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                    
                    IsBusy = false;
                }
                else if (teamProfile.TradeStatus == TradeStatusEnum.TradeGetAndGive)
                {
                    if (Money > teamProfile.Money)
                    {
                        await DialogService.DisplayAlertAsync(Texts.Error, "Nincs ennyi pénzed!", Texts.Sadface);
                    }
                    else
                    {
                        TradeGetAndGive tradeResponse = new TradeGetAndGive(teamProfile.TradeNumber);
                        tradeResponse.Reward.ItemsToTrade = TradeGiveCart;
                        tradeResponse.Reward.Money = Money;

                        teamProfile.TradeStatus = TradeStatusEnum.TradeGiveAndGet;
                        teamProfile.Inventory = Items;
                        teamProfile.TradeOut.ItemsToTrade = TradeGiveCart;
                        teamProfile.TradeOut.Money = Money;
                        teamProfile.Money -= Money;

                        var qrCode = qRService.CreateQR(tradeResponse);
                        NavigationParameters param = new NavigationParameters();
                        param.Add("code", qrCode);

                        DataRepository.SaveToFile(teamProfile);
                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                    
                    IsBusy = false;
                }

                
            }


            IsBusy = false;
        }

        #endregion

    }
}
