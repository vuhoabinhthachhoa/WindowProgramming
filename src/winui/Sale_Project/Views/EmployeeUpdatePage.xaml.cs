using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

public sealed partial class EmployeeUpdatePage : Page
{
    public EmployeeUpdateViewModel ViewModel
    {
        get;
    }

    public EmployeeUpdatePage()
    {
        ViewModel = App.GetService<EmployeeUpdateViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }


    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoBack();
    }

    private async void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
         await ViewModel.UpdateEmployee();
    }

    private async void MarkUnemployedButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.MarkEmployeeUnemployed();
    }
}
