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
using Prism.Commands;
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

        #region Private
        
        private IQRService qRService;

        private TeamProfile teamProfile;
        private CombatUnit combatUnit;
        private ProfileQr profileQr;
        private ObservableCollection<Log> logs;

        private DelegateCommand navigateToLogsCommand;
        private DelegateCommand navigateToQRScannerPageCommand;
        private DelegateCommand navigateToInventoryPageCommand;
        private DelegateCommand getProfileQRCommand;
        private DelegateCommand startTradeCommand;
        private DelegateCommand<object> raiseStatCommand;
        private DelegateCommand lastTradeQRCommand;
        private DelegateCommand showInfoCommand;
        private DelegateCommand showProfileCommand;

        #endregion

        #region Public

        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.LogsPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand NavigateToQRScannerPageCommand => navigateToQRScannerPageCommand ?? (navigateToQRScannerPageCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.QRScannerPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand NavigateToInventoryPageCommand => navigateToInventoryPageCommand ?? (navigateToInventoryPageCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.InventoryPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand GetProfileQRCommand => getProfileQRCommand ?? (getProfileQRCommand = new DelegateCommand(async () => await GetProfileQRAsync(), CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand StartTradeCommand => startTradeCommand ?? (startTradeCommand = new DelegateCommand(async () => await StartTradeAsync(), CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand<object> RaiseStatCommand => raiseStatCommand ?? (raiseStatCommand = new DelegateCommand<object>((a) => RaiseStat(a)));

        public DelegateCommand LastTradeQRCommand => lastTradeQRCommand ?? (lastTradeQRCommand = new DelegateCommand(async () => { IsBusy = true; await ToLastTradeQR(); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand ShowInfoCommand => showInfoCommand ?? (showInfoCommand = new DelegateCommand(async () => { await ShowInfo(); }));

        public DelegateCommand ShowProfileCommand => showProfileCommand ?? (showProfileCommand = new DelegateCommand(async () => await ShowProfileAsync(), CanSubmit).ObservesProperty(() => IsBusy));


        #endregion

        #region Ctor

        public ProfilePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IQRService qrService) : base(navigationService, dataRepository, dialogService)
        {
            qRService = qrService;
            Title = Texts.ProfilePage;
        }

        #endregion

        #region Properties

        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        private string lvl;

        public string Lvl
        {
            get { return lvl; }
            set { SetProperty(ref lvl, value); }
        }

        private string hp;

        public string Hp
        {
            get { return hp; }
            set { SetProperty(ref hp, value); }
        }

        private string exp;

        public string Exp
        {
            get { return exp; }
            set { SetProperty(ref exp, value); }
        }

        private string atk;

        public string Atk
        {
            get { return atk; }
            set { SetProperty(ref atk, value); }
        }

        private string def;

        public string Def
        {
            get { return def; }
            set { SetProperty(ref def, value); }
        }

        private string stamina;

        public string Stamina
        {
            get { return stamina; }
            set { SetProperty(ref stamina, value); }
        }

        private string critR;

        public string CritR
        {
            get { return critR; }
            set { SetProperty(ref critR, value); }
        }

        private string dodge;

        public string Dodge
        {
            get { return dodge; }
            set { SetProperty(ref dodge, value); }
        }

        private string points;

        public string Points
        {
            get { return points; }
            set { SetProperty(ref points, value); }
        }

        private string honor;

        public string Honor
        {
            get { return honor; }
            set { SetProperty(ref honor, value); }
        }

        private string money;

        public string Money
        {
            get { return money; }
            set { SetProperty(ref money, value); }
        }

        private bool pointIsVisible;

        public bool PointIsVisible
        {
            get { return pointIsVisible; }
            set { SetProperty(ref pointIsVisible, value); }
        }

        private Tuple<string,string> armorItem;

        public Tuple<string, string> ArmorItem
        {
            get { return armorItem; }
            set { SetProperty(ref armorItem, value); }
        }

        private Tuple<string, string> ringItem;

        public Tuple<string, string> RingItem
        {
            get { return ringItem; }
            set { SetProperty(ref ringItem, value); }
        }

        private Tuple<string, string> neckItem;

        public Tuple<string, string> NeckItem
        {
            get { return neckItem; }
            set { SetProperty(ref neckItem, value); }
        }

        private Tuple<string, string> lHandItem;

        public Tuple<string, string> LHandItem
        {
            get { return lHandItem; }
            set { SetProperty(ref lHandItem, value); }
        }

        private Tuple<string, string> rHandItem;

        public Tuple<string, string> RHandItem
        {
            get { return rHandItem; }
            set { SetProperty(ref rHandItem, value); }
        }

        private string tradeIcon;

        public string TradeIcon
        {
            get { return tradeIcon; }
            set { SetProperty(ref tradeIcon, value); }
        }

        private string arenaIcon;

        public string ArenaIcon
        {
            get { return arenaIcon; }
            set { SetProperty(ref arenaIcon, value); }
        }

        private bool lastQRIsVisible;

        public bool LastQRIsVisible
        {
            get { return lastQRIsVisible; }
            set { SetProperty(ref lastQRIsVisible, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Init();
        }

        public override async Task ToSettings()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("mode", "team");

            await NavigateTo(NavigationLinks.SettingsPage, param);
        }

        #endregion

        #region Private functions

        private void Init()
        {
            teamProfile = DataRepository.GetTeamProfile();
            SetItems();

            TradeIcon = teamProfile.TradeStatus != TradeStatusEnum.Finished ? "@drawable/ic_trade_whiteIP.png" : "@drawable/ic_trade_white.png";

            ArenaIcon = GetArenaIcon();

            LastQRIsVisible = !string.IsNullOrEmpty(teamProfile.TradeLastQR);

            Name = teamProfile.Name;

            Image = teamProfile.Image;
            Lvl = "Lvl." + teamProfile.Level + " " + teamProfile.Cast;
            Exp = "EXP: " + teamProfile.Exp + "/" + teamProfile.XpForNextLevel;
            Honor = "Honor: " + teamProfile.Honor;
            Money = "Pénz: " + teamProfile.Money;

            Points = "Elosztható pontok: " + teamProfile.StatPoint;
            PointIsVisible = teamProfile.StatPoint > 0;

            var equippedItems = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped).ToList();

            int batk = teamProfile.Atk;
            int bdef = teamProfile.Def;
            int bstamina = teamProfile.Stamina;
            int bcr = teamProfile.CritR;
            int bdodge = teamProfile.Dodge;
            int bhp = teamProfile.Hp;

            profileQr = new ProfileQr(teamProfile.Name, teamProfile.Atk, teamProfile.Def, teamProfile.CritR, teamProfile.Dodge, teamProfile.Score);

            foreach (var item in equippedItems)
            {
                batk += item.Atk;
                bdef += item.Def;
                bstamina += item.Stamina;
                bcr += item.CritR;
                bdodge += item.Dodge;
                bhp += (item.Stamina * teamProfile.HpValue);
                profileQr.Equipped.Add(item);
            }

            bcr = bcr >= 60 ? 60 : bcr;
            bdodge = bdodge >= 60 ? 60 : bdodge;

            Atk = "ATK: " + batk;
            Def = "DEF: " + bdef;
            Stamina = "STM: " + bstamina;

            Hp = "HP: " + bhp;
            CritR = "CritR: " + bcr + "%";
            Dodge = "Dodge: " + bdodge + "%";

            combatUnit = new CombatUnit(teamProfile.Name, batk, bdef, bcr, bdodge, bhp, teamProfile.House.ToString(), teamProfile.Image);
        }

        private void SetItems()
        {
            var armor = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Armor).ToList();
            ArmorItem = new Tuple<string, string>(armor.Count != 0 ? armor[0].Image : "@drawable/chest.png", armor.Count != 0 ? armor[0].SimpleString : "Nincs páncél.");

            var ring = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Ring).ToList();
            RingItem = new Tuple<string, string>(ring.Count != 0 ? ring[0].Image : "@drawable/ring.png", ring.Count != 0 ? ring[0].SimpleString : "Nincs gyűrű.");

            var neck = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Neck).ToList();
            NeckItem = new Tuple<string, string>(neck.Count != 0 ? neck[0].Image : "@drawable/neck.png", neck.Count != 0 ? neck[0].SimpleString : "Nincs nyaklánc.");

            var lhand = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.LHand).ToList();
            LHandItem = new Tuple<string, string>(lhand.Count != 0 ? lhand[0].Image : "@drawable/weapon.png", lhand.Count != 0 ? lhand[0].SimpleString : "Nincs fegyver.");

            var rhand = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.RHand).ToList();
            RHandItem = new Tuple<string, string>(rhand.Count != 0 ? rhand[0].Image : "@drawable/weapon.png", rhand.Count != 0 ? rhand[0].SimpleString : "Nincs fegyver.");
        }

        private async Task ShowProfileAsync()
        {
            var qrCode = qRService.CreateQR(profileQr);
            NavigationParameters param = new NavigationParameters();
            param.Add("code", qrCode);

            await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
        }


        private async Task GetProfileQRAsync()
        {

#if DEV

            var res = await DialogService.DisplayAlertAsync(Texts.ArenaTitle, Texts.FightQuestion, Texts.Yes, Texts.No);
            if (res)
            {
                var qrCode = qRService.CreateQR(combatUnit);
                NavigationParameters param = new NavigationParameters();
                param.Add("code", qrCode);

                await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
            }

#else

            logs = DataRepository.GetLogs();

            if(logs.Count == 0)
            {
                var res = await DialogService.DisplayAlertAsync(Texts.ArenaTitle, Texts.FightQuestion, Texts.Yes, Texts.No);
                if (res)
                {
                    var qrCode = qRService.CreateQR(combatUnit);
                    NavigationParameters param = new NavigationParameters();
                    param.Add("code", qrCode);

                    await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                }
            }
            else
            {
                var arenas = logs.FirstOrDefault(x => x.LogType == LogTypeEnum.Arena);

                if (arenas != null && arenas.Date.AddMinutes(15) > DateTime.Now)
                {
                    await DialogService.DisplayAlertAsync(Texts.Error, Texts.CantFightYet + arenas.Date.AddMinutes(15), Texts.Sadface);
                }
                else
                {
                    var res = await DialogService.DisplayAlertAsync(Texts.ArenaTitle, Texts.FightQuestion, Texts.Yes, Texts.No);
                    if (res)
                    {
                        var qrCode = qRService.CreateQR(combatUnit);
                        NavigationParameters param = new NavigationParameters();
                        param.Add("code", qrCode);

                        await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
                    }
                }
            }

#endif

        }

        private async Task StartTradeAsync()
        {
            if(teamProfile.TradeStatus == TradeStatusEnum.Finished || teamProfile.TradeStatus == TradeStatusEnum.TradeGetAndGive)
            {
                await NavigateTo(NavigationLinks.TradePage);
            }
            else if(teamProfile.TradeStatus == TradeStatusEnum.TradeGive || teamProfile.TradeStatus == TradeStatusEnum.TradeGiveAndGet || teamProfile.TradeStatus == TradeStatusEnum.TradeFirstOk || teamProfile.TradeStatus == TradeStatusEnum.TradeSecondOk)
            {
                await NavigateTo(NavigationLinks.QRScannerPage );
            }
        }

        private void RaiseStat(object a)
        {
            if (!IsBusy && teamProfile.StatPoint > 0)
            {
                IsBusy = true;

                switch ((int)a)
                {
                    case 1:
                        teamProfile.Stamina += 1;
                        break;
                    case 2:
                        teamProfile.Atk += 1;
                        break;
                    case 3:
                        teamProfile.Def += 1;
                        break;
                    default:
                        break;
                }

                DataRepository.SaveToFile(teamProfile);
                Init();
                IsBusy = false;
            }

        }

        private string GetArenaIcon()
        {
            logs = DataRepository.GetLogs();
            var arenas = logs.FirstOrDefault(x => x.LogType == LogTypeEnum.Arena);

            if (arenas != null && arenas.Date.AddMinutes(15) > DateTime.Now)
            {
                return "@drawable/ic_arena_whiteIP.png";
            }
            return "@drawable/ic_arena_white.png";
        }

        private async Task ToLastTradeQR()
        {
            var qrCode = teamProfile.TradeLastQR;
            NavigationParameters param = new NavigationParameters();
            param.Add("code", qrCode);
            await NavigateToWithoutHistory(NavigationLinks.QRPage, param);
        }

        private async Task ShowInfo()
        {
            await DialogService.DisplayAlertAsync("Info","Egy stamina == "+teamProfile.HpValue+" HP\nEgy Def 0.33%-al csökkenti a bekapott sebzést.\nCrit Rate és Dodge maximum 60% lehet.\nA Crit Rate a kritikus ütés esélye, ami duplán sebez.\nA Dodge a kitérésé, akkor nem kapsz be sebzést az adott körben.\n 1 ATK == random.Next(0,3) sebzés.","Cool");
        }

#endregion
    }
}
