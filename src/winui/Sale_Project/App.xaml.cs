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

//using Sale_Project.Services.Dao;
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
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IBrandService, BrandService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IHttpService, HttpService>();

            // Helpers
            services.AddSingleton<EmployeeValidator>();
            services.AddSingleton<EmployeeCreationRequestValidator>();
            services.AddSingleton<ProductValidator>();
            services.AddSingleton<ProductCreationRequestValidator>();

            // HTTP
            services.AddSingleton<HttpClient>();


            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<EmployeeAddViewModel>();
            services.AddTransient<EmployeeAddPage>();
            services.AddTransient<EmployeeUpdateViewModel>();
            services.AddTransient<EmployeeUpdatePage>();
            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<EmployeePage>();

            services.AddTransient<ProductAddViewModel>();
            services.AddTransient<ProductAddPage>();
            services.AddTransient<ProductUpdateViewModel>();
            services.AddTransient<ProductUpdatePage>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<ProductPage>();

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
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<DashboardPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();


            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        // Handle UI thread exceptions
        UnhandledException += App_UnhandledException;

        // Handle task-related exceptions
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        
        // Handle non-UI thread exceptions
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {

        base.OnLaunched(args);
        App.GetService<IAppNotificationService>().Show(
            string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory)
        );
        await App.GetService<IActivationService>().ActivateAsync(args);
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        ShowExceptionMessage(e.Exception);
    }

    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        ShowExceptionMessage(e.Exception);
    }

    private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
    {
        ShowExceptionMessage(e.ExceptionObject as Exception);
    }

    private async void ShowExceptionMessage(Exception ex)
    {
        var dialog = new ContentDialog
        {
            Title = "An error occurred",
            Content = ex.Message,
            CloseButtonText = "OK"
        };

        await dialog.ShowAsync();
    }
}
