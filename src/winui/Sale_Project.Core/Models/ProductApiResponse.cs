using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;

/// <summary>
/// Represents a generic API response containing paginated product data.
/// </summary>
/// <typeparam name="T">The type of the data included in the response.</typeparam>
public class ProductApiResponse<T>
{
    /// <summary>
    /// Gets or sets the data returned by the API.
    /// </summary>
    [JsonPropertyName("data")]
    public T Data
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the current page number of the response.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the size of each page in the response.
    /// </summary>
    [JsonPropertyName("size")]
    public int Size
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the total number of elements available.
    /// </summary>
    [JsonPropertyName("totalElements")]
    public int TotalElements
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the total number of pages available.
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages
    {
        get; set;
    }
}
