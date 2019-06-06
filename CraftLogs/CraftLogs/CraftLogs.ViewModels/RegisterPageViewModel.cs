using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {

        #region Private

        private DelegateCommand saveCommand;

        private DelegateCommand<HouseEnum> selectHouseCommand;

        #endregion

        #region Public

        public DelegateCommand SaveCommand => saveCommand ?? (saveCommand = new DelegateCommand(async () => await Save()));

        public DelegateCommand<HouseEnum> SelectHouseCommand => selectHouseCommand ?? (selectHouseCommand = new DelegateCommand<HouseEnum>(SelectHouse()));

        #endregion

        #region Ctor

        public RegisterPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService) : base(navigationService, dataRepository, dialogService)
        {
            Title = Texts.RegisterTitle;
        }

        #endregion

        #region Properties

        private bool isQuest;

        public bool IsQuest
        {
            get { return isQuest; }
            set { SetProperty(ref isQuest, value); }
        }

        private bool isTeam;

        public bool IsTeam
        {
            get { return isTeam; }
            set { SetProperty(ref isTeam, value); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public ObservableCollection<HouseEnum> Houses { get; set; } = new ObservableCollection<HouseEnum> { HouseEnum.House1, HouseEnum.House2, HouseEnum.House3, HouseEnum.House4, HouseEnum.House5, HouseEnum.House6 };

        private HouseEnum house;

        public HouseEnum House
        {
            get { return house; }
            set { SetProperty(ref house, value); }
        }

        public ObservableCollection<CharacterClassEnum> Classes { get; set; } = new ObservableCollection<CharacterClassEnum> { CharacterClassEnum.Mage, CharacterClassEnum.Rogue, CharacterClassEnum.Warrior };

        private CharacterClassEnum cast;

        public CharacterClassEnum Cast
        {
            get { return cast; }
            set { SetProperty(ref cast, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            var mode = parameters["mode"] as string;

            IsQuest = mode == "quest" ? true : false;

            IsTeam = mode == "team" ? true : false;
        }

        #endregion

        #region Functions

        private async Task Save()
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name))
            {
                bool sure = await DialogService.DisplayAlertAsync(Texts.Save, Texts.RegisterNameSave, Texts.Save, Texts.Cancel);
                if (sure && IsQuest)
                {
                    DataRepository.CreateQuestProfile(Name);
                    await NavigateToWithoutHistory(NavigationLinks.MainPage);
                }
                else if (sure && !IsQuest)
                {
                    DataRepository.CreateTeamProfile(Name, House, Cast);
                    await NavigateToWithoutHistory(NavigationLinks.MainPage);
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.RegisterMissingName, Texts.Ok);
            }
        }

        private void SelectHouse()
        {
            House = arg;
            System.Diagnostics.Debug.WriteLine("------------------------------------"+arg.ToString());
        }

        #endregion


    }
}
