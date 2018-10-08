using CraftLogs.Models;
using CraftLogs.Services;
using CraftLogs.Values;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace CraftLogs.Repositories.Local
{
    public class LocalDataRepository : ILocalDataRepository
    {
        #region Ctor
        public IDataService DataService { get; private set; }
        public LocalDataRepository(IDataService dataService)
        {
            DataService = dataService;
        }
        #endregion
        public string GetLogs()
        {
            DataService.CreateFile("data");
            DataService.WriteAllText("data", "Text in file");
            var a = DataService.ReadAllText("data");
            return a;
            //throw new NotImplementedException();
        }

        public void CreateSettings()
        {
            if (!DataService.IsFileExist(FileNames.Settings))
            {
                DataService.CreateFile(FileNames.Settings);

                var assembly = typeof(FileNames).GetTypeInfo().Assembly;
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
            if (DataService.IsFileExist(FileNames.Settings))
            {
                DataService.DeleteFile(FileNames.Settings);
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
            var input = DataService.ReadAllText(FileNames.Settings);
            data = JsonConvert.DeserializeObject<Settings>(input);
            return data;
        }

        public void SaveSettingsToFile(Settings data)
        {
            var json = JsonConvert.SerializeObject(data);
            DataService.WriteAllText(FileNames.Settings, json);
        }

        public bool SaveLogsToFile(string data)
        {
            throw new NotImplementedException();
        }

    }
}
