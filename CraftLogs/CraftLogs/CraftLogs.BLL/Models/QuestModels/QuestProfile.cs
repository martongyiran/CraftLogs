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
using System.Linq;

namespace CraftLogs.BLL.Models
{

    public class QuestProfile
    {
        public string QuestName { get; set; }

        public ObservableCollection<int> AvgScore { get; set; } = new ObservableCollection<int>();

        public QuestProfile(string questName)
        {
            QuestName = questName;
        }

        public double GetAvgScore()
        {
            double sum = AvgScore.Sum();
            double avg = sum / AvgScore.Count();
            return avg;
        }
    }

    public class QuestProfileQR
    {
        public string QuestName { get; set; }

        public double Avg { get; set; }

        public QuestProfileQR(QuestProfile profile)
        {
            QuestName = profile.QuestName;
            Avg = profile.GetAvgScore();
        }

        public QuestProfileQR(string name, double avg)
        {
            QuestName = name;
            Avg = avg;
        }
    }

}
