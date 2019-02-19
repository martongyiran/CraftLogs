using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QRPage : ContentPage
	{
		public QRPage ()
		{
			InitializeComponent ();
            qrcode.BarcodeOptions.Width = 600;
            qrcode.BarcodeOptions.Height = 600;
		}
	}
}