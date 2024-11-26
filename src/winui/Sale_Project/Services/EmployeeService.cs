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
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employee;
using Sale_Project.Helpers;

public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    public EmployeeService(HttpClient httpClient, IHttpService httpService, IAuthService authService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/employee") };
        _httpService = httpService;
        _authService = authService;
        _dialogService = dialogService;
    }
    // Add a new employee to the system
    public async Task<Employee> AddEmployee(EmployeeCreationRequest employeeCreationRequest)
    {
        try
        {
            var json = JsonSerializer.Serialize(employeeCreationRequest);
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<Employee>>(responseContent);

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

    // Mark an employee as unemployed (inactive)
    public async Task<bool> UnemployedEmployee(long employeeId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Prepare the request URL
            var requestUrl = $"{_httpClient.BaseAddress}/status/unemployed?employeeId={employeeId}";

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

    // Update an existing employee's details
    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        try
        {
            // remove the time and timezone from the dateOfBirth
            var options = new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            };
            var json = JsonSerializer.Serialize(employee, options);


            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Send login request as JSON and get response
            var apiResponse = await _httpClient.PutAsync(_httpClient.BaseAddress, content);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<Employee>>(responseContent);

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

    public async Task<IEnumerable<Employee>> GetAllEmployees()
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Employee>>>(responseContent);

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

    public async Task<PageData<Employee>> SearchEmployees(int page, int size, string sortField, SortType sortType, EmployeeSearchRequest employeeSearchRequest)
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

            // Serialize the EmployeeSearchRequest object into JSON
            var requestBody = JsonSerializer.Serialize(employeeSearchRequest);

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
            var responseData = JsonSerializer.Deserialize<ApiResponse<PageData<Employee>>>(responseContent);

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

    public async Task<Employee> GetEmployeeById(long employeeId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Append the employeeId to the request URL
            var requestUrl = $"{_httpClient.BaseAddress}?employeeId={employeeId}";

            var apiResponse = await _httpClient.GetAsync(requestUrl);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Read the response content as a string
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            // Deserialize the response content to the appropriate type
            var responseData = JsonSerializer.Deserialize<ApiResponse<Employee>>(responseContent);

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

