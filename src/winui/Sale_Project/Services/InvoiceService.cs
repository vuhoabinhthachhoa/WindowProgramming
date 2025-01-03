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
using Sale_Project.Core.Models.Employees;
using Sale_Project.Core.Models.Products;
using Sale_Project.Helpers;

namespace Sale_Project.Services;
public class InvoiceService : IInvoiceService
{
    private readonly IDialogService _dialogService;
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly PdfExporter _pdfExporter;

    public InvoiceService(IAuthService authService, IDialogService dialogService, HttpClient httpClient, IHttpService httpService, PdfExporter pdfExporter)
    {
        _httpClient = httpClient;
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/invoice") };
        _dialogService = dialogService;
        _httpService = httpService;
        _authService = authService;
        _pdfExporter = pdfExporter;
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
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            // Send POST request
            var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, data);

            // Handle unsuccessful response
            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            // Deserialize server response
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Invoice>>(responseContent);

            // Notify success
            await _dialogService.ShowSuccessAsync("Success", "Invoice created successfully!");
            GenerateInvoicePdf(responseData?.Data);
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

    public void GenerateInvoicePdf(Invoice invoice)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Invoice.pdf");
       
        _pdfExporter.ExportInvoiceToPdf(invoice, filePath);

        // Optionally inform the user that the PDF has been created
        _dialogService.ShowSuccessAsync("Sucess", "Invoice exported successfully!");
    }

    /// <summary>
    /// Retrieves the invoice aggregation data for a specified date range by sending a request to the server.
    /// </summary>
    /// <param name="startDate">
    /// The start date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <param name="endDate">
    /// The end date of the date range in "yyyy-MM-dd" format.
    /// </param>
    /// <returns>
    /// A <see cref="Task{InvoiceAggregation}"/> representing the asynchronous operation.
    /// Returns the <see cref="InvoiceAggregation"/> object containing aggregated data if successful; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item>Retrieves the access token from the authentication service.</item>
    /// <item>Appends the access token to the request header.</item>
    /// <item>Constructs the API request URL using the provided date range parameters.</item>
    /// <item>Sends the GET request to the server to fetch invoice aggregation data.</item>
    /// <item>Handles the server's response, including error responses.</item>
    /// <item>Deserializes the response data into an <see cref="InvoiceAggregation"/> object.</item>
    /// </list>
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Thrown if there are issues with the HTTP request, such as network problems.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown for general errors, including server-side issues or unexpected conditions.
    /// </exception>
    public async Task<InvoiceAggregation> GetInvoiceAggregationAsync(string startDate, string endDate)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var requestUrl = $"{_httpClient.BaseAddress}/aggregation?startDate={startDate}&endDate={endDate}";

            var apiResponse = await _httpClient.GetAsync(requestUrl);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<InvoiceAggregation>>(responseContent);

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
