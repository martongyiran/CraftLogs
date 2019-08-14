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
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Services.Interfaces;
using System;

namespace CraftLogs.BLL.Services
{
    public class CombatService : ICombatService
    {
        private ILocalDataRepository dataRepository;

        private Settings settings;
        private Random random;
        private DateTime date;
        private CombatUnit Player1;
        private CombatUnit Player2;
        int def1;
        int def2;
        int hp1;
        int hp2;

        public CombatService(ILocalDataRepository datarepository)
        {
            random = new Random();
            dataRepository = datarepository;
            settings = dataRepository.GetSettings();
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

            resp.CombatLog = Player1.Name + " vs. " + Player2.Name + "\n";
            resp.CombatLog += "Kör | Sebzés | Hp\n";
            resp.CombatLog += "0. | 0 - 0 | "+ hp1 + " - " + hp2 + "\n";
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

                string log = round + ". | " + player1hit + " - " + player2hit + " | " + hp1 + " - " + hp2 + "\n";
                resp.CombatLog += log;
                //System.Diagnostics.Debug.WriteLine("----- "+ log);

                round++;
            }

            resp.CombatLog += "Dps: " + Math.Round((allDmg1 / (double)round),2) + " - " + Math.Round((allDmg2 / (double)round),2) + "\n";

            if(hp1 >= 0)
            {
                resp.IsWin = false;
                //System.Diagnostics.Debug.WriteLine("----- Leader win.");
                resp.CombatLog += "----- "+Player1.Name+" a győztes.";
            }
            else if(hp2 >= 0)
            {
                resp.IsWin = true;
               // System.Diagnostics.Debug.WriteLine("----- Attacker win.");
                resp.CombatLog += "----- "+Player2.Name+" a győztes.";
            }

            resp.Money = SetMoney(leader.Hp, Player1.Hp);

            return resp;

        }

        private int Hit(CombatUnit player, int enemyDef)
        {
            return (int)(player.Atk * (100-enemyDef)/100.0);
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
            int minPoint;
            int maxPoint;
            var actHour = date.Hour;
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

            return ((int)((enemyRemainingHp / (double)originalHp) * maxPoint) - 10) * 10;
        }

    }
}
