using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Core.Models.Products;
using System.Text.Json;

namespace Sale_Project.ViewModels;

public partial class SaleViewModel : ObservableRecipient
{
    public IInvoiceService _invoiceService;
    public IDialogService _dialogService;
    public IProductService _productService;

    private Invoice _invoice = new();

    public Invoice Invoice
    {
        get => _invoice;
        set
        {
            SetProperty(ref _invoice, value);
            OnPropertyChanged(nameof(Invoice));
        }
    }

    private Product product = new();

    public Product Product
    {
        get => product;
        set
        {
            SetProperty(ref product, value);
            OnPropertyChanged(nameof(Product));
        }
    }

    private List<Product> products = new List<Product>();
    public List<Product> Products
    {
        get => products;
        set
        {
            SetProperty(ref products, value);
            OnPropertyChanged(nameof(Products));
        }
    }

    public List<SampleCustomerDataType> customers
    {
        get; set;
    }

    public SaleViewModel(IInvoiceService invoiceService, IDialogService dialogService, IProductService productService)
    {
        _invoiceService = invoiceService;
        _dialogService = dialogService;
        _productService = productService;
        LoadData();
    }

    /// <summary>
    /// Loads customer data from the "Customers.json" file and deserializes it into a list of SampleCustomerDataType.
    /// </summary>
    public void LoadData()
    {
        var path = GetJsonFilePath("Customers.json");
        var customer = File.ReadAllText(path);
        customers = JsonSerializer.Deserialize<List<SampleCustomerDataType>>(customer)!;
    }

    /// <summary>
    /// Retrieves the full file path for a given JSON file in the "MockData" directory of the Sale_Project.
    /// </summary>
    /// <param name="fileName">The name of the JSON file to retrieve the path for.</param>
    /// <returns>
    /// The full file path to the specified JSON file.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the path to the "Sale_Project" directory cannot be found.
    /// </exception>
    public string GetJsonFilePath(string fileName)
    {
        var fullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var index = fullPath.IndexOf(@"Sale_Project");

        if (index != -1)
        {
            var basePath = fullPath.Substring(0, index);

            return Path.Combine(basePath, @"Sale_Project\MockData\", fileName);
        }
        else
        {
            throw new InvalidOperationException("Invalid path");
        }
    }

    /// <summary>
    /// Creates an invoice based on the provided invoice creation request.
    /// </summary>
    /// <param name="invoiceCreationRequest">The request containing details for creating the invoice.</param>
    /// <returns>
    /// A task that represents the asynchronous operation of creating the invoice.
    /// </returns>
    public async Task CreateInvoiceAsync(InvoiceCreationRequest invoiceCreationRequest)
    {
        Invoice = await _invoiceService.CreateInvoiceAsync(invoiceCreationRequest);
    }

    /// <summary>
    /// Get all products.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation of loading the product list.
    /// </returns>
    public async Task GetProducts()
    {
        Products = await _productService.GetProductByName(" ");
    }
}
