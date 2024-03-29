﻿/*
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

using CraftLogs.BLL.Models.ArenaModels;
using Newtonsoft.Json;

namespace CraftLogs.BLL.Models
{
    public class ArenaResponse
    {
        public bool IsWin { get; set; }

        public int Money { get; set; }

        [JsonIgnore]
        public CombatLogDetailsModel CombatLog { get; set; } = new CombatLogDetailsModel();

        public string GetResoult()
        {
            return IsWin ? "Nyertél az arénában!\nA jutalmad: 1 EXP, 1 hírnév és " + Money + " $!" : "Vesztettél az arénában, de szereztél 1 EXP-et, 1 hírnevet és " + Money + " $-t!";
        }
    }
}
