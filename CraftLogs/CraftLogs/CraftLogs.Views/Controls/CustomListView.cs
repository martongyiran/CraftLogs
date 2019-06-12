using System.Windows.Input;
using Xamarin.Forms;

namespace CraftLogs.Views
{
    public class CustomListView : ListView
    {

        public CustomListView()
        {
            this.ItemTapped += this.OnItemTapped;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
                BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomListView));

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && this.Command != null && this.Command.CanExecute(e))
            {
                this.Command.Execute(e.Item);
                this.SelectedItem = null;
            }
        }
    }

}
