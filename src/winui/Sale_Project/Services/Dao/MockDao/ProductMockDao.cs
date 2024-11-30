//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Sale_Project.Contracts.Services;
//using Sale_Project.Core.Models.Products;
//using static Sale_Project.Contracts.Services.IProductDao;

//namespace Sale_Project.Services.Dao.MockDao;
//public class ProductMockDao : IProductDao
//{
//    //public Tuple<List<Product>, int> GetProducts(
//    //    int page, int rowsPerPage,
//    //    string keyword,
//    //    Dictionary<string, SortType> sortOptions
//    //)
//    //{
//    //    var Products = new List<Product>() {
//    //        new Product() {
//    //            ID = 1,
//    //            Name = "Product 1",
//    //            CategoryID = "1",
//    //            ImportPrice = 100,
//    //            SellingPrice = 200,
//    //            BranchID = "1",
//    //            InventoryQuantity = 10,
//    //            Images = "image1.jpg",
//    //            BusinessStatus = true,
//    //            Size = "M"
//    //        },
//    //        new Product() {
//    //            ID = 2,
//    //            Name = "Product 2",
//    //            CategoryID = "2",
//    //            ImportPrice = 200,
//    //            SellingPrice = 300,
//    //            BranchID = "2",
//    //            InventoryQuantity = 20,
//    //            Images = "image2.jpg",
//    //            BusinessStatus = true,
//    //            Size = "L"
//    //        },
//    //        new Product() {
//    //            ID = 3,
//    //            Name = "Product 3",
//    //            CategoryID = "3",
//    //            ImportPrice = 300,
//    //            SellingPrice = 400,
//    //            BranchID = "3",
//    //            InventoryQuantity = 30,
//    //            Images = "image3.jpg",
//    //            BusinessStatus = true,
//    //            Size = "S"
//    //        },
//    //        new Product() {
//    //            ID = 4,
//    //            Name = "Product 4",
//    //            CategoryID = "4",
//    //            ImportPrice = 400,
//    //            SellingPrice = 500,
//    //            BranchID = "4",
//    //            InventoryQuantity = 40,
//    //            Images = "image4.jpg",
//    //            BusinessStatus = true,
//    //            Size = "XL"
//    //        },
//    //        new Product() {
//    //            ID = 5,
//    //            Name = "Product 5",
//    //            CategoryID = "5",
//    //            ImportPrice = 500,
//    //            SellingPrice = 600,
//    //            BranchID = "5",
//    //            InventoryQuantity = 50,
//    //            Images = "image5.jpg",
//    //            BusinessStatus = true,
//    //            Size = "XXL"
//    //        },
//    //        new Product() {
//    //            ID = 6,
//    //            Name = "Product 6",
//    //            CategoryID = "6",
//    //            ImportPrice = 600,
//    //            SellingPrice = 700,
//    //            BranchID = "6",
//    //            InventoryQuantity = 60,
//    //            Images = "image6.jpg",
//    //            BusinessStatus = true,
//    //            Size = "M"
//    //        },
//    //        new Product() {
//    //            ID = 7,
//    //            Name = "Product 7",
//    //            CategoryID = "7",
//    //            ImportPrice = 700,
//    //            SellingPrice = 800,
//    //            BranchID = "7",
//    //            InventoryQuantity = 70,
//    //            Images = "image7.jpg",
//    //            BusinessStatus = true,
//    //            Size = "L"
//    //        },
//    //    };
//    //    // Search
//    //    var query = from e in Products
//    //                where e.Name.ToLower().Contains(keyword.ToLower())
//    //                select e;

//    //    //// Filter
//    //    //int min = 3;
//    //    //int max = 15;
//    //    //query = query.Where(item => item.ID > min && item.ID < max);

//    //    // Sort
//    //    foreach (var option in sortOptions)
//    //    {
//    //        if (option.Key == "ID")
//    //        {
//    //            if (option.Value == SortType.Ascending)
//    //            {
//    //                query = query.OrderBy(e => e.ID);
//    //            }
//    //            else
//    //            {
//    //                query = query.OrderByDescending(e => e.ID);
//    //            }
//    //        }
//    //    }

//    //    var result = query
//    //        .Skip((page - 1) * rowsPerPage)
//    //        .Take(rowsPerPage);

//    //    return new Tuple<List<Product>, int>(
//    //        result.ToList(),
//    //        query.Count()
//    //    );
//    //}

//    public bool DeleteProduct(int id)
//    {
//        return true;
//    }

//    public (bool, string) AddProduct(Product info)
//    {
//        return (true, "");
//    }

//    public (bool, string) UpdateProduct(Product info)
//    {
//        return (true, "");
//    }
//}

