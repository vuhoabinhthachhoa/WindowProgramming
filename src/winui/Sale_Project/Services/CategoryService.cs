using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;

namespace Sale_Project.Services;

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Categories;
using Sale_Project.Helpers;

/// <summary>
/// Service for managing category-related operations such as create, update, deactivate, and retrieve categories.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes the CategoryService with dependencies.
    /// </summary>
    public CategoryService(HttpClient httpClient, IHttpService httpService, IAuthService authService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/category") };
        _httpService = httpService;
        _authService = authService;
        _dialogService = dialogService;
    }

    /// <summary>
    /// Creates a new category in the system.
    /// </summary>
    public async Task<Category> CreateCategory(CategoryCreationRequest categoryCreationRequest)
    {
        try
        {
            var json = JsonSerializer.Serialize(categoryCreationRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, content);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Category>>(responseContent);

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
    /// Marks a category as inactive.
    /// </summary>
    public async Task<bool> InactiveCategory(string categoryName)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}/status/inactive?categoryName={categoryName}";
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
    /// Updates an existing category's name.
    /// </summary>
    public async Task<Category> UpdateCategory(Category category, string newCategoryName)
    {
        try
        {
            var updateRequest = new
            {
                id = category.Id,
                name = newCategoryName,
            };

            var json = JsonSerializer.Serialize(updateRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.PutAsync(_httpClient.BaseAddress, content);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Category>>(responseContent);

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
    /// Retrieves all categories from the system.
    /// </summary>
    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + "/all");

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Category>>>(responseContent);

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
    /// Retrieves a category by its ID.
    /// </summary>
    public async Task<Category> GetCategoryById(string categoryId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}?categoryId={categoryId}";
            var apiResponse = await _httpClient.GetAsync(requestUrl);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Category>>(responseContent);

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
}
