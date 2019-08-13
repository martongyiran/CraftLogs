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
using CraftLogs.BLL.Services.Interfaces;
using System;

namespace CraftLogs.BLL.Services
{
    public class CombatService : ICombatService
    {
        private Random random;
        private CombatUnit Player1;
        private CombatUnit Player2;

        public CombatService()
        {
            random = new Random();
        }

        public bool CanFight(CombatUnit player1, CombatUnit player2)
        {
            return player1.House != player2.House && player1.Name != player2.Name;
        }

        public ArenaResponse Fight(CombatUnit player1, CombatUnit player2)
        {
            ArenaResponse resp = new ArenaResponse();

            Player1 = player1;
            Player2 = player2;

            int hp1 = Player1.Hp;
            int hp2 = Player2.Hp;

            int def1 = (int)(Player1.Def * 0.33);
            int def2 = (int)(Player2.Def * 0.33);
            
            for(int i = 0; i <10; i++)
            {

            }

            throw new NotImplementedException();
        }

        private int Hit()
        {

            return 0;
        }

        private bool IsCrit(CombatUnit player)
        {
            var rnd = random.Next(0, 101);

            return player.CritR <= rnd;
        }

        private bool IsDodge(CombatUnit player)
        {
            var rnd = random.Next(0, 101);

            return player.Dodge <= rnd;
        }
    }
}
