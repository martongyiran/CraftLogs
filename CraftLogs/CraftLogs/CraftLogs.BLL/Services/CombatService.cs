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
using CraftLogs.BLL.Models.ArenaModels;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using System;

namespace CraftLogs.BLL.Services
{
    public class CombatService : ICombatService
    {
        private readonly ILocalDataRepository _dataRepository;
        private readonly Settings settings;
        private readonly Random random;
        private readonly DateTime date;

        private CombatUnit Player1;
        private CombatUnit Player2;
        int def1;
        int def2;
        int hp1;
        int hp2;

        public CombatService(ILocalDataRepository datarepository)
        {
            random = new Random();
            _dataRepository = datarepository;
            settings = _dataRepository.GetSettings();
            date = DateTime.Now;
        }

        public bool CanFight(CombatUnit player1, CombatUnit player2)
        {
            return player1.House != player2.House && player1.Name != player2.Name;
        }

        public ArenaResponse Fight(CombatUnit leader, CombatUnit attacker)
        {
            ArenaResponse resp = new ArenaResponse();

            Player1 = leader;
            Player2 = attacker;

            hp1 = Player1.Hp;
            hp2 = Player2.Hp;

            def1 = (int)(Player1.Def * 0.33);
            def2 = (int)(Player2.Def * 0.33);

            int round = 1;
            int allDmg1 = 0;
            int allDmg2 = 0;

            resp.CombatLog.Title = Player1.Name + " vs. " + Player2.Name;
            resp.CombatLog.Rounds.Add(new CombatLogUnit(0, 0, 0, hp1, hp2));

            while (hp1 >= 0 && hp2 >= 0)
            {

                var player1hit = Hit(Player1, def2);
                var player2hit = Hit(Player2, def1);

                player1hit = IsDodge(Player2) ? 0 : player1hit;
                player2hit = IsDodge(Player1) ? 0 : player2hit;

                player1hit = IsCrit(Player1) ? player1hit * 2 : player1hit;
                player2hit = IsCrit(Player2) ? player2hit * 2 : player2hit;

                hp1 -= player2hit;
                hp2 -= player1hit;

                allDmg1 += player1hit;
                allDmg2 += player2hit;

                resp.CombatLog.Rounds.Add(new CombatLogUnit(round, player1hit, player2hit, hp1, hp2));

                round++;
            }

            resp.CombatLog.DpsText = "Dps: " + Math.Round((allDmg1 / (double)round-1), 2) + " - " + Math.Round((allDmg2 / (double)round-1), 2);

            if (hp1 >= 0)
            {
                resp.IsWin = false;
                resp.CombatLog.ResultText = Player1.Name + " a győztes.";
            }
            else if (hp2 >= 0)
            {
                resp.IsWin = true;
                resp.CombatLog.ResultText = Player2.Name + " a győztes.";
            }

            resp.Money = SetMoney(leader.Hp, hp1);

            return resp;

        }

        private int Hit(CombatUnit player, int enemyDef)
        {
            var hit = 0;
            for(int i = 0; i < player.Atk; i++)
            {
                hit += random.Next(0, 3);
            }
            return (int)(hit * (100 - enemyDef) / 100.0);
        }

        private bool IsCrit(CombatUnit player)
        {
            var rnd = random.Next(0, 101);

            return rnd <= player.CritR;
        }

        private bool IsDodge(CombatUnit player)
        {
            var rnd = random.Next(0, 101);

            return rnd <= player.Dodge;
        }

        private int SetMoney(int originalHp, int enemyRemainingHp)
        {
            //Warning in DEV mode, because of #if DEV
#pragma warning disable CS0168 // Variable is declared but never used
            int minPoint;
#pragma warning restore CS0168 // Variable is declared but never used
            int maxPoint;
            enemyRemainingHp = enemyRemainingHp < 0 ? 0 : enemyRemainingHp;
            var actHour = date.Hour;
#if DEV
            maxPoint = 50;
#else
            if (settings.CraftDay == 1)
            {
                if (actHour == settings.Craft1Start)
                {
                    minPoint = settings.Craft1MinPont;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 1))
                {
                    minPoint = settings.Craft1MinPont + 2;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 2))
                {
                    minPoint = settings.Craft1MinPont + 5;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 3))
                {
                    minPoint = settings.Craft1MinPont + 7;
                    maxPoint = minPoint + 10;
                }
                else if (actHour == (settings.Craft1Start + 4))
                {
                    minPoint = settings.Craft1MinPont + 7;
                    maxPoint = minPoint + 10;
                }
                else
                {
                    maxPoint = 0;
                }

            }
            else
            {
                if (actHour == settings.Craft2Start)
                {
                    minPoint = settings.Craft2MinPont;
                    maxPoint = minPoint + 12;
                }
                else if (actHour == (settings.Craft2Start + 1))
                {
                    minPoint = settings.Craft2MinPont + 2;
                    maxPoint = minPoint + 14;
                }
                else if (actHour == (settings.Craft2Start + 2))
                {
                    minPoint = settings.Craft2MinPont + 4;
                    maxPoint = minPoint + 15;
                }
                else if (actHour == (settings.Craft2Start + 3))
                {
                    minPoint = settings.Craft2MinPont + 6;
                    maxPoint = minPoint + 18;
                }
                else if (actHour == (settings.Craft2Start + 4))
                {
                    minPoint = settings.Craft2MinPont + 6;
                    maxPoint = minPoint + 18;
                }
                else
                {
                    maxPoint = 0;
                }

        }
#endif
            var a = (1.0 - (enemyRemainingHp / (double)originalHp)) * maxPoint;
            var b = a > 10 ? a - 10 : a;
            var c = b * 10;

            return (int)c;
        }
        
    }
}
