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

/// <summary>
/// Provides services related to managing invoices, including creation, PDF export, and CSV export.
/// </summary>
public class InvoiceService : IInvoiceService
{
    private readonly IDialogService _dialogService;
    private readonly HttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IAuthService _authService;
    private readonly PdfExporter _pdfExporter;

    /// <summary>
    /// Constructs the InvoiceService with necessary services and initializers.
    /// </summary>
    public InvoiceService(IAuthService authService, IDialogService dialogService, HttpClient httpClient, IHttpService httpService, PdfExporter pdfExporter)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/invoice") };
        _dialogService = dialogService;
        _httpService = httpService;
        _authService = authService;
        _pdfExporter = pdfExporter;
    }

    /// <summary>
    /// Asynchronously creates an invoice by posting it to the server and handles the response.
    /// </summary>
    /// <param name="invoiceCreationRequest">Details needed to create the invoice.</param>
    /// <returns>The created invoice or null if the operation fails.</returns>
    public async Task<Invoice> CreateInvoiceAsync(InvoiceCreationRequest invoiceCreationRequest)
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Assigns the employee ID from the local settings to the invoice request.
            invoiceCreationRequest.employeeId = Convert.ToInt64(localSettings.Values["EmployeeId"]);

            var json = JsonSerializer.Serialize(invoiceCreationRequest);
            Debug.WriteLine("Request JSON: " + json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, data);

            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<Invoice>>(responseContent);

            await _dialogService.ShowSuccessAsync("Success", "Invoice created successfully!");
            GenerateInvoicePdf(responseData?.Data);

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

    /// <summary>
    /// Exports the provided invoice to a PDF file.
    /// </summary>
    /// <param name="invoice">The invoice to export.</param>
    public void GenerateInvoicePdf(Invoice invoice)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Invoice.pdf");
        _pdfExporter.ExportInvoiceToPdf(invoice, filePath);
        _dialogService.ShowSuccessAsync("Sucess", "Invoice exported to pdf successfully!");
    }

    /// <summary>
    /// Asynchronously exports a range of invoices to a CSV file.
    /// </summary>
    public async Task GenerateInvoicesCsv(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var requestUrl = $"{_httpClient.BaseAddress}/all?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.GetAsync(requestUrl);
            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Invoice>>>(responseContent);

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Invoices.csv");
            CsvExporter.ExportInvoicesToCsv(responseData?.Data, filePath);
            await _dialogService.ShowSuccessAsync("Success", "Invoices exported to csv successfully!");
        }
        catch (HttpRequestException ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all invoices within the specified date range.
    /// </summary>
    public async Task<IEnumerable<Invoice>> GetAllInvoices(DateOnly startDate, DateOnly endDate)
    {
        try
        {
            var requestUrl = $"{_httpClient.BaseAddress}/all?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            var token = _authService.GetAccessToken();
            _httpService.AddTokenToHeader(token, _httpClient);

            var apiResponse = await _httpClient.GetAsync(requestUrl);
            if (!apiResponse.IsSuccessStatusCode)
            {
                await _httpService.HandleErrorResponse(apiResponse);
                return null;
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ApiResponse<List<Invoice>>>(responseContent);

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
