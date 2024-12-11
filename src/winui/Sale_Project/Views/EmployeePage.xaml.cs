using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;

namespace Sale_Project.Views;

/// <summary>
/// Represents the Employee page which contains the UI and logic for managing employees.
/// </summary>
public sealed partial class EmployeePage : Page
{
    /// <summary>
    /// Gets the ViewModel for the Employee page.
    /// </summary>
    public EmployeeViewModel ViewModel
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeePage"/> class.
    /// </summary>
    public EmployeePage()
    {
        ViewModel = App.GetService<EmployeeViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    /// <summary>
    /// Handles the Click event of the AddEmployeeButton. Adds a new employee.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.AddEmployee();
    }

    /// <summary>
    /// Handles the Click event of the SearchEmployeeButton. Searches for an employee.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void SearchEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SearchEmployee();
    }

    /// <summary>
    /// Handles the Click event of the PreviousButton. Navigates to the previous page.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToPreviousPage();
    }

    /// <summary>
    /// Handles the Click event of the NextButton. Navigates to the next page.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void NextButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.GoToNextPage();
    }

    /// <summary>
    /// Handles the Click event of the SortBySalaryAscButton. Sorts employees by salary in ascending order.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void SortBySalaryAscButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortBySalaryAsc();
    }

    /// <summary>
    /// Handles the Click event of the SortBySalaryDescButton. Sorts employees by salary in descending order.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void SortBySalaryDescButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SortBySalaryDesc();
    }
}

