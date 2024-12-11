using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Sale_Project.Contracts.Services;

namespace Sale_Project.Services;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Products;
using Sale_Project.Helpers;

/// <summary>
/// Service for managing product-related operations such as add, update, deactivate, and search.
/// </summary>
public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Constructor to initialize dependencies for ProductService.
    /// </summary>
    public ProductService(HttpClient httpClient, IHttpService httpService, IAuthService authService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/product") };
        _httpService = httpService;
        _authService = authService;
        _dialogService = dialogService;
    }

    /// <summary>
    /// Adds a new product to the system.
    /// </summary>
    public async Task<Product> AddProduct(ProductCreationRequest productCreationRequest)
    {
        try
        {
            var creationRequest = new
            {
                data = new
                {
                    name = productCreationRequest.Data.Name,
                    categoryId = productCreationRequest.Data.CategoryId,
                    importPrice = productCreationRequest.Data.ImportPrice,
                    sellingPrice = productCreationRequest.Data.SellingPrice,
                    branchName = productCreationRequest.Data.BrandName,
                    inventoryQuantity = productCreationRequest.Data.InventoryQuantity,
                    size = productCreationRequest.Data.Size,
                    discountPercent = productCreationRequest.Data.DiscountPercent
                },
                file = productCreationRequest.File
            };

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(creationRequest.data), Encoding.UTF8, "application/json"), "data");
            formData.Add(creationRequest.file, "file", "product_image.jpg");

            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, formData);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Product>>(responseContent);

            return responseData.Data;
        }
        catch (HttpRequestException)
        {
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Updates details of an existing product.
    /// </summary>
    public async Task<Product> UpdateProduct(Product product, StreamContent file)
    {
        try
        {
            var updateRequest = new
            {
                data = new
                {
                    id = product.Id,
                    importPrice = product.ImportPrice,
                    sellingPrice = product.SellingPrice,
                    inventoryQuantity = product.InventoryQuantity,
                    discountPercent = product.DiscountPercent
                },
                file = file
            };

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(updateRequest.data), Encoding.UTF8, "application/json"), "data");
            formData.Add(updateRequest.file, "file", "product_image.jpg");

            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.PutAsync(_httpClient.BaseAddress, formData);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Product>>(responseContent);

            var responseProduct = responseData.Data;
            if (responseProduct.Brand.Id == 0)
                responseProduct.Brand = product.Brand;
            if (responseProduct.Category.Id == "")
                responseProduct.Category = product.Category;

            return responseProduct;
        }
        catch (HttpRequestException)
        {
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Marks a product as inactive.
    /// </summary>
    public async Task<bool> InactiveProduct(long productId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}/status/inactive?productId={productId}";
            var apiResponse = await _httpClient.PatchAsync(requestUrl, null);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return false;
            }
            return true;
        }
        catch (HttpRequestException)
        {
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return false;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Marks a product as active.
    /// </summary>
    public async Task<bool> ActiveProduct(long productId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}/status/active?productId={productId}";
            var apiResponse = await _httpClient.PatchAsync(requestUrl, null);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return false;
            }
            return true;
        }
        catch (HttpRequestException)
        {
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return false;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Retrieves all products from the system.
    /// </summary>
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);
            var apiResponse = await _httpClient.GetAsync(_httpClient.BaseAddress);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Product>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException)
        {
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Searches for products based on specified criteria.
    /// </summary>
    public async Task<PageData<Product>> SearchProducts(int page, int size, string sortField, SortType sortType, ProductSearchRequest productSearchRequest)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var queryParams = new Dictionary<string, string>
            {
                { "page", page.ToString() },
                { "size", size.ToString() },
                { "sortField", sortField },
                { "sortType", sortType.ToString() }
            };

            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var requestUrl = $"{_httpClient.BaseAddress}/search?{queryString}";
            var requestBody = JsonSerializer.Serialize(productSearchRequest);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            var apiResponse = await _httpClient.SendAsync(httpRequestMessage);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<PageData<Product>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException)
        {
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Retrieves the first product matching the specified criteria.
    /// </summary>
    public async Task<Product?> GetSelectedProduct(ProductSearchRequest productSearchRequest)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var queryParams = new Dictionary<string, string>
            {
                { "page", "1" },
                { "size", "1" },
                { "sortField", "name" },
                { "sortType", "ASC" }
            };

            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var requestUrl = $"{_httpClient.BaseAddress}/search?{queryString}";
            var requestBody = JsonSerializer.Serialize(productSearchRequest);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            var apiResponse = await _httpClient.SendAsync(httpRequestMessage);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<PageData<Product>>>(responseContent);

            return responseData?.Data.Data[0];
        }
        catch (HttpRequestException)
        {
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Retrieves a product by its name by sending a request to the server.
    /// </summary>
    /// <param name="name">The name of the product to search for.</param>
    /// <returns>
    /// A <see cref="Task{Product}"/> representing the asynchronous operation. 
    /// Returns the first matching <see cref="Product"/> if found; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Retrieves the access token and adds it to the HTTP headers.</item>
    /// <item>Sends a GET request with the product name as a query parameter.</item>
    /// <item>Handles server responses, including errors.</item>
    /// <item>Deserializes the server response into a list of products and returns the first one if available.</item>
    /// </list>
    /// </remarks>
    public async Task<List<Product>> GetProductByName(string name)
    {
        try
        {
            // Retrieve the access token
            var token = _authService.GetAccessToken();
            // Add the token to the HTTP headers
            _httpService.AddTokenToHeader(token, _httpClient);

            // Construct the request URL with the product name as a query parameter
            var requestUrl = $"{_httpClient.BaseAddress}/searchByName?name={name}";
            var apiResponse = await _httpClient.GetAsync(requestUrl);

            // Handle unsuccessful response
            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Configure JSON deserialization options
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Allows case-insensitive property matching
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignores null properties
            };

            // Deserialize the server response into a list of products
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Product>>>(responseContent, options);

            // Return the first matching product or null if no products found
            return responseData?.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }
}
