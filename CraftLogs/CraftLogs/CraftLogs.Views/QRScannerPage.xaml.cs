using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QRScannerPage : BaseScannerPage
    {

        public QRScannerPage ()
		{
            InitializeComponent ();

            this.OnScanResult += QRScannerPage_OnScanResult;
            
        }

        private void QRScannerPage_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread( () =>
            {
                IsScanning = false;
                ScanResultCommand?.Execute(result.Text);
            });
        }
    }
}