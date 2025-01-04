using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Contracts.Services;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Sale_Project.Services;
using Sale_Project.Core.Models.Brands;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for managing and displaying a list of brands with pagination, sorting, and search capabilities.
/// </summary>
public partial class BrandViewModel : ObservableRecipient, INavigationAware
{
    private const int _defaultRowsPerPage = 5;
    private readonly IBrandService _brandService;
    private readonly INavigationService _navigationService;

    /// <summary>
    /// Collection of brands to be displayed.
    /// </summary>
    public ObservableCollection<Brand> Brands { get; set; } = new ObservableCollection<Brand>();

    /// <summary>
    /// Currently selected brand in the list.
    /// </summary>
    [ObservableProperty]
    private Brand selectedBrand;

    /// <summary>
    /// Information string displaying pagination details.
    /// </summary>
    public string Info => $"Displaying {Brands.Count}/{RowsPerPage} of total {TotalItems} item(s)";

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
    } = 10;

    /// <summary>
    /// Field by which brands are sorted.
    /// </summary>
    public string SortField { get; set; } = "id";

    /// <summary>
    /// Sort direction for brands (ascending or descending).
    /// </summary>
    public SortType SortType { get; set; } = SortType.ASC;

    /// <summary>
    /// Initializes a new instance of the BrandViewModel class.
    /// </summary>
    public BrandViewModel(INavigationService navigationService, IBrandService brandService)
    {
        RowsPerPage = _defaultRowsPerPage;
        _brandService = brandService;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Handles navigation to this view model, initializing required data.
    /// </summary>
    public async void OnNavigatedTo(object parameter)
    {
        await LoadData();
    }

    /// <summary>
    /// Handles navigation away from this view model, clearing brand data.
    /// </summary>
    public void OnNavigatedFrom()
    {
        Brands.Clear();
    }

    /// <summary>
    /// Loads brand data with current filters, pagination, and sorting.
    /// </summary>
    public async Task LoadData()
    {
        var brands = await _brandService.GetAllBrands();

        if (brands == null)
        {
            return;
        }

        Brands = new ObservableCollection<Brand>(brands);
        TotalItems = brands.ToList().Count;
        TotalPages = (TotalItems + RowsPerPage - 1) / RowsPerPage;
        CurrentPage = 1;
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
    /// Navigates to the Add Brand view model.
    /// </summary>
    public void AddBrand()
    {
        _navigationService.NavigateTo(typeof(BrandAddViewModel).FullName!);
    }

    /// <summary>
    /// Navigates to the product page using the navigation service.
    /// </summary>
    public void GoToProductPage()
    {
        _navigationService.NavigateTo(typeof(ProductViewModel).FullName!); // Correctly navigates to the Product page.
    }

    /// <summary>
    /// Navigates to the category page using the navigation service.
    /// </summary>
    public void GoToCategoryPage()
    {
        _navigationService.NavigateTo(typeof(CategoryViewModel).FullName!); // Corrected to navigate to the Category page, not Brand.
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
    /// Handles changes to the selected brand, navigating to the Update Brand view model.
    /// </summary>
    partial void OnSelectedBrandChanged(Brand value)
    {
        if (value != null)
        {
            _navigationService.NavigateTo(typeof(BrandUpdateViewModel).FullName!, value);
        }
    }
}
