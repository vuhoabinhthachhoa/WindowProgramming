using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Core.Models;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Products;
using Sale_Project.Services;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Core.Models.Employees;

namespace Sale_Project.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    public IProductService _productService;
    public IDialogService _dialogService;
    public IInvoiceService _invoiceService;
    public IEmployeeService _employeeService;

    List<Product> products = new();
    public List<Product> Products
    {
        get => products;
        set
        {
            SetProperty(ref products, value);
            OnPropertyChanged(nameof(Products));
        }
    }

    InvoiceAggregation aggregation = new();
    public InvoiceAggregation Aggregation
    {
        get => aggregation;
        set
        {
            SetProperty(ref aggregation, value);
            OnPropertyChanged(nameof(Aggregation));
        }
    }

    List<EmployeeTotalInvoices> employeeTotalInvoices = new();
    public List<EmployeeTotalInvoices> EmployeeTotalInvoices
    {
        get => employeeTotalInvoices;
        set
        {
            SetProperty(ref employeeTotalInvoices, value);
            OnPropertyChanged(nameof(EmployeeTotalInvoices));
        }
    }

    ObservableCollection<Employee> sortedEmployees = new();
    public ObservableCollection<Employee> SortedEmployees
    {
        get => sortedEmployees;
        set
        {
            SetProperty(ref sortedEmployees, value);
            OnPropertyChanged(nameof(SortedEmployees));
        }
    }

    public DashboardViewModel(IProductService productService, IDialogService dialogService, IInvoiceService invoiceService, IEmployeeService employeeService)
    {
        _productService = productService;
        _dialogService = dialogService;
        _invoiceService = invoiceService;
        _employeeService = employeeService;
    }

    /// <summary>
    /// Retrieves the list of products from the product service by sending a request with a blank search query.
    /// </summary>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Calls the <see cref="IProductService.GetProductByName"/> method with a blank string as the search query.</item>
    /// <item>Updates the <see cref="Products"/> collection with the retrieved product list.</item>
    /// </list>
    /// </remarks>
    public async Task GetProducts()
    {
        Products = await _productService.GetProductByName(" ");
    }

    /// <summary>
    /// Retrieves the invoice aggregation data for a specified date range from the invoice service.
    /// </summary>
    /// <param name="startDate">
    /// The start date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <param name="endDate">
    /// The end date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Calls the <see cref="IInvoiceService.GetInvoiceAggregationAsync"/> method with the provided start and end date parameters.</item>
    /// <item>Updates the <see cref="Aggregation"/> property with the retrieved aggregation data.</item>
    /// </list>
    /// </remarks>
    public async Task GetInvoiceAggregationAsync(string startDate, string endDate)
    {
        Aggregation = await _invoiceService.GetInvoiceAggregationAsync(startDate, endDate);
        if(Aggregation == null)
        {
            return;
        }
    }

    /// <summary>
    /// Retrieves the employee total invoice data for a specified date range and sorts the employees by invoice count in descending order.
    /// </summary>
    /// <param name="startDate">
    /// The start date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <param name="endDate">
    /// The end date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Calls the <see cref="IEmployeeService.GetEmployeeByTotalInvoice"/> method with the provided start and end date parameters.</item>
    /// <item>Sorts the retrieved employee data by invoice count in descending order.</item>
    /// <item>Updates the <see cref="SortedEmployees"/> collection with the sorted employee data.</item>
    /// </list>
    /// </remarks>
    public async Task GetEmployeeByTotalInvoiceAsync(string startDate, string endDate)
    {
        EmployeeTotalInvoices = await _employeeService.GetEmployeeByTotalInvoice(startDate, endDate);
        SortedEmployees = new ObservableCollection<Employee>(EmployeeTotalInvoices.OrderByDescending(e => e.invoiceCount).Select(e => e.employeeResponse));
        if (EmployeeTotalInvoices == null)
        {
            return;
        }
    }
}
