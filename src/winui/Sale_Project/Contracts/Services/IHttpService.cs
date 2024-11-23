using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Contracts.Services;
public interface IHttpService
{
    void AddTokenToHeader(string token, HttpClient httpClient);
    //public Task<T> ParseHttpResponse<T>(HttpResponseMessage response);
    public Task<string> GetErrorMessageAsync(HttpResponseMessage response);
    public Task HandleErrorResponse(HttpResponseMessage response);
}
