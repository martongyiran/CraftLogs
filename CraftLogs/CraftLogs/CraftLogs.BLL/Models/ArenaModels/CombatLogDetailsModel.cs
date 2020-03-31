using Prism.Mvvm;
using System.Collections.Generic;

namespace CraftLogs.BLL.Models.ArenaModels
{
    public class CombatLogDetailsModel : BindableBase
    {
        private string _title;
        private string _dpsText;
        private string _resultText;
        private List<CombatLogUnit> _rounds = new List<CombatLogUnit>();

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string DpsText
        {
            get => _dpsText;
            set => SetProperty(ref _dpsText, value);
        }

        public string ResultText
        {
            get => _resultText;
            set => SetProperty(ref _resultText, value);
        }

        public List<CombatLogUnit> Rounds
        {
            get => _rounds;
            set => SetProperty(ref _rounds, value);
        }
    }

    public class CombatLogUnit : BindableBase
    {
        private int _round;
        private int _dmg1;
        private int _dmg2;
        private int _hp1;
        private int _hp2;

        public int Round
        {
            get => _round;
            set => SetProperty(ref _round, value);
        }

        public int Dmg1
        {
            get => _dmg1;
            set => SetProperty(ref _dmg1, value);
        }

        public int Dmg2
        {
            get => _dmg2;
            set => SetProperty(ref _dmg2, value);
        }

        public int Hp1
        {
            get => _hp1;
            set => SetProperty(ref _hp1, value);
        }

        public int Hp2
        {
            get => _hp2;
            set => SetProperty(ref _hp2, value);
        }
        public CombatLogUnit(int round, int dmg1, int dmg2, int hp1, int hp2)
        {
            Round = round;
            Dmg1 = dmg1;
            Dmg2 = dmg2;
            Hp1 = hp1;
            Hp2 = hp2;
        }
    }
}
