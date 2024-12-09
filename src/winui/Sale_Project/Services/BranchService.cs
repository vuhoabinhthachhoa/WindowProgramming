using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Sale_Project.Contracts.Services;

namespace Sale_Project.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employee;
using Sale_Project.Core.Models.Product;
using Sale_Project.Helpers;

public class BranchService : IBranchService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    public BranchService(HttpClient httpClient, IHttpService httpService, IAuthService authService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/branch") };
        _httpService = httpService;
        _authService = authService;
        _dialogService = dialogService;
    }
    // Add a new employee to the system
    public async Task<Branch> CreateBranch(Branch branch)
    {
        try
        {
            var json = JsonSerializer.Serialize(branch);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Send login request as JSON and get response
            var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, content);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<Branch>>(responseContent);

            return responseData.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    public async Task<bool> InactiveBranch(string branchName)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Prepare the request URL
            var requestUrl = $"{_httpClient.BaseAddress}/status/inactive?branchName={branchName}";

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
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return false;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return false;
        }
    }

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

            // Send the PUT request with form data
            var apiResponse = await _httpClient.PutAsync(_httpClient.BaseAddress, content);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<Branch>>(responseContent);

            return responseData.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<Branch>> GetAllBranches()
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);
            var apiResponse = await _httpClient.GetAsync(_httpClient.BaseAddress+"/all");

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Branch>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle or log the exception as needed
            await _dialogService.ShowErrorAsync("Error", "An error occurred while connecting to the server. Please check your internet connection and try again.");
            return null;
        }
        catch (Exception ex)
        {
            // Handle or log other exceptions as needed
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }
}

