using CraftLogs.BLL.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemView
    {
        public static readonly BindableProperty ItemProperty
            = BindableProperty.Create(
                nameof(Item),
                typeof(Item),
                typeof(ItemView));

        public static readonly BindableProperty IsInventoryViewProperty
            = BindableProperty.Create(
                nameof(IsInventoryView),
                typeof(bool),
                typeof(ItemView),
                defaultValue: false);

        public Item Item
        {
            get => (Item)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public bool IsInventoryView
        {
            get => (bool)GetValue(IsInventoryViewProperty);
            set => SetValue(IsInventoryViewProperty, value);
        }

        public ItemView()
        {
            InitializeComponent();
        }
    }
}