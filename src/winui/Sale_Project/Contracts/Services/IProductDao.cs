using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Contracts.Services;

public interface IProductDao
{
    public enum SortType
    {
        Ascending,
        Descending
    }
    //Tuple<List<Product>, int> GetProducts(
    //    int page, int rowsPerPage,
    //    string keyword,
    //    Dictionary<string, SortType> sortOptions
    //);

    bool DeleteProduct(int id);
    (bool, string) AddProduct(Product info);
    (bool, string) UpdateProduct(Product info);
}