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

using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Models
{
    public class HqReward
    {
        public int Exp { get; set; }

        public int Honor { get; set; }

        public int Money { get; set; }

        public ObservableCollection<Item> RewardItems = new ObservableCollection<Item>();

        public HqReward(int exp, int honor, int money)
        {
            Exp = exp;
            Honor = honor;
            Money = money;
        }

        public override string ToString()
        {
            string outp = "A HQ-tól kaptál:\n" + Exp + " Exp-et, " + Honor + " Honor-t és " + Money + " Pénzt\n\n";

            foreach(var item in RewardItems)
            {
                outp += item.ToString() + "\n\n";
            }

            return outp;
        }

    }
}
