using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;
using Sale_Project.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using Sale_Project.Core.Helpers;
using System.Reflection;
using System.Reflection.Metadata;
using Sale_Project.Core.Services;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.WinUI.UI.Controls;
using Sale_Project.Contracts.Services;
using Sale_Project.Services;


namespace Sale_Project.Views;
public sealed partial class EmployeePage : Page
{
    private readonly IDialogService _dialogService;
    public EmployeeViewModel ViewModel
    {
        get; set;
    }

    public EmployeePage(IDialogService dialogService)
    {
        this.InitializeComponent();
        //ViewModel = new EmployeeViewModel();
        ViewModel = App.GetService<EmployeeViewModel>();
        //this.DataContext = ViewModel;
        _dialogService = dialogService;
    }

    private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.AddEmployee();
    }

    private async void UnemployedEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem == null)
        {
            await _dialogService.ShowErrorAsync("Error","Please select an employee to delete");
            return;
        }

        var employee = itemsDataGrid.SelectedItem as Employee;
        var success = ViewModel.UnemployedEmployee(employee);

        if (success)
        {
            await _dialogService.ShowSuccessAsync("Success", "Employee is unemployed successfully");
        }
        else
        {
            await _dialogService.ShowErrorAsync("Error", "Failed to unemployed employee");
        }
    }

    private async void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem == null)
        {
            await _dialogService.ShowErrorAsync("Error", "Please select an employee to update");
            return;
        }

        var employee = itemsDataGrid.SelectedItem as Employee;

        ViewModel.UpdateEmployee(employee);
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoToPreviousPage();
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoToNextPage();
    }

    private void ItemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem != null)
        {
            UnemployedEmployeeButton.IsEnabled = true;
            UpdateEmployeeButton.IsEnabled = true;
        }
        else
        {
            UnemployedEmployeeButton.IsEnabled = false;
            UpdateEmployeeButton.IsEnabled = false;
        }
    }

    private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        if (e.Column.Tag != null)
        {
            string sortColumn = e.Column.Tag.ToString();
            bool ascending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;

            // Use reflection to sort by the column's Tag property dynamically
            var sortedEmployees = ascending
                ? ViewModel.Employees.OrderBy(emp => emp.GetType().GetProperty(sortColumn)?.GetValue(emp, null)).ToList()
                : ViewModel.Employees.OrderByDescending(emp => emp.GetType().GetProperty(sortColumn)?.GetValue(emp, null)).ToList();

            itemsDataGrid.ItemsSource = new ObservableCollection<Employee>(sortedEmployees);

            e.Column.SortDirection = ascending ? DataGridSortDirection.Ascending : DataGridSortDirection.Descending;

            // Remove sorting indicators from other columns
            foreach (var dgColumn in itemsDataGrid.Columns)
            {
                if (dgColumn.Tag != null && dgColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    dgColumn.SortDirection = null;
                }
            }
        }
    }


    //private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    //{
    //    if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
    //    {
    //        var suitableItems = new List<string>();
    //        var splitText = sender.Text.ToLower().Split(" ");
    //        foreach (var employee in ViewModel.Employees)
    //        {
    //            var found = splitText.All((key) =>
    //            {
    //                return employee.Name.ToLower().Contains(key) || employee.ID.ToString().ToLower().Contains(key);
    //            });
    //            if (found)
    //            {
    //                //suitableItems.Add($"{employee.Name} (ID: {employee.ID})");
    //                suitableItems.Add($"{employee.Name}");
    //            }
    //        }


    //        //if (string.IsNullOrEmpty(sender.Text))
    //        //{
    //        //    ViewModel.Employees.Clear();
    //        //    foreach (var employee in ViewModel.Employees)
    //        //    {
    //        //        ViewModel.Employees.Add(employee);
    //        //    }
    //        //}

    //        if (suitableItems.Count == 0)
    //        {
    //            suitableItems.Add("No results found");
    //        }
    //        sender.ItemsSource = suitableItems;
    //    }
    //    ViewModel.Search();

    //}

    // Handle user selecting an item, in our case just output the selected item.
    //private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    //{
    //    SuggestionOutput.Text = args.SelectedItem.ToString();
    //var selectedEmployee = ViewModel.Employees.FirstOrDefault(p => $"{p.Name} (ID: {p.ID})" == args.SelectedItem.ToString());
    //if (selectedEmployee != null)
    //{
    //    ViewModel.Employees.Clear();
    //    ViewModel.Employees.Add(selectedEmployee);
    //}
    //else
    //{
    //     Reset to show all employees if no specific employee is selected
    //    ViewModel.Employees.Clear();
    //    foreach (var employee in ViewModel.Employees)
    //    {
    //        ViewModel.Employees.Add(employee);
    //    }
    //}
    //}

    //private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    //{
    //    if (e.Column.Tag != null)
    //    {
    //        string sortColumn = e.Column.Tag.ToString();
    //        bool ascending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;

    //        switch (sortColumn)
    //        {
    //            case "ID":
    //                itemsDataGrid.ItemsSource = new ObservableCollection<Employee>(ascending
    //                    ? from item in ViewModel.Employees orderby item.Id ascending select item
    //                    : from item in ViewModel.Employees orderby item.Id descending select item);
    //                break;
    //            case "Name":
    //                itemsDataGrid.ItemsSource = new ObservableCollection<Employee>(ascending
    //                    ? from item in ViewModel.Employees orderby item.Name ascending select item
    //                    : from item in ViewModel.Employees orderby item.Name descending select item);
    //                break;
    //            case "Address":
    //                itemsDataGrid.ItemsSource = new ObservableCollection<Employee>(ascending
    //                    ? from item in ViewModel.Employees orderby item.Address ascending select item
    //                    : from item in ViewModel.Employees orderby item.Address descending select item);
    //                break;
    //            case "Email":
    //                itemsDataGrid.ItemsSource = new ObservableCollection<Employee>(ascending
    //                    ? from item in ViewModel.Employees orderby item.Email ascending select item
    //                    : from item in ViewModel.Employees orderby item.Email descending select item);
    //                break;
    //            case "Phonenumber":
    //                itemsDataGrid.ItemsSource = new ObservableCollection<Employee>(ascending
    //                    ? from item in ViewModel.Employees orderby item.PhoneNumber ascending select item
    //                    : from item in ViewModel.Employees orderby item.PhoneNumber descending select item);
    //                break;
    //            default:
    //                break;
    //        }

    //        e.Column.SortDirection = ascending ? DataGridSortDirection.Ascending : DataGridSortDirection.Descending;

    //        // Remove sorting indicators from other columns
    //        foreach (var dgColumn in itemsDataGrid.Columns)
    //        {
    //            if (dgColumn.Tag != null && dgColumn.Tag.ToString() != e.Column.Tag.ToString())
    //            {
    //                dgColumn.SortDirection = null;
    //            }
    //        }
    //    }
    //}
}