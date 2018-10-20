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
        private readonly IItemGeneratorService ItemGeneratorService;

        private ObservableCollection<string> itemsList;

        public ObservableCollection<string> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value); }
        }
        User player1;
        User player2;

        public ItemTestPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService, IItemGeneratorService itemGeneratorService) : base(navigationService, dataRepository, dialogService)
        {
            ItemGeneratorService = itemGeneratorService;
            Title = "Item Generator Test Page";
            ItemsList = new ObservableCollection<string>();

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //ItemsList = GenerateTestItems();
            player1 = Generateuser(1, 0);
            player2 = Generateuser(1, 0);
            GenerateTest();

        }

        private void GenerateTest()
        {
            ItemsList.Add("Tier 1, Common Characters");
            ItemsList.Add(player1.ToString());
            ItemsList.Add(player2.ToString());
            ItemsList.Add("Tier3, Epic Characters");
            player1 = Generateuser(3, 3);
            player2 = Generateuser(3, 3);
            ItemsList.Add(player1.ToString());
            ItemsList.Add(player2.ToString());
        }

        private User Generateuser(int tier, int rarity)
        {
            Item Head = ItemGeneratorService.GenerateHead(tier, (BLL.Enums.ItemRarityEnum)rarity);
            Item Chest = ItemGeneratorService.GenerateChest(tier, (BLL.Enums.ItemRarityEnum)rarity);
            Item Boots = ItemGeneratorService.GenerateBoots(tier, (BLL.Enums.ItemRarityEnum)rarity);
            Item Trinket = ItemGeneratorService.GenerateTrinket(tier, (BLL.Enums.ItemRarityEnum)rarity);
            Item RHand = ItemGeneratorService.GenerateRHand(tier, (BLL.Enums.ItemRarityEnum)rarity);
            Item LHand = null;
            if (RHand.ItemSubType != BLL.Enums.ItemSubTypeEnum.Spear && RHand.ItemSubType != BLL.Enums.ItemSubTypeEnum.Hammer)
            {
                LHand = ItemGeneratorService.GenerateLHand(tier, (BLL.Enums.ItemRarityEnum)rarity);
            }
            return new User(Head, Chest, Boots, RHand, LHand, Trinket);
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
                        res.Add(ItemGeneratorService.GenerateWeapon((BLL.Enums.ItemSubTypeEnum)i, tier, (BLL.Enums.ItemRarityEnum)rarity).ToString());
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
        public double CritD { get; set; } = 1.5;
        public int Agility { get; set; } = 15;
        public int Armor { get; set; } = 0;
        public int Stamina { get; set; } = 50;
        public Item Head { get; set; }
        public Item Chest { get; set; }
        public Item Boots { get; set; }
        public Item RHand { get; set; }
        public Item LHand { get; set; }
        public Item Trinket { get; set; }
        public int HP { get { return Stamina * 10; } }

        private Random random;
        public User(Item head, Item chest, Item boots, Item rhand, Item lHand, Item trinket)
        {
            Head = head;
            Chest = chest;
            Boots = boots;
            Trinket = trinket;
            RHand = rhand;
            LHand = lHand;

            UpdateAll();
            random = new Random();
        }

        private void UpdateAll()
        {
            Update(Head);
            Update(Chest);
            Update(Boots);
            Update(Trinket);
            Update(RHand);
            if (LHand != null)
            {
                Update(LHand);
            }
        }

        private void Update(Item item)
        {
            if (item != null)
            {
                Hit += item.HitRate;
                Crit += item.CritRate;
                CritD += item.CritDamage;
                Agility += item.Agility;
                Armor += item.Armor;
                Stamina += item.Stamina;
            }
        }

        public int Attack()
        {
            int res = Attack(RHand);

            if (LHand != null)
            {
                res += Attack(LHand);
            }

            return res;
        }

        private int Attack(Item weapon)
        {
            int res = 0;

            if (IsHit())
            {
                for (int i = 0; i < weapon.Speed; i++)
                {
                    res += GetDmg(weapon);
                }
            }

            return res;
        }

        private bool IsHit()
        {
            return random.Next(1, 101) <= Hit;
        }

        private bool IsCrit()
        {
            return random.Next(1, 101) <= Crit;
        }

        private int GetDmg(Item weapon)
        {
            if (IsCrit())
            {
                return (int)(random.Next(weapon.MinDps, weapon.Dps + 1) * CritD);
            }

            return random.Next(weapon.MinDps, weapon.Dps + 1);

        }

        public override string ToString()
        {
            string res = "";

            res += string.Format("HP: {0} \n", HP);
            res += string.Format("Armor: {0}% \n", Armor);
            res += string.Format("Stamina: {0} \n", Stamina);
            res += string.Format("Agility: {0}% \n", Agility);
            res += string.Format("Hit Rate: {0}% \n", Hit);
            res += string.Format("Crit Rate: {0}% \n", Crit);
            res += string.Format("Crit Damage: {0}% \n", CritD * 100);
            if (LHand != null)
            {
                res += string.Format("Damage: {0} \n", (RHand.Dps * RHand.Speed) + (LHand.Dps * LHand.Speed));
            }
            else
            {
                res += string.Format("Damage: {0} \n", RHand.Dps * RHand.Speed);
            }
            return res;
        }
    }
}
