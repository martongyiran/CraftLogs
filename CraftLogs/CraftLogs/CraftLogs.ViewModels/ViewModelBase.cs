using CraftLogs.Repositories.Local;
using CraftLogs.Services;
using Prism.Mvvm;
using Prism.Navigation;

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
        protected IDialogService DialogService { get; private set; }
        #endregion

        #region Ctor
        public ViewModelBase(INavigationService navigationService, ILocalDataRepository dataRepository, IDialogService dialogService)
        {
            NavigationService = navigationService;
            DataRepository = dataRepository;
            DialogService = dialogService;
        }
        #endregion

        #region Virtual
        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
        #endregion

        #region Protected functions

        protected async void NavigateTo(string navigationLink)
        {
            await NavigationService.NavigateAsync(navigationLink);
        }

        #endregion
    }
}
