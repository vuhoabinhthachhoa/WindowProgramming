using Sale_Project.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;
//using static Sale_Project.Contracts.Services.IEmployeeDao;
using Sale_Project.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Contracts.Services;

using Sale_Project.ViewModels;
using Sale_Project.Core.Models.Employee;

namespace Sale_Project.ViewModels;
public partial class EmployeeViewModel : ObservableRecipient
{
    //IEmployeeDao _dao;

    //[ObservableProperty]
    //private ObservableCollection<Employee> _employees;
    private readonly IEmployeeService _employeeService;
    private readonly INavigationService _navigationService;
    private const int _defaultRowsPerPage = 10;

    public ObservableCollection<Employee> Employees
    {
        get; set;
    }

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
    public string Keyword { get; set; } = "";
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

    public EmployeeViewModel(IEmployeeService employeeService, INavigationService navigationService)
    {
        //ServiceFactory.Register(typeof(IEmployeeDao), typeof(EmployeeJsonDao));
        // _dao = ServiceFactory.GetChildOf(typeof(IEmployeeDao)) as IEmployeeDao;

        _employeeService = employeeService;
        _navigationService = navigationService;
        RowsPerPage = _defaultRowsPerPage;
        EmployeeSearchRequest = new EmployeeSearchRequest();
        LoadData();

    }

    public void AddEmployee()
    {
        //_navigationService.NavigateTo(typeof(EmployeeAddPageViewModel).FullName!);
        //LoadData();
    }

    public bool UnemployedEmployee(Employee info)
    {
        //bool success = _dao.DeleteEmployee(info.ID); // DB

        //if (success)
        //{
        //    Employees.Remove(info); // UI
        //}
        //return success;
        return true;
    }

    public void UpdateEmployee(Employee employee)
    {
        //_navigationService.NavigateTo(typeof(EmployeeUpdatePageViewModel).FullName!, employee);
        //LoadData();
    }

    public async void LoadData()
    {
        // Fetch data asynchronously
        var employeeList = await _employeeService.SearchEmployees(CurrentPage, RowsPerPage, SortField, SortType, EmployeeSearchRequest);

        // Convert the result to ObservableCollection
        Employees = new ObservableCollection<Employee>(employeeList);

        //var (items, count) = _dao.GetEmployees(
        //    CurrentPage, RowsPerPage, Keyword,
        //    _sortOptions
        //);
        //Employees = new ObservableCollection<Employee>(
        //    items
        //);

        //if (count != TotalItems)
        //{ // Recreate PageInfos list
        //    TotalItems = count;
        //    TotalPages = (TotalItems / RowsPerPage) +
        //        (((TotalItems % RowsPerPage) == 0) ? 0 : 1);

        //    PageInfos = new();
        //    for (int i = 1; i <= TotalPages; i++)
        //    {
        //        PageInfos.Add(new PageInfo
        //        {
        //            Page = i,
        //            Total = TotalPages
        //        });
        //    }
        //}

        //if (CurrentPage > TotalPages)
        //{
        //    CurrentPage = TotalPages;
        //}

        //if (PageInfos.Count > 0)
        //{

        //    SelectedPageInfoItem = PageInfos[CurrentPage - 1];
        //}
    }
    public void GoToPage(int page)
    {
        CurrentPage = page;
        LoadData();
    }

    public void Search()
    {
        CurrentPage = 1;
        LoadData();
    }

    public void GoToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadData();
        }
    }

    public void GoToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            LoadData();
        }
    }

    //private bool _sortById = false;
    //public bool SortById
    //{
    //    get => _sortById;
    //    set
    //    {
    //        _sortById = value;
    //        if (value == true)
    //        {
    //            _sortOptions.Add("Name", SortType.Ascending);
    //        }
    //        else
    //        {
    //            if (_sortOptions.ContainsKey("Name"))
    //            {
    //                _sortOptions.Remove("Name");
    //            }
    //        }

    //        LoadData();
    //    }
    //}

    // private Dictionary<string, SortType> _sortOptions = new();
}