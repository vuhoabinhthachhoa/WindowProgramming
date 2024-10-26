using Sale_Project.Core.Models;

namespace Sale_Project.Core.Contracts.Services;
public interface IProductDataService
{
    Task<IEnumerable<Product>> LoadDataAsync();
}
