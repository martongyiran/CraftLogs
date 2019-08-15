using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void On_Support_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://paypal.me/CHlGGA"));
        }

    }
}