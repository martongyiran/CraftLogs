using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QRScannerPage : BaseScannerPage
    {

        public QRScannerPage ()
		{
            InitializeComponent ();

            this.OnScanResult += (result) =>
            {
                IsScanning = false;
                ScanResultCommand?.Execute(result.Text);
            };
        }
	}
}