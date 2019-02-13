using CraftLogs.BLL.Models;

namespace CraftLogs.BLL.Repositories.Local.Interfaces
{
    public interface ILocalDataRepository
    {

        /// <summary> 
        /// Deletes the file.
        /// </summary>
        void DeleteFile(string fileName);

        /// <summary> 
        /// Saves a model to a file.
        /// </summary>
        void SaveToFile<T>(T data);

        /// <summary> 
        /// Creates a settings file(if it does not exist) from embedded resource.
        /// </summary>
        void CreateSettings();

        /// <summary> 
        /// Returns the settings. 
        /// </summary>
        Settings GetSettings();

        /// <summary> 
        /// Resets settings.
        /// </summary>
        void ResetSettings();

        /// <summary> 
        /// Creates a logs file(if it does not exist) from embedded resource.
        /// </summary>
        void CreateLogs();

        /// <summary> 
        /// Returns all the logs. 
        /// </summary>
        Logs GetLogs();

        //TODO: Profile, Inventory, Items?? etc.
    }
}
