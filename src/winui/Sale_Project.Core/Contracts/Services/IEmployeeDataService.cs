using Sale_Project.Core.Models;

namespace Sale_Project.Core.Contracts.Services;
public interface IEmployeeDataService
{
    Task<IEnumerable<Employee>> LoadDataAsync();
}