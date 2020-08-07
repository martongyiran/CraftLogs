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
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Repositories.Local
{
    public class LocalDataRepository : ILocalDataRepository
    {
        private readonly IDataService _dataService;

        public LocalDataRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

        private T GetFile<T>(string fileName)
        {
            if (_dataService.IsFileExist(fileName))
            {
                T data;
                var input = _dataService.ReadAllText(fileName);
                data = JsonConvert.DeserializeObject<T>(input);
                if (data != null)
                {
                    return data;
                }
                throw new Exception("Value is null after deserialization.");
            }
            throw new Exception("File not found: " + fileName);
        }

        public void DeleteFile(string fileName)
        {
            if (_dataService.IsFileExist(fileName))
            {
                _dataService.DeleteFile(fileName);
            }
        }

        public void SaveToFile<T>(T data)
        {
            string fileName = "";

            if (typeof(Settings) == data.GetType())
            {
                fileName = FileNames.Settings;
            }
            else if (typeof(ObservableCollection<Log>) == data.GetType())
            {
                fileName = FileNames.Logs;
            }
            else if (typeof(TeamProfile) == data.GetType())
            {
                fileName = FileNames.TeamProfile;
            }
            else if (typeof(ShopProfile) == data.GetType())
            {
                fileName = FileNames.ShopProfile;
            }
            else if (typeof(ArenaProfile) == data.GetType())
            {
                fileName = FileNames.ArenaProfile;
            }
            else if (typeof(HqProfile) == data.GetType())
            {
                fileName = FileNames.HqProfile;
            }
            else
            {
                throw new NotImplementedException("Can't save " + data.GetType().ToString() + " type objects to file.");
            }
            var json = JsonConvert.SerializeObject(data);
            _dataService.WriteAllText(fileName, json);
        }

        public void CreateSettings()
        {
            if (!_dataService.IsFileExist(FileNames.Settings))
            {
                _dataService.CreateFile(FileNames.Settings);
                SaveToFile(_dataService.ReadFromMockData<Settings>(FileNames.Settings));
            }
        }

        public Settings GetSettings()
        {
            return GetFile<Settings>(FileNames.Settings);
        }

        public void ResetSettings()
        {
            DeleteFile(FileNames.Settings);
            CreateSettings();
        }

        public void CreateLogs()
        {
            if (!_dataService.IsFileExist(FileNames.Logs))
            {
                ObservableCollection<Log> logs = new ObservableCollection<Log>();
                _dataService.CreateFile(FileNames.Logs);
                SaveToFile(logs);
            }
        }

        public ObservableCollection<Log> GetLogs()
        {
            return GetFile<ObservableCollection<Log>>(FileNames.Logs);
        }

        public void DeleteLogs()
        {
            if (_dataService.IsFileExist(FileNames.Logs))
            {
                DeleteFile(FileNames.Logs);
            }
        }

        public void CreateTeamProfile(string name, HouseEnum house, CharacterClassEnum cast, string image)
        {
            if (!_dataService.IsFileExist(FileNames.TeamProfile))
            {
                TeamProfile profile = new TeamProfile(name, house, cast, image);
                _dataService.CreateFile(FileNames.TeamProfile);
                SaveToFile(profile);
            }
        }

        public TeamProfile GetTeamProfile()
        {
            var profile = GetFile<TeamProfile>(FileNames.TeamProfile);
            profile.Init();
            return profile;
        }

        public bool IsTeamProfileExist()
        {
            return _dataService.IsFileExist(FileNames.TeamProfile);
        }

        public void DeleteTeamProfile()
        {
            if (_dataService.IsFileExist(FileNames.TeamProfile))
            {
                DeleteFile(FileNames.TeamProfile);
            }
        }

        public void CreateShopProfile()
        {
            if (!_dataService.IsFileExist(FileNames.ShopProfile))
            {
                ShopProfile profile = new ShopProfile();
                _dataService.CreateFile(FileNames.ShopProfile);
                SaveToFile(profile);
            }
        }

        public ShopProfile GetShopProfile()
        {
            var profile = GetFile<ShopProfile>(FileNames.ShopProfile);
            profile.Init();
            return profile;
        }

        public bool IsShopProfileExist()
        {
            return _dataService.IsFileExist(FileNames.ShopProfile);
        }

        public void DeleteShopProfile()
        {
            if (_dataService.IsFileExist(FileNames.ShopProfile))
            {
                DeleteFile(FileNames.ShopProfile);
            }
        }

        public void CreateArenaProfile()
        {
            if (!_dataService.IsFileExist(FileNames.ArenaProfile))
            {
                ArenaProfile profile = new ArenaProfile();
                CombatUnit defaultUnit = new CombatUnit("Thex", 6, 5, 5, 5, 165);
                profile.Leader = defaultUnit;
                _dataService.CreateFile(FileNames.ArenaProfile);
                SaveToFile(profile);
            }
        }

        public ArenaProfile GetArenaProfile()
        {
            var profile = GetFile<ArenaProfile>(FileNames.ArenaProfile);
            return profile;
        }

        public bool IsArenaProfileExist()
        {
            return _dataService.IsFileExist(FileNames.ArenaProfile);
        }

        public void DeleteArenaProfile()
        {
            if (_dataService.IsFileExist(FileNames.ArenaProfile))
            {
                DeleteFile(FileNames.ArenaProfile);
            }
        }

        public void CreateHqProfile()
        {
            if (!_dataService.IsFileExist(FileNames.HqProfile))
            {
                HqProfile profile = new HqProfile();
                _dataService.CreateFile(FileNames.HqProfile);
                SaveToFile(profile);
            }
        }

        public HqProfile GetHqProfile()
        {
            var profile = GetFile<HqProfile>(FileNames.HqProfile);
            return profile;
        }

        public bool IsHqProfileExist()
        {
            return _dataService.IsFileExist(FileNames.HqProfile);
        }

        public void DeleteHqProfile()
        {
            if (_dataService.IsFileExist(FileNames.HqProfile))
            {
                DeleteFile(FileNames.HqProfile);
            }
        }
    }
}
