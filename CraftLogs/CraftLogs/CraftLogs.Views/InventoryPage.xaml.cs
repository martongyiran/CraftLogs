using CraftLogs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryPage : ContentPage
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            popupView.IsVisible = true;
            if (!(BindingContext is InventoryPageViewModel viewModel))
                return;

            viewModel.ItemTappedCommand.Execute(e.Item);
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            popupView.IsVisible = false;
        }
    }
}
