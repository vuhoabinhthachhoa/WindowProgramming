using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class ApiResponse<T>
{
    [JsonPropertyName("code")]
    public int Code
    {
        get; set;
    }

    [JsonPropertyName("data")]
    public T Data
    {
        get; set;
    }
}
