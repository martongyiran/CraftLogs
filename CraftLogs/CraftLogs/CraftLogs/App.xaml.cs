using Prism;
using Prism.Ioc;
using CraftLogs.ViewModels;
using CraftLogs.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Repositories.Local;
using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.BLL.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CraftLogs
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void OnStart()
        {
            base.OnStart();
#if DEV
            AppCenter.Start("android=00145b51-79b0-43ab-be63-0069f7f0c386;", typeof(Analytics), typeof(Crashes));
#elif STG
            AppCenter.Start("android=e0308c93-f3e1-4366-8a1c-95535e7309ad;", typeof(Analytics), typeof(Crashes));
#elif PRD
            AppCenter.Start("android=74a73c15-e833-43a6-b3e8-d95c2b3e6ec2;", typeof(Analytics), typeof(Crashes));
#endif
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<QuestPage, QuestPageViewModel>();
            containerRegistry.RegisterForNavigation<LogsPage, LogsPageViewModel>();
            containerRegistry.RegisterForNavigation<SelectModePage, SelectModePageViewModel>();
            containerRegistry.RegisterForNavigation<QRPage, QRPageViewModel>();
            containerRegistry.RegisterForNavigation<QRScannerPage, QRScannerPageViewModel>();

            RegisterServices(containerRegistry);
        }

        protected void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILocalDataRepository, LocalDataRepository>();
            containerRegistry.Register<IDataService, DataService>();
            containerRegistry.Register<ILoggerService, LoggerService>();
        }
    }
}
