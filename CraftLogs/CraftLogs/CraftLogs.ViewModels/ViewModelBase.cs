using System.Threading.Tasks;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace CraftLogs.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        #region Private

        private string title;
        private bool isBusy;

        #endregion

        #region Public

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        #endregion

        #region Services

        protected INavigationService NavigationService { get; private set; }
        protected ILocalDataRepository DataRepository { get; private set; }
        protected IPageDialogService DialogService { get; private set; }

        #endregion

        #region Ctor

        public ViewModelBase(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            DataRepository = dataRepository;
            DialogService = dialogService;
        }

        #endregion

        #region Virtual

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {

        }

        #endregion

        #region Protected functions

        protected async Task NavigateTo(string navigationLink)
        {
            await NavigationService.NavigateAsync(navigationLink);
        }

        protected async Task NavigateTo(string navigationLink, INavigationParameters parameters)
        {
            await NavigationService.NavigateAsync(navigationLink, parameters);
        }

        protected async Task NavigateToWithoutHistory(string navigationLink)
        {
            await NavigationService.NavigateAsync("../" + navigationLink);
        }

        protected async Task NavigateToWithoutHistory(string navigationLink, INavigationParameters parameters)
        {
            await NavigationService.NavigateAsync("../" + navigationLink, parameters);
        }

        #endregion
    }
}
