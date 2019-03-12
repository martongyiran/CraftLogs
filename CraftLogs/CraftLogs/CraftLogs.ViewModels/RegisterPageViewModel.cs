using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {

        #region Private

        private DelegateCommand saveCommand;

        #endregion

        #region Public

        public DelegateCommand SaveCommand => saveCommand ?? (saveCommand = new DelegateCommand(async () => await Save()));

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

        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        #endregion

        #region Overrides

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            var mode = parameters["mode"] as string;

            IsQuest = mode == "quest" ? true : false;
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

                }
            }
            else
            {
                await DialogService.DisplayAlertAsync(Texts.Error, Texts.RegisterMissingName, Texts.Ok);
            }
        }

        #endregion


    }
}
