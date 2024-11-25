using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
using System.Text.Json.Serialization;

public class PageData<T>
{
    [JsonPropertyName("page")]
    public int Page
    {
        get; set;
    }

    [JsonPropertyName("size")]
    public int Size
    {
        get; set;
    }

    [JsonPropertyName("totalElements")]
    public int TotalElements
    {
        get; set;
    }

    [JsonPropertyName("totalPages")]
    public int TotalPages
    {
        get; set;
    }

    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new List<T>();
}

