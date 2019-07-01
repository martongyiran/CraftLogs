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
using System.Collections.ObjectModel;
using CraftLogs.BLL.Enums;
using CraftLogs.Values;

namespace CraftLogs.BLL.Models
{
    public class TeamProfile
    {

        public string Name { get; set; }

        public HouseEnum House { get; set; }

        public CharacterClassEnum Cast { get; set; }

        public int Score { get; set; } = 0;

        public int Money { get; set; } = 0;

        public int Honor { get; set; } = 0;

        public int AllExp { get; set; } = 1;

        public int Exp
        {
            get { return CalcualteExp(); }
        }
        public int Level
        {
            get { return CalculateLevel(); }
        }

        public int XpForNextLevel
        {
            get { return CalculateXpForNextLevel(); }
        }

        public int Atk { get; set; } = 1;

        public int Def { get; set; } = 1;

        public int Stamina { get; set; } = 1;

        public int CritR { get; set; } = 1;

        public int Dodge { get; set; } = 1;

        public int Hp { get { return (Stamina * 5) + 95; } }


        public string Image { get; set; } = "@drawable/filler.png";

        public int StatPoint
        {
            get { return GetStatPoint(); }
        }

        public ObservableCollection<Item> Inventory = new ObservableCollection<Item>();

        public TeamProfile(string name, HouseEnum house, CharacterClassEnum cast, string image)
        {
            Name = name;
            House = house;
            Cast = cast;
            Image = image;
        }

        private int CalculateLevel()
        {
            if (AllExp >= 34)
            {
                return 5;
            }
            else if (AllExp >= 24)
            {
                return 4;
            }
            else if (AllExp >= 15)
            {
                return 3;
            }
            else if (AllExp >= 7)
            {
                return 2;
            }

            return 1;
        }

        private int CalcualteExp()
        {
            if (Level == 5)
            {
                return AllExp - 34;
            }
            else if (Level == 4)
            {
                return AllExp - 24;
            }
            else if (Level == 3)
            {
                return AllExp - 15;
            }
            else if (Level == 2)
            {
                return AllExp - 7;
            }

            return AllExp;
        }

        private int CalculateXpForNextLevel()
        {

            if (Level == 5)
            {
                return 9999;
            }
            else if (Level == 4)
            {
                return 14;
            }
            else if (Level == 3)
            {
                return 9;
            }
            else if (Level == 2)
            {
                return 8;
            }
            else
            {
                return 7;
            }
        }

        private int GetStatPoint()
        {
            return AllExp - Atk - Def - Stamina - CritR - Dodge + 5;
        }
    }
}
