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
    public class ProfilePageViewModel : ViewModelBase
    {
        private readonly IQRService _qRService;

        private TeamProfile _profile;
        private CombatUnit _combatUnit;
        private ObservableCollection<Log> _logs;
        private string _arenaIcon;
        private string _hpSum;
        private string _atkSum;
        private string _defSum;
        private string _staminaSum;
        private string _critRSum;
        private string _dodgeSum;
        private bool _pointIsVisible;
        private Item _armorItem;
        private Item _ringItem;
        private Item _neckItem;
        private Item _lHandItem;
        private Item _rHandItem;

        public TeamProfile Profile
        {
            get => _profile;
            set => SetProperty(ref _profile, value);
        }

        public string HpSum
        {
            get => _hpSum;
            set => SetProperty(ref _hpSum, value);
        }

        public string AtkSum
        {
            get => _atkSum;
            set => SetProperty(ref _atkSum, value);
        }

        public string DefSum
        {
            get => _defSum;
            set => SetProperty(ref _defSum, value);
        }

        public string StaminaSum
        {
            get => _staminaSum;
            set => SetProperty(ref _staminaSum, value);
        }

        public string CritRSum
        {
            get => _critRSum;
            set => SetProperty(ref _critRSum, value);
        }

        public string DodgeSum
        {
            get => _dodgeSum;
            set => SetProperty(ref _dodgeSum, value);
        }

        public bool PointIsVisible
        {
            get => _pointIsVisible;
            set => SetProperty(ref _pointIsVisible, value);
        }

        public Item ArmorItem
        {
            get => _armorItem;
            set => SetProperty(ref _armorItem, value);
        }

        public Item RingItem
        {
            get => _ringItem;
            set => SetProperty(ref _ringItem, value);
        }

        public Item NeckItem
        {
            get => _neckItem;
            set => SetProperty(ref _neckItem, value);
        }

        public Item LHandItem
        {
            get => _lHandItem;
            set => SetProperty(ref _lHandItem, value);
        }

        public Item RHandItem
        {
            get => _rHandItem;
            set => SetProperty(ref _rHandItem, value);
        }

        public string ArenaIcon
        {
            get => _arenaIcon;
            set => SetProperty(ref _arenaIcon, value);
        }

        public DelayCommand NavigateToLogsCommand => new DelayCommand(async () => await NavigateTo(NavigationLinks.LogsPage));

        public DelayCommand NavigateToQRScannerPageCommand => new DelayCommand(async () => await ReadQR());

        public DelayCommand NavigateToInventoryPageCommand => new DelayCommand(async () => await NavigateTo(NavigationLinks.InventoryPage));

        public DelayCommand GetArenaQRCommand => new DelayCommand(async () => await ExecuteGetArenaQRCommandAsync());

        public DelayCommand StartTradeCommand => new DelayCommand(async () => await NavigateTo(NavigationLinks.TradePage));

        public DelayCommand<string?> RaiseStatCommand => new DelayCommand<string?>((a) => ExecuteRaiseStatCommand(a));

        public DelayCommand ShowInfoCommand => new DelayCommand(async () => await ExecuteShowInfoCommandAsync());

        public DelayCommand<string?> ViewItemCommand
            => new DelayCommand<string?>(async (a) => { await ExecuteViewItemCommandAsync(a); });

        public ProfilePageViewModel(
            INavigationService navigationService,
            ILocalDataRepository dataRepository,
            IPageDialogService dialogService,
            IQRService qrService)
            : base(navigationService, dataRepository, dialogService)
        {
            _qRService = qrService;
            Title = Texts.Profile_Title;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Init();
        }

        public override async Task ToSettings()
        {
            var param = new NavigationParameters
            {
                { "mode", "team" }
            };

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        private void Init()
        {
            Profile = DataRepository.GetTeamProfile();

            SetItems();

            ArenaIcon = GetArenaIcon();

            PointIsVisible = Profile.StatPoint > 0;

            var equippedItems = Profile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped).ToList();

            int batk = Profile.Atk;
            int bdef = Profile.Def;
            int bstamina = Profile.Stamina;
            int bcr = Profile.CritR;
            int bdodge = Profile.Dodge;
            int bhp = Profile.Hp;

            foreach (var item in equippedItems)
            {
                batk += item.Atk;
                bdef += item.Def;
                bstamina += item.Stamina;
                bcr += item.CritR;
                bdodge += item.Dodge;
                bhp += (item.Stamina * Profile.HpValue);
            }

            bcr = bcr >= 60 ? 60 : bcr;
            bdodge = bdodge >= 60 ? 60 : bdodge;

            AtkSum = $"ATK: {batk}";
            DefSum = $"DEF: {bdef}";
            StaminaSum = $"STM: {bstamina}";

            HpSum = $"HP: {bhp}";
            CritRSum = string.Format(Texts.Profile_CritR, bcr);
            DodgeSum = string.Format(Texts.Profile_Dodge, bdodge);

            _combatUnit = new CombatUnit(Profile.Name, batk, bdef, bcr, bdodge, bhp, Profile.House.ToString(), Profile.Image);
        }

        private async Task ReadQR()
        {
            var scanResult = await _qRService.ReadQr();

            if (scanResult != null)
            {
                await NavigateToWithoutHistory(NavigationLinks.QRHandlerPage, scanResult);
            }

        }

        private void SetItems()
        {
            var armor = Profile.Inventory
                .Where((arg) => arg.State == ItemStateEnum.Equipped 
                    && arg.ItemType == ItemTypeEnum.Armor)
                .FirstOrDefault();
            ArmorItem = armor ?? new Item(1111);

            var ring = Profile.Inventory
                .Where((arg) => arg.State == ItemStateEnum.Equipped
                    && arg.ItemType == ItemTypeEnum.Ring)
                .FirstOrDefault();
            RingItem = ring ?? new Item(2222);

            var neck = Profile.Inventory
                .Where((arg) => arg.State == ItemStateEnum.Equipped
                    && arg.ItemType == ItemTypeEnum.Neck)
                .FirstOrDefault();
            NeckItem = neck ?? new Item(3333);

            var lhand = Profile.Inventory
                .Where((arg) => arg.State == ItemStateEnum.Equipped
                    && arg.ItemType == ItemTypeEnum.LHand)
                .FirstOrDefault();
            LHandItem = lhand ?? new Item(4444);

            var rhand = Profile.Inventory
                .Where((arg) => arg.State == ItemStateEnum.Equipped
                    && arg.ItemType == ItemTypeEnum.RHand)
                .FirstOrDefault();
            RHandItem = rhand ?? new Item(4444);
        }

        private async Task ExecuteGetArenaQRCommandAsync()
        {
#if DEV
            var res = await DialogService.DisplayAlertAsync(Texts.Arena_Title, Texts.Profile_ArenaDialog, Texts.Yes, Texts.No);
            if (res)
            {
                var qrCode = _qRService.CreateQR(_combatUnit);

                var param = new NavigationParameters
                {
                    { "code", qrCode }
                };

                await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
            }

#else

            _logs = DataRepository.GetLogs();

            if(_logs.Count == 0)
            {
                var res = await DialogService.DisplayAlertAsync(Texts.Arena_Title, Texts.Profile_ArenaDialog, Texts.Yes, Texts.No);
                if (res)
                {
                    var qrCode = _qRService.CreateQR(_combatUnit);

                    var param = new NavigationParameters
                    {
                        { "code", qrCode }
                    };

                    await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                }
            }
            else
            {
                var arenas = _logs.FirstOrDefault(x => x.LogType == LogTypeEnum.Arena);

                if (arenas != null && arenas.Date.AddMinutes(15) > DateTime.Now)
                {
                    await DialogService.DisplayAlertAsync(Texts.Error, Texts.Profile_CantFightNow + arenas.Date.AddMinutes(15), Texts.Ok);
                }
                else
                {
                    var res = await DialogService.DisplayAlertAsync(Texts.Arena_Title, Texts.Profile_ArenaDialog, Texts.Yes, Texts.No);
                    if (res)
                    {
                        var qrCode = _qRService.CreateQR(_combatUnit);

                        var param = new NavigationParameters
                        {
                            { "code", qrCode }
                        };

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                }
            }

#endif

        }

        private void ExecuteRaiseStatCommand(string? stat)
        {
            if (Profile.StatPoint > 0)
            {
                switch (stat)
                {
                    case "stamina":
                        Profile.Stamina += 1;
                        break;
                    case "atk":
                        Profile.Atk += 1;
                        break;
                    case "def":
                        Profile.Def += 1;
                        break;
                    default:
                        break;
                }

                DataRepository.SaveToFile(Profile);
                Init();
            }

        }

        private string GetArenaIcon()
        {
            _logs = DataRepository.GetLogs();

            var arenas = _logs.FirstOrDefault(x => x.LogType == LogTypeEnum.Arena);

            if (arenas != null
                && arenas.Date.AddMinutes(15) > DateTime.Now)
            {
                return "@drawable/ic_arena_whiteIP.png";
            }

            return "@drawable/ic_arena_white.png";
        }

        private async Task ExecuteShowInfoCommandAsync()
        {
            await DialogService.DisplayAlertAsync(Texts.Info, string.Format(Texts.Profile_Info, Profile.HpValue), Texts.Ok);
        }

        private async Task ExecuteViewItemCommandAsync(string? type)
        {
            switch (type)
            {
                case "armor":
                    if (ArmorItem.Ad != 1111)
                    {
                        var param = new NavigationParameters
                            {
                                { "item", ArmorItem }
                            };

                        await NavigateTo(NavigationLinks.InventoryPage, param);
                    }
                    else
                    {
                        await NavigateTo(NavigationLinks.InventoryPage);
                    }
                    break;
                case "ring":
                    if (RingItem.Ad != 2222)
                    {
                        var param = new NavigationParameters
                            {
                                { "item", RingItem }
                            };

                        await NavigateTo(NavigationLinks.InventoryPage, param);
                    }
                    else
                    {
                        await NavigateTo(NavigationLinks.InventoryPage);
                    }
                    break;
                case "neck":
                    if (NeckItem.Ad != 3333)
                    {
                        var param = new NavigationParameters
                            {
                                { "item", NeckItem }
                            };

                        await NavigateTo(NavigationLinks.InventoryPage, param);
                    }
                    else
                    {
                        await NavigateTo(NavigationLinks.InventoryPage);
                    }
                    break;
                case "lHand":
                    if (LHandItem.Ad != 4444)
                    {
                        var param = new NavigationParameters
                            {
                                { "item", LHandItem }
                            };

                        await NavigateTo(NavigationLinks.InventoryPage, param);
                    }
                    else
                    {
                        await NavigateTo(NavigationLinks.InventoryPage);
                    }
                    break;
                case "rHand":
                    if (RHandItem.Ad != 4444)
                    {
                        var param = new NavigationParameters
                            {
                                { "item", RHandItem }
                            };

                        await NavigateTo(NavigationLinks.InventoryPage, param);
                    }
                    else
                    {
                        await NavigateTo(NavigationLinks.InventoryPage);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
