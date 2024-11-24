using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employee;

namespace Sale_Project.ViewModels;

public partial class EmployeeViewModel : ObservableRecipient, INavigationAware
{
    //private readonly ISampleDataService _sampleDataService;
    private const int _defaultRowsPerPage = 10;


    public ObservableCollection<Employee> Employees { get; } = new ObservableCollection<Employee>();

    [ObservableProperty]
    private EmployeeSearchRequest employeeSearchRequest;

    public string Info => $"Displaying {Employees.Count}/{RowsPerPage} of total {TotalItems} item(s)";

    public ObservableCollection<PageInfo> PageInfos
    {
        get; set;
    }
    public PageInfo SelectedPageInfoItem
    {
        get; set;
    }
    //public string Keyword { get; set; } = "";
    public int CurrentPage
    {
        get; set;
    } = 1;
    public int TotalPages
    {
        get; set;
    }
    public int TotalItems { get; set; } = 0;
    public int RowsPerPage
    {
        get; set;
    }

    public string SortField
    {
        get; set;
    } = "id";
    public SortType SortType
    {
        get; set;
    } = SortType.ASC;


    public EmployeeViewModel(/*ISampleDataService sampleDataService*/)
    {
        RowsPerPage = _defaultRowsPerPage;
        EmployeeSearchRequest = new EmployeeSearchRequest();
    }

    public async void OnNavigatedTo(object parameter)
    {
        Employees.Clear();

        // TODO: Replace with real data.
        //var data = await _sampleDataService.GetGridDataAsync();

        //foreach (var item in data)
        //{
        //    Employees.Add(item);
        //}
    }

    public void OnNavigatedFrom()
    {
    }
}
