using CraftLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
		}
        
        private async void Inventory_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
        }

        private async void Logs_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            viewModel.NavigateToLogsCommand.Execute();
        }

        private async void ReadQR_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            viewModel.NavigateToQRScannerPageCommand.Execute();
        }

        private async void Trade_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
        }

        private async void Settings_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            viewModel.NavigateToSettingsCommand.Execute();
        }

    }
}