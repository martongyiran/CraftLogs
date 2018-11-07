using CraftLogs.BLL.Enums;
using CraftLogs.Values;

namespace CraftLogs.BLL.Models
{
    public class Item
    {
        public int Id { get; set; } = 0;
        public int Tier { get; set; }
        public int Ilvl { get { return GetILvl(); } }
        public ItemRarityEnum Rarity { get; set; }
        public string Name { get { return GetItemName(); } }
        public SetNameEnum SetName { get; set; }
        public int Value { get; } = 0;
        public ItemTypeEnum ItemType { get; set; }
        public ItemSubTypeEnum ItemSubType { get; set; } = 0;
        public ItemStateEnum State { get; set; } = 0;
        public int Speed { get { return GetSpeed(); } }
        public int Dps { get { return GetDps(); } }
        public int MinDps { get { return GetMinDps(); } }
        public int Armor { get { return GetArmor(); } }
        public int Stamina { get; set; } = 0;
        public int Strength { get; set; } = 0;
        public int Agility { get; set; } = 0;
        public int HitRate { get { return GetHitRate(); } }

        #region Ctor
        public Item()
        {

        }
        public Item(int tier, ItemRarityEnum rarity, ItemTypeEnum itemType)
        {
            Tier = tier;
            Rarity = rarity;
            ItemType = itemType;
        }

        public Item(int tier, ItemRarityEnum rarity, ItemTypeEnum itemType, ItemSubTypeEnum itemSubType)
        {
            Tier = tier;
            Rarity = rarity;
            ItemType = itemType;
            ItemSubType = itemSubType;
        }

        #endregion

        #region Getters

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

        private int GetDps()
        {
            switch (ItemSubType)
            {
                case ItemSubTypeEnum.None:
                    return 0;
                case ItemSubTypeEnum.Dagger:
                    return (int)(20.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Sword:
                    return (int)(25.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Axe:
                    return (int)(33.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Spear:
                    return (int)(100.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Hammer:
                    return (int)(200.0 * (1.0 + ((TierToRate() + RarityToRate()) / 10.0) - 0.1));
                case ItemSubTypeEnum.Shield:
                    return 0;
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
                case ItemSubTypeEnum.Sword:
                    return 4;
                case ItemSubTypeEnum.Axe:
                    return 3;
                case ItemSubTypeEnum.Spear:
                    return 2;
                case ItemSubTypeEnum.Hammer:
                    return 1;
                case ItemSubTypeEnum.Shield:
                    return 0;
                default:
                    return 0;
            }
        }

        private int GetHitRate()
        {
            if (ItemSubType != ItemSubTypeEnum.None && ItemSubType != ItemSubTypeEnum.Shield)
            {
                return GetTierPlusRarity() * 2;
            }
            return 0;
        }

        private int GetArmor()
        {
            if ((ItemSubType == ItemSubTypeEnum.None || ItemSubType == ItemSubTypeEnum.Shield) && ItemType != ItemTypeEnum.Trinket)
            {
                return (int)(GetTierPlusRarity() * 2.2);
            }
            return 0;
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
                res += string.Format("Találati esély: {0}% \n", HitRate);
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

            return res;
        }

        #endregion
    }
}
