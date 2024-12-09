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
using Sale_Project.Contracts.Services;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Sale_Project.Contracts.Services;
using Windows.Storage;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using static Sale_Project.Services.AuthService;
using Sale_Project.Core.Models.Product;

namespace Sale_Project.Services.Dao.JsonDao;
public class ProductJsonDao : IProductDao
{

    public (bool, string) IsValidInfo(Product info)
    {
        //if (info == null) return (false, "Product info is null");
        //if (string.IsNullOrWhiteSpace(info.Name)) return (false, "Invalid Product Name");
        //if (string.IsNullOrWhiteSpace(info.Category.ID)) return (false, "Invalid CategoryID");
        //if (info.ImportPrice <= 0) return (false, "Import Price must be greater than 0");
        //if (info.SellingPrice <= 0) return (false, "Selling Price must be greater than 0");
        //if (info.SellingPrice < info.ImportPrice) return (false, "Selling Price must be greater than Import Price");
        //if (string.IsNullOrWhiteSpace(info.Branch.Name)) return (false, "Invalid BranchID");
        //if (info.InventoryQuantity < 0) return (false, "Inventory Quantity cannot be negative");
        ////if (string.IsNullOrWhiteSpace(info.Images)) return (false, "Images are required");
        ////if (info.BusinessStatus == null) return (false, "Business Status is required");
        //if (string.IsNullOrWhiteSpace(info.Size)) return (false, "Size is required");
        //if (info.DiscountPercent < 0 || info.DiscountPercent > 100) return (false, "Discount Percent must be between 0 and 100");

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


    //public Tuple<List<Product>, int> GetProducts(
    //    int page, int rowsPerPage,
    //    string keyword,
    //    Dictionary<string, SortType> sortOptions
    //)
    //{

    //    var path = GetJsonFilePath("products.json");

    //    var Products = new List<Product>();
    //    var json = File.ReadAllText(path); 
    //    Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

    //    // Search
    //    var query = from e in Products
    //                where e.Name.ToLower().Contains(keyword.ToLower())
    //                select e;

    //    // Sort
    //    foreach (var option in sortOptions)
    //    {
    //        if (option.Key == "ID")
    //        {
    //            if (option.Value == SortType.Ascending)
    //            {
    //                query = query.OrderBy(e => e.ID);
    //            }
    //            else
    //            {
    //                query = query.OrderByDescending(e => e.ID);
    //            }
    //        }
    //    }

    //    var result = query
    //        .Skip((page - 1) * rowsPerPage)
    //        .Take(rowsPerPage);

    //    return new Tuple<List<Product>, int>(
    //        result.ToList(),
    //        query.Count()
    //    );
    //}

    public async Task<Tuple<List<Product>, int>> GetProducts(
    int page, int rowsPerPage,
    string keyword,
    Dictionary<string, SortType> sortOptions
)
    {
        try
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(AppConstants.BaseUrl + "/product/search?") })
            {
           
                var sortField = sortOptions.Keys.FirstOrDefault() ?? "name";
                var sortType = sortOptions.Values.FirstOrDefault() == SortType.ASC ? "ASC" : "DESC";

                var queryParams = new Dictionary<string, string>
                {
                    { "page", page.ToString() },
                    { "size", rowsPerPage.ToString() },
                    { "sortField", sortField },
                    { "sortType", sortType }
                };

                var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                var requestUri = httpClient.BaseAddress + queryString;

                var productSearchRequest = new
                {
                    code = "",
                    name = keyword,
                    categoryName = "",
                    branchName = "",
                    sellingPriceFrom = (int?)null,
                    sellingPriceTo = (int?)null,
                    importPriceFrom = (int?)null,
                    importPriceTo = (int?)null,
                    inventoryQuantityFrom = (int?)null,
                    inventoryQuantityTo = (int?)null,
                    discountPercentFrom = (double?)null,
                    discountPercentTo = (double?)null,
                    businessStatus = true
                };

                var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(productSearchRequest), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri)
                {
                    Content = jsonContent
                };

                //var authService = new AuthService(httpClient);
                //var token = authService.GetAccessToken();
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var apiResponse = await httpClient.SendAsync(request);

                string responseContent = await apiResponse.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<ApiResponse<ProductApiResponse<List<Product>>>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                var products = responseData?.Data?.Data;

                if (products != null)
                {
                    return new Tuple<List<Product>, int>(products, products.Count);
                }

                return new Tuple<List<Product>, int>(new List<Product>(), 0);
            }
        }
        catch (Exception ex)
        {
            return new Tuple<List<Product>, int>(new List<Product>(), 0);
        }
    }

    public bool DeleteProduct(int id)
    {
        var path = GetJsonFilePath("products.json");

        var Products = new List<Product>();
        var json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        var item = Products.Find(e => e.Id == id);
        Products.Remove(item);

        var convertedJson = Newtonsoft.Json.JsonConvert.SerializeObject(Products);
        File.WriteAllText(path, convertedJson);
        return true;
    }


    //public (bool, string) AddProduct(Product info)
    //{
    //    var (isValid, message) = IsValidInfo(info);
    //    if (!isValid) return (false, message);

    //    var path = GetJsonFilePath("products.json");

    //    var Products = new List<Product>();
    //    var json = File.ReadAllText(path);
    //    Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

    //    Products.Add(info);
    //    var convertedJson = Newtonsoft.Json.JsonConvert.SerializeObject(Products);
    //    File.WriteAllText(path, convertedJson);

    //    return (true, string.Empty);
    //}

    public async Task<(bool, string)> AddProduct(Product info, Stream fileStream)
    {
        var (isValid, message) = IsValidInfo(info);
        if (!isValid) return (false, message);

        try
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(AppConstants.BaseUrl) })
            {
                var productCreationRequest = new
                {
                    data = new
                    {
                        name = info.Name,
                        categoryId = info.Category.Id,
                        importPrice = info.ImportPrice,
                        sellingPrice = info.SellingPrice,
                        branchName = info.Branch.Name,
                        inventoryQuantity = info.InventoryQuantity,
                        size = info.Size,
                        discountPercent = info.DiscountPercent
                    },
                    file = new StreamContent(fileStream)
                };

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(productCreationRequest.data), Encoding.UTF8, "application/json"), "data");
                formData.Add(productCreationRequest.file, "file", "product_image.jpg");

                //var authService = new AuthService(httpClient);
                //var token = authService.GetAccessToken();   
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var apiResponse = await httpClient.PostAsync("/product", formData);
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                if (apiResponse.IsSuccessStatusCode)
                {
                    return (true, string.Empty);
                }
                else
                {
                    return (false, apiResponse.ReasonPhrase);
                }
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public (bool, string) UpdateProduct(Product info)
    {
        var (isValid, message) = IsValidInfo(info);
        if (!isValid) return (false, message);

        var path = GetJsonFilePath("products.json");
        var Products = new List<Product>();
        var json = File.ReadAllText(path);
        Products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);

        var item = Products.Find(e => e.Id == info.Id);
        item.Name = info.Name;
        item.Category.Id = info.Category.Id;
        item.ImportPrice = info.ImportPrice;
        item.SellingPrice = info.SellingPrice;
        item.Branch.Id = info.Branch.Id;
        item.InventoryQuantity = info.InventoryQuantity;
        item.ImageUrl = info.ImageUrl;
        item.BusinessStatus = info.BusinessStatus;
        item.Size = info.Size;
        item.DiscountPercent = info.DiscountPercent;

        var convertedJson = Newtonsoft.Json.JsonConvert.SerializeObject(Products);
        File.WriteAllText(path, convertedJson);

        return (true, string.Empty);
    }
}