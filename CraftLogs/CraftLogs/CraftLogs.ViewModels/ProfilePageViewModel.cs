﻿/*
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
using System.Linq;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {

        #region Private

        private TeamProfile teamProfile;
        private IItemGeneratorService itemGenerator; //TEMP

        private DelegateCommand navigateToLogsCommand;
        private DelegateCommand navigateToSettingsCommand;
        private DelegateCommand navigateToQRScannerPageCommand;
        private DelegateCommand navigateToInventoryPageCommand;
        private DelegateCommand getProfileQRCommand;
        private DelegateCommand<object> raiseStatCommand;

        #endregion

        #region Public

        public DelegateCommand NavigateToLogsCommand => navigateToLogsCommand ?? (navigateToLogsCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.LogsPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand NavigateToSettingsCommand => navigateToSettingsCommand ?? (navigateToSettingsCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.SettingsPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand NavigateToQRScannerPageCommand => navigateToQRScannerPageCommand ?? (navigateToQRScannerPageCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.QRScannerPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand NavigateToInventoryPageCommand => navigateToInventoryPageCommand ?? (navigateToInventoryPageCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.InventoryPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand GetProfileQRCommand => getProfileQRCommand ?? (getProfileQRCommand = new DelegateCommand(async () => await GetProfileQRAsync(), CanSubmit).ObservesProperty(() => IsBusy));

        public DelegateCommand<object> RaiseStatCommand => raiseStatCommand ?? (raiseStatCommand = new DelegateCommand<object>((a) => RaiseStat(a)));

        #endregion

        #region Ctor

        public ProfilePageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.ProfilePage;
            itemGenerator = itemGeneratorService; //TEMP
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
            SetItems();

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

            foreach(var item in equippedItems)
            {
                batk += item.Atk;
                bdef += item.Def;
                bstamina += item.Stamina;
                bcr += item.CritR;
                bdodge += item.Dodge;
                bhp += (item.Stamina * 5);
            }


            Atk = "ATK: " + batk;
            Def = "DEF: " + bdef;
            Stamina = "STM: " + bstamina;

            Hp = "HP: " + bhp;
            CritR = "CritR: " + bcr + "%";
            Dodge = "Dodge: " + bdodge + "%";


        }

        private void SetItems()
        {
            var armor = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Armor).ToList();
            ArmorItem = new Tuple<string, string>(armor.Count != 0 ? armor[0].Image : "@drawable/filler.png", armor.Count != 0 ? armor[0].SimpleString : "Nincs páncél.");

            var ring = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Ring).ToList();
            RingItem = new Tuple<string, string>(ring.Count != 0 ? ring[0].Image : "@drawable/filler.png", ring.Count != 0 ? ring[0].SimpleString : "Nincs gyűrű.");

            var neck = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Neck).ToList();
            NeckItem = new Tuple<string, string>(neck.Count != 0 ? neck[0].Image : "@drawable/filler.png", neck.Count != 0 ? neck[0].SimpleString : "Nincs nyaklánc.");

            var lhand = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.LHand).ToList();
            LHandItem = new Tuple<string, string>(lhand.Count != 0 ? lhand[0].Image : "@drawable/filler.png", lhand.Count != 0 ? lhand[0].SimpleString : "Nincs fegyver.");

            var rhand = teamProfile.Inventory.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.RHand).ToList();
            RHandItem = new Tuple<string, string>(rhand.Count != 0 ? rhand[0].Image : "@drawable/filler.png", rhand.Count != 0 ? rhand[0].SimpleString : "Nincs fegyver.");
        }

        //DEVTEST
        private async Task GetProfileQRAsync()
        {
            IsBusy = false;
            teamProfile.Inventory.Add(itemGenerator.GetRandomItem(1));
            teamProfile.Inventory.Add(itemGenerator.GetRandomItem(1));
            teamProfile.Inventory.Add(itemGenerator.GetRandomItem(1));
            teamProfile.AllExp += 1;
            DataRepository.SaveToFile(teamProfile);
            Init();
            await DialogService.DisplayAlertAsync("ok", "3 items added to inventory \n 1 exp added", "ok");
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

        #endregion
    }
}
