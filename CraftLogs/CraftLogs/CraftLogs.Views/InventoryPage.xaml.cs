using CraftLogs.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
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

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var viewModel = BindingContext as InventoryPageViewModel;

            viewModel.ActiveItem_changed += On_ActiveItemChanged;
            viewModel.Close_Details += On_Close;
        }

        private async void On_ActiveItemChanged(object e, EventArgs a)
        {
            await Task.WhenAll(
                detailsView.FadeTo(1),
                detailsFrame.ScaleYTo(1)
                );

            detailsView.InputTransparent = false;
        }

        private void On_Close(object e, EventArgs a)
        {
            Close_Clicked(e, a);
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Task.WhenAll(
                detailsView.FadeTo(0),
                detailsFrame.ScaleYTo(0)
                );

            collectionView.SelectedItem = null;
            detailsView.InputTransparent = true;
        }

        private void SwipeGestureRecognizer_Swiped_Left(object sender, SwipedEventArgs e)
        {
            Next_Clicked(sender, e);
        }

        private void SwipeGestureRecognizer_Swiped_Right(object sender, SwipedEventArgs e)
        {
            Prev_Clicked(sender, e);
        }

        async void Next_Clicked(object sender, EventArgs e)
        {
            await detailsFrame.TranslateTo(-500, 0, 125);
            await detailsFrame.TranslateTo(500, 0, 0);
            await detailsFrame.TranslateTo(0, 0, 125);


            var viewModel = BindingContext as InventoryPageViewModel;

            var index = viewModel.Items.IndexOf(viewModel.ActiveItem);
            var last = viewModel.Items.Count - 1;

            if (index + 1 > last)
            {
                collectionView.SelectedItem = viewModel.Items[0];
            }
            else
            {
                collectionView.SelectedItem = viewModel.Items[index + 1];
            }
        }

        async void Prev_Clicked(object sender, EventArgs e)
        {
            await detailsFrame.TranslateTo(500, 0, 125);
            await detailsFrame.TranslateTo(-500, 0, 0);
            await detailsFrame.TranslateTo(0, 0, 125);

            var viewModel = BindingContext as InventoryPageViewModel;

            var index = viewModel.Items.IndexOf(viewModel.ActiveItem);

            if (index - 1 >= 0)
            {
                collectionView.SelectedItem = viewModel.Items[index - 1];
            }
            else
            {
                var last = viewModel.Items.Count - 1;
                collectionView.SelectedItem = viewModel.Items[last];
            }
        }
    }
}
