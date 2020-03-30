using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileStatsView
    {
        public ProfileStatsView()
        {
            InitializeComponent();
        }

        private async void Stam_Tapped(object sender, System.EventArgs e)
        {
            await stamPlus.ScaleTo(1.2, 125);
            await stamPlus.ScaleTo(1.0, 125);
        }

        private async void Atk_Tapped(object sender, System.EventArgs e)
        {
            await atkPlus.ScaleTo(1.2, 125);
            await atkPlus.ScaleTo(1.0, 125);
        }

        private async void Def_Tapped(object sender, System.EventArgs e)
        {
            await defPlus.ScaleTo(1.2, 125);
            await defPlus.ScaleTo(1.0, 125);
        }
    }
}