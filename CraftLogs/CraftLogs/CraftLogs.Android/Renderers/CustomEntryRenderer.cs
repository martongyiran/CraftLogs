using Android.Content.Res;
using Android.Graphics.Drawables;
using CraftLogs.Droid.Renderers;
using CraftLogs.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace CraftLogs.Droid.Renderers
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
#pragma warning disable CS0618 // Type or member is obsolete
                this.Control.SetBackgroundDrawable(gd);
#pragma warning restore CS0618 // Type or member is obsolete
                Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}