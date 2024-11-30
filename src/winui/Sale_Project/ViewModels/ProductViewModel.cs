//using Sale_Project.Services;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Services.Maps;
//using static Sale_Project.Contracts.Services.IProductDao;
//using Sale_Project.Core.Models;
//using CommunityToolkit.Mvvm.ComponentModel;
//using Sale_Project.Contracts.ViewModels;
//using Sale_Project.Contracts.Services;
//using Sale_Project.Services.Dao.JsonDao;
//using Sale_Project.Core.Models.Product;

//namespace Sale_Project;
//public partial class ProductViewModel : ObservableRecipient, INotifyPropertyChanged
//{
//    IProductDao _dao;
//    public ObservableCollection<Product> Products
//    {
//        get; set;
//    }

//    public string Info
//    {
//        get
//        {
//            //return $"Displaying {Products.Count}/{RowsPerPage} of total {TotalProducts} item(s)";
//            return "";
//        }
//        set
//        {
//        }
//    }

//    public ObservableCollection<PageInfo> PageInfos
//    {
//        get; set;
//    }
//    public PageInfo SelectedPageInfoProduct
//    {
//        get; set;
//    }
//    public string Keyword { get; set; } = "";

//    public int CurrentPage
//    {
//        get; set;
//    }
//    public int TotalPages
//    {
//        get; set;
//    }
//    public int TotalProducts { get; set; } = 0;
//    public int RowsPerPage
//    {
//        get; set;
//    }

//    private bool _sortById = false;
//    public bool SortById
//    {
//        get => _sortById;
//        set
//        {
//            //_sortById = value;
//            //if (value == true)
//            //{
//            //    _sortOptions.Add("Name", SortType.Ascending);
//            //}
//            //else
//            //{
//            //    if (_sortOptions.ContainsKey("Name"))
//            //    {
//            //        _sortOptions.Remove("Name");
//            //    }
//            //}

//            LoadDataAsync();
//        }
//    }

//   //  private Dictionary<string, SortType> _sortOptions = new();

//    public ProductViewModel()
//    {
//        //ServiceFactory.Register(typeof(IProductDao), typeof(ProductJsonDao));
//        //RowsPerPage = 10;
//        //CurrentPage = 1;
//        //_dao = ServiceFactory.GetChildOf(typeof(IProductDao)) as IProductDao;


//        LoadDataAsync();


//    }   

//    public bool Remove(Product info)
//    {
//        bool success = _dao.DeleteProduct(info.ID); // DB

//        if (success)
//        {
//            Products.Remove(info); // UI
//        }
//        return success;
//    }

//    public async Task LoadDataAsync()
//    {


//        //var (items, count) = await _dao.GetProducts(
//        //    CurrentPage, RowsPerPage, Keyword,
//        //    _sortOptions
//        //);
//        //Products = new ObservableCollection<Product>(
//        //    items
//        //);
//        //var (items, count) = _dao.GetProducts(
//        //    CurrentPage, RowsPerPage, Keyword,
//        //    _sortOptions
//        //);
//        //Products = new ObservableCollection<Product>(
//        //    items
//        //);

//        //if (count != TotalProducts)
//        //{ // Recreate PageInfos list
//        //    TotalProducts = count;
//        //    TotalPages = (TotalProducts / RowsPerPage) +
//        //        (((TotalProducts % RowsPerPage) == 0) ? 0 : 1);

//        //    PageInfos = new();
//        //    for (int i = 1; i <= TotalPages; i++)
//        //    {
//        //        PageInfos.Add(new PageInfo
//        //        {
//        //            Page = i,
//        //            Total = TotalPages
//        //        });
//        //    }
//        //}

//        //if (CurrentPage > TotalPages)
//        //{
//        //    CurrentPage = TotalPages;
//        //}

//        //if (PageInfos.Count > 0)
//        //{

//        //    SelectedPageInfoProduct = PageInfos[CurrentPage - 1];
//        //}
//    }
//    public void GoToPage(int page)
//    {
//        CurrentPage = page;
//        LoadDataAsync();
//    }

//    public void Search()
//    {
//        CurrentPage = 1;
//        LoadDataAsync();
//    }

//    public void GoToPreviousPage()
//    {
//        if (CurrentPage > 1)
//        {
//            CurrentPage--;
//            LoadDataAsync();
//        }
//    }

//    public void GoToNextPage()
//    {
//        if (CurrentPage < TotalPages)
//        {
//            CurrentPage++;
//            LoadDataAsync();
//        }
//    }
//}


using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Product;
using Sale_Project.Contracts.Services;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Sale_Project.ViewModels;

public partial class ProductViewModel : ObservableRecipient, INavigationAware
{
    //private readonly ISampleDataService _sampleDataService;
    private const int _defaultRowsPerPage = 5;
    private readonly IProductService _productService;
    private readonly INavigationService _navigationService;


    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

    [ObservableProperty]
    private ProductSearchRequest productSearchRequest;

    [ObservableProperty]
    private Product selectedProduct;

    public string Info => $"Displaying {Products.Count}/{RowsPerPage} of total {TotalItems} item(s)";

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


    public ProductViewModel(INavigationService navigationService, IProductService productService)
    {

        RowsPerPage = _defaultRowsPerPage;
        ProductSearchRequest = new ProductSearchRequest();
        _productService = productService;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {
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

    public async Task SortBySalaryAsc()
    {
        if (SortField == "name" && SortType == SortType.ASC)
        {
            SetDefaultValue();
        }
        else
        {
            SortField = "name";
            SortType = SortType.ASC;
            CurrentPage = 1;
        }
        await LoadData();
    }

    public async Task SortBySalaryDesc()
    {
        if (SortField == "name" && SortType == SortType.DESC)
        {
            SetDefaultValue();
        }
        else
        {
            SortField = "name";
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
