using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models.Categories;

public class CategoryCreationRequest
{
    [JsonPropertyName("id")]
    public string Id
    {
        get; set;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }
}
