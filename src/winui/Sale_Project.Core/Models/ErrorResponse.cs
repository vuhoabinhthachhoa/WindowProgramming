using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace Sale_Project.Core.Models;

public class ErrorResponse
{
    [JsonPropertyName("code")]
    public int Code
    {
        get; set;
    }

    [JsonPropertyName("message")]
    public string Message
    {
        get; set;
    }
}
