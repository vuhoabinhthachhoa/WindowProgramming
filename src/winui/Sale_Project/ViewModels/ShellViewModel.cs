using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

using Sale_Project.Contracts.Services;

namespace Sale_Project.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    public ICommand MenuFileExitCommand
    {
        get;
    }

    public ICommand MenuSettingsCommand
    {
        get;
    }

    public ICommand MenuViewsAccountCommand
    {
        get;
    }

    public ICommand MenuViewsSaleCommand
    {
        get;
    }

    public ICommand MenuViewsReportCommand
    {
        get;
    }

    public ICommand MenuViewsCustomerCommand
    {
        get;
    }

    public ICommand MenuViewsProductsCommand
    {
        get;
    }

    public ICommand MenuViewsDashboardCommand
    {
        get;
    }

    public INavigationService NavigationService
    {
        get;
    }

    public ShellViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;

        MenuFileExitCommand = new RelayCommand(OnMenuFileExit);
        MenuSettingsCommand = new RelayCommand(OnMenuSettings);
        MenuViewsAccountCommand = new RelayCommand(OnMenuViewsAccount);
        MenuViewsSaleCommand = new RelayCommand(OnMenuViewsSale);
        MenuViewsReportCommand = new RelayCommand(OnMenuViewsReport);
        MenuViewsCustomerCommand = new RelayCommand(OnMenuViewsCustomer);
        MenuViewsProductsCommand = new RelayCommand(OnMenuViewsProducts);
        MenuViewsDashboardCommand = new RelayCommand(OnMenuViewsDashboard);
    }

    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private void OnMenuFileExit() => Application.Current.Exit();

    private void OnMenuSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);

    private void OnMenuViewsAccount() => NavigationService.NavigateTo(typeof(AccountViewModel).FullName!);

    private void OnMenuViewsSale() => NavigationService.NavigateTo(typeof(SaleViewModel).FullName!);

    private void OnMenuViewsReport() => NavigationService.NavigateTo(typeof(ReportViewModel).FullName!);

    private void OnMenuViewsCustomer() => NavigationService.NavigateTo(typeof(CustomerViewModel).FullName!);

    private void OnMenuViewsProducts() => NavigationService.NavigateTo(typeof(ProductViewModel).FullName!);

    private void OnMenuViewsDashboard() => NavigationService.NavigateTo(typeof(DashboardViewModel).FullName!);

    private void OnMenuViewsMain() => NavigationService.NavigateTo(typeof(DashboardViewModel).FullName!);
}
