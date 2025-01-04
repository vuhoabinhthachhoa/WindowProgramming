using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Sale_Project.Core.Models.Brands;
using Sale_Project.Core.Models.Categories;

namespace Sale_Project.Core.Models.Products;

/// <summary>
/// Represents a product with properties for product details and notifications for property changes.
/// </summary>
public class TimeRange : INotifyPropertyChanged
{
    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Helper method to raise the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private DateOnly startDate;
    private DateOnly endDate;

    public DateOnly StartDate
    {
        get
        {
            return startDate;
        }
        set
        {
            if (startDate != value)
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
    }

    public DateOnly EndDate
    {
        get
        {
            return endDate;
        }
        set
        {
            if (endDate != value)
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }

    public TimeRange()
    {
        StartDate = new DateOnly(2024, 01, 01);
        EndDate = DateOnly.FromDateTime(DateTime.Now);
    }
 
    public override string ToString()
    {
        return string.Empty;
    }
}
