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

        /// <summary>
        /// Creates a QuestProfile file.
        /// </summary>
        void CreateQuestProfile(string name);

        /// <summary>
        /// Returns the QuestProfile.
        /// </summary>
        /// <returns></returns>
        QuestProfile GetQuestProfile();

        /// <summary>
        /// Checks if QuestProfile exist.
        /// </summary>
        /// <returns></returns>
        bool IsQuestProfileExist();

        /// <summary>
        /// Deletes QuestProfile file.
        /// </summary>
        void DeleteQuestProfile();

        /// <summary>
        /// Creates a TeamProfile file.
        /// </summary>
        void CreateTeamProfile(string name, HouseEnum house, CharacterClassEnum cast, string image);

        /// <summary>
        /// Returns the TeamProfile.
        /// </summary>
        /// <returns></returns>
        TeamProfile GetTeamProfile();

        /// <summary>
        /// Checks if TeamProfile exist.
        /// </summary>
        /// <returns></returns>
        bool IsTeamProfileExist();

        /// <summary>
        /// Deletes TeamProfile file.
        /// </summary>
        void DeleteTeamProfile();
        //TODO: Profile, Inventory, Items?? etc.
    }
}
