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