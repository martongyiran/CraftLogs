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
        public string Name { get; set; }

        public int Score { get; set; }

        public int Atk { get; set; }

        public int Def { get; set; }


        public int CritR { get; set; }

        public int Dodge { get; set; }

        [JsonIgnore]
        public string Image { get; set; }
        
        public ObservableCollection<Item> Equipped = new ObservableCollection<Item>();

        public ProfileQr(string name, int atk, int def, int critR, int dodge, int score)
        {
            Name = name;
            Atk = atk;
            Def = def;
            CritR = critR;
            Dodge = dodge;
            Score = score;
        }
    }
}
