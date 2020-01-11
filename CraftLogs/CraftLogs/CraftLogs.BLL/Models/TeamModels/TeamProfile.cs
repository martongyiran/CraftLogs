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
using Prism.Mvvm;

namespace CraftLogs.BLL.Models
{
    public class TeamProfile : BindableBase
    {
        private string _name;
        private HouseEnum _house;
        private CharacterClassEnum _cast;
        private int _score;
        private int _money;
        private int _honor;
        private int _allExp;
        private int _atk;
        private int _def;
        private int _stamina;
        private string _image;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public HouseEnum House
        {
            get => _house;
            set => SetProperty(ref _house, value);
        }

        public CharacterClassEnum Cast
        {
            get => _cast;
            set => SetProperty(ref _cast, value);
        }

        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        public int Money
        {
            get => _money;
            set => SetProperty(ref _money, value);
        }

        public int Honor
        {
            get => _honor;
            set => SetProperty(ref _honor, value);
        }

        public int AllExp
        {
            get => _allExp;
            set => SetProperty(ref _allExp, value);
        }

        public int Atk
        {
            get => _atk;
            set => SetProperty(ref _atk, value);
        }

        public int Def
        {
            get => _def;
            set => SetProperty(ref _def, value);
        }

        public int Stamina
        {
            get => _stamina;
            set => SetProperty(ref _stamina, value);
        }

        public int CritR => GetCrit();

        public int Dodge => GetDodge();

        public int Hp => (Stamina * HpValue) + 95;

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public ObservableCollection<Item> Inventory = new ObservableCollection<Item>();

        public Guid TradeNumber { get; set; }

        public TradeStatusEnum TradeStatus { get; set; } = TradeStatusEnum.Finished;

        public TradeReward TradeOut = new TradeReward();

        public TradeReward TradeIn = new TradeReward();

        public string TradeWith { get; set; }

        public string TradeLastQR { get; set; }

        public int Exp => CalcualteExp();

        public int Level => CalculateLevel();

        public int XpForNextLevel => CalculateXpForNextLevel();

        public int StatPoint => GetStatPoint();

        public int HpValue => GetHpValue();

        public string LevelText => "Lvl." + Level + " " + Cast;

        public string ExpText => "EXP: " + Exp + "/" + XpForNextLevel;

        public TeamProfile(
            string name,
            HouseEnum house,
            CharacterClassEnum cast,
            string image)
        {
            Name = name;
            House = house;
            Cast = cast;
            Image = image;
            Atk = Cast == CharacterClassEnum.Mage ? 3 : 1;
            Def = Cast == CharacterClassEnum.Warrior ? 3 : 1;
            Stamina = 1;
            AllExp = 1;
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
            var norm = Cast == CharacterClassEnum.Rogue ? 9 : 7;
            return AllExp - Atk - Def - Stamina - CritR - Dodge + norm;
        }

        private int GetHpValue()
        {
            return Cast switch
            {
                CharacterClassEnum.Warrior => 6,
                CharacterClassEnum.Mage => 4,
                _ => 5,
            };
        }

        private int GetCrit()
        {
            return Cast == CharacterClassEnum.Rogue ? 3 : 1;
        }

        private int GetDodge()
        {
            return Cast == CharacterClassEnum.Rogue ? 3 : 1;
        }

        public void Init()
        {
            foreach (var item in Inventory)
            {
                item.SetStats(item.StatsFromQR);
            }

            foreach (var item in TradeIn.ItemsToTrade)
            {
                item.SetStats(item.StatsFromQR);
            }

            foreach (var item in TradeOut.ItemsToTrade)
            {
                item.SetStats(item.StatsFromQR);
            }
        }
    }
}
