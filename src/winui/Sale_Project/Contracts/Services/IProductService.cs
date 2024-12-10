using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.Contracts.Services;
public interface IProductService
{
    // Add a new product to the system
    Task<Product> AddProduct(ProductCreationRequest product);

    // Mark an product 
    Task<bool> InactiveProduct(long productId);
    Task<bool> ActiveProduct(long productId);

    // Update an existing product's details
    Task<Product> UpdateProduct(Product product, StreamContent file);

    // Optionally, add a method to retrieve products
    //Task<Product> GetProductById(long productId);

    // Optionally, add a method to list all products
    Task<IEnumerable<Product>> GetAllProducts();

    Task<PageData<Product>> SearchProducts(int page, int size, string sortField, SortType sortType, ProductSearchRequest productSearchRequest);

    Task<Product?> GetSelectedProduct(ProductSearchRequest productSearchRequest);

    Task<Product> GetProductByName(string name);
}
