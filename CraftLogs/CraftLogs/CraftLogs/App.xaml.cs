using Prism;
using Prism.Ioc;
using CraftLogs.ViewModels;
using CraftLogs.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using CraftLogs.Repositories.Local;
using CraftLogs.Services;

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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();

            RegisterServices(containerRegistry);
        }

        protected void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILocalDataRepository, LocalDataRepository>();
            containerRegistry.Register<IDataService, DataService>();
        }
    }
}
