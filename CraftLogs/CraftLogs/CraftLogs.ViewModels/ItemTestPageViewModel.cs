using System;
using System.Collections.ObjectModel;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class ItemTestPageViewModel : ViewModelBase
    {
        private readonly IItemGeneratorService itemGeneratorService;

        private ObservableCollection<string> itemsList;

        public ObservableCollection<string> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value); }
        }
        User player1;
        User player2;
        User player3;
        User player4;
        User player5;

        public ItemTestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService) : base(navigationService, dataRepository, dialogService)
        {
            this.itemGeneratorService = itemGeneratorService;
            Title = "Item Generator Test Page";
            player1 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Dagger, 1, BLL.Enums.ItemRarityEnum.Common));
            player2 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Sword, 1, BLL.Enums.ItemRarityEnum.Common));
            player3 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Axe, 1, BLL.Enums.ItemRarityEnum.Common));
            player4 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Spear, 1, BLL.Enums.ItemRarityEnum.Common));
            player5 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Hammer, 1, BLL.Enums.ItemRarityEnum.Common));
            ItemsList = new ObservableCollection<string>();

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //ItemsList = GenerateTestItems();
            DoTest(player1, player2);
            DoTest(player1, player3);
            DoTest(player1, player4);
            DoTest(player1, player5);

            player1 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Dagger, 3, BLL.Enums.ItemRarityEnum.Epic));
            player2 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Sword, 3, BLL.Enums.ItemRarityEnum.Epic));
            player3 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Axe, 3, BLL.Enums.ItemRarityEnum.Epic));
            player4 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Spear, 3, BLL.Enums.ItemRarityEnum.Epic));
            player5 = new User(this.itemGeneratorService.GenerateWeapon(BLL.Enums.ItemSubTypeEnum.Hammer, 3, BLL.Enums.ItemRarityEnum.Epic));

            DoTest(player1, player2);
            DoTest(player1, player3);
            DoTest(player1, player4);
            DoTest(player1, player5);
        }

        private void DoTest(User pl1, User pl2)
        {
            IsBusy = true;
            int p1 = 0;
            int p2 = 0;

            for (int i = 1; i <= 10000; i++)
            {
                p1 += pl1.Attack();
                p2 += pl2.Attack();
            }
            p1 /= 10000;
            p2 /= 10000;

            ItemsList.Add(string.Format("Tier: {4}, {5} \n {0} vs. {1} \n {2} - {3}", pl1.Weapon.ItemSubType.ToString(), pl2.Weapon.ItemSubType.ToString(), p1, p2, pl1.Weapon.Tier, pl1.Weapon.Rarity.ToString()));

            IsBusy = false;
        }

        private ObservableCollection<string> GenerateTestItems()
        {
            ObservableCollection<string> res = new ObservableCollection<string>();
            for (int i = 1; i < 6; i++)
            {
                int tier = 1;
                int rarity = 0;
                for (int j = 1; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        res.Add(itemGeneratorService.GenerateWeapon((BLL.Enums.ItemSubTypeEnum)i, tier, (BLL.Enums.ItemRarityEnum)rarity).ToString());
                        if (rarity == 3)
                        {
                            rarity = 0;
                        }
                        else
                        {
                            rarity++;
                        }
                    }
                    if (tier == 3)
                    {
                        tier = 1;
                    }
                    else
                    {
                        tier++;
                    }
                }
            }

            return res;
        }
    }
    public class User
    {
        public int Hit { get; set; } = 40;
        public int Crit { get; set; } = 15;
        public double CritD { get; } = 1.5;
        public Item Weapon { get; set; }
        private Random random;

        public User(Item weapon)
        {
            Weapon = weapon;
            //Hit += weapon.HitRate;
            //Crit += weapon.CritRate;
            random = new Random();
        }

        public int Attack()
        {
            int res = 0;

            for (int i = 0; i <= Weapon.Speed; i++)
            {
                res += GetDmg();
            }

            return res;
        }

        private bool IsHit()
        {
            return Hit <= random.Next(1, 101);
        }

        private bool IsCrit()
        {
            return Crit <= random.Next(1, 101);
        }

        private int GetDmg()
        {
            /*if (IsHit())
            {
                if (IsCrit())
                {
                    return (int)(random.Next(Weapon.MinDps, Weapon.Dps + 1) * CritD);
                }
                return random.Next(Weapon.MinDps, Weapon.Dps + 1);
            }
            return 0;*/
            return Weapon.Dps;
        }
    }
}
