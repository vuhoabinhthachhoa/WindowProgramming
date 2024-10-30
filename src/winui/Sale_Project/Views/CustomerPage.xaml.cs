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
namespace Sale_Project.Views;
using System.Reflection;
using System.Reflection.Metadata;
using Sale_Project.Core.Services;

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

    private void UpdateData()
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\customers.json");

        ViewModel.Source.Clear();
        var json = File.ReadAllText(path);
        Customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(json);

        foreach (var item in Customers)
        {
            ViewModel.Source.Add(item);
        }
    }

    private async void InitializeAsync()
    {
        await LoadCustomersAsync();
    }

    private async Task LoadCustomersAsync()
    {
        var productDataService = App.GetService<ICustomerDataService>();
        Customers = (await productDataService.LoadDataAsync()).ToList();
        ViewModel.Source.Clear();
        foreach (var product in Customers)
        {
            ViewModel.Source.Add(product);
        }
    }

    // Handle text change and present suitable items
    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var product in Customers)
            {
                var found = splitText.All((key) =>
                {
                    return product.Name.ToLower().Contains(key) || product.Id.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add($"{product.Name} (ID: {product.Id})");
                }
            }

            if (string.IsNullOrEmpty(sender.Text))

            {
                ViewModel.Source.Clear();
                foreach (var product in Customers)
                {
                    ViewModel.Source.Add(product);
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
        var selectedCustomer = Customers.FirstOrDefault(p => $"{p.Name} (ID: {p.Id})" == args.SelectedItem.ToString());
        if (selectedCustomer != null)
        {
            ViewModel.Source.Clear();
            ViewModel.Source.Add(selectedCustomer);
        }
        else
        {
            // Reset to show all products if no specific product is selected
            ViewModel.Source.Clear();
            foreach (var product in Customers)
            {
                ViewModel.Source.Add(product);
            }
        }
    }

    private async void ShowAddCustomerDialog_Click(object sender, RoutedEventArgs e)
    {
        var addCustomerDialog = new AddCustomerDialog(); // Declare addCustomerDialog here

        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Thêm khách hàng",
            PrimaryButtonText = "Lưu",
            CloseButtonText = "Bỏ qua",
            DefaultButton = ContentDialogButton.Primary,
            Content = addCustomerDialog
        };

        dialog.PrimaryButtonClick += (s, args) => AddCustomerDialog_PrimaryButtonClick(s, args, addCustomerDialog); // Pass addCustomerDialog to the event handler
        var result = await dialog.ShowAsync();
    }

    private void AddCustomerDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args, AddCustomerDialog addCustomerDialog)
    {
        if (!addCustomerDialog.ValidateInput())
        {
            args.Cancel = true;
            return;
        }
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\customers.json");

        var json = File.ReadAllText(path);
        var list = JsonConvert.DeserializeObject<List<Customer>>(json);
        var product = addCustomerDialog.GetCustomer(); // Use addCustomerDialog here
        list.Add(product); // Add the product from the dialog

        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        UpdateData();
    }


    private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
    {
        var item = itemsDataGrid.SelectedItem as Customer;

        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\customers.json");

        var json = File.ReadAllText(path);
        var list = JsonConvert.DeserializeObject<List<Customer>>(json);

        var product = list.Single(p => p.Id == item.Id);
        list.Remove(product);

        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        // Update ViewModel Source from the updated JSON file
        UpdateData();
    }
}