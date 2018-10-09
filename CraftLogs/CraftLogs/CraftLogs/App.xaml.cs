using Prism;
using Prism.Ioc;
using CraftLogs.ViewModels;
using CraftLogs.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using CraftLogs.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using CraftLogs.BLL.Repositories.Local.Interfaces;
using CraftLogs.BLL.Repositories.Local;

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
            AppCenter.Start("android=5068d5dd-97bf-4fb3-bfdb-4d80a9a05a7b;", typeof(Analytics), typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<QuestPage, QuestPageViewModel>();

            RegisterServices(containerRegistry);
        }

        protected void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILocalDataRepository, LocalDataRepository>();
            containerRegistry.Register<IDataService, DataService>();
            containerRegistry.Register<IDialogService, DialogService>();
        }
    }
}
