using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QRScannerPage : ZXingScannerPage
    {
        MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions
        {
            AutoRotate = false,
            UseFrontCameraIfAvailable = true,
            TryHarder = true,
            PossibleFormats = new List<ZXing.BarcodeFormat>
        {
           ZXing.BarcodeFormat.QR_CODE
        }
        };
        public QRScannerPage (MobileBarcodeScanningOptions options)
		{
            InitializeComponent ();
            DefaultOverlayTopText = "Align the barcode within the frame";
            DefaultOverlayShowFlashButton = true;

            this.OnScanResult += (result) =>
            {
                // Stop scanning
                this.IsScanning = false;
                
                DefaultOverlayBottomText = result.Text;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                });
                
            };
        }

	}
}