using System.IO;
using System.Reflection;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using System.Text.Json;
using System.ComponentModel;

namespace Sale_Project.Core.Services;
public class CustomerDataService : ICustomerDataService, INotifyPropertyChanged
{
    private List<Customer> _allCustomers;

    public CustomerDataService()
    {
    }

    public IEnumerable<Customer> AllCustomers()
    {
        //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Sale_Project.Core\MockData\Customers.json");
        string path = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        @"..\..\..\..\..\..\MockData\customers.json");

        var result = new List<Customer>();
        string json = System.IO.File.ReadAllText(path);
        result = JsonSerializer.Deserialize<List<Customer>>(json);
        return result;
    }

    public async Task<IEnumerable<Customer>> LoadDataAsync()
    {
        _allCustomers ??= new List<Customer>(AllCustomers());

        await Task.CompletedTask;
        return _allCustomers;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}