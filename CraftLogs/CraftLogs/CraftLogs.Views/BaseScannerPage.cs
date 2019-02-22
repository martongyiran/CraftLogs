using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace CraftLogs.Views
{
    public partial class BaseScannerPage : ZXingScannerPage
    {


        #region ScanResultCommand

        public static readonly BindableProperty ScanResultCommandProperty =
            BindableProperty.Create(nameof(ScanResultCommand), typeof(ICommand), typeof(BaseScannerPage), null);

        public ICommand ScanResultCommand
        {
            get => (ICommand)GetValue(ScanResultCommandProperty);
            set => SetValue(ScanResultCommandProperty, value);
        }

        #endregion

        static readonly MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions
        {
            AutoRotate = false,
            UseFrontCameraIfAvailable = false,
            TryHarder = true,
            PossibleFormats = new List<ZXing.BarcodeFormat>
        {
           ZXing.BarcodeFormat.QR_CODE
        }
        };

        public BaseScannerPage()
            :base (options)
        {
        }
    }
}
