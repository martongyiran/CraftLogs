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
        #region Ctor
        private readonly IDataService dataService;
        public LocalDataRepository(IDataService dataService)
        {
            this.dataService = dataService;
        }
        #endregion

        #region Private functions

        private T GetFile<T>(string fileName)
        {
            if (dataService.IsFileExist(fileName))
            {
                T data;
                var input = dataService.ReadAllText(fileName);
                data = JsonConvert.DeserializeObject<T>(input);
                if (data != null)
                {
                    return data;
                }
                throw new Exception("Value is null after deserialization.");
            }
            throw new Exception("File not found: " + fileName);
        }

        #endregion

        #region Public functions

        #region General

        public void DeleteFile(string fileName)
        {
            if (dataService.IsFileExist(fileName))
            {
                dataService.DeleteFile(fileName);
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
            else if (typeof(ProfileQr) == data.GetType())
            {
                fileName = FileNames.SpectatorProfile;
            }
            else
            {
                throw new NotImplementedException("Can't save " + data.GetType().ToString() + " type objects to file.");
            }
            var json = JsonConvert.SerializeObject(data);
            dataService.WriteAllText(fileName, json);
        }

        #endregion

        #region Settings

        public void CreateSettings()
        {
            if (!dataService.IsFileExist(FileNames.Settings))
            {
                dataService.CreateFile(FileNames.Settings);
                SaveToFile(dataService.ReadFromMockData<Settings>(FileNames.Settings));
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

        #endregion

        #region Logs

        public void CreateLogs()
        {
            if (!dataService.IsFileExist(FileNames.Logs))
            {
                ObservableCollection<Log> logs = new ObservableCollection<Log>();
                dataService.CreateFile(FileNames.Logs);
                SaveToFile(logs);
            }
        }

        public ObservableCollection<Log> GetLogs()
        {
            return GetFile<ObservableCollection<Log>>(FileNames.Logs);
        }

        public void DeleteLogs()
        {
            if (dataService.IsFileExist(FileNames.Logs))
            {
                DeleteFile(FileNames.Logs);
            }
        }

        #endregion
        
        #region Teamprofile

        public void CreateTeamProfile(string name, HouseEnum house, CharacterClassEnum cast, string image)
        {
            if (!dataService.IsFileExist(FileNames.TeamProfile))
            {
                TeamProfile profile = new TeamProfile(name, house, cast, image);
                dataService.CreateFile(FileNames.TeamProfile);
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
            return dataService.IsFileExist(FileNames.TeamProfile);
        }

        public void DeleteTeamProfile()
        {
            if (dataService.IsFileExist(FileNames.TeamProfile))
            {
                DeleteFile(FileNames.TeamProfile);
            }
        }

        #endregion

        #region Shop

        public void CreateShopProfile()
        {
            if (!dataService.IsFileExist(FileNames.ShopProfile))
            {
                ShopProfile profile = new ShopProfile();
                dataService.CreateFile(FileNames.ShopProfile);
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
            return dataService.IsFileExist(FileNames.ShopProfile);
        }

        public void DeleteShopProfile()
        {
            if (dataService.IsFileExist(FileNames.ShopProfile))
            {
                DeleteFile(FileNames.ShopProfile);
            }
        }

        #endregion

        #region Arena

        public void CreateArenaProfile()
        {
            if (!dataService.IsFileExist(FileNames.ArenaProfile))
            {
                ArenaProfile profile = new ArenaProfile();
                CombatUnit defaultUnit = new CombatUnit("Thex", 6, 5, 5, 5, 165);
                profile.Leader = defaultUnit;
                dataService.CreateFile(FileNames.ArenaProfile);
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
            return dataService.IsFileExist(FileNames.ArenaProfile);
        }

        public void DeleteArenaProfile()
        {
            if (dataService.IsFileExist(FileNames.ArenaProfile))
            {
                DeleteFile(FileNames.ArenaProfile);
            }
        }

        #endregion

        #region HQ

        public void CreateHqProfile()
        {
            if (!dataService.IsFileExist(FileNames.HqProfile))
            {
                HqProfile profile = new HqProfile();
                dataService.CreateFile(FileNames.HqProfile);
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
            return dataService.IsFileExist(FileNames.HqProfile);
        }

        public void DeleteHqProfile()
        {
            if (dataService.IsFileExist(FileNames.HqProfile))
            {
                DeleteFile(FileNames.HqProfile);
            }
        }

        #endregion

        #region Spectator

        public void CreateSpectatorProfile()
        {
            if (!dataService.IsFileExist(FileNames.SpectatorProfile))
            {
                ProfileQr profile = new ProfileQr();
                dataService.CreateFile(FileNames.SpectatorProfile);
                SaveToFile(profile);
            }
        }

        public ProfileQr GetSpectatorProfile()
        {
            var profile = GetFile<ProfileQr>(FileNames.SpectatorProfile);
            profile.Init();
            return profile;
        }

        public bool IsSpectatorProfileExist()
        {
            return dataService.IsFileExist(FileNames.SpectatorProfile);
        }

        public void DeleteSpectatorProfile()
        {
            if (dataService.IsFileExist(FileNames.SpectatorProfile))
            {
                DeleteFile(FileNames.SpectatorProfile);
            }
        }

        #endregion

        #endregion
    }
}
