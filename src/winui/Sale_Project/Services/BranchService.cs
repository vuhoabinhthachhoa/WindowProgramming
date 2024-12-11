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
using Sale_Project.Core.Models.Employee;
using Sale_Project.Core.Models.Product;
using Sale_Project.Helpers;

/// <summary>
/// Service for managing branch-related operations such as create, update, deactivate, and retrieve branches.
/// </summary>
public class BranchService : IBranchService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes the BranchService with dependencies.
    /// </summary>
    public BranchService(HttpClient httpClient, IHttpService httpService, IAuthService authService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/branch") };
        _httpService = httpService;
        _authService = authService;
        _dialogService = dialogService;
    }

    /// <summary>
    /// Creates a new branch in the system.
    /// </summary>
    public async Task<Branch> CreateBranch(Branch branch)
    {
        try
        {
            var json = JsonSerializer.Serialize(branch);
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<Branch>>(responseContent);

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
    /// Marks a branch as inactive.
    /// </summary>
    public async Task<bool> InactiveBranch(string branchName)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}/status/inactive?branchName={branchName}";
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
    /// Updates the details of an existing branch.
    /// </summary>
    public async Task<Branch> UpdateBranch(Branch branch, string newBranchName)
    {
        try
        {
            var updateRequest = new
            {
                name = branch.Name,
                newName = newBranchName
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<Branch>>(responseContent);

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
    /// Retrieves all branches from the system.
    /// </summary>
    public async Task<IEnumerable<Branch>> GetAllBranches()
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Branch>>>(responseContent);

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
