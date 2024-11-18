using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Activation;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Services;
using Sale_Project.Helpers;
using Sale_Project.Models;
using Sale_Project.Notifications;
using Sale_Project.Services;
using Sale_Project.Services.Dao;
using Sale_Project.ViewModels;
using Sale_Project.Views;

namespace Sale_Project;

public partial class App : Application
{
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<UIManagerService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IDialogService, DialogService>();

            // HTTP
            services.AddSingleton<HttpClient>();


            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<AccountViewModel>();
            services.AddTransient<AccountPage>();
            services.AddTransient<SaleViewModel>();
            services.AddTransient<SalePage>();
            services.AddTransient<CustomerDetailPage>();
            services.AddTransient<CustomerDetailViewModel>();
            services.AddTransient<ReportViewModel>();
            services.AddTransient<ReportPage>();
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<CustomerPage>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<ProductPage>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<DashboardPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {

        base.OnLaunched(args);
        App.GetService<IAppNotificationService>().Show(
            string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory)
        );
        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}