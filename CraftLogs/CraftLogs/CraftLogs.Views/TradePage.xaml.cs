using CraftLogs.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradePage
    {
        public TradePage()
        {
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, System.EventArgs e)
        {
            popupView.IsVisible = true;
            if (!(BindingContext is TradePageViewModel viewModel))
                return;

            var item = (sender as ItemView).Item;

            viewModel.ItemTappedCommand.Execute(item);
        }

        protected override bool OnBackButtonPressed()
        {
            if (cartView.IsVisible && (BindingContext is TradePageViewModel viewModel))
            {
                viewModel.CloseCartCommand.Execute();
                return true;
            }

            return base.OnBackButtonPressed();
        }

        async void Close_Clicked(object sender, System.EventArgs e)
        {
            await popupView.FadeTo(0.0);
            popupView.IsVisible = false;
            await popupView.FadeTo(1.0, 0);
        }

        private async void TradeDetailsPopup_Tapped(object sender, System.EventArgs e)
        {
            await Task.WhenAll(
                    tradeDetailsPopup.FadeTo(0),
                    tradeDetailsFrame.ScaleTo(0),
                    tradeDetailsFrame.ScaleYTo(0),
                    tradeDetails.ScaleYTo(1)
                    );
            tradeDetailsPopup.InputTransparent = true;
        }

        private async void TradeDetails_Tapped(object sender, System.EventArgs e)
        {
            await Task.WhenAll(
                    tradeDetailsPopup.FadeTo(1),
                    tradeDetailsFrame.ScaleTo(1),
                    tradeDetailsFrame.ScaleYTo(1),
                    tradeDetails.ScaleYTo(0)
                    );
            tradeDetailsPopup.InputTransparent = false;
        }
    }
}