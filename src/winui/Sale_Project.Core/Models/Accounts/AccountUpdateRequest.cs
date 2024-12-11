using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Sale_Project.Core.Models.Accounts;

public class AccountUpdateRequest
{
    [JsonPropertyName("PhoneNumber")]
    public string PhoneNumber
    {
        get; set;
    }

    [JsonPropertyName("Email")]
    public string Email
    {
        get; set;
    }

    [JsonPropertyName("DateOfBirth")]
    public DateTimeOffset DateOfBirth
    {
        get; set;
    }

    [JsonPropertyName("Address")]
    public string Address
    {
        get; set;
    }

    [JsonPropertyName("Area")]
    public string Area
    {
        get; set;
    }

    [JsonPropertyName("Ward")]
    public string Ward
    {
        get; set;
    }

    [JsonPropertyName("Notes")]
    public string Notes
    {
        get; set;
    }
}