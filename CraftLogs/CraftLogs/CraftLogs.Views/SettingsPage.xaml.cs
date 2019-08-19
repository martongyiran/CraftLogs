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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await csiga.TranslateTo(0, 500, 0);
            csiga.IsVisible = true;
            await csiga.TranslateTo(0, 0, 1000, Easing.Linear);
            await csiga.TranslateTo(0, 500, 700, Easing.Linear);
            csiga.IsVisible = false;
        }
    }
}