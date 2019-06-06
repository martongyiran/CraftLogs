using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeamRegisterView : ContentView
    {
        public TeamRegisterView()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            housesList.IsVisible = !housesList.IsVisible;
        }
    }
}
