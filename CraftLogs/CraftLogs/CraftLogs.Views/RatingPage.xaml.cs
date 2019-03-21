using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RatingPage : ContentPage
	{
		public RatingPage ()
		{
			InitializeComponent ();
            
            scorePicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                scorePicker.Unfocus();
            };
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            scorePicker.Focus();
        }
    }
}