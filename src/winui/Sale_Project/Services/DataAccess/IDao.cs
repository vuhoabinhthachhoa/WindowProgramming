﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project;

public interface IDao
{
    public enum SortType
    {
        Ascending,
        Descending
    }
    Tuple<List<Product>, int> GetProducts(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    );

    bool DeleteProduct(int id);
    bool AddProduct(Product info);
    bool UpdateProduct(Product info);
}

