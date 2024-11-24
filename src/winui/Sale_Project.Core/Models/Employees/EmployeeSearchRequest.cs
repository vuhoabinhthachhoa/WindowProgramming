using System;
using System.Collections.Generic;
using System.ComponentModel; // Required for INotifyPropertyChanged
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Sale_Project.Core.Models.Employee;

public class EmployeeSearchRequest : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // Helper method to raise the PropertyChanged event
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private long? _id;
    [JsonPropertyName("id")]
    public long? Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
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
    [JsonPropertyName("jobTitle")]
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

    private decimal? _salaryFrom;
    [JsonPropertyName("salaryFrom")]
    public decimal? SalaryFrom
    {
        get => _salaryFrom;
        set
        {
            if (_salaryFrom != value)
            {
                _salaryFrom = value;
                OnPropertyChanged(nameof(SalaryFrom));
            }
        }
    }

    private decimal? _salaryTo;
    [JsonPropertyName("salaryTo")]
    public decimal? SalaryTo
    {
        get => _salaryTo;
        set
        {
            if (_salaryTo != value)
            {
                _salaryTo = value;
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
