using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;

namespace Sale_Project.ViewModels;

public partial class CustomerViewModel : ObservableRecipient, INavigationAware
{
    private readonly ICustomerDataService _customerDataService;

    public ObservableCollection<Customer> Source { get; } = new ObservableCollection<Customer>();

    public CustomerViewModel(ICustomerDataService customerDataService)
    {
        _customerDataService = customerDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();
        var data = await _customerDataService.LoadDataAsync();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}