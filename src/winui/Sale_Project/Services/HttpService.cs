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
public class HttpService : IHttpService
{
    private readonly IDialogService _dialogService;

    public HttpService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public void AddTokenToHeader(string token, HttpClient httpClient)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Token cannot be null or empty", nameof(token));
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    //public async Task<T> ParseHttpResponse<T>(HttpResponseMessage response)
    //{
    //    var responseContent = await response.Content.ReadAsStringAsync();
    //    return JsonSerializer.Deserialize<T>(responseContent);
    //}

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

    public async Task HandleErrorResponse(HttpResponseMessage response)
    {
        var errorMessage = await GetErrorMessageAsync(response);

        _dialogService.ShowErrorAsync("Error", errorMessage);
    }
}
