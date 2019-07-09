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

using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
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

        public DelegateCommand GetProfileQRCommand => getProfileQRCommand ?? (getProfileQRCommand = new DelegateCommand(async () => await GetProfileQRAsync(), CanSubmit).ObservesProperty(()=>IsBusy));

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

        private Item tempItem;

        public Item TempItem
        {
            get { return tempItem; }
            set { SetProperty(ref tempItem, value); }
        }

        private Item armorItem;

        public Item ArmorItem
        {
            get { return armorItem; }
            set { SetProperty(ref armorItem, value); }
        }

        private Item trinketItem1;

        public Item TrinketItem1
        {
            get { return trinketItem1; }
            set { SetProperty(ref trinketItem1, value); }
        }

        private Item trinketItem2;

        public Item TrinketItem2
        {
            get { return trinketItem2; }
            set { SetProperty(ref trinketItem2, value); }
        }

        private Item weaponItem1;

        public Item WeaponItem1
        {
            get { return weaponItem1; }
            set { SetProperty(ref weaponItem1, value); }
        }

        private Item weaponItem2;

        public Item WeaponItem2
        {
            get { return weaponItem2; }
            set { SetProperty(ref weaponItem2, value); }
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

            Name = teamProfile.Name;

            Image = teamProfile.Image;
            Lvl = "Lvl." + teamProfile.Level + " " + teamProfile.Cast;
            Exp = "EXP: " + teamProfile.Exp + "/" + teamProfile.XpForNextLevel;
            Honor = "Honor: " + teamProfile.Honor;
            Money = "Pénz: " + teamProfile.Money;

            Points = "Elosztható pontok: " + teamProfile.StatPoint;
            PointIsVisible = teamProfile.StatPoint > 0;

            Atk = "ATK: " + teamProfile.Atk;
            Def = "DEF: " + teamProfile.Def;
            Stamina = "STM: " + teamProfile.Stamina;

            Hp = "HP: " + teamProfile.Hp;
            CritR = "CritR: " + teamProfile.CritR + "%";
            Dodge = "Dodge: " + teamProfile.Dodge + "%";

            SetItems();
        }

        private void SetItems()
        {
            //For testing
            TempItem = new Item(1, BLL.Enums.ItemRarityEnum.Rare, BLL.Enums.ItemTypeEnum.Armor, BLL.Enums.CharacterClassEnum.Mage, "4 2 5 0 6");

            ArmorItem = TempItem;

            TrinketItem1 = TempItem;
            TrinketItem2 = TempItem;

            WeaponItem1 = TempItem;
            WeaponItem2 = TempItem;
        }

        //DEVTEST
        private async Task GetProfileQRAsync()
        {
            IsBusy = false;
            teamProfile.Inventory.Add(itemGenerator.GetRandomItem(1));
            teamProfile.AllExp += 1;
            DataRepository.SaveToFile(teamProfile);
            Init();
            await DialogService.DisplayAlertAsync("ok", "1 item added to inventory \n 1 exp added", "ok");
        }

        private void RaiseStat(object a)
        {
            if(!IsBusy && teamProfile.StatPoint > 0)
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
