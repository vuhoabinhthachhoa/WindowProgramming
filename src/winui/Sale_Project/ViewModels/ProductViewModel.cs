using Sale_Project.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;
using static Sale_Project.Contracts.Services.IProductDao;
using Sale_Project.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Contracts.Services;
using Sale_Project.Services.Dao.JsonDao;

namespace Sale_Project;
public partial class ProductViewModel : ObservableRecipient, INotifyPropertyChanged
{
    IProductDao _dao;
    public ObservableCollection<Product> Products
    {
        get; set;
    }

    public string Info
    {
        get
        {
            //return $"Displaying {Products.Count}/{RowsPerPage} of total {TotalProducts} item(s)";
            return "";
        }
    }

    public ObservableCollection<PageInfo> PageInfos
    {
        get; set;
    }
    public PageInfo SelectedPageInfoProduct
    {
        get; set;
    }
    public string Keyword { get; set; } = "";

    public int CurrentPage
    {
        get; set;
    }
    public int TotalPages
    {
        get; set;
    }
    public int TotalProducts { get; set; } = 0;
    public int RowsPerPage
    {
        get; set;
    }

    private bool _sortById = false;
    public bool SortById
    {
        get => _sortById;
        set
        {
            //_sortById = value;
            //if (value == true)
            //{
            //    _sortOptions.Add("Name", SortType.Ascending);
            //}
            //else
            //{
            //    if (_sortOptions.ContainsKey("Name"))
            //    {
            //        _sortOptions.Remove("Name");
            //    }
            //}

            LoadDataAsync();
        }
    }

   //  private Dictionary<string, SortType> _sortOptions = new();

    public ProductViewModel()
    {
        //ServiceFactory.Register(typeof(IProductDao), typeof(ProductJsonDao));
        //RowsPerPage = 10;
        //CurrentPage = 1;
        //_dao = ServiceFactory.GetChildOf(typeof(IProductDao)) as IProductDao;
        
        LoadDataAsync();
    }   

    public bool Remove(Product info)
    {
        bool success = _dao.DeleteProduct(info.ID); // DB

        if (success)
        {
            Products.Remove(info); // UI
        }
        return success;
    }

    public async Task LoadDataAsync()
    {

        var (items, count) = await _dao.GetProducts(
            CurrentPage, RowsPerPage, Keyword,
            _sortOptions
        );
        Products = new ObservableCollection<Product>(
            items
        );

        //if (count != TotalProducts)
        //{ // Recreate PageInfos list
        //    TotalProducts = count;
        //    TotalPages = (TotalProducts / RowsPerPage) +
        //        (((TotalProducts % RowsPerPage) == 0) ? 0 : 1);

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

        //    SelectedPageInfoProduct = PageInfos[CurrentPage - 1];
        //}
    }
    public void GoToPage(int page)
    {
        CurrentPage = page;
        LoadDataAsync();
    }

    public void Search()
    {
        CurrentPage = 1;
        LoadDataAsync();
    }

    public void GoToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadDataAsync();
        }
    }

    public void GoToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            LoadDataAsync();
        }
    }
}
