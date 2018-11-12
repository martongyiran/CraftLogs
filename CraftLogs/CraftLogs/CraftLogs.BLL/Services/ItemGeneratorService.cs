using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Services.Interfaces;
using System;

namespace CraftLogs.BLL.Services
{
    public class ItemGeneratorService : IItemGeneratorService
    {
        #region Private
        private Random random;

        private void GetBonusStat(Item item)
        {
            switch (item.Rarity)
            {
                case ItemRarityEnum.Common:
                    if(item.ItemType == ItemTypeEnum.Trinket)
                    {
                        GenerateBonusStat(item);
                    }
                    break;
                case ItemRarityEnum.Uncommon:
                    GenerateBonusStat(item);
                    break;
                case ItemRarityEnum.Rare:
                    GenerateBonusStat(item);
                    GenerateBonusStat(item);
                    break;
                case ItemRarityEnum.Epic:
                    GenerateBonusStat(item);
                    GenerateBonusStat(item);
                    GenerateBonusStat(item);
                    break;
                case ItemRarityEnum.Legend:
                    GenerateBonusStat(item);
                    GenerateBonusStat(item);
                    GenerateBonusStat(item);
                    GenerateBonusStat(item);
                    break;
                default:
                    break;
            }
        }

        private void GenerateBonusStat(Item item)
        {
            int luck = random.Next(1, 5);
            switch (luck)
            {
                case 1:
                    item.Strength += item.GetTierPlusRarity(); //todo
                    break;
                case 2:
                    item.Agility += item.GetTierPlusRarity(); //todo
                    break;
                case 3:
                    item.Stamina += item.GetTierPlusRarity(); //todo
                    break;
                case 4:
                    item.Intellect += item.GetTierPlusRarity(); //todo
                    break;
            }
        }

        private SetNameEnum GetSetName()
        {
            return (SetNameEnum)random.Next(1, 5);
        }

        private Item GenerateArmor(ItemTypeEnum itemType, int tier, ItemRarityEnum rarity)
        {
            Item generated = new Item(tier, rarity, itemType);
            GetBonusStat(generated);
            generated.SetName = GetSetName();

            return generated;
        }

        private void GenerateHand(Item generated)
        {
            SetSubType(generated);
            GetBonusStat(generated);
            generated.SetName = GetSetName();
        }

        private void SetSubType(Item generated)
        {
           generated.ItemSubType = (ItemSubTypeEnum)random.Next(1, 7);
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

        public Item GenerateHand(int tier, ItemRarityEnum rarity)
        {
            Item generated = new Item(tier, rarity, ItemTypeEnum.Hand);
            GenerateHand(generated);

            return generated;
        }

        public Item GenerateTrinket(int tier, ItemRarityEnum rarity)
        {
            Item generated = new Item(tier, rarity, ItemTypeEnum.Trinket);

            GetBonusStat(generated);
            generated.SetName = GetSetName();

            return generated;
            
        }

        public Item GenerateRandom()
        {
            var type = random.Next(1, 6);
            var tier = random.Next(1, 4);
            var rarity = random.Next(0, 4);

            switch (type)
            {
                case 1:
                    return GenerateBoots(tier, (ItemRarityEnum)rarity);
                case 2:
                    return GenerateChest(tier, (ItemRarityEnum)rarity);
                case 3:
                    return GenerateHead(tier, (ItemRarityEnum)rarity);
                case 4:
                    return GenerateHand(tier, (ItemRarityEnum)rarity);
                case 5:
                    return GenerateTrinket(tier, (ItemRarityEnum)rarity);
                default:
                    return new Item();
            }

        }

        public Item GenerateWeapon(ItemSubTypeEnum itemSubType, int tier, ItemRarityEnum rarity)
        {
            Item generated = new Item(tier, rarity, ItemTypeEnum.Hand);

            generated.ItemSubType = itemSubType;
            GetBonusStat(generated);
            generated.SetName = GetSetName();

            return generated;
        }

        #endregion
    }
}
