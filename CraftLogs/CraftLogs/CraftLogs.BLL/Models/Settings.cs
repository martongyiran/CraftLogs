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

using CraftLogs.BLL.Enums;
using Prism.Mvvm;

namespace CraftLogs.BLL.Models
{
    public class Settings : BindableBase
    {
        private int _craftDay;
        private int _craft1Start;
        private int _craft2Start;
        private int _craft1MinPont;
        private int _craft2MinPont;
        private AppModeEnum _appMode;


        public int CraftDay
        {
            get => _craftDay;
            set => SetProperty(ref _craftDay, value);
        }

        public int Craft1Start
        {
            get => _craft1Start;
            set => SetProperty(ref _craft1Start, value);
        }

        public int Craft2Start
        {
            get => _craft2Start;
            set => SetProperty(ref _craft2Start, value);
        }

        public int Craft1MinPont
        {
            get => _craft1MinPont;
            set => SetProperty(ref _craft1MinPont, value);
        }

        public int Craft2MinPont
        {
            get => _craft2MinPont;
            set => SetProperty(ref _craft2MinPont, value);
        }

        public AppModeEnum AppMode
        {
            get => _appMode;
            set => SetProperty(ref _appMode, value);
        }

    }
}
