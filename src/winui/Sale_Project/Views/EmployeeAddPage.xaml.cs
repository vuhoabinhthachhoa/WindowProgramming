using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

public sealed partial class EmployeeAddPage : Page
{
    public EmployeeAddViewModel ViewModel
    {
        get;
    }

    public EmployeeAddPage()
    {
        ViewModel = App.GetService<EmployeeAddViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoBack();
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.AddEmployee();
    }

    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.Register();
    }
}
