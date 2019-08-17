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

namespace CraftLogs.BLL.Services.Interfaces
{
    public interface IItemGeneratorService
    {
        /// <summary>
        /// Returns a random item based on given Tier.
        /// </summary>
        /// <param name="tier"></param>
        /// <returns>Random item.</returns>
        Item GetRandomItem(int tier);

        /// <summary>
        /// Returns a specific item based on tier and type.
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="itemType"></param>
        /// <returns>A brand new item.</returns>
        Item GetSpecificItem(int tier, ItemTypeEnum itemType);

        Item GetLegendary(LegendaryEnum legendary);
    }
}
