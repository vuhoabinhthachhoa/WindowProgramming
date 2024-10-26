using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;

namespace Sale_Project.Core.Services;
public class ProductDataService : IProductDataService
{
    private List<Product> _allProducts;

    public ProductDataService()
    {
    }

    private static IEnumerable<Product> AllProducts()
    {
        return new List<Product>()
        {
            new Product()
            {
                // create new product
                Id = "1",
                Code = "SP001",
                Name = "Product 1",
                Category_id = "1",
                Import_price = 1000,
                Selling_price = 2000,
                Branch_id = "1",
                Inventory_quantity = 10,
                Images = "Image 1",
                Business_status = "Active",
                Size = "M",
                Discount_percent = 0.1
            },

            new Product()
            {
                // create new product
                Id = "2",
                Code = "SP002",
                Name = "Product 2",
                Category_id = "2",
                Import_price = 2000,
                Selling_price = 3000,
                Branch_id = "2",
                Inventory_quantity = 20,
                Images = "Image 2",
                Business_status = "Active",
                Size = "L",
                Discount_percent = 0.2
            },

            new Product()
            {
                Id = "3",
                Code = "SP003",
                Name = "Product 3",
                Category_id = "3",
                Import_price = 3000,
                Selling_price = 4000,
                Branch_id = "3",
                Inventory_quantity = 30,
                Images = "Image 3",
                Business_status = "Active",
                Size = "S",
                Discount_percent = 0.3

            }

        };
    }

    public async Task<IEnumerable<Product>> LoadDataAsync()
    {
        _allProducts ??= new List<Product>(AllProducts());

        await Task.CompletedTask;
        return _allProducts;
    }
}
