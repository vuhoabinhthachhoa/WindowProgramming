using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Sale_Project.Core.Models;

namespace Sale_Project.Core.Models.Products;

/// <summary>
/// Represents a product with properties for product details and notifications for property changes.
/// </summary>
public class Product : INotifyPropertyChanged
{
    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Helper method to raise the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private int id = 0;
    private string code = string.Empty;
    private string name = string.Empty;
    private Category category = new Category();
    private double importPrice = 0;
    private double sellingPrice = 0;
    private Brand brand = new Brand();
    private int inventoryQuantity = 0;
    private string imageUrl = string.Empty;
    private string cloudinaryImageId = string.Empty;
    private bool businessStatus = false;
    private string size = string.Empty;
    private double discountPercent = 0.0;

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id
    {
        get => id;
        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the product code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code
    {
        get => code;
        set
        {
            if (code != value)
            {
                code = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the product category.
    /// </summary>
    [JsonPropertyName("category")]
    public Category Category
    {
        get => category;
        set
        {
            if (category != value)
            {
                category = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the import price of the product.
    /// </summary>
    [JsonPropertyName("importPrice")]
    public double ImportPrice
    {
        get => importPrice;
        set
        {
            if (importPrice != value)
            {
                importPrice = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selling price of the product.
    /// </summary>
    [JsonPropertyName("sellingPrice")]
    public double SellingPrice
    {
        get => sellingPrice;
        set
        {
            if (sellingPrice != value)
            {
                sellingPrice = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the brand associated with the product.
    /// </summary>
    [JsonPropertyName("branch")]
    public Brand Brand
    {
        get => brand;
        set
        {
            if (brand != value)
            {
                brand = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the inventory quantity of the product.
    /// </summary>
    [JsonPropertyName("inventoryQuantity")]
    public int InventoryQuantity
    {
        get => inventoryQuantity;
        set
        {
            if (inventoryQuantity != value)
            {
                inventoryQuantity = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the URL of the product's image.
    /// </summary>
    [JsonPropertyName("imageUrl")]
    public string ImageUrl
    {
        get => imageUrl;
        set
        {
            if (imageUrl != value)
            {
                imageUrl = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the Cloudinary image ID for the product.
    /// </summary>
    [JsonPropertyName("cloudinaryImageId")]
    public string CloudinaryImageId
    {
        get => cloudinaryImageId;
        set
        {
            if (cloudinaryImageId != value)
            {
                cloudinaryImageId = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the business status of the product.
    /// </summary>
    [JsonPropertyName("businessStatus")]
    public bool BusinessStatus
    {
        get => businessStatus;
        set
        {
            if (businessStatus != value)
            {
                businessStatus = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the size of the product.
    /// </summary>
    [JsonPropertyName("size")]
    public string Size
    {
        get => size;
        set
        {
            if (size != value)
            {
                size = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the discount percentage for the product.
    /// </summary>
    [JsonPropertyName("discountPercent")]
    public double DiscountPercent
    {
        get => discountPercent;
        set
        {
            if (discountPercent != value)
            {
                discountPercent = value;
                OnPropertyChanged();
            }
        }
    }

    public override string ToString()
    {
        return string.Empty;
    }
}
