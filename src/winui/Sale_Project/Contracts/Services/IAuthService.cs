using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Contracts.Services;
public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password);
    //Task<HttpResponseMessage> SignUpAsync(UserModel user);
    bool IsAuthenticated
    {
        get;
    }
    void Logout();

    public string GetAccessToken();

    public UserRole GetUserRole();

}
