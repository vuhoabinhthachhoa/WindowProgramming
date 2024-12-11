using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Products;
using Sale_Project.Contracts.Services;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Sale_Project.Services;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for managing and displaying a list of products with pagination, sorting, and search capabilities.
/// </summary>
public partial class ProductViewModel : ObservableRecipient, INavigationAware
{
    private const int _defaultRowsPerPage = 5;
    private readonly IProductService _productService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;
    private readonly INavigationService _navigationService;

    /// <summary>
    /// Collection of products to be displayed.
    /// </summary>
    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

    /// <summary>
    /// Search request object containing filters for product search.
    /// </summary>
    [ObservableProperty]
    private ProductSearchRequest productSearchRequest;

    /// <summary>
    /// Currently selected product in the list.
    /// </summary>
    [ObservableProperty]
    private Product selectedProduct;

    /// <summary>
    /// List of brand names for filtering.
    /// </summary>
    [ObservableProperty]
    private string[] _brands;

    /// <summary>
    /// List of category names for filtering.
    /// </summary>
    [ObservableProperty]
    private string[] _categories;

    /// <summary>
    /// Information string displaying pagination details.
    /// </summary>
    public string Info => $"Displaying {Products.Count}/{RowsPerPage} of total {TotalItems} item(s)";

    /// <summary>
    /// Array of available sizes for products.
    /// </summary>
    public string[] Size { get; set; } = new string[] { "S", "M", "L", "XL", "XXL" };

    /// <summary>
    /// Collection of pagination details.
    /// </summary>
    public ObservableCollection<PageInfo> PageInfos
    {
        get; set;
    }

    /// <summary>
    /// Selected pagination item.
    /// </summary>
    public PageInfo SelectedPageInfoItem
    {
        get; set;
    }

    /// <summary>
    /// Current page number in pagination.
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// Total number of pages available.
    /// </summary>
    public int TotalPages
    {
        get; set;
    }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    public int TotalItems { get; set; } = 0;

    /// <summary>
    /// Number of rows per page.
    /// </summary>
    public int RowsPerPage
    {
        get; set;
    }

    /// <summary>
    /// Field by which products are sorted.
    /// </summary>
    public string SortField { get; set; } = "id";

    /// <summary>
    /// Sort direction for products (ascending or descending).
    /// </summary>
    public SortType SortType { get; set; } = SortType.ASC;

    /// <summary>
    /// Initializes a new instance of the ProductViewModel class.
    /// </summary>
    public ProductViewModel(INavigationService navigationService, IBrandService brandService, ICategoryService categoryService, IProductService productService)
    {
        RowsPerPage = _defaultRowsPerPage;
        ProductSearchRequest = new ProductSearchRequest();
        _productService = productService;
        _brandService = brandService;
        _categoryService = categoryService;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Handles navigation to this view model, initializing required data.
    /// </summary>
    public async void OnNavigatedTo(object parameter)
    {
        var brandNames = await _brandService.GetAllBrands();
        Brands = brandNames?.Select(brand => brand.Name).ToArray();

        var categoryNames = await _categoryService.GetAllCategories();
        Categories = categoryNames?.Select(category => category.Name).ToArray();

        await LoadData();
    }

    /// <summary>
    /// Handles navigation away from this view model, clearing product data.
    /// </summary>
    public void OnNavigatedFrom()
    {
        Products.Clear();
    }

    /// <summary>
    /// Loads product data with current filters, pagination, and sorting.
    /// </summary>
    public async Task LoadData()
    {
        var pageData = await _productService.SearchProducts(CurrentPage, RowsPerPage, SortField, SortType, ProductSearchRequest);

        if (pageData == null)
        {
            return;
        }

        Products = new ObservableCollection<Product>(pageData.Data);
        TotalItems = pageData.TotalElements;
        TotalPages = pageData.TotalPages;
        CurrentPage = pageData.Page;
    }

    /// <summary>
    /// Navigates to a specific page in pagination.
    /// </summary>
    public async Task GoToPage(int page)
    {
        CurrentPage = page;
        await LoadData();
    }

    /// <summary>
    /// Performs a search based on the current filters.
    /// </summary>
    public async Task SearchProduct()
    {
        CurrentPage = 1;
        await LoadData();
    }

    /// <summary>
    /// Navigates to the Add Product view model.
    /// </summary>
    public void AddProduct()
    {
        _navigationService.NavigateTo(typeof(ProductAddViewModel).FullName!);
    }

    /// <summary>
    /// Navigates to the previous page in pagination, if possible.
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
    /// Navigates to the next page in pagination, if possible.
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
    /// Sorts products by ID in ascending order.
    /// </summary>
    public async Task SortByIDAsc()
    {
        if (SortField == "id" && SortType == SortType.ASC)
        {
            SetDefaultValue();
        }
        else
        {
            SortField = "id";
            SortType = SortType.ASC;
            CurrentPage = 1;
        }
        await LoadData();
    }

    /// <summary>
    /// Sorts products by ID in descending order.
    /// </summary>
    public async Task SortByIDDesc()
    {
        if (SortField == "id" && SortType == SortType.DESC)
        {
            SetDefaultValue();
        }
        else
        {
            SortField = "id";
            SortType = SortType.DESC;
            CurrentPage = 1;
        }
        await LoadData();
    }

    /// <summary>
    /// Resets sorting to default values.
    /// </summary>
    public void SetDefaultValue()
    {
        CurrentPage = 1;
        SortType = SortType.ASC;
        SortField = "id";
    }

    /// <summary>
    /// Handles changes to the selected product, navigating to the Update Product view model.
    /// </summary>
    partial void OnSelectedProductChanged(Product value)
    {
        if (value != null)
        {
            _navigationService.NavigateTo(typeof(ProductUpdateViewModel).FullName!, value);
        }
    }
}
