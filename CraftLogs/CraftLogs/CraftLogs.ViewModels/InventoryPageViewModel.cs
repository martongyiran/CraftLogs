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

        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private Item _activeItem;
        private bool _isPopupVisible;

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
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
            IsBusy = true;
            base.OnNavigatedTo(parameters);

            ActiveItem = parameters["item"] as Item;

            Init();

            IsPopupVisible = ActiveItem != null;
            IsBusy = false;
        }

        private void Init()
        {
            Items = new ObservableCollection<Item>(DataRepository.GetTeamProfile().Inventory.OrderByDescending(x => x.State).ThenBy(y => y.UsableFor));
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

                foreach (var item in Items.ToList())
                {
                    if (item.Id == ActiveItem.Id)
                    {
                        Items.Remove(item);
                        profile.Money += item.Value;
                    }
                }

                profile.Inventory = Items;
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
                foreach (var item in Items)
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

                profile.Inventory = Items;
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
