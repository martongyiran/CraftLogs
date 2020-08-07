using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CraftLogs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollapsiblePanel
    {
        public static readonly BindableProperty IsCollapsedProperty
            = BindableProperty.Create(nameof(IsCollapsed), typeof(bool), typeof(CollapsiblePanel), true, BindingMode.TwoWay);

        public static readonly BindableProperty LegendProperty
            = BindableProperty.Create(nameof(Legend), typeof(string), typeof(CollapsiblePanel), null);

        public CollapsiblePanel()
        {
            InitializeComponent();
        }

        public bool IsCollapsed
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }

        public string Legend
        {
            get => (string)GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }

        private void Toggler_Clicked(object sender, System.EventArgs e)
        {
            IsCollapsed = !IsCollapsed;
        }
    }
}