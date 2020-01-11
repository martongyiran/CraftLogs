using CraftLogs.BLL.Models;
using Xamarin.Forms;

namespace CraftLogs.Views
{
    public partial class ItemControl
    {
        public ItemControl()
        {
            InitializeComponent();
        }

        public Item BindedItem
        {
            get { return (Item)GetValue(BindedItemProperty); }
            set { SetValue(BindedItemProperty, value); }
        }

        public static readonly BindableProperty BindedItemProperty =
                BindableProperty.Create(nameof(BindedItem), typeof(Item), typeof(ItemControl));

    }
}
