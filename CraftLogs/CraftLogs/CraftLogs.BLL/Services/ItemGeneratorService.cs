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
            string statsForQr = GetStats(GetStatPool(tier, itemRarity), itemType);

            return new Item(tier, itemRarity, itemType, usableFor, statsForQr);
        }

        public Item GetSpecificItem(int tier, ItemTypeEnum itemType)
        {
            ItemRarityEnum itemRarity = GetRarity();
            CharacterClassEnum usableFor = GetClass();
            string statsForQr = GetStats(GetStatPool(tier, itemRarity), itemType);
            return new Item(tier, itemRarity, itemType, usableFor, statsForQr);
        }

        private string GetStats(int statPool, ItemTypeEnum itemType)
        {
            switch (itemType)
            {
                case ItemTypeEnum.Armor:
                    return GetArmorStats(statPool);
                case ItemTypeEnum.OneHand:
                    return GetOHWeaponStats(statPool / 2);
                case ItemTypeEnum.Trinket:
                    return GetTrinketStats(statPool);
                case ItemTypeEnum.TwoHand:
                    return GetTHWeaponStats(statPool);
                default:
                    return string.Empty;
            }
        }

        private string GetTHWeaponStats(int statPool)
        {
            int stamina = 0;
            int crit = 0;
            int dodge = 0;
            int atk = (int)(statPool * 0.3);
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
            int shld = random.Next(0, 1);
            bool isShield = shld == 0;

            if (isShield)
            {
                int stamina = 0;
                int crit = 0;
                int dodge = 0;
                int def = (int)(statPool * 0.15);
                statPool -= def;

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
            else
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
            int type = random.Next(1, 5);
            switch (type)
            {
                case 1:
                    return ItemTypeEnum.Armor;
                case 2:
                    return ItemTypeEnum.OneHand;
                case 3:
                    return ItemTypeEnum.Trinket;
                case 4:
                    return ItemTypeEnum.TwoHand;
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

    }
}
