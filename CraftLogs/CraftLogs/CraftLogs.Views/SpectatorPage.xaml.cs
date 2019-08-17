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
	public partial class SpectatorPage : ContentPage
	{
		public SpectatorPage ()
		{
			InitializeComponent ();
		}
        private void On_Support_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://paypal.me/CHlGGA"));
        }
    }
}