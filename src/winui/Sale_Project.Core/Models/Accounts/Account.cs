using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Sale_Project.Core.Models.Employees;

namespace Sale_Project.Core.Models.Accounts;

public class Account : INotifyPropertyChanged
{
    private long _id;
    private string _username;
    private Employee _employee;
    private string _phoneNumber;
    private string _email;
    private DateTimeOffset _dateOfBirth;
    private string _address;
    private string _area;
    private string _ward;
    private string _notes;
    private Role _role;

    [JsonPropertyName("id")]
    public long id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }
    }

    [JsonPropertyName("username")]
    public string username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged(nameof(username));
            }
        }
    }

    [JsonPropertyName("employee")]
    public Employee employee
    {
        get => _employee;
        set
        {
            if (_employee != value)
            {
                _employee = value;
                OnPropertyChanged(nameof(employee));
            }
        }
    }

    [JsonPropertyName("phoneNumber")]
    public string phoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (_phoneNumber != value)
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(phoneNumber));
            }
        }
    }

    [JsonPropertyName("email")]
    public string email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(email));
            }
        }
    }

    [JsonPropertyName("dateOfBirth")]
    public DateTimeOffset dateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (_dateOfBirth != value)
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(dateOfBirth));
            }
        }
    }

    [JsonPropertyName("address")]
    public string address
    {
        get => _address;
        set
        {
            if (_address != value)
            {
                _address = value;
                OnPropertyChanged(nameof(address));
            }
        }
    }

    [JsonPropertyName("area")]
    public string area
    {
        get => _area;
        set
        {
            if (_area != value)
            {
                _area = value;
                OnPropertyChanged(nameof(area));
            }
        }
    }

    [JsonPropertyName("ward")]
    public string ward
    {
        get => _ward;
        set
        {
            if (_ward != value)
            {
                _ward = value;
                OnPropertyChanged(nameof(ward));
            }
        }
    }

    [JsonPropertyName("notes")]
    public string notes
    {
        get => _notes;
        set
        {
            if (_notes != value)
            {
                _notes = value;
                OnPropertyChanged(nameof(notes));
            }
        }
    }

    [JsonPropertyName("role")]
    public Role role
    {
        get => _role;
        set
        {
            if (_role != value)
            {
                _role = value;
                OnPropertyChanged(nameof(role));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class Role
{
    public string name
    {
        get; set;
    }
    public string description
    {
        get; set;
    }
}