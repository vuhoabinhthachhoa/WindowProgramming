using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Contracts.Services;
using Windows.Storage;
using System.Diagnostics;

namespace Sale_Project.Services;
public class InvoiceService : IInvoiceService
{
    private readonly IDialogService _dialogService;
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;

    public InvoiceService(IAuthService authService, IDialogService dialogService, HttpClient httpClient, IHttpService httpService)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/invoice") };
        _dialogService = dialogService;
        _httpService = httpService;
    }

    /// <summary>
    /// Creates an invoice by sending an invoice creation request to the server.
    /// </summary>
    /// <param name="invoiceCreationRequest">
    /// The <see cref="InvoiceCreationRequest"/> object containing details required for creating the invoice.
    /// </param>
    /// <returns>
    /// A <see cref="Task{Invoice}"/> representing the asynchronous operation.
    /// Returns the created <see cref="Invoice"/> object if successful; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Assigns the employee ID from local settings to the request object.</item>
    /// <item>Serializes the request object to JSON and sends it in a POST request.</item>
    /// <item>Handles the server response, including errors.</item>
    /// <item>Notifies the user of the operation's success or failure.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if there are issues with the HTTP request.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown for general errors, including server-side issues.
    /// </exception>
    public async Task<Invoice> CreateInvoiceAsync(InvoiceCreationRequest invoiceCreationRequest)
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Retrieve and assign employee ID from local settings
            invoiceCreationRequest.employeeId = Convert.ToInt64(localSettings.Values["EmployeeId"]);

            // Serialize request data to JSON
            var json = JsonSerializer.Serialize(invoiceCreationRequest);
            Debug.WriteLine("Request JSON: " + json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Add access token to HTTP headers
            var token = GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Send POST request
            var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, data);

            // Handle unsuccessful response
            if (!apiResponse.IsSuccessStatusCode)
            {
                var error = await apiResponse.Content.ReadAsStringAsync();
                Debug.WriteLine("Server error: " + error);
                throw new Exception(error);
            }

            // Deserialize server response
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Invoice>>(responseContent);

            // Notify success
            await _dialogService.ShowSuccessAsync("Success", "Invoice created successfully!");
            return responseData?.Data;
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

    /// <summary>
    /// Retrieves the stored access token from the application's local settings.
    /// </summary>
    /// <returns>
    /// The access token as a string if it exists; otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    public string GetAccessToken()
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        // Retrieve the AccessToken from local settings
        if (localSettings.Values.TryGetValue("AccessToken", out var token))
        {
            return token as string;
        }

        return null;
    }
}
