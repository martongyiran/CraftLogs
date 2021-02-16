using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await csiga.TranslateTo(0, 500, 0);
            csiga.IsVisible = true;
            await csiga.TranslateTo(0, 0, 250, Easing.Linear);
            await Task.Delay(250);
            await csiga.TranslateTo(0, 500, 250, Easing.Linear);
            csiga.IsVisible = false;
        }
    }
}