using System.Text.Json;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Services;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for the Shell view, responsible for handling navigation and menu commands.
/// </summary>
public partial class ShellViewModel : ObservableRecipient
{
    /// <summary>
    /// Indicates whether the back navigation is enabled.
    /// </summary>
    [ObservableProperty]
    private bool isBackEnabled;

    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="authService">The authentication service.</param>
    /// <param name="dialogService">The dialog service.</param>
    public ShellViewModel(INavigationService navigationService, IAuthService authService, IDialogService dialogService)
    {
        _authService = authService;
        _dialogService = dialogService;
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
        MenuViewsEmployeeCommand = new RelayCommand(OnMenuViewsEmployee);
    }

    /// <summary>
    /// Gets the command to exit the application.
    /// </summary>
    public ICommand MenuFileExitCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the settings view.
    /// </summary>
    public ICommand MenuSettingsCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the account view.
    /// </summary>
    public ICommand MenuViewsAccountCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the sale view.
    /// </summary>
    public ICommand MenuViewsSaleCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the report view.
    /// </summary>
    public ICommand MenuViewsReportCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the customer view.
    /// </summary>
    public ICommand MenuViewsCustomerCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the products view.
    /// </summary>
    public ICommand MenuViewsProductsCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the dashboard view.
    /// </summary>
    public ICommand MenuViewsDashboardCommand
    {
        get;
    }

    /// <summary>
    /// Gets the command to navigate to the employee view.
    /// </summary>
    public ICommand MenuViewsEmployeeCommand
    {
        get;
    }

    /// <summary>
    /// Gets the navigation service.
    /// </summary>
    public INavigationService NavigationService
    {
        get;
    }

    /// <summary>
    /// Handles the navigation event to update the back navigation state.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The navigation event arguments.</param>
    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    /// <summary>
    /// Exits the application.
    /// </summary>
    private void OnMenuFileExit() => Application.Current.Exit();

    /// <summary>
    /// Navigates to the settings view.
    /// </summary>
    private void OnMenuSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);

    /// <summary>
    /// Navigates to the account view.
    /// </summary>
    private void OnMenuViewsAccount() => NavigationService.NavigateTo(typeof(AccountViewModel).FullName!);

    /// <summary>
    /// Navigates to the sale view.
    /// </summary>
    private void OnMenuViewsSale() => NavigationService.NavigateTo(typeof(SaleViewModel).FullName!);

    /// <summary>
    /// Navigates to the report view if the user has admin privileges.
    /// </summary>
    private void OnMenuViewsReport()
    {
        UserRole userRole = _authService.GetUserRole();
        if (userRole != UserRole.ADMIN)
        {
            _dialogService.ShowErrorAsync("Access Denied", "You do not have permission to access this page.");
            return;
        }
        NavigationService.NavigateTo(typeof(ReportViewModel).FullName!);
    }

    /// <summary>
    /// Navigates to the customer view if the user has admin privileges.
    /// </summary>
    private void OnMenuViewsCustomer()
    {
        UserRole userRole = _authService.GetUserRole();
        if (userRole != UserRole.ADMIN)
        {
            _dialogService.ShowErrorAsync("Access Denied", "You do not have permission to access this page.");
            return;
        }
        NavigationService.NavigateTo(typeof(CustomerViewModel).FullName!);
    }

    /// <summary>
    /// Navigates to the products view if the user has admin privileges.
    /// </summary>
    private void OnMenuViewsProducts()
    {
        UserRole userRole = _authService.GetUserRole();
        if (userRole != UserRole.ADMIN)
        {
            _dialogService.ShowErrorAsync("Access Denied", "You do not have permission to access this page.");
            return;
        }
        NavigationService.NavigateTo(typeof(ProductViewModel).FullName!);
    }

    /// <summary>
    /// Navigates to the dashboard view.
    /// </summary>
    private void OnMenuViewsDashboard() => NavigationService.NavigateTo(typeof(DashboardViewModel).FullName!);

    /// <summary>
    /// Navigates to the main view.
    /// </summary>
    private void OnMenuViewsMain() => NavigationService.NavigateTo(typeof(DashboardViewModel).FullName!);

    /// <summary>
    /// Navigates to the employee view if the user has admin privileges.
    /// </summary>
    private void OnMenuViewsEmployee()
    {
        UserRole userRole = _authService.GetUserRole();
        if (userRole != UserRole.ADMIN)
        {
            _dialogService.ShowErrorAsync("Access Denied", "You do not have permission to access this page.");
            return;
        }
        NavigationService.NavigateTo(typeof(EmployeeViewModel).FullName!);
    }
}
