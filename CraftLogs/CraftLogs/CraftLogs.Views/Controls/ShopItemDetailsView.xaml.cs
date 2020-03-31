using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopItemDetailsView
    {
        public ShopItemDetailsView()
        {
            InitializeComponent();
        }

        async void Close_Clicked(object sender, System.EventArgs e)
        {
            await detailsView.FadeTo(0.0);
            detailsView.IsVisible = false;
            await detailsView.FadeTo(1.0, 0);
        }
    }
}