using System.ComponentModel;

namespace Sale_Project.Core.Models;

public class TopSale : INotifyPropertyChanged
{
    public string Name
    {
        get; set;
    }
    public string Address
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;


}
