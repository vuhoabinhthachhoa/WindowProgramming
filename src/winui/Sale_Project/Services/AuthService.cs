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

namespace Sale_Project.Services;
public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private bool _isAuthenticated;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/auth") };
    }

    public bool IsAuthenticated => _isAuthenticated;

    public async Task<bool> LoginAsync(string username, string password)
    {
        var loginData = new { username, password };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send login request as JSON and get response
        var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress + "/login", content);
        
        if(!apiResponse.IsSuccessStatusCode)
        {
            return false;
        }
        
        // Read the response content as a string
        var responseContent = await apiResponse.Content.ReadAsStringAsync();

        // Deserialize the response content to the appropriate type
        var responseData = JsonSerializer.Deserialize<ApiResponse<Token>>(responseContent);
        if(responseData!= null)
        {
            var token = responseData.Data;
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["AccessToken"] = token.Value;

            // get the role from token and store to the local storage
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Value);

            // Extract the scope claim
            var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == "scope")?.Value;

            if (userRole != null)
            {
                // Store the scope in local settings
                localSettings.Values["UserRole"] = userRole;
            }
            return true;
        }
        return false;
        
    }


    //public async Task<HttpResponseMessage> SignUpAsync(UserModel user)
    //{
    //    var response = await _httpClient.PostAsJsonAsync("http://localhost:3000/users", user);
    //    return response;
    //}

    public void Logout()
    {
        _isAuthenticated = false;
    }

    public async Task FetchUserIdAsync()
    {
        try
        {
            // Get the access token from local storage
            var localSettings = ApplicationData.Current.LocalSettings;
            var accessToken = localSettings.Values["AccessToken"] as string;

            // Prepare the request message with authorization header if token is available
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/users/me");

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                // Retrieve user data from the response
                var userData = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

                if (userData != null && userData.TryGetValue("id", out var userId))
                {
                    // Check if userId is of type int and cast it safely
                    if (userId is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
                    {
                        int id = jsonElement.GetInt32(); // Get the integer value
                        localSettings.Values["UserId"] = id; // Store the integer userId
                    }
                    else if (userId is double doubleId) // In case the ID is a double
                    {
                        localSettings.Values["UserId"] = (int)doubleId; // Cast to int and store
                    }
                    else
                    {
                        Console.WriteLine("User ID is not in the expected format.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user ID: {ex.Message}");
        }
    }


    public string GetAccessToken()
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values.TryGetValue("AccessToken", out var token))
        {
            return token as string;
        }

        return null;
    }

    public UserRole GetUserRole()
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values.TryGetValue("UserRole", out var role))
        {
            return Enum.Parse<UserRole>(role as string);
        }

        return UserRole.USER;
    }

}
