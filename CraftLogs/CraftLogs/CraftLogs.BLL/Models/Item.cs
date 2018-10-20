using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Helpers;

namespace CraftLogs.BLL.Models
{
    public class Item
    {
        public int Id { get; set; } = 0;
        public int Tier { get; set; }
        public int Ilvl { get; set; } = 0;
        public ItemRarityEnum Rarity { get; set; }
        public string Name { get; set; }
        public SetNameEnum SetName { get; set; }
        public int Value { get; set; } = 0;
        public ItemTypeEnum ItemType { get; set; }
        public ItemSubTypeEnum ItemSubType { get; set; } = 0;
        public ItemStateEnum State { get; set; } = 0;
        public int Speed { get; set; } = 0;
        public int HitRate { get; set; } = 0;
        public int Dps { get; set; } = 0;
        public int MinDps { get; set; } = 0;
        public int Armor { get; set; } = 0;
        public int Stamina { get; set; } = 0;
        public int CritRate { get; set; } = 0;
        public double CritDamage { get; set; } = 0;
        public int Agility { get; set; } = 0;

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
                res += string.Format("{0}% védelem \n", Armor);
            }
            if (Stamina != 0)
            {
                res += string.Format("+ {0} állóképesség \n", Stamina);
            }
            if (CritRate != 0)
            {
                res += string.Format("+ {0}% kritikus ütés esély \n", CritRate);
            }
            if (CritDamage != 0)
            {
                res += string.Format("+ {0}% kritikus sebzés \n", CritDamage * 100);
            }
            if (Agility != 0)
            {
                res += string.Format("+ {0}% kitérés esély \n", Agility);
            }

            return res;
        }

        #endregion
    }
}
