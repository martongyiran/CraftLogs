using Android.Text;
using Android.Views;
using CraftLogs.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace CraftLogs.Droid.Renderers
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Gravity = GravityFlags.CenterHorizontal;

                if (e.OldElement == null)
                    Control.InputType = InputTypes.TextFlagNoSuggestions;
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}