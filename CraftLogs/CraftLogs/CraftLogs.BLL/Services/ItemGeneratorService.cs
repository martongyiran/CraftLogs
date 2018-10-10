using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Helpers;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Services.Interfaces;
using System;

namespace CraftLogs.BLL.Services
{
    public class ItemGeneratorService : IItemGeneratorService
    {
        #region Private
        private Random random;
        private int DpsToMinDmg(Item item)
        {
            return 0;
        }
        private int DpsToMaxDmg(Item item)
        {
            return 0;
        }

        private int GetDefRate(int tier, ItemRarityEnum rarity)
        {
            return (int)((ItemRateConverter.TierToRate(tier) + ItemRateConverter.RarityToRate(rarity)) * 1.5);
        }

        private int GetArmor(int tier, ItemRarityEnum rarity)
        {
            return (int)((ItemRateConverter.TierToRate(tier) + ItemRateConverter.RarityToRate(rarity)) * 12.5);
        }

        private void GetBonusStat(Item item)
        {
            if (item.Rarity != ItemRarityEnum.Common)
            {
                int luck = random.Next(1, 3);
                switch (luck)
                {
                    case 1:
                        item.CritRate = GetBonusSecStat(item.Tier, item.Rarity);
                        break;
                    case 2:
                        item.Agility = GetBonusSecStat(item.Tier, item.Rarity);
                        break;
                    case 3:
                        item.Stamina = GetBonusSecStat(item.Tier, item.Rarity);
                        break;
                }
            }
        }

        private int GetBonusSecStat(int tier, ItemRarityEnum rarity)
        {
            return (ItemRateConverter.TierToRate(tier) + ItemRateConverter.RarityToRate(rarity));
        }

        #endregion

        #region Ctor
        public ItemGeneratorService()
        {
            random = new Random();
        }

        #endregion

        #region Public functions
        public Item GenerateBoots(int tier, ItemRarityEnum rarity)
        {
            Item generated = new Item(tier, rarity, ItemTypeEnum.Boots);
            generated.DefRate = GetDefRate(tier, rarity);
            generated.Armor = GetArmor(tier, rarity);
            GetBonusStat(generated);
            return generated;
        }

        public Item GenerateChest(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateConsumable(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateHead(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateLHand(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateRHand(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateTrinket(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
