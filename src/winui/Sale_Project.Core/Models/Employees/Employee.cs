using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Sale_Project.Core.Models.Employees;

public class Employee : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // Helper method to raise the PropertyChanged event
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private long _id;
    private string _name;
    private string? _phoneNumber;
    private string _citizenId;
    private string _jobTitle;
    private double _salary;
    private string? _email;
    private DateTimeOffset? _dateOfBirth;
    private string? _address;
    private string? _area;
    private string? _ward;
    private bool _employmentStatus;

    [JsonPropertyName("id")]
    public long Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (_phoneNumber != value)
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("citizenId")]
    public string CitizenId
    {
        get => _citizenId;
        set
        {
            if (_citizenId != value)
            {
                _citizenId = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("jobTitle")]
    public string? JobTitle
    {
        get => _jobTitle;
        set
        {
            if (_jobTitle != value)
            {
                _jobTitle = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("salary")]
    public double Salary
    {
        get => _salary;
        set
        {
            if (_salary != value)
            {
                _salary = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("email")]
    public string? Email
    {
        get => _email;
        set
        {
            var newValue = string.IsNullOrEmpty(value) ? null : value;
            if (_email != newValue)
            {
                _email = newValue;
                OnPropertyChanged();
            }
        }
    }

// Thinh Dep Trai

    [JsonPropertyName("dateOfBirth")]
    public DateTimeOffset? DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (_dateOfBirth != value)
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("address")]
    public string? Address
    {
        get => _address;
        set
        {
            if (_address != value)
            {
                _address = value ;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("area")]
    public string? Area
    {
        get => _area;
        set
        {
            if (_area != value)
            {
                _area = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("ward")]
    public string? Ward
    {
        get => _ward;
        set
        {
            if (_ward != value)
            {
                _ward = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("employmentStatus")]
    public bool EmploymentStatus
    {
        get => _employmentStatus;
        set
        {
            if (_employmentStatus != value)
            {
                _employmentStatus = value;
                OnPropertyChanged();
            }
        }
    }
}
