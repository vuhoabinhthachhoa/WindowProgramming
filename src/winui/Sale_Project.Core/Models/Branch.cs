﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;

public class Branch : INotifyPropertyChanged
{
    private int id = 0;
    private string name = "";
    private bool businessStatus = true;

    [JsonPropertyName("id")]
    public int ID
    {
        get => id;
        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }
    }

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

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

