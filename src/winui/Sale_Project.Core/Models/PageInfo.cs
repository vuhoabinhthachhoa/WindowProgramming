using System.ComponentModel;

namespace Sale_Project.Core.Models;

public class PageInfo : INotifyPropertyChanged
{
    public int Page
    {
        get; set;
    }
    public int Total
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}