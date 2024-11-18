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

    private readonly IUserDao _iUserDao;
    private User _currentUser = new();

    public User CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    public async Task LoginAsync(string username, string password)
    {
        try
        {
            var users = await _iUserDao.GetUsersAsync();

            var user = users?.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                CurrentUser = user;
                var uiManager = App.GetService<UIManagerService>();

                if (uiManager.ShellPage != null)
                {
                    uiManager.ShellPage.Startup.Visibility = Visibility.Collapsed;
                    uiManager.ShellPage.Main.Visibility = Visibility.Visible;
                }

                System.Diagnostics.Debug.WriteLine($"CurrentUser: {CurrentUser.Username} {CurrentUser.UserRole}");
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Login Failed",
                    Content = "Invalid username or password.",
                    CloseButtonText = "Ok"
                };

                if (dialog.XamlRoot == null)
                {
                    dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
                }

                await dialog.ShowAsync();
            }
        }
        catch (FileNotFoundException)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = "User data file not found.",
                CloseButtonText = "OK"
            };

            if (errorDialog.XamlRoot == null)
            {
                errorDialog.XamlRoot = App.MainWindow.Content.XamlRoot;
            }

            await errorDialog.ShowAsync();
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


        var users = await _iUserDao.GetUsersAsync();

        var isStoreNameNew = !users.Exists(user => user.StoreName == storeName);

        var newUser = new Sale_Project.Core.Models.User
        {
            Username = username,
            Password = password,
            Email = email,
            StoreName = storeName,
            UserRole = isStoreNameNew ? "Admin" : "User"
        };

        await _iUserDao.AddUserAsync(newUser);
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


    public ShellViewModel(INavigationService navigationService)
    {
        // _iUserDao = new UserJsonDao();
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

    private void OnMenuViewsEmployee() => NavigationService.NavigateTo(typeof(EmployeeViewModel).FullName!);

}
