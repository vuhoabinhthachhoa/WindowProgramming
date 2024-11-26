using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
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
        DataContext = ViewModel;
    }

    private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.AddEmployee();
    }
    private async void SearchEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchEmployee();
    }

    private async void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
         await ViewModel.GoToPreviousPage();
    }

    private async void NextButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToNextPage();
    }

    private async void SortBySalaryAscButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortBySalaryAsc();
    }

    private async void SortBySalaryDescButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortBySalaryDesc();
    }



}
