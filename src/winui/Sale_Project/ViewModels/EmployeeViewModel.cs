using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employee;
using Sale_Project.Contracts.Services;


namespace Sale_Project.ViewModels;

public partial class EmployeeViewModel : ObservableRecipient, INavigationAware
{
    //private readonly ISampleDataService _sampleDataService;
    private const int _defaultRowsPerPage = 5;
    private readonly IEmployeeService _employeeService;
    private readonly INavigationService _navigationService;


    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

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


    public EmployeeViewModel(INavigationService navigationService, IEmployeeService employeeService)
    {
    
        RowsPerPage = _defaultRowsPerPage;
        EmployeeSearchRequest = new EmployeeSearchRequest();
        _employeeService = employeeService;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        await LoadData();
        // TODO: Replace with real data.
        //var data = await _sampleDataService.GetGridDataAsync();

        //foreach (var item in data)
        //{
        //    Employees.Add(item);
        //}
    }

    public void OnNavigatedFrom()
    {
        Employees.Clear();
    }

    public async Task LoadData()
    {
        // Fetch data asynchronously
        var pageData = await _employeeService.SearchEmployees(CurrentPage, RowsPerPage, SortField, SortType, EmployeeSearchRequest);


        // Convert the result to ObservableCollection
        Employees = new ObservableCollection<Employee>(pageData.Data);
        TotalItems = pageData.TotalElements;
        TotalPages = pageData.TotalPages;
        CurrentPage = pageData.Page;
    }

    public async Task GoToPage(int page)
    {
        CurrentPage = page;
        await LoadData();
    }

    public async Task SearchEmployee()
    {
        CurrentPage = 1;
        await LoadData();
    }

    public async Task GoToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadData();
        }
    }

    public async Task GoToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadData();
        }
    }

    public async Task SortBySalaryAsc()
    {
        if(SortField == "salary" && SortType == SortType.ASC)
        {
            SetDefaultValue();
        }
        else
        {
            SortField = "salary";
            SortType = SortType.ASC;
            CurrentPage = 1;
        }
        await LoadData();
    }

    public async Task SortBySalaryDesc()
    {
        if (SortField == "salary" && SortType == SortType.DESC)
        {
            SetDefaultValue();
        }
        else
        {
            SortField = "salary";
            SortType = SortType.DESC;
            CurrentPage = 1;
        }
        await LoadData();
    }

    public void SetDefaultValue()
    {
        CurrentPage = 1;
        SortType = SortType.ASC;
        SortField = "id";
    }

}
