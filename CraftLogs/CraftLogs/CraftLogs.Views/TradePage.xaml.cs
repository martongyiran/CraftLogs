using CraftLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradePage : ContentPage
    {
        public TradePage()
        {
            InitializeComponent();
        }
        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (!(BindingContext is TradePageViewModel viewModel))
                return;

            viewModel.ItemCommand.Execute(e.Item);
        }

        private void BindableToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is TradePageViewModel viewModel))
                return;

            pickers.IsVisible = !pickers.IsVisible;
            shop.IsVisible = !shop.IsVisible;
            cartMenu.IsVisible = !cartMenu.IsVisible;
            cartList.IsVisible = !cartList.IsVisible;
            getList.IsVisible = false;
            getListButton.Text = "Kapom";
        }

        private void TradeList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(BindingContext is TradePageViewModel viewModel))
                return;

            viewModel.RemoveItemCommand.Execute(e.Item);

            if (viewModel.TradeGiveCart.Count == 0)
            {
                pickers.IsVisible = !pickers.IsVisible;
                shop.IsVisible = !shop.IsVisible;
                cartMenu.IsVisible = !cartMenu.IsVisible;
                cartList.IsVisible = !cartList.IsVisible;
            }
        }

        private void EmptyButton_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is TradePageViewModel viewModel))
                return;

            pickers.IsVisible = !pickers.IsVisible;
            shop.IsVisible = !shop.IsVisible;
            cartMenu.IsVisible = !cartMenu.IsVisible;
            cartList.IsVisible = !cartList.IsVisible;

            viewModel.EmptyCommand.Execute();
        }

        private void TradeButton_Clicked(object sender, System.EventArgs e)
        {
            if (!(BindingContext is TradePageViewModel viewModel))
                return;

            viewModel.CheckOutCommand.Execute();
        }

        private void GetListVisibility_Clicked(object sender, System.EventArgs e)
        {
            getList.IsVisible = !getList.IsVisible;
            if (getList.IsVisible)
            {
                getListButton.Text = "Adom";
            }
            else
            {
                getListButton.Text = "Kapom";
            }
        }
    }
}