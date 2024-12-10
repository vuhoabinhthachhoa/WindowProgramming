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
using Sale_Project.Core.Helpers;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Products;
using Sale_Project.Helpers;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    public ProductService(HttpClient httpClient, IHttpService httpService, IAuthService authService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/product") };
        _httpService = httpService;
        _authService = authService;
        _dialogService = dialogService;
    }
    // Add a new product to the system
    public async Task<Product> AddProduct(ProductCreationRequest productCreationRequest)
    {
        try
        {
            //var json = JsonSerializer.Serialize(productCreationRequest);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            var creationRequest = new
            {
                data = new
                {
                    name = productCreationRequest.Data.Name,
                    categoryId = productCreationRequest.Data.CategoryId,
                    importPrice = productCreationRequest.Data.ImportPrice,
                    sellingPrice = productCreationRequest.Data.SellingPrice,
                    branchName = productCreationRequest.Data.BranchName,
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

            // Send login request as JSON and get response
            var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, formData);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<Product>>(responseContent);

            return responseData.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }

    }

    // Update an existing product's details
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

            // Send the PUT request with form data
            var apiResponse = await _httpClient.PutAsync(_httpClient.BaseAddress, formData);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<Product>>(responseContent);

            var responseProduct = responseData.Data;
            if (responseProduct.Branch.ID == 0)
                responseProduct.Branch = product.Branch;
            if (responseProduct.Category.ID == "")
                responseProduct.Category = product.Category;

            return responseProduct;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    // Mark an product as inactive 
    public async Task<bool> InactiveProduct(long productId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Prepare the request URL
            var requestUrl = $"{_httpClient.BaseAddress}/status/inactive?productId={productId}";

            // Send the PATCH request
            var apiResponse = await _httpClient.PatchAsync(requestUrl, null);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return false;
            }
            return true;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return false;
        }

    }

    // Mark an product as active 
    public async Task<bool> ActiveProduct(long productId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Prepare the request URL
            var requestUrl = $"{_httpClient.BaseAddress}/status/active?productId={productId}";

            // Send the PATCH request
            var apiResponse = await _httpClient.PatchAsync(requestUrl, null);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return false;
            }
            return true;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return false;
        }

    }

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

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Product>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }

    }

    public async Task<PageData<Product>> SearchProducts(int page, int size, string sortField, SortType sortType, ProductSearchRequest productSearchRequest)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Construct query parameters for pagination and sorting
            var queryParams = new Dictionary<string, string>
        {
            { "page", page.ToString() },
            { "size", size.ToString() },
            { "sortField", sortField },
            { "sortType", sortType.ToString() }
        };

            // Build the query string
            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

            // Prepare the HTTP request URL
            var requestUrl = $"{_httpClient.BaseAddress}/search?{queryString}";

            // Serialize the ProductSearchRequest object into JSON
            var requestBody = JsonSerializer.Serialize(productSearchRequest);

            // Manually create the GET request with a body
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            // Send the GET request
            var apiResponse = await _httpClient.SendAsync(httpRequestMessage);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read and deserialize the response content
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<PageData<Product>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }


    }

    //public async Task<Product> GetProductById(long productId)
    //{
    //    try
    //    {
    //        var token = _authService.GetAccessToken();
    //        _httpService.AddTokenToHeader(token, _httpClient);

    //        // Append the productId to the request URL
    //        var requestUrl = $"{_httpClient.BaseAddress}?productId={productId}";

    //        var apiResponse = await _httpClient.GetAsync(requestUrl);

    //        if (!apiResponse.IsSuccessStatusCode)
    //        {
    //            await _httpService.HandleErrorResponse(apiResponse);
    //            return null;
    //        }

    //        // Read the response content as a string
    //        var responseContent = await apiResponse.Content.ReadAsStringAsync();

    //        // Deserialize the response content to the appropriate type
    //        var responseData = JsonSerializer.Deserialize<ApiResponse<Product>>(responseContent);

    //        return responseData?.Data;
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        // Handle or log the exception as needed
    //        await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
    //        return null;
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle or log other exceptions as needed
    //        await _dialogService.ShowErrorAsync("Error", ex.Message);
    //        return null;
    //    }
    //}

    public async Task<Product?> GetSelectedProduct(ProductSearchRequest productSearchRequest)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Construct query parameters for pagination and sorting
            var queryParams = new Dictionary<string, string>
            {
            { "page", "1" },
            { "size", "1" },
            { "sortField", "name" },
            { "sortType", "ASC" }
        };

            //ProductSearchRequest productSearchRequest = new ProductSearchRequest
            //{
            //    Code = code,
            //    Name = name,
            //    CategoryName = categoryName,
            //    BranchName = branchName
            //};

            // Build the query string
            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

            // Prepare the HTTP request URL
            var requestUrl = $"{_httpClient.BaseAddress}/search?{queryString}";

            // Serialize the ProductSearchRequest object into JSON
            var requestBody = JsonSerializer.Serialize(productSearchRequest);

            // Manually create the GET request with a body
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            // Send the GET request
            var apiResponse = await _httpClient.SendAsync(httpRequestMessage);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read and deserialize the response content
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<PageData<Product>>>(responseContent);

            return responseData?.Data.Data[0];
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
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
    public async Task<Product> GetProductByName(string name)
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
            return responseData?.Data?.FirstOrDefault();
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions
            await _dialogService.ShowErrorAsync("Error", ex.Message);
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

