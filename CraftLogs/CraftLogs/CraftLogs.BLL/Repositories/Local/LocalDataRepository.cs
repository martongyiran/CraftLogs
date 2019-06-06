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
            else if (typeof(QuestProfile) == data.GetType())
            {
                fileName = FileNames.QuestProfile;
            }
            else if (typeof(TeamProfile) == data.GetType())
            {
                fileName = FileNames.TeamProfile;
            }
            else
            {
                throw new NotImplementedException("Can't save " + data.GetType().ToString() + "type objects to file.");
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

        #endregion

        #region QuestProfile

        public void CreateQuestProfile(string name)
        {
            if (!dataService.IsFileExist(FileNames.QuestProfile))
            {
                QuestProfile profile = new QuestProfile(name);
                dataService.CreateFile(FileNames.QuestProfile);
                SaveToFile(profile);
            }
        }

        public QuestProfile GetQuestProfile()
        {
            return GetFile<QuestProfile>(FileNames.QuestProfile);
        }

        public bool IsQuestProfileExist()
        {
            return dataService.IsFileExist(FileNames.QuestProfile);
        }

        public void DeleteQuestProfile()
        {
            if (dataService.IsFileExist(FileNames.QuestProfile))
            {
                DeleteFile(FileNames.QuestProfile);
            }
        }

        #endregion

        #region Teamprofile

        public void CreateTeamProfile(string name, HouseEnum house, CharacterClassEnum cast)
        {
            if (!dataService.IsFileExist(FileNames.TeamProfile))
            {
                TeamProfile profile = new TeamProfile(name, house, cast);
                dataService.CreateFile(FileNames.TeamProfile);
                SaveToFile(profile);
            }
        }

        public TeamProfile GetTeamProfile()
        {
            return GetFile<TeamProfile>(FileNames.TeamProfile);
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

        #endregion
    }
}
