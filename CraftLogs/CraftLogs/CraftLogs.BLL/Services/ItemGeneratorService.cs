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
using CraftLogs.BLL.MockData;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace CraftLogs.BLL.Services
{
    public class ItemGeneratorService : IItemGeneratorService
    {

        private readonly Random _random;

        public ItemGeneratorService()
        {
            _random = new Random();
        }

        public Item GetRandomItem(int tier)
        {
            ItemRarityEnum itemRarity = GetRarity();
            ItemTypeEnum itemType = GetItemType();
            CharacterClassEnum usableFor = GetClass();
            var nameandimage = GetNameAndImage(usableFor, itemType);
            string statsForQr = GetStats(GetStatPool(tier, itemRarity), itemType);
            return new Item(tier, itemRarity, itemType, usableFor, statsForQr, GetNameAndImage(usableFor, itemType));
        }

        public Item GetSpecificItem(int tier, ItemTypeEnum itemType)
        {
            ItemRarityEnum itemRarity = GetRarity();
            CharacterClassEnum usableFor = GetClass();
            string statsForQr = GetStats(GetStatPool(tier, itemRarity), itemType);
            var nameandimage = GetNameAndImage(usableFor, itemType);
            return new Item(tier, itemRarity, itemType, usableFor, statsForQr, GetNameAndImage(usableFor, itemType));
        }

        private int GetNameAndImage(CharacterClassEnum usableFor, ItemTypeEnum itemType)
        {
            if (usableFor == CharacterClassEnum.Mage && itemType == ItemTypeEnum.Armor)
            {
                int rnd = _random.Next(0, ItemNameHandler.MageArmors.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Mage && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = _random.Next(0, ItemNameHandler.MageWeapons.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Rogue && itemType == ItemTypeEnum.Armor)
            {
                int rnd = _random.Next(0, ItemNameHandler.RogueArmors.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Rogue && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = _random.Next(0, ItemNameHandler.RogueWeapons.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Warrior && itemType == ItemTypeEnum.Armor)
            {
                int rnd = _random.Next(0, ItemNameHandler.WarriorArmors.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Warrior && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = _random.Next(0, ItemNameHandler.WarriorWeapons.Count);
                return rnd;
            }
            else if (itemType == ItemTypeEnum.Ring)
            {
                int rnd = _random.Next(0, ItemNameHandler.Rings.Count);
                return rnd;
            }
            else if (itemType == ItemTypeEnum.Neck)
            {
                int rnd = _random.Next(0, ItemNameHandler.Necks.Count);
                return rnd;
            }

            return 999;
        }


        private string GetStats(int statPool, ItemTypeEnum itemType)
        {
            return itemType switch
            {
                ItemTypeEnum.Armor => GetArmorStats(statPool),
                ItemTypeEnum.LHand => GetOHWeaponStats(statPool / 2),
                ItemTypeEnum.RHand => GetOHWeaponStats(statPool / 2),
                ItemTypeEnum.Neck => GetTrinketStats(statPool),
                ItemTypeEnum.Ring => GetTrinketStats(statPool),
                _ => string.Empty,
            };
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
                int rnd = _random.Next(1, 6);

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
                    default:
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
                int rnd = _random.Next(1, 5);

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
                    default:
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
                int rnd = _random.Next(1, 5);

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
                    default:
                        break;
                }
            }

            return "0 " + def + " " + stamina + " " + crit + " " + dodge;
            //atk,def,stamina,crit,dodge
        }

        private CharacterClassEnum GetClass()
        {
            int @class = _random.Next(1, 4);
            return @class switch
            {
                1 => CharacterClassEnum.Mage,
                2 => CharacterClassEnum.Rogue,
                3 => CharacterClassEnum.Warrior,
                _ => throw new Exception("ItemGeneratorService.cs/GetClass: invalid random integer."),
            };
        }

        private ItemRarityEnum GetRarity()
        {
            int rare = _random.Next(1, 11);
            return rare < 9 ? ItemRarityEnum.Common : ItemRarityEnum.Rare;
        }

        private ItemTypeEnum GetItemType()
        {
            int type = _random.Next(1, 6);
            return type switch
            {
                1 => ItemTypeEnum.Armor,
                2 => ItemTypeEnum.LHand,
                3 => ItemTypeEnum.RHand,
                4 => ItemTypeEnum.Neck,
                5 => ItemTypeEnum.Ring,
                _ => throw new Exception("ItemGeneratorService.cs/GetItemType: invalid random integer."),
            };
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

        public Item GetLegendary(LegendaryEnum legendary)
        {
            
            if(legendary == LegendaryEnum.Baetylus)
            {
                ItemRarityEnum itemRarity = ItemRarityEnum.Legendary;
                ItemTypeEnum itemType = ItemTypeEnum.Ring;
                CharacterClassEnum usableFor = CharacterClassEnum.Mage;
                string statsForQr = "15 15 60 15 15";

                return new Item(3, itemRarity, itemType, usableFor, statsForQr,0);
            }
            else if(legendary == LegendaryEnum.Brisingamen)
            {
                ItemRarityEnum itemRarity = ItemRarityEnum.Legendary;
                ItemTypeEnum itemType = ItemTypeEnum.Neck;
                CharacterClassEnum usableFor = CharacterClassEnum.Rogue;
                string statsForQr = "30 30 30 15 15";

                return new Item(3, itemRarity, itemType, usableFor, statsForQr,1);
            }
            else if(legendary == LegendaryEnum.Mjolnir)
            {
                ItemRarityEnum itemRarity = ItemRarityEnum.Legendary;
                ItemTypeEnum itemType = ItemTypeEnum.RHand;
                CharacterClassEnum usableFor = CharacterClassEnum.Warrior;
                string statsForQr = "30 15 0 0 15";

                return new Item(3, itemRarity, itemType, usableFor, statsForQr,2);
            }
            else
            {
                return null;
            }
        }
    }

    public enum LegendaryEnum
    {
        None,
        Mjolnir,
        Baetylus,
        Brisingamen
    }
}
