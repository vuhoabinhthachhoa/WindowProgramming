using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Sale_Project.Core.Models;
public class RegistrationRequest : INotifyPropertyChanged
{
    private UserRole role;
    private string username;
    private string password;
    private long employeeId;
    private string? notes;

    [JsonPropertyName("role")]
    public UserRole Role
    {
        get => role;
        set
        {
            if (role != value)
            {
                role = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("username")]
    public string Username
    {
        get => username;
        set
        {
            if (username != value)
            {
                username = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("password")]
    public string Password
    {
        get => password;
        set
        {
            if (password != value)
            {
                password = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("employeeId")]
    public long EmployeeId
    {
        get => employeeId;
        set
        {
            if (employeeId != value)
            {
                employeeId = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("notes")]
    public string? Notes
    {
        get => notes;
        set
        {
            if (notes != value)
            {
                notes = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
