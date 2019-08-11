using CraftLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShopPage : ContentPage
	{
		public ShopPage ()
		{
			InitializeComponent ();
		}

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            popupView.IsVisible = true;
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;

            viewModel.ItemTappedCommand.Execute(e.Item);
        }

        private void Buy_Clicked(object sender, System.EventArgs e)
        {
            popupView.IsVisible = true;
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;

            viewModel.BuyTappedCommand.Execute();
            popupView.IsVisible = false;
        }

        private void Handle_Clicked(object sender, System.EventArgs e)
        {
            popupView.IsVisible = false;
        }

        private void BindableToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            pickers.IsVisible = !pickers.IsVisible;
            shop.IsVisible = !shop.IsVisible;
            cartMenu.IsVisible = !cartMenu.IsVisible;
            cartList.IsVisible = !cartList.IsVisible;
        }

        private void CartList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;

            viewModel.RemoveItemTappedCommand.Execute(e.Item);
        }

        private void EmptyButton_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;
            
            pickers.IsVisible = !pickers.IsVisible;
            shop.IsVisible = !shop.IsVisible;
            cartMenu.IsVisible = !cartMenu.IsVisible;
            cartList.IsVisible = !cartList.IsVisible;

            viewModel.EmptyTappedCommand.Execute();
        }

        private void CheckOutButton_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;

            viewModel.CheckOutTappedCommand.Execute();
        }
    }
}