using Sale_Project.Services;
using Sale_Project;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;
using static Sale_Project.IDao;
using Sale_Project.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.ViewModels;

namespace Sale_Project;
public partial class ProductViewModel : ObservableRecipient, INotifyPropertyChanged
{
    IDao _dao;
    public ObservableCollection<Product> Products
    {
        get; set;
    }

    public string Info
    {
        get
        {
            return $"Displaying {Products.Count}/{RowsPerPage} of total {TotalItems} item(s)";
        }
    }

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
    }
    public int TotalPages
    {
        get; set;
    }
    public int TotalItems { get; set; } = 0;
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
            _sortById = value;
            if (value == true)
            {
                _sortOptions.Add("Name", SortType.Ascending);
            }
            else
            {
                if (_sortOptions.ContainsKey("Name"))
                {
                    _sortOptions.Remove("Name");
                }
            }

            LoadData();
        }
    }

    private Dictionary<string, SortType> _sortOptions = new();

    public ProductViewModel()
    {
        RowsPerPage = 10;
        CurrentPage = 1;
        _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        
        LoadData();
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

    public void LoadData()
    {

        var (items, count) = _dao.GetProducts(
            CurrentPage, RowsPerPage, Keyword,
            _sortOptions
        );
        Products = new ObservableCollection<Product>(
            items
        );

        if (count != TotalItems)
        { // Recreate PageInfos list
            TotalItems = count;
            TotalPages = (TotalItems / RowsPerPage) +
                (((TotalItems % RowsPerPage) == 0) ? 0 : 1);

            PageInfos = new();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageInfos.Add(new PageInfo
                {
                    Page = i,
                    Total = TotalPages
                });
            }
        }

        if (CurrentPage > TotalPages)
        {
            CurrentPage = TotalPages;
        }

        if (PageInfos.Count > 0)
        {

            SelectedPageInfoItem = PageInfos[CurrentPage - 1];
        }
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
}
