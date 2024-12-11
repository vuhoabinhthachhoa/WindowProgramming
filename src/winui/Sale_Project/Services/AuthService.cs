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
using Sale_Project.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Sale_Project.Core.Models.Accounts;
using Sale_Project.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Diagnostics;
using Sale_Project.Core.Models.Employees;

namespace Sale_Project.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IDialogService _dialogService;

    public AuthService(HttpClient httpClient, IHttpService httpService, IDialogService dialogService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/auth") };
        _httpService = httpService;
        _dialogService = dialogService;
    }

    /// <summary>
    /// Asynchronously handles the login process by sending a JSON request to the login API 
    /// with the provided username and password, and processes the response to retrieve
    /// and store authentication details locally.
    /// </summary>
    /// <param name="username">The username for login.</param>
    /// <param name="password">The password for login.</param>
    /// <returns>
    /// A <see cref="Task{bool}"/> representing the asynchronous operation. 
    /// Returns <c>true</c> if the login is successful; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Creates a JSON object containing the username and password.</item>
    /// <item>Sends the JSON data to the login API endpoint via an HTTP POST request.</item>
    /// <item>Validates the API response for success.</item>
    /// <item>If successful, deserializes the response to retrieve the access token.</item>
    /// <item>Stores the access token and user role in local settings for future use.</item>
    /// <item>Handles errors by showing an error dialog if the response data is null.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if the HTTP request fails to connect or an unexpected response is received.
    /// </exception>
    public async Task<bool> LoginAsync(string username, string password)
    {
        // Create an anonymous object with the username and password
        var loginData = new { username, password };

        // Serialize the login data to JSON
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send login request as JSON and get response
        var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress + "/login", content);

        // Check if the response indicates success
        if (!apiResponse.IsSuccessStatusCode)
        {
            return false;
        }

        // Read the response content as a string
        var responseContent = await apiResponse.Content.ReadAsStringAsync();

        // Deserialize the response content to the appropriate type
        var responseData = JsonSerializer.Deserialize<ApiResponse<Token>>(responseContent);
        System.Diagnostics.Debug.WriteLine($"Response Content: {responseContent}");

        if (responseData != null)
        {
            var token = responseData.Data;

            // Store the access token in local settings
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["AccessToken"] = token.Value;

            // Get the role from token and store it in local settings
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Value);

            // Extract the scope claim
            var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == "scope")?.Value;

            if (userRole != null)
            {
                // Store the scope in local settings
                localSettings.Values["UserRole"] = userRole;
            }

            await GetAccountAsync();

            return true;
        }
        else
        {
            // Show error dialog if response data is null
            await _dialogService.ShowErrorAsync("Error", "Error response data");
        }

        return false;
    }

    /// <summary>
    /// Asynchronously retrieves account details from the server using the access token 
    /// and processes the response to update local settings.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{Account}"/> representing the asynchronous operation. 
    /// Returns an <see cref="Account"/> object if the operation is successful, or <c>null</c> if an error occurs.
    /// </returns>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>Retrieves the access token and adds it to the HTTP request header.</item>
    /// <item>Sends a GET request to the server's base address.</item>
    /// <item>Checks the API response for success, and handles errors if necessary.</item>
    /// <item>Deserializes the response content into an <see cref="ApiResponse{T}"/> object containing account details.</item>
    /// <item>Stores the employee ID from the account data in local settings, if available.</item>
    /// <item>Catches and displays errors using a dialog service in case of HTTP or general exceptions.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if the HTTP request encounters issues such as connection failures.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown if any other unexpected error occurs.
    /// </exception>
    public async Task<Account> GetAccountAsync()
    {
        try
        {
            // Retrieve the access token and add it to the HTTP request header
            var token = GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Send a GET request to the server
            var apiResponse = await _httpClient.GetAsync(_httpClient.BaseAddress);

            // Check if the API response is successful
            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null!;
            }

            // Read and deserialize the response content
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<ApiResponse<Account>>(responseContent, options);

            // Update local settings with the employee ID
            var localSettings = ApplicationData.Current.LocalSettings;
            if (responseData!.Data.employee != null)
            {
                localSettings.Values["EmployeeId"] = responseData.Data.employee.Id.ToString();
            }

            return responseData.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null!;
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null!;
        }
    }

    /// <summary>
    /// Asynchronously sends a request to change the user's password by providing the old and new passwords.
    /// </summary>
    /// <param name="oldPassword">The current password of the user.</param>
    /// <param name="newPassword">The new password to be set for the user.</param>
    /// <returns>
    /// A <see cref="Task{bool}"/> representing the asynchronous operation. 
    /// Returns <c>true</c> if the password change is successful; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Constructs a JSON object with the old and new passwords.</item>
    /// <item>Sends a PATCH request to the server with the password data as JSON.</item>
    /// <item>Checks the response status to determine success or failure.</item>
    /// <item>Logs the response content or any errors encountered during the process.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if the HTTP request encounters issues such as connection failures.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown if any other unexpected error occurs.
    /// </exception>
    public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
    {
        try
        {
            // Construct the data object and serialize it to JSON
            var data = new { oldPassword, newPassword };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send a PATCH request to the change-password API endpoint
            var apiResponse = await _httpClient.PatchAsync(_httpClient.BaseAddress + "/change-password", content);

            // Check if the response indicates success
            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return false;
            }

            // Read and log the response content
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"Response: {responseContent}");
            return true;
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions
            System.Diagnostics.Debug.WriteLine($"HttpRequestException: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Asynchronously updates account information on the server and retrieves the updated account details.
    /// </summary>
    /// <param name="account">The <see cref="Account"/> object containing updated account information.</param>
    /// <returns>
    /// A <see cref="Task{Account}"/> representing the asynchronous operation. 
    /// Returns an updated <see cref="Account"/> object if the operation is successful, or <c>null</c> if an error occurs.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Serializes the provided <see cref="Account"/> object to JSON, including a custom <see cref="DateTimeConverter"/>.</item>
    /// <item>Adds an access token to the HTTP request header.</item>
    /// <item>Sends a PUT request to update the account on the server.</item>
    /// <item>Deserializes the server's response to retrieve the updated account details.</item>
    /// <item>Catches and logs exceptions, showing an error dialog when necessary.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if the HTTP request encounters issues such as connection failures.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown if any other unexpected error occurs.
    /// </exception>
    public async Task<Account> UpdateAccount(Account account)
    {
        try
        {
            // Configure serialization options with a custom DateTime converter
            var options = new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            };
            var json = JsonSerializer.Serialize(account, options);

            // Prepare the JSON content for the HTTP request
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Add the access token to the HTTP request header
            var token = GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Send a PUT request to update the account information
            var apiResponse = await _httpClient.PutAsync(_httpClient.BaseAddress, content);

            // Handle error responses
            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null!;
            }

            // Read and deserialize the response content
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Account>>(responseContent);

            return responseData?.Data;
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null!;
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            await _dialogService.ShowErrorAsync("Error", ex.Message);
            return null!;
        }
    }

    /// <summary>
    /// Logs the user out by clearing stored authentication and role information.
    /// </summary>
    /// <remarks>
    /// This method removes the `AccessToken` and `UserRole` values from the application's local settings.
    /// It ensures that sensitive information is cleared from the device storage upon logout.
    /// </remarks>
    public void Logout()
    {
        // Access LocalSettings
        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        // Remove the AccessToken and UserRole
        localSettings.Values.Remove("AccessToken");
        localSettings.Values.Remove("UserRole");

        // Optionally, you can log this action for debugging
        System.Diagnostics.Debug.WriteLine("AccessToken and UserRole removed from LocalSettings.");
    }

    /// <summary>
    /// Retrieves the stored access token from the application's local settings.
    /// </summary>
    /// <returns>
    /// The access token as a string if it exists, or <c>null</c> if no token is stored.
    /// </returns>
    public string GetAccessToken()
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values.TryGetValue("AccessToken", out var token))
        {
            return token as string;
        }

        return null;
    }

    /// <summary>
    /// Retrieves the user's role from the application's local settings.
    /// </summary>
    /// <returns>
    /// A <see cref="UserRole"/> enumeration value representing the user's role.
    /// Defaults to <see cref="UserRole.USER"/> if no role is stored.
    /// </returns>
    public UserRole GetUserRole()
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values.TryGetValue("UserRole", out var role))
        {
            return Enum.Parse<UserRole>(role as string);
        }

        return UserRole.USER;
    }

    /// <summary>
    /// Registers a new user by sending a registration request to the server.
    /// </summary>
    /// <param name="registrationRequest">
    /// The <see cref="RegistrationRequest"/> object containing the user's registration details.
    /// </param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation.
    /// Returns <c>true</c> if the registration is successful, or <c>false</c> if an error occurs.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Serializes the registration data to JSON.</item>
    /// <item>Adds the stored access token to the HTTP request header for authorization.</item>
    /// <item>Sends a POST request to the registration endpoint.</item>
    /// <item>Handles error responses appropriately.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if the HTTP request encounters issues such as connection failures.
    /// </exception>
    public async Task<bool> Register(RegistrationRequest registrationRequest)
    {
        var json = JsonSerializer.Serialize(registrationRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var token = GetAccessToken();
        _httpService.AddTokenToHeader(token, _httpClient);

        // Send registration request as JSON and get response
        var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress + "/register", content);

        if (!apiResponse.IsSuccessStatusCode)
        {
            await _httpService.HandleErrorResponse(apiResponse);
            return false;
        }

        return true;
    }
}