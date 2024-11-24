using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Sale_Project.ViewModels;

namespace Sale_Project.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class EmployeePage : Page
{
    public EmployeeViewModel ViewModel
    {
        get;
    }

    public EmployeePage()
    {
        ViewModel = App.GetService<EmployeeViewModel>();
        InitializeComponent();
    }

    private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        //ViewModel.AddEmployee();
    }

    private async void UnemployedEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        //if (itemsDataGrid.SelectedItem == null)
        //{
        //    await _dialogService.ShowErrorAsync("Error", "Please select an employee to delete");
        //    return;
        //}

        //var employee = itemsDataGrid.SelectedItem as Employee;
        //var success = ViewModel.UnemployedEmployee(employee);

        //if (success)
        //{
        //    await _dialogService.ShowSuccessAsync("Success", "Employee is unemployed successfully");
        //}
        //else
        //{
        //    await _dialogService.ShowErrorAsync("Error", "Failed to unemployed employee");
        //}
    }

    private async void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        //if (itemsDataGrid.SelectedItem == null)
        //{
        //    await _dialogService.ShowErrorAsync("Error", "Please select an employee to update");
        //    return;
        //}

        //var employee = itemsDataGrid.SelectedItem as Employee;

        //ViewModel.UpdateEmployee(employee);
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        //ViewModel.GoToPreviousPage();
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        //ViewModel.GoToNextPage();
    }
}
