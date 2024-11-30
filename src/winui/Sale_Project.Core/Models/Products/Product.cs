using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Sale_Project.Core.Models;

public class Product : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // Helper method to raise the PropertyChanged event
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private int id = 0;
    private string code = string.Empty;
    private string name = string.Empty;
    private Category category = new Category();
    private float importPrice = 0.0f;
    private float sellingPrice = 0.0f;
    private Branch branch = new Branch();
    private int inventoryQuantity = 0;
    private string imageUrl = string.Empty;
    private string cloudinaryImageId = string.Empty;
    private bool businessStatus = false;
    private string size = string.Empty;
    private double discountPercent = 0.0;

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

    [JsonPropertyName("importPrice")]
    public float ImportPrice
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

    [JsonPropertyName("sellingPrice")]
    public float SellingPrice
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

    [JsonPropertyName("branch")]
    public Branch Branch
    {
        get => branch;
        set
        {
            if (branch != value)
            {
                branch = value;
                OnPropertyChanged();
            }
        }
    }

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
}
