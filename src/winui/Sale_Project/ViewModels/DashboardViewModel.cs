using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Sale_Project.Core.Models;

namespace Sale_Project.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<TopSale> contacts;

    public DashboardViewModel()
    {
        contacts = new ObservableCollection<TopSale>
        {
            new () { Name = "John Doe", Address = "123 Example St" },
            new () { Name = "Jane Smith", Address = "456 Another Rd" },
            new () { Name = "Olivia Saphere", Address = "79 Saint Michael, Los Angeles" },
            new () { Name = "Hash Mille", Address = "S12 Baker Street, London" },
            new () { Name = "Alice Johnson", Address = "789 Elm St, Springfield" },
            new () { Name = "Bob Brown", Address = "101 Maple Ave, Metropolis" }
        };
    }
}
