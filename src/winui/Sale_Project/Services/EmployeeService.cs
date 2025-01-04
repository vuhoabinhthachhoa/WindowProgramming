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
using Sale_Project.Core.Models.Employees;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Helpers;

/// <summary>
/// Service class for managing employees.
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="httpService">The HTTP service.</param>
    /// <param name="authService">The authentication service.</param>
    /// <param name="dialogService">The dialog service.</param>
    public EmployeeService(HttpClient httpClient, IHttpService httpService, IAuthService authService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/employee") };
        _httpService = httpService;
        _authService = authService;
        _dialogService = dialogService;
    }

    /// <summary>
    /// Adds a new employee to the system.
    /// </summary>
    /// <param name="employeeCreationRequest">The employee creation request.</param>
    /// <returns>The created employee.</returns>
    public async Task<Employee> AddEmployee(EmployeeCreationRequest employeeCreationRequest)
    {
        try
        {
            var json = JsonSerializer.Serialize(employeeCreationRequest);
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<Employee>>(responseContent);

            return responseData.Data;
        }
        catch (HttpRequestException ex)
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
    /// Marks an employee as unemployed (inactive).
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <returns>True if the operation was successful; otherwise, false.</returns>
    public async Task<bool> UnemployedEmployee(long employeeId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}/status/unemployed?employeeId={employeeId}";
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
    /// Updates an existing employee's details.
    /// </summary>
    /// <param name="employee">The employee to update.</param>
    /// <returns>The updated employee.</returns>
    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            };
            var json = JsonSerializer.Serialize(employee, options);
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<Employee>>(responseContent);

            return responseData.Data;
        }
        catch (HttpRequestException ex)
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
    /// Gets all employees.
    /// </summary>
    /// <returns>A list of all employees.</returns>
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

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Employee>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
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
    /// Searches for employees based on the specified criteria.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The page size.</param>
    /// <param name="sortField">The field to sort by.</param>
    /// <param name="sortType">The sort type (ascending or descending).</param>
    /// <param name="employeeSearchRequest">The employee search request.</param>
    /// <returns>A page of employees matching the search criteria.</returns>
    public async Task<PageData<Employee>> SearchEmployees(int page, int size, string sortField, SortType sortType, EmployeeSearchRequest employeeSearchRequest)
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

            var requestBody = JsonSerializer.Serialize(employeeSearchRequest);
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
            var responseData = JsonSerializer.Deserialize<ApiResponse<PageData<Employee>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
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
    /// Gets an employee by their identifier.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <returns>The employee with the specified identifier.</returns>
    public async Task<Employee> GetEmployeeById(long employeeId)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}?employeeId={employeeId}";
            var apiResponse = await _httpClient.GetAsync(requestUrl);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Employee>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
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
    /// Retrieves the total invoices for employees within a specified date range by sending a request to the server.
    /// </summary>
    /// <param name="startDate">
    /// The start date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <param name="endDate">
    /// The end date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <returns>
    /// A <see cref="Task{List{EmployeeTotalInvoices}}"/> representing the asynchronous operation.
    /// Returns a list of <see cref="EmployeeTotalInvoices"/> objects containing the total invoices data for employees if successful; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Retrieves the access token from the authentication service.</item>
    /// <item>Appends the access token to the request header.</item>
    /// <item>Constructs the API request URL using the provided date range parameters.</item>
    /// <item>Sends the GET request to the server to fetch employee total invoices data.</item>
    /// <item>Handles the server's response, including error responses.</item>
    /// <item>Deserializes the response data into a list of <see cref="EmployeeTotalInvoices"/> objects.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if there are issues with the HTTP request, such as network problems.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown for general errors, including server-side issues or unexpected conditions.
    /// </exception>
    public async Task<List<EmployeeTotalInvoices>> GetEmployeeByTotalInvoice(string startDate, string endDate)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}/total-invoices?startDate={startDate}&endDate={endDate}";

            var apiResponse = await _httpClient.GetAsync(requestUrl);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<EmployeeTotalInvoices>>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null;
        }
    }
}

