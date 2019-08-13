/*
Copyright 2019 Gyirán Márton Áron

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
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftLogs.BLL.Services.Interfaces
{
    public interface ICombatService
    {
        /// <summary>
        /// Returns if the two player are able to fight.
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns>If the two players are able to fight true else false.</returns>
        bool CanFight(CombatUnit player1, CombatUnit player2);

        /// <summary>
        /// Creates an ArenaResponse
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns>ArenaResponse, statics about the fight.</returns>
        ArenaResponse Fight(CombatUnit player1, CombatUnit player2);

    }
}
