﻿using CraftLogs.BLL.Enums;
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
            return (int)(GetTierPlusRarity(tier, rarity) * 1.5);
        }

        private int GetHitRate(int tier, ItemRarityEnum rarity)
        {
            return GetTierPlusRarity(tier, rarity) * 2;
        }

        private int GetArmor(int tier, ItemRarityEnum rarity)
        {
            return (int)(GetTierPlusRarity(tier, rarity) * 12.5);
        }

        private void GetBonusStat(Item item)
        {
            if (item.Rarity != ItemRarityEnum.Common)
            {
                int luck = random.Next(1, 4);
                switch (luck)
                {
                    case 1:
                        item.CritRate = GetTierPlusRarity(item.Tier, item.Rarity);
                        break;
                    case 2:
                        item.Agility = GetTierPlusRarity(item.Tier, item.Rarity);
                        break;
                    case 3:
                        item.Stamina = GetTierPlusRarity(item.Tier, item.Rarity);
                        break;
                }
            }
        }

        private int GetTierPlusRarity(int tier, ItemRarityEnum rarity)
        {
            return (ItemConverter.TierToRate(tier) + ItemConverter.RarityToRate(rarity));
        }

        private SetNameEnum GetSetName()
        {
            return (SetNameEnum)random.Next(1, 5);
        }

        private Item GenerateArmor(ItemTypeEnum itemType, int tier, ItemRarityEnum rarity)
        {
            Item generated = new Item(tier, rarity, itemType);
            generated.DefRate = GetDefRate(tier, rarity);
            generated.Armor = GetArmor(tier, rarity);
            generated.Ilvl = GetTierPlusRarity(tier, rarity) * 10;
            GetBonusStat(generated);
            generated.SetName = GetSetName();
            generated.Name = ItemConverter.ItemToItemName(generated);

            return generated;
        }

        private void SetSubType(Item generated)
        {
            switch (generated.ItemType)
            {
                case ItemTypeEnum.RHand:
                    generated.ItemSubType = (ItemSubTypeEnum)random.Next(1, 6);
                    break;
                case ItemTypeEnum.LHand:
                    generated.ItemSubType = random.Next(1, 5) == 4 ? ItemSubTypeEnum.Shield : (ItemSubTypeEnum)random.Next(1, 4);
                    break;
                default:
                    break;
            }
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
            return GenerateArmor(ItemTypeEnum.Boots, tier, rarity);
        }

        public Item GenerateChest(int tier, ItemRarityEnum rarity)
        {
            return GenerateArmor(ItemTypeEnum.Chest, tier, rarity);
        }

        public Item GenerateConsumable(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateHead(int tier, ItemRarityEnum rarity)
        {
            return GenerateArmor(ItemTypeEnum.Head, tier, rarity);
        }

        public Item GenerateLHand(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateRHand(int tier, ItemRarityEnum rarity)
        {
            Item generated = new Item(tier, rarity, ItemTypeEnum.RHand);
            SetSubType(generated);
            generated.HitRate = GetHitRate(tier, rarity);
            generated.Speed = ItemConverter.ItemSubTypeToSpeed(generated.ItemSubType);
            generated.Dps = ItemConverter.ItemSubTypeToDps(generated.ItemSubType);
            generated.MinDmg = ItemConverter.ItemSubTypeToMinDmg(generated.ItemSubType);
            generated.MaxDmg = ItemConverter.ItemSubTypeToMaxDmg(generated.ItemSubType);
            generated.Ilvl = GetTierPlusRarity(tier, rarity) * 10;
            GetBonusStat(generated);
            generated.SetName = GetSetName();
            generated.Name = ItemConverter.ItemToItemName(generated);

            return generated;
        }

        public Item GenerateTrinket(int tier, ItemRarityEnum rarity)
        {
            throw new NotImplementedException();
        }

        public Item GenerateRandom()
        {
            var type = random.Next(1, 4);
            var tier = random.Next(1, 4);
            var rarity = random.Next(1, 5);

            switch (type)
            {
                case 1:
                    return GenerateBoots(tier, (ItemRarityEnum)rarity);
                case 2:
                    return GenerateChest(tier, (ItemRarityEnum)rarity);
                case 3:
                    return GenerateHead(tier, (ItemRarityEnum)rarity);
                default:
                    return new Item();
            }

        }
        #endregion
    }
}
