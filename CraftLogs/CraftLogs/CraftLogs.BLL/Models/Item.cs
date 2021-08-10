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
            else if (Rarity == ItemRarityEnum.Legendary && ItemType == ItemTypeEnum.Ring)
            {
                return Legends[Ad].Item1;
            }
            else if (Rarity == ItemRarityEnum.Legendary && ItemType == ItemTypeEnum.Neck)
            {
                return Legends[Ad].Item1;
            }
            else if (Rarity == ItemRarityEnum.Legendary && ItemType == ItemTypeEnum.RHand)
            {
                return Legends[Ad].Item1;
            }
            else if (UsableFor == CharacterClassEnum.Mage && ItemType == ItemTypeEnum.Armor)
            {
                return MageArmors[Ad].Item1;
            }
            else if (UsableFor == CharacterClassEnum.Mage && (ItemType == ItemTypeEnum.LHand || ItemType == ItemTypeEnum.RHand))
            {
                return MageWeapons[Ad].Item1;
            }
            else if (UsableFor == CharacterClassEnum.Rogue && ItemType == ItemTypeEnum.Armor)
            {
                return RogueArmors[Ad].Item1;
            }
            else if (UsableFor == CharacterClassEnum.Rogue && (ItemType == ItemTypeEnum.LHand || ItemType == ItemTypeEnum.RHand))
            {
                return RogueWeapons[Ad].Item1;
            }
            else if (UsableFor == CharacterClassEnum.Warrior && ItemType == ItemTypeEnum.Armor)
            {
                return WarriorArmors[Ad].Item1;
            }
            else if (UsableFor == CharacterClassEnum.Warrior && (ItemType == ItemTypeEnum.LHand || ItemType == ItemTypeEnum.RHand))
            {
                return WarriorWeapons[Ad].Item1;
            }
            else if (ItemType == ItemTypeEnum.Ring)
            {
                return Rings[Ad].Item1;
            }
            else if (ItemType == ItemTypeEnum.Neck)
            {
                return Necks[Ad].Item1;
            }
            return "";
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
            else if (Rarity == ItemRarityEnum.Legendary && ItemType == ItemTypeEnum.Ring)
            {
                return Legends[Ad].Item2;
            }
            else if (Rarity == ItemRarityEnum.Legendary && ItemType == ItemTypeEnum.Neck)
            {
                return Legends[Ad].Item2;
            }
            else if (Rarity == ItemRarityEnum.Legendary && ItemType == ItemTypeEnum.RHand)
            {
                return Legends[Ad].Item2;
            }
            else if (UsableFor == CharacterClassEnum.Mage && ItemType == ItemTypeEnum.Armor)
            {
                return MageArmors[Ad].Item2;
            }
            else if (UsableFor == CharacterClassEnum.Mage && (ItemType == ItemTypeEnum.LHand || ItemType == ItemTypeEnum.RHand))
            {
                return MageWeapons[Ad].Item2;
            }
            else if (UsableFor == CharacterClassEnum.Rogue && ItemType == ItemTypeEnum.Armor)
            {
                return RogueArmors[Ad].Item2;
            }
            else if (UsableFor == CharacterClassEnum.Rogue && (ItemType == ItemTypeEnum.LHand || ItemType == ItemTypeEnum.RHand))
            {
                return RogueWeapons[Ad].Item2;
            }
            else if (UsableFor == CharacterClassEnum.Warrior && ItemType == ItemTypeEnum.Armor)
            {
                return WarriorArmors[Ad].Item2;
            }
            else if (UsableFor == CharacterClassEnum.Warrior && (ItemType == ItemTypeEnum.LHand || ItemType == ItemTypeEnum.RHand))
            {
                return WarriorWeapons[Ad].Item2;
            }
            else if (ItemType == ItemTypeEnum.Ring)
            {
                return Rings[Ad].Item2;
            }
            else if (ItemType == ItemTypeEnum.Neck)
            {
                return Necks[Ad].Item2;
            }
            return "";
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

        private readonly List<Tuple<string, string>> Legends = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Baetylus","@drawable/baetylus.png"),
        new Tuple<string,string>("Brísingamen","@drawable/brisingamen.png"),
        new Tuple<string,string>("MJÖLNIR","@drawable/mjolnir.png")
        };

        private readonly List<Tuple<string, string>> MageArmors = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Bordó kimonó","@drawable/mage_armor_1.png"),
        new Tuple<string,string>("Zöld tunika","@drawable/mage_armor_2.png"),
        new Tuple<string,string>("Piros palást","@drawable/mage_armor_3.png"),
        new Tuple<string,string>("Intrikás mellvért","@drawable/mage_armor_4.png"),
        new Tuple<string,string>("Piros tunika","@drawable/mage_armor_5.png"),
        new Tuple<string,string>("Intrikás kimonó","@drawable/mage_armor_6.png"),
        new Tuple<string,string>("Felvillanyozó páncél","@drawable/mage_armor_7.png"),
        new Tuple<string,string>("Kék kimonó","@drawable/mage_armor_8.png"),
        new Tuple<string,string>("Lila palást","@drawable/mage_armor_9.png"),
        new Tuple<string,string>("Arany kimonó","@drawable/mage_armor_10.png"),
        new Tuple<string,string>("Kék tunika","@drawable/mage_armor_11.png"),
        new Tuple<string,string>("Bordó tunika","@drawable/mage_armor_12.png"),
        new Tuple<string,string>("Zöld susogó","@drawable/mage_armor_13.png"),
        new Tuple<string,string>("Kék palást","@drawable/mage_armor_14.png"),
        new Tuple<string,string>("Szörmés kabát","@drawable/mage_armor_15.png")
        };

        private readonly List<Tuple<string, string>> RogueArmors = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Lila bőrvért","@drawable/rogue_armor_1.png"),
        new Tuple<string,string>("Arany bőrvért","@drawable/rogue_armor_2.png"),
        new Tuple<string,string>("Intrikás bőrvért","@drawable/rogue_armor_3.png"),
        new Tuple<string,string>("Lopakodó gambeson","@drawable/rogue_armor_4.png"),
        new Tuple<string,string>("Demóna mellvértje","@drawable/rogue_armor_5.png"),
        new Tuple<string,string>("Bűzös bőrruha","@drawable/rogue_armor_6.png"),
        new Tuple<string,string>("Tüskés páncél","@drawable/rogue_armor_7.png"),
        new Tuple<string,string>("Intrikás páncél","@drawable/rogue_armor_8.png"),
        new Tuple<string,string>("Könnyed bőrvért","@drawable/rogue_armor_9.png"),
        new Tuple<string,string>("Felvillanyozó mellvért","@drawable/rogue_armor_10.png"),
        new Tuple<string,string>("Bronzos páncél","@drawable/rogue_armor_11.png"),
        new Tuple<string,string>("Penészes mellvért","@drawable/rogue_armor_12.png"),
        new Tuple<string,string>("Szegecses bőrruha","@drawable/rogue_armor_13.png"),
        new Tuple<string,string>("Cserzett bőr ruha","@drawable/rogue_armor_14.png"),
        new Tuple<string,string>("Téli bunda","@drawable/rogue_armor_15.png")
        };

        private readonly List<Tuple<string, string>> WarriorArmors = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Aranyozott mellvért","@drawable/warrior_armor_1.png"),
        new Tuple<string,string>("Előkelő páncél","@drawable/warrior_armor_2.png"),
        new Tuple<string,string>("Díszes páncél","@drawable/warrior_armor_3.png"),
        new Tuple<string,string>("Egyszerű vasing","@drawable/warrior_armor_4.png"),
        new Tuple<string,string>("Kék mellvért","@drawable/warrior_armor_5.png"),
        new Tuple<string,string>("Szőrmés életmentő","@drawable/warrior_armor_6.png"),
        new Tuple<string,string>("Ezüstözött mellvért","@drawable/warrior_armor_7.png"),
        new Tuple<string,string>("Fagyos öltözet","@drawable/warrior_armor_8.png"),
        new Tuple<string,string>("Homokszínű öltözet","@drawable/warrior_armor_9.png"),
        new Tuple<string,string>("Intrikás életmentő","@drawable/warrior_armor_10.png"),
        new Tuple<string,string>("Fénylő mellvért","@drawable/warrior_armor_11.png"),
        new Tuple<string,string>("Kamubőr vért","@drawable/warrior_armor_12.png"),
        new Tuple<string,string>("Felvillanyozó öltözet","@drawable/warrior_armor_13.png"),
        new Tuple<string,string>("Aszimmetrikus védelmező","@drawable/warrior_armor_14.png"),
        new Tuple<string,string>("Lila harcivért","@drawable/warrior_armor_15.png")
        };

        private readonly List<Tuple<string, string>> MageWeapons = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Tüzes élmény","@drawable/mage_weapon_1.png"),
        new Tuple<string,string>("Jeges élmény","@drawable/mage_weapon_2.png"),
        new Tuple<string,string>("Természeti csapás","@drawable/mage_weapon_3.png"),
        new Tuple<string,string>("Tüzes odacsapó","@drawable/mage_weapon_4.png"),
        new Tuple<string,string>("Jeges odacsapó","@drawable/mage_weapon_5.png"),
        new Tuple<string,string>("Jeges bökdöső","@drawable/mage_weapon_6.png"),
        new Tuple<string,string>("Tüzes bökdöső","@drawable/mage_weapon_7.png"),
        new Tuple<string,string>("Jeges büntető","@drawable/mage_weapon_8.png"),
        new Tuple<string,string>("Természet botja","@drawable/mage_weapon_9.png"),
        new Tuple<string,string>("Mérgező pálca","@drawable/mage_weapon_10.png"),
        new Tuple<string,string>("Mérgező szurony","@drawable/mage_weapon_11.png"),
        new Tuple<string,string>("Jég botja","@drawable/mage_weapon_12.png"),
        new Tuple<string,string>("Havas büntető","@drawable/mage_weapon_13.png"),
        new Tuple<string,string>("Fényes élmény","@drawable/mage_weapon_14.png"),
        new Tuple<string,string>("Idézés tollas botja","@drawable/mage_weapon_15.png"),
        new Tuple<string,string>("Orosz varázspálca","@drawable/mage_weapon_16.png")
        };

        private readonly List<Tuple<string, string>> RogueWeapons = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Veszélyes bökő","@drawable/rogue_weapon_1.png"),
        new Tuple<string,string>("Hegyes bökő","@drawable/rogue_weapon_2.png"),
        new Tuple<string,string>("Tüzes fájdalom","@drawable/rogue_weapon_3.png"),
        new Tuple<string,string>("Unikornis szarva","@drawable/rogue_weapon_4.png"),
        new Tuple<string,string>("Komoly dugóhúzó","@drawable/rogue_weapon_5.png"),
        new Tuple<string,string>("Recés bökő","@drawable/rogue_weapon_6.png"),
        new Tuple<string,string>("Görbe büntető","@drawable/rogue_weapon_7.png"),
        new Tuple<string,string>("Dupla pengés élmény","@drawable/rogue_weapon_8.png"),
        new Tuple<string,string>("Goblin levágott ujja","@drawable/rogue_weapon_9.png"),
        new Tuple<string,string>("Kéknyelű fájdalom","@drawable/rogue_weapon_10.png"),
        new Tuple<string,string>("Görbe fájdalom","@drawable/rogue_weapon_11.png"),
        new Tuple<string,string>("Szike","@drawable/rogue_weapon_12.png"),
        new Tuple<string,string>("Merry Xmas","@drawable/rogue_weapon_13.png"),
        new Tuple<string,string>("Pirosmarkolatú kék penge","@drawable/rogue_weapon_14.png"),
        new Tuple<string,string>("Görbe kékség","@drawable/rogue_weapon_15.png")
        };

        private readonly List<Tuple<string, string>> WarriorWeapons = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Megmunkált balta","@drawable/warrior_weapon_1.png"),
        new Tuple<string,string>("Pattintott kőbalta","@drawable/warrior_weapon_2.png"),
        new Tuple<string,string>("Fejlettebb kőbalta","@drawable/warrior_weapon_3.png"),
        new Tuple<string,string>("Sertésdaraboló","@drawable/warrior_weapon_4.png"),
        new Tuple<string,string>("Kobaktörő","@drawable/warrior_weapon_5.png"),
        new Tuple<string,string>("Fejlett fémbalta","@drawable/warrior_weapon_6.png"),
        new Tuple<string,string>("Véres fejsze","@drawable/warrior_weapon_7.png"),
        new Tuple<string,string>("Hentesbárd","@drawable/warrior_weapon_8.png"),
        new Tuple<string,string>("Lefejező","@drawable/warrior_weapon_9.png"),
        new Tuple<string,string>("Tömzsi lefejző","@drawable/warrior_weapon_10.png"),
        new Tuple<string,string>("Megmunkált kőbalta","@drawable/warrior_weapon_11.png"),
        new Tuple<string,string>("Hősök csákánya","@drawable/warrior_weapon_12.png"),
        new Tuple<string,string>("Kétélű csoda","@drawable/warrior_weapon_13.png"),
        new Tuple<string,string>("Recés balta","@drawable/warrior_weapon_14.png"),
        new Tuple<string,string>("Fűrészes balta","@drawable/warrior_weapon_15.png")
        };

        private readonly List<Tuple<string, string>> Rings = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Prométeusz tüzes munkája","@drawable/ring_1.png"),
        new Tuple<string,string>("Dalma jeges áldása","@drawable/ring_2.png"),
        new Tuple<string,string>("Cetli zafír jegygyűrűje","@drawable/ring_3.png"),
        new Tuple<string,string>("Zodi tüskés boxere","@drawable/ring_4.png"),
        new Tuple<string,string>("Boric$ ünnepi gyűrűje","@drawable/ring_5.png"),
        new Tuple<string,string>("Cica rettentő haragja","@drawable/ring_6.png"),
        new Tuple<string,string>("Anett megfékezhetetlen dühe","@drawable/ring_7.png"),
        new Tuple<string,string>("Bajusz viking ajándéka","@drawable/ring_8.png"),
        new Tuple<string,string>("Bucsi felhevült karikagyűrűje","@drawable/ring_9.png"),
        new Tuple<string,string>("Elena fényes szerencsegyűrűje","@drawable/ring_10.png"),
        new Tuple<string,string>("nemecsek jádeköves gyűrűje","@drawable/ring_11.png"),
        new Tuple<string,string>("Kacsa rubin szerencsegyűrűje","@drawable/ring_12.png"),
        new Tuple<string,string>("Dézintegráló gyűrű","@drawable/ring_13.png"),
        new Tuple<string,string>("Dóra gyűrűje","@drawable/ring_14.png"),
        new Tuple<string,string>("Thor szőrmókjának nyakörve","@drawable/ring_15.png")
        };

        private readonly List<Tuple<string, string>> Necks = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Arki türkiz nyakéke","@drawable/neck_1.png"),
        new Tuple<string,string>("Róka smaragdos nyaklánca","@drawable/neck_2.png"),
        new Tuple<string,string>("Pite rubin ékköve","@drawable/neck_3.png"),
        new Tuple<string,string>("Zozi igéző nyakéke","@drawable/neck_4.png"),
        new Tuple<string,string>("Mészi kampós nyaklánca","@drawable/neck_5.png"),
        new Tuple<string,string>("Dante ametiszt nyaklánca","@drawable/neck_6.png"),
        new Tuple<string,string>("Hexa smaragd ékköve","@drawable/neck_7.png"),
        new Tuple<string,string>("Apa kék spirálköve","@drawable/neck_8.png"),
        new Tuple<string,string>("Lucy ametiszt uralomköve","@drawable/neck_9.png"),
        new Tuple<string,string>("Portál smaragdos uralomköve","@drawable/neck_10.png"),
        new Tuple<string,string>("Foxi indián nyakéke","@drawable/neck_11.png"),
        new Tuple<string,string>("Kiki fényes zafírköve","@drawable/neck_12.png"),
        new Tuple<string,string>("Brandy ametiszt uralomköve","@drawable/neck_13.png"),
        new Tuple<string,string>("Maci fényes smaragdköve","@drawable/neck_14.png"),
        new Tuple<string,string>("Leves kagylós barátságnyakéke","@drawable/neck_15.png"),
        new Tuple<string,string>("Martin zafír rúnaköve","@drawable/neck_16.png"),
        new Tuple<string,string>("Zeus vörös uralomköve","@drawable/neck_17.png"),
        new Tuple<string,string>("Suu aranyos uralomköve","@drawable/neck_18.png")
        };
    }
}
