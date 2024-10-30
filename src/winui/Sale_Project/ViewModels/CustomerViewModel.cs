using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Contracts.ViewModels;
using Sale_Project.Core.Contracts.Services;
using Sale_Project.Core.Models;

namespace Sale_Project.ViewModels;

public partial class CustomerViewModel : ObservableRecipient, INavigationAware, INotifyPropertyChanged
{
    private readonly ICustomerDataService _productDataService;

    public ObservableCollection<Customer> Source { get; } = new ObservableCollection<Customer>();

    public CustomerViewModel(ICustomerDataService productDataService)
    {
        _productDataService = productDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();
        var data = await _productDataService.LoadDataAsync();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }


    public void OnNavigatedFrom()
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;
}