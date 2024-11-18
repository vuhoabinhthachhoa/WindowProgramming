using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using Sale_Project.Core.Models;
using Sale_Project.Contracts.Services;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Services;
using System.Text.Json;

namespace Sale_Project.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    public ShellViewModel(INavigationService navigationService, IAuthService authService, IDialogService dialogService)
    {
        //_iUserDao = new UserJsonDao();
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


    public async Task LoginAsync(string username, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await _dialogService.ShowErrorAsync(
                    "Login Failed",
                    "Username and password cannot be empty.");
                return;
            }

            var loginSuccessful = await _authService.LoginAsync(username, password);
            if (loginSuccessful)
            {
                
                var uiManager = App.GetService<UIManagerService>();

                if (uiManager.ShellPage != null)
                {
                    uiManager.ShellPage.Startup.Visibility = Visibility.Collapsed;
                    uiManager.ShellPage.Main.Visibility = Visibility.Visible;
                    OnMenuViewsDashboard();
                }
                // System.Diagnostics.Debug.WriteLine($"CurrentUser: {CurrentUser.Username} {CurrentUser.UserRole}");
                await _dialogService.ShowSuccessAsync(
                   "Success",
                   "Login successful!");
            }
        
            else
            {
                await _dialogService.ShowErrorAsync(
                   "Login Failed",
                   "Invalid username or password.");
            }
        }
        catch (Exception ex)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = $"An error occurred: {ex.Message}",
                CloseButtonText = "OK"
            };

            if (errorDialog.XamlRoot == null)
            {
                errorDialog.XamlRoot = App.MainWindow.Content.XamlRoot;
            }

            await errorDialog.ShowAsync();
        }
        
        
    }

    public async Task RegisterAsync(string username, string password, string email, string storeName)
    {


        // var users = await _iUserDao.GetUsersAsync();

        //var isStoreNameNew = !users.Exists(user => user.StoreName == storeName);

        //var newUser = new Sale_Project.Core.Models.User
        //{
        //    Username = username,
        //    Password = password,
        //    Email = email,
        //    StoreName = storeName,
        //    UserRole = isStoreNameNew ? "Admin" : "User"
        //};

        // await _iUserDao.AddUserAsync(newUser);
    }

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

    public ICommand MenuViewsEmployeeCommand
    {
        get;
    }

    public INavigationService NavigationService
    {
        get;
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
