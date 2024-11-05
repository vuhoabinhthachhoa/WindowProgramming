using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Contracts.Services;

public interface IDao
{
    Task<List<Sale_Project.Core.Models.User>> GetUsersAsync();
    Task SaveUsersAsync(List<User> users);
    Task RegisterUserAsync(User newUser);
}
