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

using CraftLogs.BLL.Models;
using System.Collections.ObjectModel;

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
        ObservableCollection<Log> GetLogs();

        //TODO: Profile, Inventory, Items?? etc.
    }
}
