using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftLogs.ViewModels
{
    public class QRPageViewModel : ViewModelBase
    {
        #region Private

        private string qrCode;

        #endregion

        #region Public

        public string QrCode
        {
            get { return qrCode; }
            set { SetProperty(ref qrCode, value); }
        }

        #endregion

        #region ctor

        public QRPageViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            QrCode = parameters["code"] as string;
        }
        
        #endregion
    }
}
