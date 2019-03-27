/*
Copyright 2018 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using System;
using CraftLogs.BLL.Enums;
using Newtonsoft.Json;

namespace CraftLogs.BLL.Models
{
    public class Item
    {
        public int Tier { get; set; }
        public ItemRarityEnum Rarity { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public CharacterClassEnum UsableFor { get; set; }
        public string StatsForQR { get { return GetStatsForQR(); } }

        [JsonIgnore]
        public string Id { get; private set; }

        [JsonIgnore]
        public string Name { get { return GetName(); } }

        [JsonIgnore]
        public ItemStateEnum State { get; set; } = 0;

        [JsonIgnore]
        public int Atk { get; set; } = 0;

        [JsonIgnore]
        public int Def { get; set; } = 0;

        [JsonIgnore]
        public int Stamina { get; set; } = 0;

        [JsonIgnore]
        public int CritR { get; set; } = 0;

        [JsonIgnore]
        public int Dodge { get; set; } = 0;

        [JsonIgnore]
        public int Value { get { return GetValue(); } }

        [JsonIgnore]
        public string Image { get { return GetImage(); } }

        #region Ctor

        public Item()
        {
            Id = GenerateId();
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="rarity"></param>
        /// <param name="itemType"></param>
        /// <param name="usableFor"></param>
        /// <param name="statsForQR"> atk,def,stamina,crit,dodge</param>
        public Item(int tier, ItemRarityEnum rarity, ItemTypeEnum itemType, CharacterClassEnum usableFor, string statsForQR)
        {
            Id = GenerateId();
            Tier = tier;
            Rarity = rarity;
            ItemType = itemType;
            UsableFor = usableFor;
            SetStats(statsForQR);
        }

        #endregion

        #region Getters

        private string GenerateId()
        {
            var guid = Guid.NewGuid().ToString();
            return guid;
        }

        private string GetName()
        {
            return "Item" + Id;
        }

        private string GetStatsForQR()
        {
            return Atk + " " + Def + " " + Stamina + " " + CritR + " " + Dodge;
        }

        private int GetValue()
        {
            return Tier * 15;
        }

        private void SetStats(string statsForQr)
        {
            var array = statsForQr.Split(null);
            Atk = int.Parse(array[0]);
            Def = int.Parse(array[1]);
            Stamina = int.Parse(array[2]);
            CritR = int.Parse(array[3]);
            Dodge = int.Parse(array[4]);
        }

        private string GetImage()
        {
            return string.Empty;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            string res = string.Format("Neve: {0} \n", Name);
            res += string.Format("Ritkaság: Tier {0}, {1}\n", Tier, Rarity);
            res += string.Format("Típusa: {0} \n", ItemType);
            res += string.Format("Kaszt: {0} \n", UsableFor);
            res += string.Format("Statok:\n{0} ATK\n{1} DEF\n{2} STM\n{3} CR\n{4} DDG\n", Atk, Def, Stamina, CritR, Dodge);
            //TODO
            return res;
        }

        #endregion
    }
}
