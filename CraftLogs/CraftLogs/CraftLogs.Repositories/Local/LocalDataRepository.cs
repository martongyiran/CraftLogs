using CraftLogs.Services;
using System;
using System.Threading.Tasks;

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
        public string GetLogsAsync()
        {
            DataService.CreateFile("data");
            DataService.WriteAllText("data", "Text in file");
            var a = DataService.ReadAllText("data");
            return a;
           //throw new NotImplementedException();
        }

        public Task<string> GetSettingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveLogsToFileAsync(string data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveSettingsToFileAsync(string data)
        {
            throw new NotImplementedException();
        }
    }
}
