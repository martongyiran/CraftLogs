using CraftLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();
            Image1.FadeTo(0.5, 0);
            Image2.FadeTo(0.5, 0);
            Image3.FadeTo(0.5, 0);
        }
        private async void CharacterSelect_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is RegisterPageViewModel viewModel))
                return;

            await Image1.FadeTo(1.0, 100);
            await Image2.FadeTo(0.5, 100);
            await Image3.FadeTo(0.5, 100);
            viewModel.SelectCommand.Execute(1);
        }

        private async void CharacterSelect_Tapped2(object sender, System.EventArgs e)
        {
            if (!(BindingContext is RegisterPageViewModel viewModel))
                return;

            await Image2.FadeTo(1.0, 100);
            await Image1.FadeTo(0.5, 100);
            await Image3.FadeTo(0.5, 100);
            viewModel.SelectCommand.Execute(2);
        }

        private async void CharacterSelect_Tapped3(object sender, System.EventArgs e)
        {
            if (!(BindingContext is RegisterPageViewModel viewModel))
                return;

            await Image3.FadeTo(1.0, 100);
            await Image1.FadeTo(0.5, 100);
            await Image2.FadeTo(0.5, 100);
            viewModel.SelectCommand.Execute(3);
        }
    }
}