﻿using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Newtonsoft.Json;
using System;

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

        public void CreateSettings()
        {
            if (!dataService.IsFileExist(FileNames.Settings))
            {
                dataService.CreateFile(FileNames.Settings);
                SaveToFile(dataService.ReadFromMockData<Settings>(FileNames.Settings));
            }
        }

        public void DeleteFile(string fileName)
        {
            if (dataService.IsFileExist(fileName))
            {
                dataService.DeleteFile(fileName);
            }
        }

        public void ResetSettings()
        {
            DeleteFile(FileNames.Settings);
            CreateSettings();
        }

        public Settings GetSettings()
        {
            return GetFile<Settings>(FileNames.Settings);
        }

        public void CreateLogs()
        {
            throw new NotImplementedException();
        }

        public void DeleteLogs()
        {
            throw new NotImplementedException();
        }

        public Logs GetLogs()
        {
            return GetFile<Logs>(FileNames.Logs);
        }

        public void SaveToFile<T>(T data)
        {
            string fileName = "";
            if (typeof(Settings) == data.GetType())
            {
                fileName = FileNames.Settings;
            }
            else if (typeof(Logs) == data.GetType())
            {
                fileName = FileNames.Logs;
            }
            else
            {
                throw new NotImplementedException("Can't save " + data.GetType().ToString() + "type objects to file.");
            }
            var json = JsonConvert.SerializeObject(data);
            dataService.WriteAllText(fileName, json);
        }
    }
}
