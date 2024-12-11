﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;

/// <summary>
/// Represents a branch with properties for ID, name, and business status, along with property change notifications.
/// </summary>
public class Branch : INotifyPropertyChanged
{
    private int id = 0;
    private string name = "";
    private bool businessStatus = true;

    /// <summary>
    /// Gets or sets the ID of the branch.
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
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    /// <summary>
    /// Gets or sets the name of the branch.
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
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    /// <summary>
    /// Gets or sets the business status of the branch.
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
                OnPropertyChanged(nameof(BusinessStatus));
            }
        }
    }

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Helper method to raise the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
