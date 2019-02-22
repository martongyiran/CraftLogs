using CraftLogs.BLL.Repositories.Local.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftLogs.ViewModels
{
    public class QRHandlerViewModel : ViewModelBase
    {
        #region Private

        private string response;

        #endregion

        #region Public

        public string Response
        {
            get { return response; }
            set { SetProperty(ref response, value); }
        }

        #endregion


        #region Ctor

        public QRHandlerViewModel(INavigationService navigationService, ILocalDataRepository dataRepository, IPageDialogService dialogService)
            : base(navigationService, dataRepository, dialogService)
        {
            Title = "QR Handler Page";
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var lul = parameters["res"] as string;
            Response = lul != null ? lul : "none";
        }

        #endregion

    }
}
