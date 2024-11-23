using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models.Employee;
public class EmployeeCreationRequest
{
    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber
    {
        get; set;
    }

    [JsonPropertyName("citizenId")]
    public string CitizenId
    {
        get; set;
    }

    [JsonPropertyName("jobTitle")]
    public string JobTitle
    {
        get; set;
    }

    [JsonPropertyName("salary")]
    public decimal Salary
    {
        get; set;
    }

    [JsonPropertyName("email")]
    public string Email
    {
        get; set;
    }

    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth
    {
        get; set;
    }

    [JsonPropertyName("address")]
    public string Address
    {
        get; set;
    }

    [JsonPropertyName("area")]
    public string Area
    {
        get; set;
    }

    [JsonPropertyName("ward")]
    public string Ward
    {
        get; set;
    }
}
