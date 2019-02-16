using System;
using CraftLogs.BLL.Enums;
using Newtonsoft.Json;

namespace CraftLogs.BLL.Models
{
    public class Item
    {
        public string Id { get; private set; }
        public int Tier { get; set; }
        public ItemRarityEnum Rarity { get; set; }
        public string Name { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public CharacterClassEnum UsableFor { get; set; }
        public ItemStateEnum State { get; set; } = 0;

        public int Hp { get; set; } = 0;
        public int Def { get; set; } = 0; 
        public int Atk { get; set; } = 0; 
        public int CritR { get; set; } = 0; 
        public int Dodge { get; set; } = 0;

        [JsonIgnore]
        public int Value { get { return GetValue(); } }

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

        #endregion

        #region Getters

        private string GenerateId()
        {
            var ticks = DateTime.Now.Ticks;
            var guid = Guid.NewGuid().ToString();
            var uniqueId = ticks.ToString() + '-' + guid;
            return uniqueId;
        }

        private int GetValue()
        {
            return Tier * 15;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            string res = string.Format("Neve: {0} \n", Name);
            //TODO
            return res;
        }

        #endregion
    }
}
