using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
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

        private Settings settings;

        private DelegateCommand saveCommand;
        private DelegateCommand cancelCommand;
        private DelegateCommand<object> selectCommand;

        #endregion

        #region Public

        public DelegateCommand SaveCommand => saveCommand ?? (saveCommand = new DelegateCommand(async () => await Save()));
        public DelegateCommand CancelCommand => cancelCommand ?? (cancelCommand = new DelegateCommand(async () => await Cancel(), CanSubmit).ObservesProperty(() => IsBusy));
        public DelegateCommand<object> SelectCommand => selectCommand ?? (selectCommand = new DelegateCommand<object>( (a) => Select(a)));

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

        public List<string> Houses { get; set; } = new List<string> { Texts.House1, Texts.House2, Texts.House3, Texts.House4, Texts.House5, Texts.House6 };

        private string house;

        public string House
        {
            get { return house; }
            set { SetProperty(ref house, value); SetHousePrefix(); SetImages(); }
        }

        public List<CharacterClassEnum> Classes { get; set; } = new List<CharacterClassEnum> { CharacterClassEnum.Mage, CharacterClassEnum.Rogue, CharacterClassEnum.Warrior };

        private CharacterClassEnum cast;

        public CharacterClassEnum Cast
        {
            get { return cast; }
            set { SetProperty(ref cast, value); SetCastPrefix(); SetImages(); }
        }

        private string img1;

        public string Img1
        {
            get { return img1; }
            set { SetProperty(ref img1, value); }
        }

        private string img2;

        public string Img2
        {
            get { return img2; }
            set { SetProperty(ref img2, value); }
        }

        private string img3;

        public string Img3
        {
            get { return img3; }
            set { SetProperty(ref img3, value); }
        }

        private string housePrefix;

        private string castPrefix;

        private string selectedImage = null;

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            settings = DataRepository.GetSettings();
        }

        #endregion

        #region Functions

        private async Task Save()
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrEmpty(selectedImage))
            {
                bool sure = await DialogService.DisplayAlertAsync(Texts.Save, Texts.RegisterNameSave, Texts.Save, Texts.Cancel);
                if (sure )
                {
                        settings.AppMode = AppModeEnum.Team;
                        DataRepository.SaveToFile(settings);
                        DataRepository.CreateTeamProfile(Name, NameToEnum(House), Cast, selectedImage);
                        DataRepository.CreateLogs();
                        await NavigateToWithoutHistory(NavigationLinks.ProfilePage);
                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.RegisterMissing, Texts.Ok);
            }
        }

        private async Task Cancel()
        {
            IsBusy = true;

            var settings = DataRepository.GetSettings();
            settings.AppMode = AppModeEnum.None;
            DataRepository.SaveToFile(settings);
            await NavigateToWithoutHistory(NavigationLinks.SelectModePage);
        }

        private void SetHousePrefix()
        {
            switch (NameToEnum(House))
            {
                case HouseEnum.House1:
                    housePrefix = "h1_";
                    break;
                case HouseEnum.House2:
                    housePrefix = "h2_";
                    break;
                case HouseEnum.House3:
                    housePrefix = "h3_";
                    break;
                case HouseEnum.House4:
                    housePrefix = "h4_";
                    break;
                case HouseEnum.House5:
                    housePrefix = "h5_";
                    break;
                case HouseEnum.House6:
                    housePrefix = "h6_";
                    break;
            }
        }

        private HouseEnum NameToEnum(string name)
        {
            switch (name)
            {
                case Texts.House1:
                    return HouseEnum.House1;
                case Texts.House2:
                    return HouseEnum.House2;
                case Texts.House3:
                    return HouseEnum.House3;
                case Texts.House4:
                    return HouseEnum.House4;
                case Texts.House5:
                    return HouseEnum.House5;
                case Texts.House6:
                    return HouseEnum.House6;
                default:
                    return HouseEnum.House1;
            }
        }

        private void SetCastPrefix()
        {
            switch (Cast)
            {
                case CharacterClassEnum.Mage:
                    castPrefix = "mage";
                    break;
                case CharacterClassEnum.Warrior:
                    castPrefix = "warrior";
                    break;
                case CharacterClassEnum.Rogue:
                    castPrefix = "rogue";
                    break;
            }
        }

        private string GetImgUrl(string num)
        {
            return "@drawable/" + housePrefix + castPrefix + num + ".png";
        }

        private void SetImages()
        {
            Img1 = GetImgUrl("1");
            Img2 = GetImgUrl("2");
            Img3 = GetImgUrl("3");
        }

        private void Select(object a)
        {
            switch ((int)a)
            {
                case 1:
                    selectedImage = Img1;
                    break;
                case 2:
                    selectedImage = Img2;
                    break;
                case 3:
                    selectedImage = Img3;
                    break;
            }
        }

        #endregion

    }
}
