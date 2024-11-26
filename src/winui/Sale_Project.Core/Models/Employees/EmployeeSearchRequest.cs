using System;
using System.Collections.Generic;
using System.ComponentModel; // Required for INotifyPropertyChanged
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace Sale_Project.Core.Models.Employee;

public class EmployeeSearchRequest : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // Helper method to raise the PropertyChanged event
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // if we use long, int, or decimal, the numberbox cannot bind value
    private double? _id;
    [JsonPropertyName("id")]
    public double? Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _id = null;
                }
                else
                {
                    _id = value;
                }
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    private string _name = string.Empty;
    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    private string _phoneNumber = string.Empty;
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (_phoneNumber != value)
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
    }

    private string _citizenId = string.Empty;
    [JsonPropertyName("citizenId")]
    public string CitizenId
    {
        get => _citizenId;
        set
        {
            if (_citizenId != value)
            {
                _citizenId = value;
                OnPropertyChanged(nameof(CitizenId));
            }
        }
    }

    private string _jobTitle = string.Empty;
    [JsonPropertyName("jobTitile")] // set as jobTitile to match the server
    public string JobTitle 
    {
        get => _jobTitle;
        set
        {
            if (_jobTitle != value)
            {
                _jobTitle = value;
                OnPropertyChanged(nameof(JobTitle));
            }
        }
    }

    private double? _salaryFrom;
    [JsonPropertyName("salaryFrom")]
    public double? SalaryFrom
    {
        get => _salaryFrom;
        set
        {
            if (_salaryFrom != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _salaryFrom = null;
                }
                else
                {
                    _salaryFrom = value;
                }
                OnPropertyChanged(nameof(SalaryFrom));
                Debug.WriteLine($"SalaryFrom set to: {_salaryFrom}");
            }
        }
    }

    private double? _salaryTo;
    [JsonPropertyName("salaryTo")]
    public double? SalaryTo
    {
        get => _salaryTo;
        set
        {
            if (_salaryTo != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _salaryTo = null;
                }
                else
                {
                    _salaryTo = value;
                }
                OnPropertyChanged(nameof(SalaryTo));
            }
        }
    }

    private bool _employmentStatus = true;
    [JsonPropertyName("employmentStatus")]
    public bool EmploymentStatus
    {
        get => _employmentStatus;
        set
        {
            if (_employmentStatus != value)
            {
                _employmentStatus = value;
                OnPropertyChanged(nameof(EmploymentStatus));
            }
        }
    }
}
