using CraftLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestPage : ContentPage
	{
		public QuestPage ()
		{
			InitializeComponent ();
        }

        private async void Settings_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is QuestPageViewModel viewModel))
                return;

            var view = sender as Image;
            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            if (viewModel.NavigateToSettingsCommand.CanExecute())
                viewModel.NavigateToSettingsCommand.Execute();
        }

        private async void Start_Tapped(object sender, System.EventArgs e)
        {
            if (!(BindingContext is QuestPageViewModel viewModel))
                return;

            var view = sender as Image;

            await view.FadeTo(0.5, 100);
            await view.FadeTo(1.0, 100);
            await scoreLabel.FadeTo(0.0, 0);
            await scoreSlider.FadeTo(0.0, 0);
            await scoreButton.FadeTo(0.0, 0);

            viewModel.StartCommand.Execute();

            await scoreLabel.FadeTo(1.0, 200);
            await scoreSlider.FadeTo(1.0, 200);
            await scoreButton.FadeTo(1.0, 200);
        }
    }
}