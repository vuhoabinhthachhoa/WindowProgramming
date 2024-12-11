using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;

namespace Sale_Project.Core.Models.Product;

/// <summary>
/// Represents a request to create a new product, including its data and an optional file.
/// </summary>
public class ProductCreationRequest
{
    /// <summary>
    /// The product data to be used for creating a new product.
    /// </summary>
    [JsonPropertyName("data")]
    public ProductData Data { get; set; } = new ProductData();

    /// <summary>
    /// The file associated with the product, such as an image.
    /// </summary>
    [JsonPropertyName("file")]
    public StreamContent File
    {
        get; set;
    }
}

/// <summary>
/// Represents the detailed data for a product, such as name, category, pricing, and more.
/// </summary>
public class ProductData
{
    /// <summary>
    /// The name of the product.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// The ID of the product's category.
    /// </summary>
    public string CategoryId { get; set; } = "";

    /// <summary>
    /// The import price of the product.
    /// </summary>
    public double ImportPrice { get; set; } = 0;

    /// <summary>
    /// The selling price of the product.
    /// </summary>
    public double SellingPrice { get; set; } = 0;

    /// <summary>
    /// The brand name associated with the product.
    /// </summary>
    public string BrandName { get; set; } = "";

    /// <summary>
    /// The inventory quantity available for the product.
    /// </summary>
    public int InventoryQuantity
    {
        get; set;
    }

    /// <summary>
    /// The size of the product.
    /// </summary>
    public string Size
    {
        get; set;
    }

    /// <summary>
    /// The discount percentage for the product.
    /// </summary>
    public double DiscountPercent
    {
        get; set;
    }
}
