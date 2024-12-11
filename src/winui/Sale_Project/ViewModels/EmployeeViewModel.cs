using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employees;
using Sale_Project.Contracts.Services;
using System.Diagnostics;
using Newtonsoft.Json.Linq;


namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for managing employees.
/// </summary>
public partial class EmployeeViewModel : ObservableRecipient, INavigationAware
{
    private const int _defaultRowsPerPage = 5;
    private readonly IEmployeeService _employeeService;
    private readonly INavigationService _navigationService;

    /// <summary>
    /// Gets or sets the collection of employees.
    /// </summary>
    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

    [ObservableProperty]
    private EmployeeSearchRequest employeeSearchRequest;

    [ObservableProperty]
    private Employee selectedEmployee;

    /// <summary>
    /// Gets the information about the current display status.
    /// </summary>
    public string Info => $"Displaying {Employees.Count}/{RowsPerPage} of total {TotalItems} item(s)";

    /// <summary>
    /// Gets or sets the collection of page information.
    /// </summary>
    public ObservableCollection<PageInfo> PageInfos { get; set; }

    /// <summary>
    /// Gets or sets the selected page information item.
    /// </summary>
    public PageInfo SelectedPageInfoItem { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int TotalItems { get; set; } = 0;

    /// <summary>
    /// Gets or sets the number of rows per page.
    /// </summary>
    public int RowsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the field to sort by.
    /// </summary>
    public string SortField { get; set; } = "id";

    /// <summary>
    /// Gets or sets the sort type.
    /// </summary>
    public SortType SortType { get; set; } = SortType.ASC;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="employeeService">The employee service.</param>
    public EmployeeViewModel(INavigationService navigationService, IEmployeeService employeeService)
    {
        RowsPerPage = _defaultRowsPerPage;
        EmployeeSearchRequest = new EmployeeSearchRequest();
        _employeeService = employeeService;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Called when navigated to the view.
    /// </summary>
    /// <param name="parameter">The navigation parameter.</param>
    public async void OnNavigatedTo(object parameter)
    {
        await LoadData();
    }

    /// <summary>
    /// Called when navigated from the view.
    /// </summary>
    public void OnNavigatedFrom()
    {
        Employees.Clear();
    }

    /// <summary>
    /// Loads the employee data asynchronously.
    /// </summary>
    public async Task LoadData()
    {
        var pageData = await _employeeService.SearchEmployees(CurrentPage, RowsPerPage, SortField, SortType, EmployeeSearchRequest);

        if (pageData == null)
        {
            return;
        }

        Employees = new ObservableCollection<Employee>(pageData.Data);
        TotalItems = pageData.TotalElements;
        TotalPages = pageData.TotalPages;
        CurrentPage = pageData.Page;
    }

    /// <summary>
    /// Navigates to the specified page.
    /// </summary>
    /// <param name="page">The page number.</param>
    public async Task GoToPage(int page)
    {
        CurrentPage = page;
        await LoadData();
    }

    /// <summary>
    /// Searches for employees based on the search request.
    /// </summary>
    public async Task SearchEmployee()
    {
        CurrentPage = 1;
        await LoadData();
    }

    /// <summary>
    /// Navigates to the add employee view.
    /// </summary>
    public void AddEmployee()
    {
        _navigationService.NavigateTo(typeof(EmployeeAddViewModel).FullName!);
    }

    /// <summary>
    /// Navigates to the previous page.
    /// </summary>
    public async Task GoToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadData();
        }
    }

    /// <summary>
    /// Navigates to the next page.
    /// </summary>
    public async Task GoToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadData();
        }
    }

    /// <summary>
    /// Sorts the employees by salary in ascending order.
    /// </summary>
    public async Task SortBySalaryAsc()
    {
        if (SortField == "salary" && SortType == SortType.ASC)
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

    /// <summary>
    /// Sorts the employees by salary in descending order.
    /// </summary>
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

    /// <summary>
    /// Sets the default values for sorting and pagination.
    /// </summary>
    public void SetDefaultValue()
    {
        CurrentPage = 1;
        SortType = SortType.ASC;
        SortField = "id";
    }

    /// <summary>
    /// Called when the selected employee changes.
    /// </summary>
    /// <param name="value">The selected employee.</param>
    partial void OnSelectedEmployeeChanged(Employee value)
    {
        if (value != null)
        {
            _navigationService.NavigateTo(typeof(EmployeeUpdateViewModel).FullName!, value.Id);
        }
    }
}
