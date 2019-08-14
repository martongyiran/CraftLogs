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

using System;
using System.Collections.Generic;
using System.Text;

namespace CraftLogs.BLL.Models
{
    public class CombatUnit
    {

        public string Name { get; set; }

        public int Atk { get; set; } = 1;

        public int Def { get; set; } = 1;
        

        public int CritR { get; set; } = 1;

        public int Dodge { get; set; } = 1;

        public int Hp { get; set; }

        public string Image { get; set; } 

        public string House { get; set; }

        public CombatUnit(string name, int atk, int def, int critR, int dodge, int hp, string house = "none", string image = "@drawable/peon.png")
        {
            Name = name;
            Atk = atk;
            Def = def;
            CritR = critR;
            Dodge = dodge;
            Hp = hp;
            House = house;
            Image = image;
        }

    }
}
