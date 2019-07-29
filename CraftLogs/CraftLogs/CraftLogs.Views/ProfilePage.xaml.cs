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
            if (viewModel.NavigateToInventoryPageCommand.CanExecute())
                viewModel.NavigateToInventoryPageCommand.Execute();
        }

        private async void Logs_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            if (viewModel.NavigateToLogsCommand.CanExecute())
                viewModel.NavigateToLogsCommand.Execute();
        }

        private async void ReadQR_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            if (viewModel.NavigateToQRScannerPageCommand.CanExecute())
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
            if (viewModel.NavigateToSettingsCommand.CanExecute())
                viewModel.NavigateToSettingsCommand.Execute();
        }

        private async void Plus_Tapped1(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            viewModel.RaiseStatCommand.Execute(1);
        }

        private async void Plus_Tapped2(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            viewModel.RaiseStatCommand.Execute(2);
        }

        private async void Plus_Tapped3(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ProfilePageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            viewModel.RaiseStatCommand.Execute(3);
        }
    }
}