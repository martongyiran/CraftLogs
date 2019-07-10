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

using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace CraftLogs.BLL.Services
{
    public class ItemGeneratorService : IItemGeneratorService
    {

        #region Private

        private Random random;

        #endregion

        #region Ctor

        public ItemGeneratorService()
        {
            random = new Random();
        }

        #endregion

        public Item GetRandomItem(int tier)
        {
            ItemRarityEnum itemRarity = GetRarity();
            ItemTypeEnum itemType = GetItemType();
            CharacterClassEnum usableFor = GetClass();
            var nameandimage = GetNameAndImage(usableFor, itemType);
            string name = nameandimage.Item1;
            string img = nameandimage.Item2;
            string statsForQr = GetStats(GetStatPool(tier, itemRarity), itemType);

            return new Item(tier, itemRarity, itemType, usableFor, statsForQr, name, img);
        }

        public Item GetSpecificItem(int tier, ItemTypeEnum itemType)
        {
            ItemRarityEnum itemRarity = GetRarity();
            CharacterClassEnum usableFor = GetClass();
            string statsForQr = GetStats(GetStatPool(tier, itemRarity), itemType);
            var nameandimage = GetNameAndImage(usableFor, itemType);
            string name = nameandimage.Item1;
            string img = nameandimage.Item2;
            return new Item(tier, itemRarity, itemType, usableFor, statsForQr, name, img);
        }

        private Tuple<string, string> GetNameAndImage(CharacterClassEnum usableFor, ItemTypeEnum itemType)
        {
            if (usableFor == CharacterClassEnum.Mage && itemType == ItemTypeEnum.Armor)
            {
                int rnd = random.Next(0, MageArmors.Count - 1);
                return MageArmors[rnd];
            }
            else if (usableFor == CharacterClassEnum.Mage && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = random.Next(0, MageWeapons.Count - 1);
                return MageWeapons[rnd];
            }
            else if (usableFor == CharacterClassEnum.Rogue && itemType == ItemTypeEnum.Armor)
            {
                int rnd = random.Next(0, RogueArmors.Count - 1);
                return RogueArmors[rnd];
            }
            else if (usableFor == CharacterClassEnum.Rogue && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = random.Next(0, RogueWeapons.Count - 1);
                return RogueWeapons[rnd];
            }
            else if (usableFor == CharacterClassEnum.Warrior && itemType == ItemTypeEnum.Armor)
            {
                int rnd = random.Next(0, WarriorArmors.Count - 1);
                return WarriorArmors[rnd];
            }
            else if (usableFor == CharacterClassEnum.Warrior && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = random.Next(0, WarriorWeapons.Count - 1);
                return WarriorWeapons[rnd];
            }
            else if (itemType == ItemTypeEnum.Ring)
            {
                int rnd = random.Next(0, Rings.Count - 1);
                return Rings[rnd];
            }
            else if (itemType == ItemTypeEnum.Neck)
            {
                int rnd = random.Next(0, Necks.Count - 1);
                return Necks[rnd];
            }

            return new Tuple<string, string>("","");
        }


        private string GetStats(int statPool, ItemTypeEnum itemType)
        {
            switch (itemType)
            {
                case ItemTypeEnum.Armor:
                    return GetArmorStats(statPool);
                case ItemTypeEnum.LHand:
                    return GetOHWeaponStats(statPool / 2);
                case ItemTypeEnum.RHand:
                    return GetOHWeaponStats(statPool / 2);
                case ItemTypeEnum.Neck:
                    return GetTrinketStats(statPool);
                case ItemTypeEnum.Ring:
                    return GetTrinketStats(statPool);
                default:
                    return string.Empty;
            }
        }

        private string GetTrinketStats(int statPool)
        {
            int stamina = 0;
            int crit = 0;
            int dodge = 0;
            int def = 0;
            int atk = 0;

            while (statPool != 0)
            {
                int rnd = random.Next(1, 6);

                switch (rnd)
                {
                    case 1:
                        stamina++;
                        statPool--;
                        break;
                    case 2:
                        crit++;
                        statPool--;
                        break;
                    case 3:
                        dodge++;
                        statPool--;
                        break;
                    case 4:
                        def++;
                        statPool--;
                        break;
                    case 5:
                        atk++;
                        statPool--;
                        break;
                }
            }

            return atk + " " + def + " " + stamina + " " + crit + " " + dodge;
            //atk,def,stamina,crit,dodge
        }

        private string GetOHWeaponStats(int statPool)
        {
            int stamina = 0;
            int crit = 0;
            int dodge = 0;
            int atk = (int)(statPool * 0.15);
            statPool -= atk;

            while (statPool != 0)
            {
                int rnd = random.Next(1, 5);

                switch (rnd)
                {
                    case 1:
                        stamina++;
                        statPool--;
                        break;
                    case 2:
                        crit++;
                        statPool--;
                        break;
                    case 3:
                        dodge++;
                        statPool--;
                        break;
                    case 4:
                        atk++;
                        statPool--;
                        break;
                }
            }

            return atk + " " + "0" + " " + stamina + " " + crit + " " + dodge;
            //atk,def,stamina,crit,dodge
        }

        private string GetArmorStats(int statPool)
        {
            int stamina = (int)(statPool * 0.3);
            int crit = 0;
            int dodge = 0;
            int def = 0;
            statPool -= stamina;

            while (statPool != 0)
            {
                int rnd = random.Next(1, 5);

                switch (rnd)
                {
                    case 1:
                        stamina++;
                        statPool--;
                        break;
                    case 2:
                        crit++;
                        statPool--;
                        break;
                    case 3:
                        dodge++;
                        statPool--;
                        break;
                    case 4:
                        def++;
                        statPool--;
                        break;
                }
            }

            return "0 " + def + " " + stamina + " " + crit + " " + dodge;
            //atk,def,stamina,crit,dodge
        }

        private CharacterClassEnum GetClass()
        {
            int @class = random.Next(1, 4);
            switch (@class)
            {
                case 1:
                    return CharacterClassEnum.Mage;
                case 2:
                    return CharacterClassEnum.Rogue;
                case 3:
                    return CharacterClassEnum.Warrior;
                default:
                    throw new Exception("ItemGeneratorService.cs/GetClass: invalid random integer.");
            }
        }

        private ItemRarityEnum GetRarity()
        {
            int rare = random.Next(1, 11);
            return rare < 9 ? ItemRarityEnum.Common : ItemRarityEnum.Rare;
        }

        private ItemTypeEnum GetItemType()
        {
            int type = random.Next(1, 6);
            switch (type)
            {
                case 1:
                    return ItemTypeEnum.Armor;
                case 2:
                    return ItemTypeEnum.LHand;
                case 3:
                    return ItemTypeEnum.RHand;
                case 4:
                    return ItemTypeEnum.Neck;
                case 5:
                    return ItemTypeEnum.Ring;
                default:
                    throw new Exception("ItemGeneratorService.cs/GetItemType: invalid random integer.");
            }
        }

        private int GetStatPool(int tier, ItemRarityEnum itemRarity)
        {
            int statPool = 0;

            if (tier == 1 && itemRarity == ItemRarityEnum.Common)
            {
                statPool = 15;
            }
            else if (tier == 1 && itemRarity == ItemRarityEnum.Rare)
            {
                statPool = 25;
            }
            else if (tier == 2 && itemRarity == ItemRarityEnum.Common)
            {
                statPool = 45;
            }
            else if (tier == 2 && itemRarity == ItemRarityEnum.Rare)
            {
                statPool = 55;
            }
            else if (tier == 3 && itemRarity == ItemRarityEnum.Common)
            {
                statPool = 90;
            }
            else if (tier == 3 && itemRarity == ItemRarityEnum.Rare)
            {
                statPool = 100;
            }

            return statPool;
        }

        private readonly List<Tuple<string, string>> MageArmors = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Fireworm Robes","@drawable/mage_armor_1.png")
        };

        private readonly List<Tuple<string, string>> RogueArmors = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Armor of the Fang","@drawable/rogue_armor_1.png")
        };

        private readonly List<Tuple<string, string>> WarriorArmors = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Breastplate of Valor","@drawable/warrior_armor_1.png")
        };

        private readonly List<Tuple<string, string>> MageWeapons = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Firebelcher","@drawable/mage_weapon_1.png")
        };

        private readonly List<Tuple<string, string>> RogueWeapons = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Scout's Blade","@drawable/rogue_weapon_1.png")
        };

        private readonly List<Tuple<string, string>> WarriorWeapons = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Annihilator","@drawable/warrior_weapon_1.png")
        };

        private readonly List<Tuple<string, string>> Rings = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Advisor's Ring","@drawable/ring_1.png"),
        new Tuple<string,string>("Ring of Defense","@drawable/ring_2.png"),
        new Tuple<string,string>("Black Pearl Ring","@drawable/ring_3.png")
        };

        private readonly List<Tuple<string, string>> Necks = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Scout's Medallion","@drawable/neck_1.png"),
        new Tuple<string,string>("Brilliant Necklace","@drawable/neck_2.png"),
        new Tuple<string,string>("Amulet of the Moon","@drawable/neck_3.png")
        };
    }
}
