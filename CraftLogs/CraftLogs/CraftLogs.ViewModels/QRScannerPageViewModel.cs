using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.Values;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace CraftLogs.ViewModels
{
    public class QRScannerPageViewModel : ViewModelBase
    {
        #region Private

        private string scannedCode ="none";

        #endregion

        #region Public

        public string ScannedCode
        {
            get { return scannedCode; }
            set { SetProperty(ref scannedCode, value); }
        }

        #endregion

        #region ctor

        public QRScannerPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {

        }

        #endregion

        #region Public functions

        public async override void Destroy()
        {
            base.Destroy();
            NavigationParameters param = new NavigationParameters();
            param.Add("res", ScannedCode);
            await NavigateTo(NavigationLinks.MainPage, param);
        }

        #endregion
    }
}
