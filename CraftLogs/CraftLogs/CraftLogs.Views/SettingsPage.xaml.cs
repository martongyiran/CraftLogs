using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            dayPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                dayPicker.Unfocus();
            };

            c1startPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                c1startPicker.Unfocus();
            };

            c2startPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                c2startPicker.Unfocus();
            };

            c1pointPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                c1pointPicker.Unfocus();
            };

            c2pointPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                c2pointPicker.Unfocus();
            };

        }
        
    }
}