using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale_Project.Views;

public sealed partial class CustomerPage : Page
{
    public CustomerViewModel ViewModel
    {
        get;
    }

    private List<Customer> Customers = new();

    public CustomerPage()
    {
        ViewModel = App.GetService<CustomerViewModel>();
        InitializeComponent();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadCustomersAsync();
    }

    private async Task LoadCustomersAsync()
    {
        var customerDataService = App.GetService<ICustomerDataService>();
        Customers = (await customerDataService.LoadDataAsync()).ToList();
        ViewModel.Source.Clear();
        foreach (var customer in Customers)
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
            foreach (var customer in Customers)
            {
                var found = splitText.All((key) =>
                {
                    return customer.Name.ToLower().Contains(key) || customer.Id.ToLower().Contains(key) || customer.Phonenumber.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add($"{customer.Name} (ID: {customer.Id}) ({customer.Phonenumber})");
                }
            }

            if (string.IsNullOrEmpty(sender.Text))
            {
                ViewModel.Source.Clear();
                foreach (var customer in Customers)
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
        var selectedCustomer = Customers.FirstOrDefault(p => $"{p.Name} (ID: {p.Id}) ({p.Phonenumber})" == args.SelectedItem.ToString());
        if (selectedCustomer != null)
        {
            ViewModel.Source.Clear();
            ViewModel.Source.Add(selectedCustomer);
        }
        else
        {
            // Reset to show all customers if no specific customer is selected
            ViewModel.Source.Clear();
            foreach (var customer in Customers)
            {
                ViewModel.Source.Add(customer);
            }
        }
    }
}