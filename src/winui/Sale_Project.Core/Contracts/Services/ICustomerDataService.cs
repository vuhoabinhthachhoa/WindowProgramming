using System.ComponentModel;
using Sale_Project.Core.Models;

namespace Sale_Project.Core.Contracts.Services;
public interface ICustomerDataService : INotifyPropertyChanged
{
    Task<IEnumerable<Customer>> LoadDataAsync();


}