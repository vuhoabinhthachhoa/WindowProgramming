using System.IO;
using System.Reflection;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;
using System.Text.Json;
using System.ComponentModel;

namespace Sale_Project.Core.Services;
public class ProductDataService : IProductDataService, INotifyPropertyChanged
{
    private List<Product> _allProducts;

    public ProductDataService()
    {
    }

    public IEnumerable<Product> AllProducts()
    {
        //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Sale_Project.Core\MockData\products.json");
        string path = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        @"..\..\..\..\..\..\MockData\products.json");

        var result = new List<Product>();
        string json = System.IO.File.ReadAllText(path);
        result = JsonSerializer.Deserialize<List<Product>>(json);
        return result;
    }

    public async Task<IEnumerable<Product>> LoadDataAsync()
    {
        _allProducts ??= new List<Product>(AllProducts());

        await Task.CompletedTask;
        return _allProducts;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
