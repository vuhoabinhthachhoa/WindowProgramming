using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using static Sale_Project.Contracts.Services.IProductDao;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sale_Project.Core.Helpers;
using Newtonsoft.Json;
using Sale_Project.Contracts.Services;
using System.Runtime.CompilerServices;

namespace Sale_Project.Services.Dao.JsonDao;
public class ProductJsonDao : IProductDao
{

    public (bool, string) IsValidInfo(Product info)
    {
        if (info == null) return (false, "Product info is null");
        if (string.IsNullOrWhiteSpace(info.Name)) return (false, "Invalid Product Name");
        if (string.IsNullOrWhiteSpace(info.CategoryID)) return (false, "Invalid CategoryID");
        if (info.ImportPrice <= 0) return (false, "Import Price must be greater than 0");
        if (info.SellingPrice <= 0) return (false, "Selling Price must be greater than 0");
        if (info.SellingPrice < info.ImportPrice) return (false, "Selling Price must be greater than Import Price");
        if (string.IsNullOrWhiteSpace(info.BranchID)) return (false, "Invalid BranchID");
        if (info.InventoryQuantity < 0) return (false, "Inventory Quantity cannot be negative");
        //if (string.IsNullOrWhiteSpace(info.Images)) return (false, "Images are required");
        //if (info.BusinessStatus == null) return (false, "Business Status is required");
        if (string.IsNullOrWhiteSpace(info.Size)) return (false, "Size is required");
        if (info.DiscountPercent < 0 || info.DiscountPercent > 100) return (false, "Discount Percent must be between 0 and 100");

        return (true, string.Empty);
    }

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


    public Tuple<List<Product>, int> GetProducts(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    )
    {
        //        var path = Path.Combine(
        //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        //@"..\..\..\..\..\..\MockData\products.json");

        var path = GetJsonFilePath("products.json");

        var Products = new List<Product>();
        var json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        // Search
        var query = from e in Products
                    where e.Name.ToLower().Contains(keyword.ToLower())
                    select e;

        //// Filter
        //int min = 3;
        //int max = 15;
        //query = query.Where(item => item.ID > min && item.ID < max);

        // Sort
        foreach (var option in sortOptions)
        {
            if (option.Key == "ID")
            {
                if (option.Value == SortType.Ascending)
                {
                    query = query.OrderBy(e => e.ID);
                }
                else
                {
                    query = query.OrderByDescending(e => e.ID);
                }
            }
        }

        var result = query
            .Skip((page - 1) * rowsPerPage)
            .Take(rowsPerPage);

        return new Tuple<List<Product>, int>(
            result.ToList(),
            query.Count()
        );
    }

    public bool DeleteProduct(int id)
    {
        var path = GetJsonFilePath("products.json");

        var Products = new List<Product>();
        var json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        var item = Products.Find(e => e.ID == id);
        Products.Remove(item);

        var convertedJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
        File.WriteAllText(path, convertedJson);
        return true;
    }

    public (bool, string) AddProduct(Product info)
    {
        var (isValid, message) = IsValidInfo(info);
        if (!isValid) return (false, message);

        var path = GetJsonFilePath("products.json");

        var Products = new List<Product>();
        var json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        Products.Add(info);
        var convertedJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        return (true, string.Empty);
    }

    public (bool, string) UpdateProduct(Product info)
    {
        var (isValid, message) = IsValidInfo(info);
        if (!isValid) return (false, message);

        var path = GetJsonFilePath("products.json");
        var Products = new List<Product>();
        var json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        var item = Products.Find(e => e.ID == info.ID);
        item.Name = info.Name;
        item.CategoryID = info.CategoryID;
        item.ImportPrice = info.ImportPrice;
        item.SellingPrice = info.SellingPrice;
        item.BranchID = info.BranchID;
        item.InventoryQuantity = info.InventoryQuantity;
        item.Images = info.Images;
        item.BusinessStatus = info.BusinessStatus;
        item.Size = info.Size;
        item.DiscountPercent = info.DiscountPercent;

        var convertedJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        return (true, string.Empty);
    }
}