using CraftLogs.BLL.Models;

namespace CraftLogs.BLL.Repositories.Local.Interfaces
{
    public interface ILocalDataRepository
    {

        /// <summary> 
        /// Creates a logs file(if it does not exist) from embedded resource.
        /// </summary>
        void CreateLogs();
        /// <summary> 
        /// Returns all the logs. 
        /// </summary>
        Logs GetLogs();
        /// <summary> 
        /// Creates a settings file(if it does not exist) from embedded resource.
        /// </summary>
        void CreateSettings();
        /// <summary> 
        /// Deletes the file.
        /// </summary>
        void DeleteFile(string fileName);
        /// <summary> 
        /// Returns the settings. 
        /// </summary>
        Settings GetSettings();
        /// <summary> 
        /// Resets settings.
        /// </summary>
        void ResetSettings();
        /// <summary> 
        /// Saves a model to a file.
        /// </summary>
        void SaveToFile<T>(T data);

        //TODO: Profile, Inventory, Items?? etc.
    }
}
