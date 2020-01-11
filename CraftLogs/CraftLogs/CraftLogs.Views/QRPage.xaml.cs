using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QRPage
	{
		public QRPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            qrcode.BarcodeOptions.Width = 600;
            qrcode.BarcodeOptions.Height = 600;
		}
	}
}