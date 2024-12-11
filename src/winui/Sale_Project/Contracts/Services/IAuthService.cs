using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Accounts;

namespace Sale_Project.Contracts.Services;
public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password);
    //Task<HttpResponseMessage> SignUpAsync(UserModel user);
    void Logout();

    Task<Account> GetAccountAsync();

    Task<bool> ChangePasswordAsync(string oldpassword, string newpassword);

    Task<Account> UpdateAccount(Account account);

    public string GetAccessToken();

    public UserRole GetUserRole();

    Task<bool> Register(RegistrationRequest registrationRequest);


}
