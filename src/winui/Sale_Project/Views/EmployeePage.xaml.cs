using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale_Project.Views;

public sealed partial class EmployeePage : Page
{
    public EmployeeViewModel ViewModel
    {
        get;
    }

    private List<Employee> Employees = new();

    public EmployeePage()
    {
        ViewModel = App.GetService<EmployeeViewModel>();
        InitializeComponent();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadEmployeesAsync();
    }

    private async Task LoadEmployeesAsync()
    {
        var customerDataService = App.GetService<IEmployeeDataService>();
        Employees = (await customerDataService.LoadDataAsync()).ToList();
        ViewModel.Source.Clear();
        foreach (var customer in Employees)
        {
            ViewModel.Source.Add(customer);
        }
    }

    // Handle text change and present suitable items
    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var customer in Employees)
            {
                var found = splitText.All((key) =>
                {
                    return customer.Name.ToLower().Contains(key) || customer.Id.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add($"{customer.Name} (ID: {customer.Id})");
                }
            }

            if (string.IsNullOrEmpty(sender.Text))
            {
                ViewModel.Source.Clear();
                foreach (var customer in Employees)
                {
                    ViewModel.Source.Add(customer);
                }
            }

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }

    }

    // Handle user selecting an item, in our case just output the selected item.
    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        SuggestionOutput.Text = args.SelectedItem.ToString();
        var selectedEmployee = Employees.FirstOrDefault(p => $"{p.Name} (ID: {p.Id})" == args.SelectedItem.ToString());
        if (selectedEmployee != null)
        {
            ViewModel.Source.Clear();
            ViewModel.Source.Add(selectedEmployee);
        }
        else
        {
            // Reset to show all customers if no specific customer is selected
            ViewModel.Source.Clear();
            foreach (var customer in Employees)
            {
                ViewModel.Source.Add(customer);
            }
        }
    }
}