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

using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Models
{
    public class QuestReward
    {
        public string From { get; set; }
        public int Score { get; set; }
        public int Money { get; set; }
        public int Honor { get; set; }

        public ObservableCollection<Item> Items = new ObservableCollection<Item>();

        public QuestReward(string from, int score, int money, int honor, ObservableCollection<Item> items)
        {
            From = from;
            Score = score;
            Money = money;
            Honor = honor;
            Items = items;
        }

        public QuestReward()
        {

        }

        public override string ToString()
        {
            string outp = "Állomás: " + From + "\n" + "+" + Money + " $, " + Honor + " honor, 1 Exp \n";

            foreach(var item in Items)
            {
                outp += "\n" + item.ToString();
            }

            return outp; 
        }
    }
}
