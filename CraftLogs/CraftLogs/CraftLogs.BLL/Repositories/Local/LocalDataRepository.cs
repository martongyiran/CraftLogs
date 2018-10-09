using CraftLogs.BLL.Models;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Services;
using CraftLogs.Values;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

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
        public string GetLogs()
        {
            throw new NotImplementedException();
        }

        public void CreateSettings()
        {
            if (!dataService.IsFileExist(FileNames.Settings))
            {
                dataService.CreateFile(FileNames.Settings);

                var assembly = typeof(LocalDataRepository).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(string.Format(FileNames.FileAssembly, FileNames.Settings));

                Settings data;
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject<Settings>(json);
                }

                SaveSettingsToFile(data);
            }
        }

        public void DeleteSettings()
        {
            if (dataService.IsFileExist(FileNames.Settings))
            {
                dataService.DeleteFile(FileNames.Settings);
            }
        }

        public void ResetSettings()
        {
            DeleteSettings();
            CreateSettings();
        }

        public Settings GetSettings()
        {
            Settings data = new Settings();
            var input = dataService.ReadAllText(FileNames.Settings);
            data = JsonConvert.DeserializeObject<Settings>(input);
            if (data != null)
            {
                return data;
            }
            throw new NullReferenceException("Settings is null.");
        }

        public void SaveSettingsToFile(Settings data)
        {
            var json = JsonConvert.SerializeObject(data);
            dataService.WriteAllText(FileNames.Settings, json);
        }

        public bool SaveLogsToFile(string data)
        {
            throw new NotImplementedException();
        }

    }
}
