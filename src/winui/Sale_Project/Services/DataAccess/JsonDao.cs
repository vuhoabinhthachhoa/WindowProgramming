using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using static Sale_Project.IDao;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sale_Project.Core.Helpers;
using Newtonsoft.Json;

namespace Sale_Project;
public class JsonDao : IDao
{
    public Tuple<List<Product>, int> GetProducts(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    )
    {
        string path = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
@"..\..\..\..\..\..\MockData\products.json");

        var Products = new List<Product>();
        string json = System.IO.File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);


        // Search
        var query = from e in Products
                    where e != null && e.Name.ToLower().Contains(keyword.ToLower())
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
        string path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\products.json");

        var Products = new List<Product>();
        string json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        var item = Products.Find(e => e.ID == id);
        Products.Remove(item);

        var convertedJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
        File.WriteAllText(path, convertedJson);
        return true;
    }

    public bool AddProduct(Product info)
    {
        string path = Path.Combine(
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
@"..\..\..\..\..\..\MockData\products.json");

        var Products = new List<Product>();
        string json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        Products.Add(info);
        var convertedJson = JsonConvert.SerializeObject(Products, Formatting.Indented);
        File.WriteAllText(path, convertedJson);

        return true;
    }

    public bool UpdateProduct(Product info)
    {
        // write update logic here
        string path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"..\..\..\..\..\..\MockData\products.json");
        var Products = new List<Product>();
        string json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        // update logic
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



        return true;
    }
}

