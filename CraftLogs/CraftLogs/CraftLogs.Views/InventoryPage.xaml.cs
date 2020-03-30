using CraftLogs.BLL.Models;
using CraftLogs.ViewModels;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryPage
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, System.EventArgs e)
        {
            popupView.IsVisible = true;
            if (!(BindingContext is InventoryPageViewModel viewModel))
                return;

            var item = (sender as ItemView).Item;

            viewModel.ItemTappedCommand.Execute(item);
        }
    }
}
