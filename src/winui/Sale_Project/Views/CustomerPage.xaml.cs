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


namespace Sale_Project.Views;
public sealed partial class CustomerPage : Page
{
    public CustomerViewModel ViewModel
    {
        get; set;
    }
    public CustomerPage()
    {
        this.InitializeComponent();
        ViewModel = new CustomerViewModel();
        // DataContext = this;
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var customer in ViewModel.Customers)
            {
                var found = splitText.All((key) =>
                {
                    return customer.Name.ToLower().Contains(key) || customer.ID.ToString().ToLower().Contains(key);
                });
                if (found)
                {
                    //suitableItems.Add($"{customer.Name} (ID: {customer.ID})");
                    suitableItems.Add($"{customer.Name}");
                }
            }


            //if (string.IsNullOrEmpty(sender.Text))
            //{
            //    ViewModel.Customers.Clear();
            //    foreach (var customer in ViewModel.Customers)
            //    {
            //        ViewModel.Customers.Add(customer);
            //    }
            //}

            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }
        ViewModel.Search();

    }

    // Handle user selecting an item, in our case just output the selected item.
    //private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    //{
    //    SuggestionOutput.Text = args.SelectedItem.ToString();
    //var selectedCustomer = ViewModel.Customers.FirstOrDefault(p => $"{p.Name} (ID: {p.ID})" == args.SelectedItem.ToString());
    //if (selectedCustomer != null)
    //{
    //    ViewModel.Customers.Clear();
    //    ViewModel.Customers.Add(selectedCustomer);
    //}
    //else
    //{
    //     Reset to show all customers if no specific customer is selected
    //    ViewModel.Customers.Clear();
    //    foreach (var customer in ViewModel.Customers)
    //    {
    //        ViewModel.Customers.Add(customer);
    //    }
    //}
    //}

    private void addCustomerButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(CustomerAddPage));

    }

    private async void deleteCustomerButton_Click(object sender, RoutedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem == null)
        {
            return;
        }

        var customer = itemsDataGrid.SelectedItem as Customer;
        bool success = ViewModel.Remove(customer);

        if (success)
        {
            await new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Delete",
                Content = "Delete successfully",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
        else
        {
            await new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Delete",
                Content = "Delete failed",
                CloseButtonText = "Cannot delete customer with id: " + customer.ID
            }.ShowAsync();
        }
    }

    private void updateCustomerButton_Click(object sender, RoutedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem == null)
        {
            return;
        }

        var customer = itemsDataGrid.SelectedItem as Customer;

        Frame.Navigate(typeof(CustomerUpdatePage), customer);
    }

    private void previousButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoToPreviousPage();
    }

    private void nextButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.GoToNextPage();
    }

    private void itemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (itemsDataGrid.SelectedItem != null)
        {
            deleteCustomerButton.IsEnabled = true;
            updateCustomerButton.IsEnabled = true;
        }
        else
        {
            deleteCustomerButton.IsEnabled = false;
            updateCustomerButton.IsEnabled = false;
        }
    }

    private void dataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        if (e.Column.Tag != null)
        {
            string sortColumn = e.Column.Tag.ToString();
            bool ascending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;

            switch (sortColumn)
            {
                case "ID":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Customer>(ascending
                        ? from item in ViewModel.Customers orderby item.ID ascending select item
                        : from item in ViewModel.Customers orderby item.ID descending select item);
                    break;
                case "Name":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Customer>(ascending
                        ? from item in ViewModel.Customers orderby item.Name ascending select item
                        : from item in ViewModel.Customers orderby item.Name descending select item);
                    break;
                case "Address":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Customer>(ascending
                        ? from item in ViewModel.Customers orderby item.Address ascending select item
                        : from item in ViewModel.Customers orderby item.Address descending select item);
                    break;
                case "Email":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Customer>(ascending
                        ? from item in ViewModel.Customers orderby item.Email ascending select item
                        : from item in ViewModel.Customers orderby item.Email descending select item);
                    break;
                case "Phonenumber":
                    itemsDataGrid.ItemsSource = new ObservableCollection<Customer>(ascending
                        ? from item in ViewModel.Customers orderby item.Phonenumber ascending select item
                        : from item in ViewModel.Customers orderby item.Phonenumber descending select item);
                    break;
                default:
                    break;
            }

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
}