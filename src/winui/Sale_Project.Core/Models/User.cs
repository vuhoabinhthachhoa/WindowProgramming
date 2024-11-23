using System;
using System.Text.Json.Serialization;
using Sale_Project.Core.Models;

public class User
{
    [JsonPropertyName("id")]
    public long Id
    {
        get; set;
    }

    [JsonPropertyName("username")]
    public string Username
    {
        get; set;
    }

    [JsonPropertyName("password")]
    public string Password
    {
        get; set;
    }

    [JsonPropertyName("role_name")]
    public string RoleName
    {
        get; set;
    }

    [JsonPropertyName("employee")]
    public Employee Employee
    {
        get; set;
    }

    [JsonPropertyName("phonenumber")]
    public string PhoneNumber
    {
        get; set;
    }

    [JsonPropertyName("email")]
    public string Email
    {
        get; set;
    }

    [JsonPropertyName("date_of_birth")]
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

    [JsonPropertyName("role")]
    public UserRole Role
    {
        get; set;
    }

    [JsonPropertyName("notes")]
    public string Notes
    {
        get; set;
    }
}
