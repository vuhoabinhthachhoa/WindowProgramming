using System.ComponentModel;

namespace Sale_Project;

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

