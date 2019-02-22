﻿/*
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

using CraftLogs.BLL.Enums;

namespace CraftLogs.BLL.Models
{
    public class Settings
    {
        public int CraftDay { get; set; }
        public int Craft1Start { get; set; }
        public int Craft2Start { get; set; }
        public int Craft1MinPont { get; set; }
        public int Craft2MinPont { get; set; }
        public AppModeEnum AppMode { get; set; }
    }
}
