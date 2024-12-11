using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Contracts.Services;
using System.Text.Json;
using System.Net.Http.Headers;
using Sale_Project.Core.Models;


namespace Sale_Project.Services;
/// <summary>
/// Service for handling HTTP requests and responses.
/// </summary>
public class HttpService : IHttpService
{
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpService"/> class.
    /// </summary>
    /// <param name="dialogService">The dialog service to show error messages.</param>
    public HttpService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Adds a Bearer token to the HTTP client's authorization header.
    /// </summary>
    /// <param name="token">The token to add.</param>
    /// <param name="httpClient">The HTTP client to which the token will be added.</param>
    /// <exception cref="ArgumentException">Thrown when the token is null or empty.</exception>
    public void AddTokenToHeader(string token, HttpClient httpClient)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Token cannot be null or empty", nameof(token));
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Gets the error message from the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the error message.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the response is null.</exception>
    public async Task<string> GetErrorMessageAsync(HttpResponseMessage response)
    {
        if (response == null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);

        if (errorResponse != null)
        {
            return errorResponse.Message;
        }

        return "Unknown error occurred.";
    }

    /// <summary>
    /// Handles the error response by showing an error dialog.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleErrorResponse(HttpResponseMessage response)
    {
        var errorMessage = await GetErrorMessageAsync(response);

        await _dialogService.ShowErrorAsync("Error", errorMessage);
    }
}
