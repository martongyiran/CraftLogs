using CraftLogs.BLL.Models;

namespace CraftLogs.BLL.Repositories.Local.Interfaces
{
    public interface ILocalDataRepository
    {
        /// <summary> 
        /// Returns all the logs. wip 
        /// </summary>
        string GetLogs();
        /// <summary> Save logs to file. </summary>
        /// <param name="data">Logs type object.</param>
        bool SaveLogsToFile(string data);
        /// <summary> 
        /// Creates a settings file(if it does not exist) from embedded resource.
        /// </summary>
        void CreateSettings();
        /// <summary> 
        /// Deletes the settings file.
        /// </summary>
        void DeleteSettings();
        /// <summary> 
        /// Returns the settings. 
        /// </summary>
        Settings GetSettings();
        /// <summary> 
        /// Resets settings.
        /// </summary>
        void ResetSettings();
        /// <summary> Save settings to file. </summary>
        /// <param name="data">Settings type object.</param>
        void SaveSettingsToFile(Settings data);

        //TODO: Profile, Inventory, Items?? etc.
    }
}
