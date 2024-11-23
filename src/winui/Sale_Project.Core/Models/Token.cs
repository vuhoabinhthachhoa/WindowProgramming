using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class Token
{
    [JsonPropertyName("token")]
    public string Value
    {
        get; set;
    }
}

