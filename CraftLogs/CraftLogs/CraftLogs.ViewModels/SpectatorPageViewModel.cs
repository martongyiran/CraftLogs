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
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class SpectatorPageViewModel : ViewModelBase
    {
        #region Private

        ProfileQr profile;
        private DelegateCommand navigateToQRScannerPageCommand;

        #endregion

        #region Public

        public DelegateCommand NavigateToQRScannerPageCommand => navigateToQRScannerPageCommand ?? (navigateToQRScannerPageCommand = new DelegateCommand(async () => { IsBusy = true; await NavigateTo(NavigationLinks.QRScannerPage); }, CanSubmit).ObservesProperty(() => IsBusy));

        #endregion

        #region Ctor

        public SpectatorPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = "CraftLogs Spectator";
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

        private Tuple<string, string> armorItem;

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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsBusy = true;

            Init();

            IsBusy = false;
        }

        #endregion

        #region Private functions

        private async Task Init()
        {
            DataRepository.CreateSpectatorProfile();
            profile = DataRepository.GetSpectatorProfile();

            SetItems();
            
            Name = profile.a;

            Image = profile.n;
            Lvl = "Lvl." + profile.g + " " + profile.b;
            Exp = "EXP: " + profile.f + "/" + profile.h;
            Honor = "Honor: " + profile.d;
            Money = "Pénz: " + profile.c;
            

            int batk = profile.i;
            int bdef = profile.j;
            int bstamina = profile.k;
            int bcr = profile.l;
            int bdodge = profile.m;
            int bhp = profile.Hp;

            foreach (var item in profile.o)
            {
                batk += item.Atk;
                bdef += item.Def;
                bstamina += item.Stamina;
                bcr += item.CritR;
                bdodge += item.Dodge;
                bhp += (item.Stamina * profile.HpValue);
            }

            bcr = bcr >= 60 ? 60 : bcr;
            bdodge = bdodge >= 60 ? 60 : bdodge;

            Atk = "ATK: " + batk;
            Def = "DEF: " + bdef;
            Stamina = "STM: " + bstamina;

            Hp = "HP: " + bhp;
            CritR = "CritR: " + bcr + "%";
            Dodge = "Dodge: " + bdodge + "%";

        }

        private void SetItems()
        {
            var armor = profile.o.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Armor).ToList();
            ArmorItem = new Tuple<string, string>(armor.Count != 0 ? armor[0].Image : "@drawable/chest.png", armor.Count != 0 ? armor[0].SimpleString : "Nincs páncél.");

            var ring = profile.o.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Ring).ToList();
            RingItem = new Tuple<string, string>(ring.Count != 0 ? ring[0].Image : "@drawable/ring.png", ring.Count != 0 ? ring[0].SimpleString : "Nincs gyűrű.");

            var neck = profile.o.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.Neck).ToList();
            NeckItem = new Tuple<string, string>(neck.Count != 0 ? neck[0].Image : "@drawable/neck.png", neck.Count != 0 ? neck[0].SimpleString : "Nincs nyaklánc.");

            var lhand = profile.o.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.LHand).ToList();
            LHandItem = new Tuple<string, string>(lhand.Count != 0 ? lhand[0].Image : "@drawable/weapon.png", lhand.Count != 0 ? lhand[0].SimpleString : "Nincs fegyver.");

            var rhand = profile.o.Where((arg) => arg.State == ItemStateEnum.Equipped && arg.ItemType == ItemTypeEnum.RHand).ToList();
            RHandItem = new Tuple<string, string>(rhand.Count != 0 ? rhand[0].Image : "@drawable/weapon.png", rhand.Count != 0 ? rhand[0].SimpleString : "Nincs fegyver.");
        }

        #endregion
    }
}
