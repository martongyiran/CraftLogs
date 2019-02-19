using Android.App;
using Android.OS;

namespace CraftLogs.Droid
{
#if DEV
    [Activity(Label = "cl dev", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
#elif STG
    [Activity(Label = "cl stg", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
#elif PRD
    [Activity(Label = "CraftLogs", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
#endif
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
        }
    }
}