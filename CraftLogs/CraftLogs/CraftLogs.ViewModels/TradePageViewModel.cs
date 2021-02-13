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

        private ObservableCollection<Item> _items;
        private ObservableCollection<Item> _itemsForTrade = new ObservableCollection<Item>();
        private string _targetName;
        private int _tradeMoney;
        private Item _activeItem;
        private bool _isPopupVisible;
        private bool _isCartVisible;

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ObservableCollection<Item> ItemsForTrade
        {
            get => _itemsForTrade;
            set => SetProperty(ref _itemsForTrade, value);
        }

        public string TargetName
        {
            get => _targetName;
            set => SetProperty(ref _targetName, value);
        }

        public int TradeMoney
        {
            get => _tradeMoney;
            set => SetProperty(ref _tradeMoney, value);
        }

        public Item ActiveItem
        {
            get => _activeItem;
            set => SetProperty(ref _activeItem, value);
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

        public int CartSize => ItemsForTrade.Count;

        public DelayCommand<object> ItemTappedCommand => new DelayCommand<object>((a) => ExecuteItemTappedCommand(a));

        public DelayCommand BuyCommand => new DelayCommand(async () => await ExecuteBuyCommandAsync());

       // public DelayCommand EmptyCommand => new DelayCommand(async () => await ExecuteEmptyCommandAsync());

       // public DelayCommand CheckOutCommand => new DelayCommand(async () => await ExecuteCheckOutCommand());

       // public DelayCommand CloseCartCommand => new DelayCommand(ExecuteCloseCartCommand);

        //public DelayCommand CheckCartCommand => new DelayCommand(ExecuteCheckCartCommand);

       // public DelayCommand<object> RemoveItemCommand => new DelayCommand<object>((a) => ExecuteRemoveItemCommand(a));

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
            Items = new ObservableCollection<Item>(_teamProfile.Inventory.Where(x => x.State == ItemStateEnum.Backpack).ToList());
        }

        private void ExecuteItemTappedCommand(object o)
        {
            ActiveItem = o as Item;
        }

        private void ExecuteCloseCartCommand()
        {
            IsCartVisible = false;
        }

        private async Task ExecuteBuyCommandAsync()
        {
            if (ItemsForTrade.Count < 5)
            {
                var tempitems = Items;
                tempitems.Remove(ActiveItem);
                var templist = ItemsForTrade;
                templist.Add(ActiveItem);

                ItemsForTrade = templist;
                Items = tempitems;
            }
            else
            {
                await DialogService.DisplayAlertAsync("", "Maximum 5 tárgyat adhatsz egyszerre!", Texts.Ok);
            }

            RaisePropertyChanged(nameof(CartSize));
            IsPopupVisible = false;
        }

        private async Task ExecuteSendCommandAsync()
        {
            var data = new TradeModel()
            {
                Target = TargetName,
                Money = TradeMoney,
                TradeItems = ItemsForTrade
            };
            data.SetTradeId();



            var qrCode = _qRService.CreateQR(data);
            var param = new NavigationParameters
                        {
                            { "code", qrCode }
                        };

            DataRepository.SaveToFile(_teamProfile);

            await NavigateToWithoutHistoryDouble(NavigationLinks.QRPage, param);
        }
    }
}
