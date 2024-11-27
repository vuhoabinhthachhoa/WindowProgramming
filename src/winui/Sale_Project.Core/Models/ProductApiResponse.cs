using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class ProductApiResponse<T>
{
    [JsonPropertyName("data")]
    public T Data
    {
        get; set;
    }

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


}
