using CraftLogs.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRHandlerPage
    {
        public QRHandlerPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var viewModel = BindingContext as QRHandlerViewModel;

            viewModel.ActiveItem_changed += On_ActiveItemChanged;
        }

        private async void On_ActiveItemChanged(object e, EventArgs a)
        {
            await Task.WhenAll(
                detailsView.FadeTo(1),
                detailsFrame.ScaleYTo(1)
                );

            detailsView.InputTransparent = false;
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await CloseDetails();
        }

        private async Task CloseDetails()
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


            var viewModel = BindingContext as QRHandlerViewModel;

            var index = viewModel.Rewards.IndexOf(viewModel.ActiveItem);
            var last = viewModel.Rewards.Count - 1;

            if (index + 1 > last)
            {
                collectionView.SelectedItem = viewModel.Rewards[0];
            }
            else
            {
                collectionView.SelectedItem = viewModel.Rewards[index + 1];
            }
        }

        async void Prev_Clicked(object sender, EventArgs e)
        {
            await detailsFrame.TranslateTo(500, 0, 125);
            await detailsFrame.TranslateTo(-500, 0, 0);
            await detailsFrame.TranslateTo(0, 0, 125);

            var viewModel = BindingContext as QRHandlerViewModel;

            var index = viewModel.Rewards.IndexOf(viewModel.ActiveItem);

            if (index - 1 >= 0)
            {
                collectionView.SelectedItem = viewModel.Rewards[index - 1];
            }
            else
            {
                var last = viewModel.Rewards.Count - 1;
                collectionView.SelectedItem = viewModel.Rewards[last];
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if(detailsView.Opacity == 1)
            {
                CloseDetails();
            }
            return true;
        }
    }
}