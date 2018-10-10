using CraftLogs.BLL.Enums;

namespace CraftLogs.BLL.Models
{
    public class Item
    {
        public int Id { get; set; } = 0;
        public int Tier { get; set; }
        public ItemRarityEnum Rarity { get; set; }
        public string Name { get; set; }
        public SetNameEnum SetName { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public ItemSubTypeEnum ItemSubType { get; set; } = 0;
        public ItemStateEnum State { get; set; } = 0;
        public int Speed { get; set; } = 0;
        public int HitRate { get; set; } = 0;
        public int DefRate { get; set; } = 0;
        public int Dps { get; set; } = 0;
        public int MinDmg { get; set; } = 0;
        public int MaxDmg { get; set; } = 0;
        public int Armor { get; set; } = 0;
        public int Stamina { get; set; } = 0;
        public int CritRate { get; set; } = 0;
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
        #endregion
    }
}
