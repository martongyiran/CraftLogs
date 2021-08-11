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
        private readonly ILoggerService _logger;

        private TeamProfile _teamProfile;

        private ObservableCollection<Item> _items;
        private ObservableCollection<Item> _itemsForTrade = new ObservableCollection<Item>();
        private string _targetName;
        private int _tradeMoney = 0;
        private Item _activeItem;
        private bool _isPopupVisible;
        private bool _isCartVisible;
        private bool _canTrade;

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
            set
            {
                if (value > _teamProfile?.Money)
                {
                    value = _teamProfile.Money;
                }
                else if (value < 0)
                {
                    value = 0;
                }

                if(SetProperty(ref _tradeMoney, value))
                {
                    RaisePropertyChanged(nameof(CanEmpty));
                }
            }
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

        public bool CanTrade
        {
            get => _canTrade;
            set => SetProperty(ref _canTrade, value);
        }

        public int CartSize => ItemsForTrade.Count;

        public bool CanEmpty => ItemsForTrade.Count > 0 || TradeMoney > 0;

        public bool CanSelectMore => ItemsForTrade.Count < 5;

        public DelayCommand<object> ItemTappedCommand => new DelayCommand<object>((a) => ExecuteItemTappedCommand(a));

        public DelayCommand SelectItemCommand => new DelayCommand(async () => await ExecuteSelectItemCommandAsync());

        public DelayCommand EmptyCommand => new DelayCommand(ExecuteEmptyCommand);

        public DelayCommand CompleteTradeCommand => new DelayCommand(async () => await ExecuteCompleteTradeCommandAsync());

        public DelayCommand CloseCartCommand => new DelayCommand(ExecuteCloseCartCommand);

        public DelayCommand CheckCartCommand => new DelayCommand(ExecuteCheckCartCommand);

        public DelayCommand<object> RemoveItemCommand => new DelayCommand<object>((a) => ExecuteRemoveItemCommand(a));

        public TradePageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IQRService qrService,
            ILoggerService loggerService)
            : base(navigationService, dataRepository, dialogService)
        {
            _qRService = qrService;
            Title = Texts.Trade_Title;
            _logger = loggerService;
            IsBusy = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Task.Run(() =>
            {
                Init();
                IsBusy = false;
            });
        }

        private void Init()
        {
            _teamProfile = DataRepository.GetTeamProfile();
            Items = new ObservableCollection<Item>(_teamProfile.Inventory.Where(x => x.State == ItemStateEnum.Backpack).ToList());

            RaisePropertyChanged(nameof(CartSize));
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanSelectMore));
        }

        private void ExecuteItemTappedCommand(object o)
        {
            ActiveItem = o as Item;
        }

        private void ExecuteCloseCartCommand()
        {
            IsCartVisible = false;
        }

        private async Task ExecuteSelectItemCommandAsync()
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
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanSelectMore));
            IsPopupVisible = false;
        }

        private void ExecuteEmptyCommand()
        {
            if (ItemsForTrade.Count == 0)
            {
                TradeMoney = 0;
                return;
            }
            else
            {
                Items = new ObservableCollection<Item>();
                ItemsForTrade = new ObservableCollection<Item>();
                Items = new ObservableCollection<Item>(
                    _teamProfile.Inventory
                        .Where(x => x.State == ItemStateEnum.Backpack)
                        .OrderBy(y => y.UsableFor)
                        .ToList());
            }

            TradeMoney = 0;

            RaisePropertyChanged(nameof(CartSize));
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanSelectMore));
            IsPopupVisible = false;
        }

        private void ExecuteRemoveItemCommand(object o)
        {
            IsBusy = true;

            var sitem = o as Item;

            var tempitems = ItemsForTrade;
            tempitems.Remove(sitem);
            var templist = Items;
            templist.Add(sitem);

            ItemsForTrade = tempitems;
            Items = new ObservableCollection<Item>(templist.OrderBy(y => y.UsableFor));


            if (CartSize == 0)
            {
                IsCartVisible = false;
            }

            RaisePropertyChanged(nameof(CartSize));
            RaisePropertyChanged(nameof(CanEmpty));
            RaisePropertyChanged(nameof(CanSelectMore));

            IsBusy = false;
        }

        private void ExecuteCheckCartCommand()
        {
            IsCartVisible = true;
            CanTrade = !string.IsNullOrEmpty(TargetName)
                && CanEmpty;
        }

        private async Task ExecuteCompleteTradeCommandAsync()
        {
            var response = await DialogService.DisplayAlertAsync("Csere", "Ha rosszul írtad be a csapat nevét, vagy nem olvassa le a QR-t, akkor buktad a cuccokat!", Texts.Ok, Texts.Cancel);
            if (response)
            {
                var data = new TradeModel()
                {
                    Target = TargetName,
                    Money = TradeMoney,
                    TradeItems = ItemsForTrade,
                    From = _teamProfile.Name
                };
                data.SetTradeId();

                _teamProfile.Inventory = Items;
                _teamProfile.Money -= TradeMoney;
                DataRepository.SaveToFile(_teamProfile);
                _logger.CreateTradeLog(data, true);

                var qrCode = _qRService.CreateQR(data);
                var param = new NavigationParameters
                        {
                            { "code", qrCode }
                        };

                await NavigateToWithoutHistoryDouble(NavigationLinks.QRPage, param);
            }
        }
    }
}
