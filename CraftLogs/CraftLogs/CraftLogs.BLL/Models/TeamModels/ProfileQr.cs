/*
Copyright 2019 Gyirán Márton Áron

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
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Models
{
    public class ProfileQr
    {
        /// <summary>
        /// Name
        /// </summary>
        public string a { get; set; } = "none"; 

        /// <summary>
        /// Cast
        /// </summary>
        public CharacterClassEnum b { get; set; } 

        /// <summary>
        /// Money
        /// </summary>
        public int c { get; set; } 

        /// <summary>
        /// Honor
        /// </summary>
        public int d { get; set; } 

        /// <summary>
        /// Score
        /// </summary>
        public int e { get; set; }

        /// <summary>
        /// Exp
        /// </summary>
        public int f { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public int g { get; set; }

        /// <summary>
        /// XpForNextLevel
        /// </summary>
        public int h { get; set; }

        /// <summary>
        /// Atk
        /// </summary>
        public int i { get; set; }

        /// <summary>
        /// Def
        /// </summary>
        public int j { get; set; }

        /// <summary>
        /// Stamina
        /// </summary>
        public int k { get; set; }

        /// <summary>
        /// CritR
        /// </summary>
        public int l { get; set; }

        /// <summary>
        /// Dodge
        /// </summary>
        public int m { get; set; }

        /// <summary>
        /// Image
        /// </summary>
        public string n { get; set; }

        /// <summary>
        /// Equipped
        /// </summary>
        public ObservableCollection<Item> o = new ObservableCollection<Item>();

        [JsonIgnore]
        public int Hp { get { return (k * HpValue) + 95; } }

        [JsonIgnore]
        public int HpValue { get { return GetHpValue(); } }
        
        public ProfileQr()
        {

        }

        private int GetHpValue()
        {
            switch (b)
            {
                case CharacterClassEnum.Warrior:
                    return 6;
                case CharacterClassEnum.Mage:
                    return 4;
                default:
                    return 5;
            }
        }

        public void Init()
        {
            foreach (var item in o)
            {
                item.SetStats(item.StatsFromQR);
            }
            
        }

    }
}
