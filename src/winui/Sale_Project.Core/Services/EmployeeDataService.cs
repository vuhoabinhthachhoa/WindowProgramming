using System.IO;
using System.Reflection;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using System.Text.Json;
using System.ComponentModel;

namespace Sale_Project.Core.Services;
public class EmployeeDataService : IEmployeeDataService, INotifyPropertyChanged
{
    private List<Employee> _allEmployees;

    public EmployeeDataService()
    {
    }

    public IEnumerable<Employee> AllEmployees()
    {
        //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Sale_Project.Core\MockData\Employees.json");
        string path = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        @"..\..\..\..\..\..\MockData\employees.json");

        var result = new List<Employee>();
        var json = System.IO.File.ReadAllText(path);
        result = JsonSerializer.Deserialize<List<Employee>>(json);
        return result;
    }

    public async Task<IEnumerable<Employee>> LoadDataAsync()
    {
        _allEmployees ??= new List<Employee>(AllEmployees());

        await Task.CompletedTask;
        return _allEmployees;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}