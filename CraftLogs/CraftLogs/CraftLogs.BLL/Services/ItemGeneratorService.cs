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
                int rnd = _random.Next(0, MageArmors.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Mage && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = _random.Next(0, MageWeapons.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Rogue && itemType == ItemTypeEnum.Armor)
            {
                int rnd = _random.Next(0, RogueArmors.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Rogue && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = _random.Next(0, RogueWeapons.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Warrior && itemType == ItemTypeEnum.Armor)
            {
                int rnd = _random.Next(0, WarriorArmors.Count);
                return rnd;
            }
            else if (usableFor == CharacterClassEnum.Warrior && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                int rnd = _random.Next(0, WarriorWeapons.Count);
                return rnd;
            }
            else if (itemType == ItemTypeEnum.Ring)
            {
                int rnd = _random.Next(0, Rings.Count);
                return rnd;
            }
            else if (itemType == ItemTypeEnum.Neck)
            {
                int rnd = _random.Next(0, Necks.Count);
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
        new Tuple<string,string>("Megmunkélt kőbalta","@drawable/warrior_weapon_11.png"),
        new Tuple<string,string>("Hősök csákánya","@drawable/warrior_weapon_12.png"),
        new Tuple<string,string>("Kétélű csoda","@drawable/warrior_weapon_13.png"),
        new Tuple<string,string>("Recés balta","@drawable/warrior_weapon_14.png"),
        new Tuple<string,string>("Fűrészes balta","@drawable/warrior_weapon_15.png")
        };

        private readonly List<Tuple<string, string>> Rings = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Prométeusz tüzes munkája","@drawable/ring_1.png"),
        new Tuple<string,string>("Klári jeges áldása","@drawable/ring_2.png"),
        new Tuple<string,string>("Pelcsi zafír jegygyűrűje","@drawable/ring_3.png"),
        new Tuple<string,string>("Neme$ tüskés boxere","@drawable/ring_4.png"),
        new Tuple<string,string>("Boric$ ünnepi gyűrűje","@drawable/ring_5.png"),
        new Tuple<string,string>("Cica rettentő haragja","@drawable/ring_6.png"),
        new Tuple<string,string>("Anett megfékezhetetlen dühe","@drawable/ring_7.png"),
        new Tuple<string,string>("Marcika viking ajándéka","@drawable/ring_8.png"),
        new Tuple<string,string>("Cucu felhevült karikagyűrűje","@drawable/ring_9.png"),
        new Tuple<string,string>("Zodi fényes szerencsegyűrűje","@drawable/ring_10.png"),
        new Tuple<string,string>("nemecsek jádeköves gyűrűje","@drawable/ring_11.png"),
        new Tuple<string,string>("Jim rubin szerencsegyűrűje","@drawable/ring_12.png"),
        new Tuple<string,string>("Dézintegráló gyűrű","@drawable/ring_13.png"),
        new Tuple<string,string>("A Vimbablak kulcsa","@drawable/ring_14.png"),
        new Tuple<string,string>("Thor markolatgombja","@drawable/ring_15.png")
        };

        private readonly List<Tuple<string, string>> Necks = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Zseton türkiz nyakéke","@drawable/neck_1.png"),
        new Tuple<string,string>("Chili smaragdos nyaklánca","@drawable/neck_2.png"),
        new Tuple<string,string>("Bab rubin ékköve","@drawable/neck_3.png"),
        new Tuple<string,string>("Roti igéző nyakéke","@drawable/neck_4.png"),
        new Tuple<string,string>("BigBen kampós nyaklánca","@drawable/neck_5.png"),
        new Tuple<string,string>("Csüti ametiszt nyaklánca","@drawable/neck_6.png"),
        new Tuple<string,string>("Kara smaragd ékköve","@drawable/neck_7.png"),
        new Tuple<string,string>("Gyümi kék spirálköve","@drawable/neck_8.png"),
        new Tuple<string,string>("Zozi ametiszt uralomköve","@drawable/neck_9.png"),
        new Tuple<string,string>("Andi smaragdos uralomköve","@drawable/neck_10.png"),
        new Tuple<string,string>("Bércy indián nyakéke","@drawable/neck_11.png"),
        new Tuple<string,string>("Pixie fényes zafírköve","@drawable/neck_12.png"),
        new Tuple<string,string>("Lhora ametiszt uralomköve","@drawable/neck_13.png"),
        new Tuple<string,string>("Cápa fényes smaragdköve","@drawable/neck_14.png"),
        new Tuple<string,string>("Bombó kagylós barátságnyakéke","@drawable/neck_15.png"),
        new Tuple<string,string>("M.I. zafír rúnaköve","@drawable/neck_16.png"),
        new Tuple<string,string>("Ruszki vörös uralomköve","@drawable/neck_17.png"),
        new Tuple<string,string>("Kuni aranyos uralomköve","@drawable/neck_18.png")
        };

        private readonly List<Tuple<string, string>> Legends = new List<Tuple<string, string>>()
        {
        new Tuple<string,string>("Baetylus","@drawable/baetylus.png"),
        new Tuple<string,string>("Brísingamen","@drawable/brisingamen.png"),
        new Tuple<string,string>("MJÖLNIR","@drawable/mjolnir.png")
        };

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
