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

    private void UpdateData()
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\employees.json");

        ViewModel.Source.Clear();
        var json = File.ReadAllText(path);
        Employees = System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(json);

        foreach (var item in Employees)
        {
            ViewModel.Source.Add(item);
        }
    }

    private async void InitializeAsync()
    {
        await LoadEmployeesAsync();
    }

    private async Task LoadEmployeesAsync()
    {
        var employeeDataService = App.GetService<IEmployeeDataService>();
        Employees = (await employeeDataService.LoadDataAsync()).ToList();
        ViewModel.Source.Clear();
        foreach (var employee in Employees)
        {
            ViewModel.Source.Add(employee);
        }
    }

    // Handle text change and present suitable items
    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var employee in Employees)
            {
                var found = splitText.All((key) =>
                {
                    return employee.Name.ToLower().Contains(key) || employee.Id.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add($"{employee.Name} (ID: {employee.Id})");
                }
            }

            if (string.IsNullOrEmpty(sender.Text))

            {
                ViewModel.Source.Clear();
                foreach (var employee in Employees)
                {
                    ViewModel.Source.Add(employee);
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
            // Reset to show all employees if no specific employee is selected
            ViewModel.Source.Clear();
            foreach (var employee in Employees)
            {
                ViewModel.Source.Add(employee);
            }
        }
    }

    private async void ShowAddEmployeeDialog_Click(object sender, RoutedEventArgs e)
    {
        var addEmployeeDialog = new AddEmployeeDialog(); // Declare addEmployeeDialog here

        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Thêm nhân viên",
            PrimaryButtonText = "Lưu",
            CloseButtonText = "Bỏ qua",
            DefaultButton = ContentDialogButton.Primary,
            Content = addEmployeeDialog
        };

        dialog.PrimaryButtonClick += (s, args) => AddEmployeeDialog_PrimaryButtonClick(s, args, addEmployeeDialog); // Pass addEmployeeDialog to the event handler
        var result = await dialog.ShowAsync();
    }

    private void AddEmployeeDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args, AddEmployeeDialog addEmployeeDialog)
    {
        if (!addEmployeeDialog.ValidateInput())
        {
            args.Cancel = true;
            return;
        }
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\employees.json");

        var json = File.ReadAllText(path);
        var list = JsonConvert.DeserializeObject<List<Employee>>(json);
        var employee = addEmployeeDialog.GetEmployee(); // Use addEmployeeDialog here
        list.Add(employee); // Add the employee from the dialog

        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        UpdateData();
    }


    private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
    {
        var item = itemsDataGrid.SelectedItem as Employee;

        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\employees.json");

        var json = File.ReadAllText(path);
        var list = JsonConvert.DeserializeObject<List<Employee>>(json);

        var employee = list.Single(p => p.Id == item.Id);
        list.Remove(employee);

        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        // Update ViewModel Source from the updated JSON file
        UpdateData();
    }
}