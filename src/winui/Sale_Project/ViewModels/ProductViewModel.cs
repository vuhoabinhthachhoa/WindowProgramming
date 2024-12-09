using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Product;
using Sale_Project.Contracts.Services;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Sale_Project.Services;

namespace Sale_Project.ViewModels;

public partial class ProductViewModel : ObservableRecipient, INavigationAware
{
    //private readonly ISampleDataService _sampleDataService;
    private const int _defaultRowsPerPage = 5;
    private readonly IProductService _productService;
    private readonly IBranchService _branchService;
    private readonly ICategoryService _categoryService;
    private readonly INavigationService _navigationService;


    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

    [ObservableProperty]
    private ProductSearchRequest productSearchRequest;

    [ObservableProperty]
    private Product selectedProduct;

    [ObservableProperty]
    private string[] _branches;

    [ObservableProperty]
    private string[] _categories;

    public string Info => $"Displaying {Products.Count}/{RowsPerPage} of total {TotalItems} item(s)";

    public string[] Size { get; set; } = new string[] { "S", "M", "L", "XL", "XXL" };

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


    public ProductViewModel(INavigationService navigationService, IBranchService branchService, ICategoryService categoryService, IProductService productService)
    {

        RowsPerPage = _defaultRowsPerPage;
        ProductSearchRequest = new ProductSearchRequest();
        _productService = productService;
        _branchService = branchService;
        _categoryService = categoryService;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        var branchNames = await _branchService.GetAllBranches();
        Branches = branchNames?.Select(branch => branch.Name).ToArray();

        // Fetch category names and set the Categories property
        var categoryNames = await _categoryService.GetAllCategories();
        Categories = categoryNames?.Select(category => category.Name).ToArray();


        await LoadData();
        // TODO: Replace with real data.
        //var data = await _sampleDataService.GetGridDataAsync();

        //foreach (var item in data)
        //{
        //    Products.Add(item);
        //}
    }

    public void OnNavigatedFrom()
    {
        Products.Clear();
    }

    public async Task LoadData()
    {

        // Fetch data asynchronously
        var pageData = await _productService.SearchProducts(CurrentPage, RowsPerPage, SortField, SortType, ProductSearchRequest);

        if (pageData == null)
        {
            return; // Do nothing if pageData is null
        }

        // Convert the result to ObservableCollection
        Products = new ObservableCollection<Product>(pageData.Data);
        TotalItems = pageData.TotalElements;
        TotalPages = pageData.TotalPages;
        CurrentPage = pageData.Page;
        
    }

    public async Task GoToPage(int page)
    {
        CurrentPage = page;
        await LoadData();
    }

    public async Task SearchProduct()
    {
        CurrentPage = 1;
        await LoadData();
    }

    public void AddProduct()
    {
        _navigationService.NavigateTo(typeof(ProductAddViewModel).FullName!);
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

    public void SetDefaultValue()
    {
        CurrentPage = 1;
        SortType = SortType.ASC;
        SortField = "id";
    }

    partial void OnSelectedProductChanged(Product value)
    {
        if (value != null)
        {
            _navigationService.NavigateTo(typeof(ProductUpdateViewModel).FullName!, value);
        }
    }

    //public async Task BusinessStatusChanged()
    //{
    //    await LoadData();
    //}
}
