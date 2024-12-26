using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sale_Project.Core.Models.Brands;

namespace Sale_Project.Core.Models.Brands;

/// <summary>
/// Represents a brand with properties for ID, name, and business status, along with property change notifications.
/// </summary>
public class Brand : INotifyPropertyChanged
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

    private int id = 0;
    private string name = string.Empty;
    private bool businessStatus = true;

    /// <summary>
    /// Gets or sets the ID of the brand.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id
    {
        get => id;
        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the name of the brand.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the business status of the brand.
    /// </summary>
    [JsonPropertyName("businessStatus")]
    public bool BusinessStatus
    {
        get => businessStatus;
        set
        {
            if (businessStatus != value)
            {
                businessStatus = value;
                OnPropertyChanged();
            }
        }
    }

    public override string ToString()
    {
        return string.Empty;
    }
}

