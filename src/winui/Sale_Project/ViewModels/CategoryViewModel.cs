using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Contracts.Services;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Sale_Project.Services;
using Sale_Project.Core.Models.Categories;

namespace Sale_Project.ViewModels;

/// <summary>
/// ViewModel for managing and displaying a list of categorys with pagination, sorting, and search capabilities.
/// </summary>
public partial class CategoryViewModel : ObservableRecipient, INavigationAware
{
    private const int _defaultRowsPerPage = 5;
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly INavigationService _navigationService;

    /// <summary>
    /// Collection of categorys to be displayed.
    /// </summary>
    public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

    /// <summary>
    /// Currently selected category in the list.
    /// </summary>
    [ObservableProperty]
    private Category selectedCategory;

    /// <summary>
    /// Information string displaying pagination details.
    /// </summary>
    public string Info => $"Displaying {Categories.Count}/{RowsPerPage} of total {TotalItems} item(s)";

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
    } = 5;

    /// <summary>
    /// Field by which categorys are sorted.
    /// </summary>
    public string SortField { get; set; } = "id";

    /// <summary>
    /// Sort direction for categorys (ascending or descending).
    /// </summary>
    public SortType SortType { get; set; } = SortType.ASC;

    /// <summary>
    /// Initializes a new instance of the CategoryViewModel class.
    /// </summary>
    public CategoryViewModel(INavigationService navigationService, ICategoryService categoryService)
    {
        RowsPerPage = _defaultRowsPerPage;
        _categoryService = categoryService;
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
    /// Handles navigation away from this view model, clearing category data.
    /// </summary>
    public void OnNavigatedFrom()
    {
        Categories.Clear();
    }

    /// <summary>
    /// Loads category data with current filters, pagination, and sorting.
    /// </summary>
    public async Task LoadData()
    {
        var categories = await _categoryService.GetAllCategories();

        if (categories == null)
        {
            return;
        }

        Categories = new ObservableCollection<Category>(categories);
        TotalItems = categories.ToList().Count;
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
    /// Performs a search based on the current filters.
    /// </summary>
    public async Task SearchCategory()
    {
        CurrentPage = 1;
        await LoadData();
    }

    /// <summary>
    /// Navigates to the Add Category view model.
    /// </summary>
    public void AddCategory()
    {
        _navigationService.NavigateTo(typeof(CategoryAddViewModel).FullName!);
    }

    /// <summary>
    /// Navigates to the product page using the navigation service.
    /// </summary>
    public void GoToProductPage()
    {
        _navigationService.NavigateTo(typeof(ProductViewModel).FullName!); // Correctly navigates to the Product page using the full name of the ProductViewModel.
    }

    /// <summary>
    /// Navigates to the brand page using the navigation service.
    /// </summary>
    public void GoToBrandPage()
    {
        _navigationService.NavigateTo(typeof(BrandViewModel).FullName!); // Correctly navigates to the Brand page using the full name of the BrandViewModel.
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
    /// Sorts categorys by ID in ascending order.
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
    /// Sorts categorys by ID in descending order.
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
    /// Handles changes to the selected category, navigating to the Update Category view model.
    /// </summary>
    partial void OnSelectedCategoryChanged(Category value)
    {
        if (value != null)
        {
            _navigationService.NavigateTo(typeof(CategoryUpdateViewModel).FullName!, value);
        }
    }
}
