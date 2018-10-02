using System.Threading.Tasks;

namespace CraftLogs.Repositories.Local
{
    public interface ILocalDataRepository
    {
        /// <summary> Returns all the logs. wip </summary>
        string GetLogsAsync();
        /// <summary> Save logs to file. </summary>
        /// <param name="data">Logs type object.</param>
        Task<bool> SaveLogsToFileAsync(string data);
        /// <summary> Returns the settings. </summary>
        Task<string> GetSettingsAsync();
        /// <summary> Save settings to file. </summary>
        /// <param name="data">Settings type object.</param>
        Task<bool> SaveSettingsToFileAsync(string data);

        //TODO: Profile, Inventory, Items?? etc.
    }
}
