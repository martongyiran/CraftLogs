using CraftLogs.BLL.Enums;
using System;
using System.Collections.Generic;

namespace CraftLogs.BLL.MockData
{
    public static class ItemNameHandler
    {
        public static string GetItemName(ItemRarityEnum rarity, ItemTypeEnum itemType, CharacterClassEnum usableFor, int ad)
        {
            if (rarity == ItemRarityEnum.Legendary && itemType == ItemTypeEnum.Ring)
            {
                return Legends[ad].Item1;
            }
            else if (rarity == ItemRarityEnum.Legendary && itemType == ItemTypeEnum.Neck)
            {
                return Legends[ad].Item1;
            }
            else if (rarity == ItemRarityEnum.Legendary && itemType == ItemTypeEnum.RHand)
            {
                return Legends[ad].Item1;
            }
            else if (usableFor == CharacterClassEnum.Mage && itemType == ItemTypeEnum.Armor)
            {
                return MageArmors[ad].Item1;
            }
            else if (usableFor == CharacterClassEnum.Mage && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                return MageWeapons[ad].Item1;
            }
            else if (usableFor == CharacterClassEnum.Rogue && itemType == ItemTypeEnum.Armor)
            {
                return RogueArmors[ad].Item1;
            }
            else if (usableFor == CharacterClassEnum.Rogue && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                return RogueWeapons[ad].Item1;
            }
            else if (usableFor == CharacterClassEnum.Warrior && itemType == ItemTypeEnum.Armor)
            {
                return WarriorArmors[ad].Item1;
            }
            else if (usableFor == CharacterClassEnum.Warrior && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                return WarriorWeapons[ad].Item1;
            }
            else if (itemType == ItemTypeEnum.Ring)
            {
                return Rings[ad].Item1;
            }
            else if (itemType == ItemTypeEnum.Neck)
            {
                return Necks[ad].Item1;
            }

            return string.Empty;
        }

        public static string GetItemImage(ItemRarityEnum rarity, ItemTypeEnum itemType, CharacterClassEnum usableFor, int ad)
        {
            if (rarity == ItemRarityEnum.Legendary && itemType == ItemTypeEnum.Ring)
            {
                return Legends[ad].Item2;
            }
            else if (rarity == ItemRarityEnum.Legendary && itemType == ItemTypeEnum.Neck)
            {
                return Legends[ad].Item2;
            }
            else if (rarity == ItemRarityEnum.Legendary && itemType == ItemTypeEnum.RHand)
            {
                return Legends[ad].Item2;
            }
            else if (usableFor == CharacterClassEnum.Mage && itemType == ItemTypeEnum.Armor)
            {
                return MageArmors[ad].Item2;
            }
            else if (usableFor == CharacterClassEnum.Mage && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                return MageWeapons[ad].Item2;
            }
            else if (usableFor == CharacterClassEnum.Rogue && itemType == ItemTypeEnum.Armor)
            {
                return RogueArmors[ad].Item2;
            }
            else if (usableFor == CharacterClassEnum.Rogue && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                return RogueWeapons[ad].Item2;
            }
            else if (usableFor == CharacterClassEnum.Warrior && itemType == ItemTypeEnum.Armor)
            {
                return WarriorArmors[ad].Item2;
            }
            else if (usableFor == CharacterClassEnum.Warrior && (itemType == ItemTypeEnum.LHand || itemType == ItemTypeEnum.RHand))
            {
                return WarriorWeapons[ad].Item2;
            }
            else if (itemType == ItemTypeEnum.Ring)
            {
                return Rings[ad].Item2;
            }
            else if (itemType == ItemTypeEnum.Neck)
            {
                return Necks[ad].Item2;
            }

            return string.Empty;
        }

        public static readonly List<Tuple<string, string>> MageArmors = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> RogueArmors = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> WarriorArmors = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> MageWeapons = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> RogueWeapons = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> WarriorWeapons = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> Rings = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> Necks = new List<Tuple<string, string>>()
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

        public static List<Tuple<string, string>> Legends = new List<Tuple<string, string>>()
        {
            new Tuple<string,string>("Baetylus","@drawable/baetylus.png"),
            new Tuple<string,string>("Brísingamen","@drawable/brisingamen.png"),
            new Tuple<string,string>("MJÖLNIR","@drawable/mjolnir.png")
        };
    }
}
