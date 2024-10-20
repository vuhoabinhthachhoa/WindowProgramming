using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

using Project.Contracts.Services;

namespace Project.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    public ICommand MenuFileExitCommand
    {
        get;
    }

    public ICommand MenuViewsDataGridCommand
    {
        get;
    }

    public ICommand MenuViewsCustomerCommand
    {
        get;
    }

    public ICommand MenuViewsProductPricingCommand
    {
        get;
    }

    public ICommand MenuViewsProductCategoryCommand
    {
        get;
    }

    public ICommand MenuSettingsCommand
    {
        get;
    }

    public ICommand MenuViewsOverviewCommand
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
        //MenuViewsDataGridCommand = new RelayCommand(OnMenuViewsDataGrid);
        MenuViewsCustomerCommand = new RelayCommand(OnMenuViewsCustomer);
        MenuViewsProductPricingCommand = new RelayCommand(OnMenuViewsProductPricing);
        MenuViewsProductCategoryCommand = new RelayCommand(OnMenuViewsProductCategory);
        MenuSettingsCommand = new RelayCommand(OnMenuSettings);
        MenuViewsOverviewCommand = new RelayCommand(OnMenuViewsOverview);
    }

    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private void OnMenuFileExit() => Application.Current.Exit();

    //private void OnMenuViewsDataGrid() => NavigationService.NavigateTo(typeof(DataGridViewModel).FullName!);

    private void OnMenuViewsCustomer() => NavigationService.NavigateTo(typeof(CustomerViewModel).FullName!);

    private void OnMenuViewsProductPricing() => NavigationService.NavigateTo(typeof(ProductPricingViewModel).FullName!);

    private void OnMenuViewsProductCategory() => NavigationService.NavigateTo(typeof(ProductCategoryViewModel).FullName!);

    private void OnMenuSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);

    private void OnMenuViewsOverview() => NavigationService.NavigateTo(typeof(OverviewViewModel).FullName!);
}
