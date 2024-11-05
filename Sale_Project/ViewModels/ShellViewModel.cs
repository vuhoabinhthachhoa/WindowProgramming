using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using Sale_Project.Core.Models;
using Sale_Project.Contracts.Services;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Services;

namespace Sale_Project.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    private readonly IDao _dataAccess;
    private User _currentUser = new();
    private readonly IDao _dao;

    public User CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    public async Task LoginAsync(string username, string password)
    {
        try
        {
            var users = await _dataAccess.GetUsersAsync();

            var hashedPassword = HashPassword(password);
            var user = users?.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

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
                await ShowLoginErrorDialog();
            }
        }
        catch (FileNotFoundException)
        {
            await ShowErrorDialog("User data file not found.");
        }
        catch (Exception ex)
        {
            await ShowErrorDialog($"An error occurred: {ex.Message}");
        }
    }

    public async Task RegisterUserAsync(string username, string password, string email, string storeName)
    {
        var users = await _dao.GetUsersAsync();
        var isStoreNameNew = !users.Exists(user => user.StoreName == storeName);

        // Băm mật khẩu trước khi lưu
        var hashedPassword = HashPassword(password);

        var newUser = new User
        {
            Username = username,
            Password = hashedPassword, // Lưu mật khẩu đã băm
            Email = email,
            StoreName = storeName,
            UserRole = isStoreNameNew ? "Admin" : "User"
        };

        await _dao.RegisterUserAsync(newUser);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash); 
    }

    // Phương thức để hiển thị thông báo lỗi đăng nhập
    private async Task ShowLoginErrorDialog()
    {
        ContentDialog dialog = new ContentDialog
        {
            Title = "Login Failed",
            Content = "Invalid username or password.",
            CloseButtonText = "OK"
        };

        if (dialog.XamlRoot == null)
        {
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
        }

        await dialog.ShowAsync();
    }

    // Phương thức để hiển thị thông báo lỗi chung
    private async Task ShowErrorDialog(string message)
    {
        ContentDialog errorDialog = new ContentDialog
        {
            Title = "Error",
            Content = message,
            CloseButtonText = "OK"
        };

        if (errorDialog.XamlRoot == null)
        {
            errorDialog.XamlRoot = App.MainWindow.Content.XamlRoot;
        }

        await errorDialog.ShowAsync();
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

    public INavigationService NavigationService
    {
        get;
    }

    public ShellViewModel(INavigationService navigationService, IDao dao)
    {
        _dataAccess = new UserJsonDao();
        _dao = dao;
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
