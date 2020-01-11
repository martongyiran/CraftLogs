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

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            settingsIcon.IsVisible = !settingsIcon.IsVisible;
            reloadIcon.IsVisible = !reloadIcon.IsVisible;
            buyitIcon.IsVisible = !buyitIcon.IsVisible;
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

            viewModel.BuyCommand.Execute();
            settingsIcon.IsVisible = !settingsIcon.IsVisible;
            reloadIcon.IsVisible = !reloadIcon.IsVisible;
            buyitIcon.IsVisible = !buyitIcon.IsVisible;
            popupView.IsVisible = false;
        }

        private void Handle_Clicked(object sender, System.EventArgs e)
        {
            settingsIcon.IsVisible = !settingsIcon.IsVisible;
            reloadIcon.IsVisible = !reloadIcon.IsVisible;
            buyitIcon.IsVisible = !buyitIcon.IsVisible;
            popupView.IsVisible = false;
        }

        private void BindableToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;
            if(viewModel.ShoppingCart.Count != 0)
            {
                pickers.IsVisible = !pickers.IsVisible;
                shop.IsVisible = !shop.IsVisible;
                cartMenu.IsVisible = !cartMenu.IsVisible;
                cartList.IsVisible = !cartList.IsVisible;
                settingsIcon.IsVisible = !settingsIcon.IsVisible;
                reloadIcon.IsVisible = !reloadIcon.IsVisible;
                buyitIcon.IsVisible = !buyitIcon.IsVisible;
            }
            else
            {
                viewModel.DispalyCartIsEmptyCommand.Execute();
            }
        }

        private void CartList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;

            viewModel.RemoveItemCommand.Execute(e.Item);

            if (viewModel.ShoppingCart.Count == 0)
            {
                pickers.IsVisible = !pickers.IsVisible;
                shop.IsVisible = !shop.IsVisible;
                cartMenu.IsVisible = !cartMenu.IsVisible;
                cartList.IsVisible = !cartList.IsVisible;
                settingsIcon.IsVisible = !settingsIcon.IsVisible;
                reloadIcon.IsVisible = !reloadIcon.IsVisible;
                buyitIcon.IsVisible = !buyitIcon.IsVisible;
            }
        }

        private void EmptyButton_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;
            
            pickers.IsVisible = !pickers.IsVisible;
            shop.IsVisible = !shop.IsVisible;
            cartMenu.IsVisible = !cartMenu.IsVisible;
            cartList.IsVisible = !cartList.IsVisible;
            settingsIcon.IsVisible = !settingsIcon.IsVisible;
            reloadIcon.IsVisible = !reloadIcon.IsVisible;
            buyitIcon.IsVisible = !buyitIcon.IsVisible;

            viewModel.EmptyCommand.Execute();
        }

        private void CheckOutButton_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is ShopPageViewModel viewModel))
                return;

            viewModel.CheckOutCommand.Execute();
        }
    }
}