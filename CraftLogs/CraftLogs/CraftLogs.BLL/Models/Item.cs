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
using System.Collections.Generic;
using CraftLogs.BLL.Enums;
using CraftLogs.BLL.MockData;
using CraftLogs.Values;
using Newtonsoft.Json;

namespace CraftLogs.BLL.Models
{
    public class Item
    {
        public int Tier { get; set; }
        public ItemRarityEnum Rarity { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public CharacterClassEnum UsableFor { get; set; }
        public string StatsFromQR { get; set; }
        public int Ad { get; set; }

        [JsonIgnore]
        public string Id { get; private set; }

        [JsonIgnore]
        public string Name { get { return GetName(); } }

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

        [JsonIgnore]
        public string InvString { get { return GetInvString(); } }

        public Item()
        {
            Id = GenerateId();
        }

        public Item(int ad)
        {
            Ad = ad;
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="rarity"></param>
        /// <param name="itemType"></param>
        /// <param name="usableFor"></param>
        /// <param name="statsFromQR"> atk,def,stamina,crit,dodge</param>
        public Item(int tier, ItemRarityEnum rarity, ItemTypeEnum itemType, CharacterClassEnum usableFor, string statsFromQR, int ad = 999)
        {
            Id = GenerateId();
            Tier = tier;
            Rarity = rarity;
            ItemType = itemType;
            UsableFor = usableFor;
            StatsFromQR = statsFromQR;
            Ad = ad;
            SetStats(statsFromQR);
        }

        private string GenerateId()
        {
            var guid = Guid.NewGuid().ToString();
            return guid;
        }

        private int GetValue()
        {
            return Tier switch
            {
                1 => 16,
                2 => 50,
                3 => 100,
                _ => 0,
            };
        }

        private string GetInvString()
        {
            string res = string.Empty;
            if (Atk != 0)
            {
                res += string.Format("+{0} ATK \n", Atk);
            }
            if (Def != 0)
            {
                res += string.Format("+{0} DEF \n", Def);
            }
            if (Stamina != 0)
            {
                res += string.Format("+{0} STM \n", Stamina);
            }
            if (CritR != 0)
            {
                res += string.Format("+{0} CritR \n", CritR);
            }
            if (Dodge != 0)
            {
                res += string.Format("+{0} Dodge", Dodge);
            }

            return res;
        }

        private string GetName()
        {
            if(Ad == 1111)
            {
                return Texts.Profile_NoArmor;
            }
            else if (Ad == 2222)
            {
                return Texts.Profile_NoRing;
            }
            else if (Ad == 3333)
            {
                return Texts.Profile_NoNeck;
            }
            else if (Ad == 4444)
            {
                return Texts.Profile_NoWeapon;
            }
            else
            {
                return ItemNameHandler.GetItemName(Rarity, ItemType, UsableFor, Ad);
            }
        }

        private string GetImage()
        {
            if (Ad == 1111)
            {
                return "@drawable/chest.png";
            }
            else if (Ad == 2222)
            {
                return "@drawable/ring.png";
            }
            else if (Ad == 3333)
            {
                return "@drawable/neck.png";
            }
            else if (Ad == 4444)
            {
                return "@drawable/weapon.png";
            }
            else
            {
                return ItemNameHandler.GetItemImage(Rarity, ItemType, UsableFor, Ad);
            }

        }

        public override string ToString()
        {
            string res = string.Format("Neve: {0} \n", Name);
            res += string.Format("Ritkaság: Tier {0}, {1}\n", Tier, Rarity);
            res += string.Format("Típusa: {0} \n", ItemType);
            res += string.Format("Kaszt: {0} \n", UsableFor);
            res += string.Format("Statok:\n{0} ATK\n{1} DEF\n{2} STM\n{3} CR\n{4} DDG\n", Atk, Def, Stamina, CritR, Dodge);
            res += string.Format("Értéke: {0} \n", Value);
            //TODO
            return res;
        }

        public void SetStats(string statsForQr)
        {
            var array = statsForQr.Split(null);
            Atk = int.Parse(array[0]);
            Def = int.Parse(array[1]);
            Stamina = int.Parse(array[2]);
            CritR = int.Parse(array[3]);
            Dodge = int.Parse(array[4]);
        }
    }
}
