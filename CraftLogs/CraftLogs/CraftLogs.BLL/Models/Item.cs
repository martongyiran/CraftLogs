using System;
using CraftLogs.BLL.Enums;
using CraftLogs.Values;
using Newtonsoft.Json;

namespace CraftLogs.BLL.Models
{
    public class Item
    {
        public string Id { get; private set; }
        public int Tier { get; set; }
        [JsonIgnore]
        public int Ilvl { get { return GetILvl(); } }
        public ItemRarityEnum Rarity { get; set; }
        [JsonIgnore]
        public string Name { get { return GetItemName(); } }
        public SetNameEnum SetName { get; set; }
        [JsonIgnore]
        public int Value { get { return GetValue(); } }
        public ItemTypeEnum ItemType { get; set; }
        public ItemSubTypeEnum ItemSubType { get; set; } = 0;
        public ItemStateEnum State { get; set; } = 0;
        [JsonIgnore]
        public int Speed { get { return GetSpeed(); } }
        [JsonIgnore]
        public int Dps { get { return GetDps(); } }
        [JsonIgnore]
        public int MinDps { get { return GetMinDps(); } }
        [JsonIgnore]
        public int Armor { get { return GetArmor(); } }
        public int Stamina { get; set; } = 0; //+hp és 1 stamina = +1% armor
        public int Strength { get; set; } = 0; //+1% dmg
        public int Agility { get; set; } = 0; //+x% CR
        public int Intellect { get; set; } = 0; //+x% HitRate
        [JsonIgnore]
        public int HitRate { get { return GetHitRate(); } }

        [JsonIgnore]
        public string Text_TypeRarityILvl { get { return GetTypeRarityILvl(); } }
        [JsonIgnore]
        public string Text_Bonuses { get { return GetBonuses(); } }
        [JsonIgnore]
        public string Text_Values { get { return GetValues(); } }

        #region Ctor
        public Item()
        {
            Id = GenerateId();
        }

        public Item(int tier, ItemRarityEnum rarity, ItemTypeEnum itemType)
        {
            Tier = tier;
            Rarity = rarity;
            ItemType = itemType;
            Id = GenerateId();
        }

        public Item(int tier, ItemRarityEnum rarity, ItemTypeEnum itemType, ItemSubTypeEnum itemSubType)
        {
            Tier = tier;
            Rarity = rarity;
            ItemType = itemType;
            ItemSubType = itemSubType;
            Id = GenerateId();
        }

        #endregion

        #region Getters

        private string GenerateId()
        {
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            var uniqueId = ticks.ToString() + '-' + guid;
            return uniqueId;
        }

        private string GetItemName()
        {
            var res = ItemSetNameEnumToString() + " ";
            if (ItemSubType != ItemSubTypeEnum.None)
            {
                res += ItemSubTypeToString();
            }
            else
            {
                res += ItemTypeToString();
            }

            return res;
        }

        private string ItemTypeToString()
        {
            switch (ItemType)
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

        private string ItemSetNameEnumToString()
        {
            switch (SetName)
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

        private string ItemSubTypeToString()
        {
            switch (ItemSubType)
            {
                case ItemSubTypeEnum.None:
                    return "";
                case ItemSubTypeEnum.Dagger:
                    return Texts.Dagger;
                case ItemSubTypeEnum.Bow:
                    return Texts.Bow;
                case ItemSubTypeEnum.Sword:
                    return Texts.Sword;
                case ItemSubTypeEnum.Hammer:
                    return Texts.Hammer;
                case ItemSubTypeEnum.Wand:
                    return Texts.Wand;
                case ItemSubTypeEnum.Staff:
                    return Texts.Staff;
                default:
                    return "";
            }
        }

        private int GetDps()
        {
            switch (ItemSubType)
            {
                case ItemSubTypeEnum.None:
                    return 0;
                case ItemSubTypeEnum.Dagger:
                    return (int)(20.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Bow:
                    return (int)(25.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Sword:
                    return (int)(33.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Hammer:
                    return (int)(50.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Wand:
                    return (int)(100.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Staff:
                    return (int)(100.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                default:
                    return 0;
            }
        }

        public int TierToRate()
        {
            switch (Tier)
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

        public int RarityToRate()
        {
            switch (Rarity)
            {
                case ItemRarityEnum.Common:
                    return 0;
                case ItemRarityEnum.Uncommon:
                    return 1;
                case ItemRarityEnum.Rare:
                    return 2;
                case ItemRarityEnum.Epic:
                    return 3;
                case ItemRarityEnum.Legend:
                    return 4;
                default:
                    return 0;
            }
        }

        public int GetTierPlusRarity()
        {
            return (TierToRate() + RarityToRate());
        }

        private int GetILvl()
        {
            return GetTierPlusRarity() * 10;
        }

        private int GetMinDps()
        {
            if (Dps != 0)
            {
                return (int)(GetDps() * 0.8);
            }
            return 0;
        }

        private int GetSpeed()
        {
            switch (ItemSubType)
            {
                case ItemSubTypeEnum.None:
                    return 0;
                case ItemSubTypeEnum.Dagger:
                    return 5;
                case ItemSubTypeEnum.Bow:
                    return 4;
                case ItemSubTypeEnum.Sword:
                    return 3;
                case ItemSubTypeEnum.Hammer:
                    return 2;
                case ItemSubTypeEnum.Wand:
                    return 1;
                case ItemSubTypeEnum.Staff:
                    return 1;
                default:
                    return 0;
            }
        }

        private int GetHitRate()
        {
            if (ItemType == ItemTypeEnum.Hand)
            {
                return GetTierPlusRarity() * 2;
            }
            return 0;
        }

        private int GetArmor()
        {
            if (ItemType != ItemTypeEnum.Hand && ItemType != ItemTypeEnum.Trinket)
            {
                return (GetTierPlusRarity() * 10);
            }
            return 0;
        }

        private int GetValue()
        {
            return GetTierPlusRarity() * 15;
        }

        private string GetTypeRarityILvl()
        {
            return string.Format("{0} - {1} ({2} iLvl)", ItemType.ToString(), Rarity.ToString(), Ilvl);
        }

        private string GetBonuses()
        {
            string res = "";
            if (Strength != 0)
            {
                res += string.Format("+ {0} erő \n", Strength);
            }
            if (Stamina != 0)
            {
                res += string.Format("+ {0} állóképesség \n", Stamina);
            }
            if (Agility != 0)
            {
                res += string.Format("+ {0} fürgeség \n", Agility);
            }
            if (Intellect != 0)
            {
                res += string.Format("+ {0} intelligencia \n", Intellect);
            }

            return res;
        }

        private string GetValues()
        {
            string res = "";

            if (Speed != 0)
            {
                res += string.Format("Sebesség: {0} \n", Speed);
            }
            if (HitRate != 0)
            {
                res += string.Format("Találati esély: + {0}% \n", HitRate);
            }
            if (Dps != 0)
            {
                res += string.Format("Sebzés: {0} - {1} ( Dps: {2} ) \n", MinDps * Speed, Dps * Speed, Dps);
            }
            if (Armor != 0)
            {
                res += string.Format("{0} védelem \n", Armor);
            }

            return res;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            string res = string.Format("Neve: {0} \n", Name);
            res += string.Format("Típus: {0} \n", ItemType.ToString());
            res += string.Format("Tier: {0} \n", Tier);
            res += string.Format("iLvl: {0} \n", Ilvl);
            res += string.Format("Ritkaság: {0} \n", Rarity.ToString());
            if (Speed != 0)
            {
                res += string.Format("Sebesség: {0} \n", Speed);
            }
            if (HitRate != 0)
            {
                res += string.Format("Találati esély: + {0}% \n", HitRate);
            }
            if (Dps != 0)
            {
                res += string.Format("Sebzés: {0} - {1} ( Dps: {2} ) \n", MinDps * Speed, Dps * Speed, Dps);
            }
            if (Armor != 0)
            {
                res += string.Format("{0} védelem \n", Armor);
            }
            if (Strength != 0)
            {
                res += string.Format("+ {0} erő \n", Strength);
            }
            if (Stamina != 0)
            {
                res += string.Format("+ {0} állóképesség \n", Stamina);
            }
            if (Agility != 0)
            {
                res += string.Format("+ {0} fürgeség \n", Agility);
            }
            if (Intellect != 0)
            {
                res += string.Format("+ {0} intelligencia \n", Intellect);
            }

            return res;
        }

        #endregion
    }
}
