using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePictureSelectorView
    {

        public static readonly BindableProperty SelectedImageProperty
            = BindableProperty.Create(
                nameof(SelectedImage),
                typeof(string),
                typeof(ProfilePictureSelectorView),
                defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty Image1Property
            = BindableProperty.Create(
                nameof(Image1),
                typeof(string),
                typeof(ProfilePictureSelectorView),
                defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty Image2Property
            = BindableProperty.Create(
                nameof(Image2),
                typeof(string),
                typeof(ProfilePictureSelectorView),
                defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty Image3Property
            = BindableProperty.Create(
                nameof(Image3),
                typeof(string),
                typeof(ProfilePictureSelectorView),
                defaultBindingMode: BindingMode.TwoWay);

        public string SelectedImage
        {
            get => (string)GetValue(SelectedImageProperty);
            set => SetValue(SelectedImageProperty, value);
        }

        public string Image1
        {
            get => (string)GetValue(Image1Property);
            set => SetValue(Image1Property, value);
        }

        public string Image2
        {
            get => (string)GetValue(Image2Property);
            set => SetValue(Image2Property, value);
        }

        public string Image3
        {
            get => (string)GetValue(Image3Property);
            set => SetValue(Image3Property, value);
        }

        public ProfilePictureSelectorView()
        {
            InitializeComponent();
        }

        private void Slot1_Tapped(object sender, EventArgs e)
        {
            SelectedImage = Image1;
            Slot1.BackgroundColor = Color.FromHex("#8e8e8e");
            Slot2.BackgroundColor = Color.Transparent;
            Slot3.BackgroundColor = Color.Transparent;
        }

        private void Slot2_Tapped(object sender, EventArgs e)
        {
            SelectedImage = Image2;
            Slot2.BackgroundColor = Color.FromHex("#8e8e8e");
            Slot1.BackgroundColor = Color.Transparent;
            Slot3.BackgroundColor = Color.Transparent;
        }

        private void Slot3_Tapped(object sender, EventArgs e)
        {
            SelectedImage = Image3;
            Slot3.BackgroundColor = Color.FromHex("#8e8e8e");
            Slot1.BackgroundColor = Color.Transparent;
            Slot2.BackgroundColor = Color.Transparent;
        }
    }
}