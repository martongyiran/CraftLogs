using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class QRScannerPageViewModel : ViewModelBase
    {
        #region Private

        private DelegateCommand<string> getResultCommand;

        #endregion

        #region Public

        public DelegateCommand<string> GetResultCommand => getResultCommand ?? (getResultCommand = new DelegateCommand<string>(async (a) => await HandleResult(a)));
        
        #endregion

        #region ctor

        public QRScannerPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = "QR Scanner Page";
        }

        #endregion

        private async Task HandleResult(string text)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("res", text);
            await NavigateToWithoutHistory(NavigationLinks.QRHandlerPage, param);
        }
    }
}
