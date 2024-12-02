using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;

namespace Sale_Project.Core.Models.Product;


public class ProductCreationRequest
{
    [JsonPropertyName("data")]
    public ProductData Data { get; set; } = new ProductData();

    [JsonPropertyName("file")]
    public StreamContent File
    {
        get; set;
    }
}

public class ProductData
{
    public string Name { get; set; } = "";
    public string CategoryId { get; set; } = "";
    public double ImportPrice { get; set; } = 0;
    public double SellingPrice { get; set; } = 0;
    public string BranchName { get; set; } = "";
    public int InventoryQuantity
    {
        get; set;
    }
    public string Size
    {
        get; set;
    }
    public double DiscountPercent
    {
        get; set;
    }
}

