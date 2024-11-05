using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Contracts.Services;

// IDao.cs
public interface IUserDao
{
    Task<List<Sale_Project.Core.Models.User>> GetUsersAsync();
}
