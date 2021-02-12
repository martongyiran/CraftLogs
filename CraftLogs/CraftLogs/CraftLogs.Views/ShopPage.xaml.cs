using CraftLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShopPage
	{
		public ShopPage ()
		{
			InitializeComponent ();
		}

        void Handle_ItemTapped(object sender, System.EventArgs e)
        {
            popupView.IsVisible = true;
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;

            var item = (sender as ItemView).Item;

            viewModel.ItemTappedCommand.Execute(item);
        }

        protected override bool OnBackButtonPressed()
        {
            if (cartView.IsVisible && (BindingContext is ShopPageViewModel viewModel))
            {
                viewModel.CloseCartCommand.Execute();
            }
            return true;
        }
    }
}