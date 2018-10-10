﻿using System;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.Values;

namespace CraftLogs.BLL.Helpers
{
    public static class ItemConverter
    {
        public static string ItemToItemName(Item item)
        {
            var res = ItemSetNameEnumToString(item.SetName) + " ";
            if(item.ItemSubType != ItemSubTypeEnum.None)
            {
                res += ItemSubTypeToName(item.ItemSubType);
            }
            else
            {
                res += ItemTypeToName(item.ItemType);
            }

            return res;
        }

        private static string ItemTypeToName(ItemTypeEnum itemType)
        {
            switch (itemType)
            {
                case ItemTypeEnum.Head:
                    return Texts.Head;
                case ItemTypeEnum.Chest:
                    return Texts.Chest;
                case ItemTypeEnum.Boots:
                    return Texts.Boots;
                case ItemTypeEnum.Trinket:
                    return Texts.Trinket;
                default:
                    return "";
            }
        }

        public static string ItemSetNameEnumToString(SetNameEnum setName)
        {
            switch (setName)
            {
                case SetNameEnum.None:
                    return "";
                case SetNameEnum.Set1:
                    return Texts.Set1;
                case SetNameEnum.Set2:
                    return Texts.Set2;
                case SetNameEnum.Set3:
                    return Texts.Set3;
                case SetNameEnum.Set4:
                    return Texts.Set4;
                case SetNameEnum.Set5:
                    return Texts.Set5;
                default:
                    return "";
            }
        }

        public static int TierToRate(int tier)
        {
            switch (tier)
            {
                case 1:
                    return 1;
                case 2:
                    return 4;
                case 3:
                    return 7;
                default:
                    return 0;
            }
        }

        public static int RarityToRate(ItemRarityEnum itemRarity)
        {
            switch (itemRarity)
            {
                case ItemRarityEnum.Common:
                    return 0;
                case ItemRarityEnum.Uncommon:
                    return 1;
                case ItemRarityEnum.Rare:
                    return 2;
                case ItemRarityEnum.Epic:
                    return 3;
                default:
                    return 0;
            }
        }

        public static int ItemSubTypeToDps(ItemSubTypeEnum itemSubType)
        {
            switch (itemSubType)
            {
                case ItemSubTypeEnum.None:
                    return 0;
                case ItemSubTypeEnum.Dagger:
                    return 27;
                case ItemSubTypeEnum.Sword:
                    return 33;
                case ItemSubTypeEnum.Axe:
                    return 43;
                case ItemSubTypeEnum.Spear:
                    return 100;
                case ItemSubTypeEnum.Hammer:
                    return 150;
                case ItemSubTypeEnum.Shield:
                    return 0;
                default:
                    return 0;
            }
        }

        public static int ItemSubTypeToSpeed(ItemSubTypeEnum itemSubType)
        {
            switch (itemSubType)
            {
                case ItemSubTypeEnum.None:
                    return 0;
                case ItemSubTypeEnum.Dagger:
                    return 6;
                case ItemSubTypeEnum.Sword:
                    return 5;
                case ItemSubTypeEnum.Axe:
                    return 4;
                case ItemSubTypeEnum.Spear:
                    return 3;
                case ItemSubTypeEnum.Hammer:
                    return 2;
                case ItemSubTypeEnum.Shield:
                    return 0;
                default:
                    return 0;
            }
        }

        private static string ItemSubTypeToName(ItemSubTypeEnum itemSubType)
        {
            switch (itemSubType)
            {
                case ItemSubTypeEnum.None:
                    return "";
                case ItemSubTypeEnum.Dagger:
                    return Texts.Dagger;
                case ItemSubTypeEnum.Sword:
                    return Texts.Sword;
                case ItemSubTypeEnum.Axe:
                    return Texts.Axe;
                case ItemSubTypeEnum.Spear:
                    return Texts.Spear;
                case ItemSubTypeEnum.Hammer:
                    return Texts.Hammer;
                case ItemSubTypeEnum.Shield:
                    return Texts.Shield;
                case ItemSubTypeEnum.Food:
                    return Texts.Food;
                default:
                    return "";
            }
        }
    }
}
