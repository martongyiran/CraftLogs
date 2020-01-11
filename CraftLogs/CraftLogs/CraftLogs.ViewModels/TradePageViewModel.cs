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
        private readonly IQRService _qRService;

        private TeamProfile _teamProfile;
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private ObservableCollection<Item> _selectedItems = new ObservableCollection<Item>();
        private ObservableCollection<Item> _tradeGiveCart = new ObservableCollection<Item>();
        private ObservableCollection<Item> _tradeGetCart = new ObservableCollection<Item>();
        private ItemTypeEnum _selectedItemType;
        private CharacterClassEnum _selectedItemClass;
        private int _selectedItemTier;
        private bool _noItem;
        private Item _activeItem;
        private bool _getListIsVisible;
        private int _money;
        private string _incMoney;

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

        public ObservableCollection<Item> TradeGiveCart
        {
            get => _tradeGiveCart;
            set => SetProperty(ref _tradeGiveCart, value);
        }

        public ObservableCollection<Item> TradeGetCart
        {
            get => _tradeGetCart;
            set => SetProperty(ref _tradeGetCart, value);
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

        public bool GetListIsVisible
        {
            get => _getListIsVisible;
            set => SetProperty(ref _getListIsVisible, value);
        }

        public int Money
        {
            get => _money;
            set => SetProperty(ref _money, value);
        }

        public string IncMoney
        {
            get => _incMoney;
            set => SetProperty(ref _incMoney, value);
        }

        public DelayCommand<object> ItemCommand => new DelayCommand<object>(async (a) => await ExecuteItemTappedCommandAsync(a));
        public DelayCommand<object> RemoveItemCommand => new DelayCommand<object>((a) => ExecuteRemoveItemCommand(a));
        public DelayCommand EmptyCommand => new DelayCommand(ExecuteEmptyCommand);
        public DelayCommand CheckOutCommand => new DelayCommand(async () => await ExecuteCheckOutCommandAscync());

        public TradePageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            _qRService = qrService;
            Title = Texts.Trade_Title;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Init();
        }

        private void Init()
        {
            _teamProfile = DataRepository.GetTeamProfile();

            SelectedItemType = ItemTypeEnum.All;
            SelectedItemClass = CharacterClassEnum.Mage;
            SelectedItemTier = 2;

            if (_teamProfile.TradeStatus == TradeStatusEnum.Finished)
            {
                GetListIsVisible = false;
                Items = _teamProfile.Inventory;
            }
            else if (_teamProfile.TradeStatus == TradeStatusEnum.TradeGetAndGive)
            {
                TradeGetCart = _teamProfile.TradeIn.ItemsToTrade;
                Items = _teamProfile.Inventory;
                IncMoney = _teamProfile.TradeIn.Money + " pénz";
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
        }

        private async Task ExecuteItemTappedCommandAsync(object o)
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
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.Trade_ItemLimit, Texts.Ok);
            }
        }

        private void ExecuteRemoveItemCommand(object o)
        {
            var sitem = o as Item;

            var tempitems = TradeGiveCart;
            tempitems.Remove(sitem);
            var templist = Items;
            templist.Add(sitem);

            TradeGiveCart = tempitems;
            Items = templist;

            FilterList();
        }

        private void ExecuteEmptyCommand()
        {
            Items = new ObservableCollection<Item>();
            SelectedItems = new ObservableCollection<Item>();
            TradeGiveCart = new ObservableCollection<Item>();
            Items = DataRepository.GetTeamProfile().Inventory;

            FilterList();
        }

        private async Task ExecuteCheckOutCommandAscync()
        {
            var response = await DialogService.DisplayAlertAsync(Texts.Trade_Title, Texts.Trade_Dialog, Texts.Trade_Title, Texts.Cancel);
            if (response)
            {
                if (_teamProfile.TradeStatus == TradeStatusEnum.Finished)
                {
                    if(Money > _teamProfile.Money)
                    {
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.Trade_NotEnoughMoney, Texts.Ok);
                    }
                    else
                    {
                        var tradeResponse = new TradeGive();
                        tradeResponse.Reward.ItemsToTrade = TradeGiveCart;
                        tradeResponse.Reward.Money = Money;
                        tradeResponse.Name = _teamProfile.Name;

                        _teamProfile.TradeStatus = TradeStatusEnum.TradeGive;
                        _teamProfile.Inventory = Items;
                        _teamProfile.TradeOut.ItemsToTrade = TradeGiveCart;
                        _teamProfile.TradeOut.Money = Money;
                        _teamProfile.TradeNumber = tradeResponse.TradeNumber;
                        _teamProfile.Money -= Money;

                        var qrCode = _qRService.CreateQR(tradeResponse);
                        var param = new NavigationParameters
                        {
                            { "code", qrCode }
                        };

                        _teamProfile.TradeLastQR = qrCode;

                        DataRepository.SaveToFile(_teamProfile);

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                }
                else if (_teamProfile.TradeStatus == TradeStatusEnum.TradeGetAndGive)
                {
                    if (Money > _teamProfile.Money)
                    {
                        await DialogService.DisplayAlertAsync(Texts.Error, Texts.Trade_NotEnoughMoney, Texts.Ok);
                    }
                    else
                    {
                        var tradeResponse = new TradeGetAndGive(_teamProfile.TradeNumber);
                        tradeResponse.Reward.ItemsToTrade = TradeGiveCart;
                        tradeResponse.Reward.Money = Money;
                        tradeResponse.Name = _teamProfile.Name;

                        _teamProfile.TradeStatus = TradeStatusEnum.TradeGiveAndGet;
                        _teamProfile.Inventory = Items;
                        _teamProfile.TradeOut.ItemsToTrade = TradeGiveCart;
                        _teamProfile.TradeOut.Money = Money;
                        _teamProfile.Money -= Money;

                        var qrCode = _qRService.CreateQR(tradeResponse);
                        var param = new NavigationParameters
                        {
                            { "code", qrCode }
                        };

                        _teamProfile.TradeLastQR = qrCode;

                        DataRepository.SaveToFile(_teamProfile);

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                }

                
            }
        }
    }
}
