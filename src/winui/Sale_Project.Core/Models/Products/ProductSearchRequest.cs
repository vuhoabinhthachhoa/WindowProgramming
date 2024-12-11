using System;
using System.Collections.Generic;
using System.ComponentModel; // Required for INotifyPropertyChanged
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace Sale_Project.Core.Models.Product;

/// <summary>
/// Represents a request to search for products with various filter criteria.
/// </summary>
public class ProductSearchRequest : INotifyPropertyChanged
{
    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Helper method to raise the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string _code;

    /// <summary>
    /// Gets or sets the product code for search.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code
    {
        get => _code;
        set
        {
            if (_code != value)
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }
    }

    private string _name;

    /// <summary>
    /// Gets or sets the product name for search.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    private string _categoryName;

    /// <summary>
    /// Gets or sets the category name for search.
    /// </summary>
    [JsonPropertyName("categoryName")]
    public string CategoryName
    {
        get => _categoryName;
        set
        {
            if (_categoryName != value)
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
    }

    private string _branchName;

    /// <summary>
    /// Gets or sets the brand name for search.
    /// </summary>
    [JsonPropertyName("branchName")]
    public string BranchName
    {
        get => _branchName;
        set
        {
            if (_branchName != value)
            {
                _branchName = value;
                OnPropertyChanged(nameof(BranchName));
            }
        }
    }

    private double? _sellingPriceFrom;

    /// <summary>
    /// Gets or sets the minimum selling price for search.
    /// </summary>
    [JsonPropertyName("sellingPriceFrom")]
    public double? SellingPriceFrom
    {
        get => _sellingPriceFrom;
        set
        {
            if (_sellingPriceFrom != value)
            {
                _sellingPriceFrom = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(SellingPriceFrom));
            }
        }
    }

    private double? _sellingPriceTo;

    /// <summary>
    /// Gets or sets the maximum selling price for search.
    /// </summary>
    [JsonPropertyName("sellingPriceTo")]
    public double? SellingPriceTo
    {
        get => _sellingPriceTo;
        set
        {
            if (_sellingPriceTo != value)
            {
                _sellingPriceTo = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(SellingPriceTo));
            }
        }
    }

    private double? _importPriceFrom;

    /// <summary>
    /// Gets or sets the minimum import price for search.
    /// </summary>
    [JsonPropertyName("importPriceFrom")]
    public double? ImportPriceFrom
    {
        get => _importPriceFrom;
        set
        {
            if (_importPriceFrom != value)
            {
                _importPriceFrom = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(ImportPriceFrom));
            }
        }
    }

    private double? _importPriceTo;

    /// <summary>
    /// Gets or sets the maximum import price for search.
    /// </summary>
    [JsonPropertyName("importPriceTo")]
    public double? ImportPriceTo
    {
        get => _importPriceTo;
        set
        {
            if (_importPriceTo != value)
            {
                _importPriceTo = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(ImportPriceTo));
            }
        }
    }

    private double? _inventoryQuantityFrom;

    /// <summary>
    /// Gets or sets the minimum inventory quantity for search.
    /// </summary>
    [JsonPropertyName("inventoryQuantityFrom")]
    public double? InventoryQuantityFrom
    {
        get => _inventoryQuantityFrom;
        set
        {
            if (_inventoryQuantityFrom != value)
            {
                _inventoryQuantityFrom = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(InventoryQuantityFrom));
            }
        }
    }

    private double? _inventoryQuantityTo;

    /// <summary>
    /// Gets or sets the maximum inventory quantity for search.
    /// </summary>
    [JsonPropertyName("inventoryQuantityTo")]
    public double? InventoryQuantityTo
    {
        get => _inventoryQuantityTo;
        set
        {
            if (_inventoryQuantityTo != value)
            {
                _inventoryQuantityTo = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(InventoryQuantityTo));
            }
        }
    }

    private double? _discountPercentFrom;

    /// <summary>
    /// Gets or sets the minimum discount percentage for search.
    /// </summary>
    [JsonPropertyName("discountPercentFrom")]
    public double? DiscountPercentFrom
    {
        get => _discountPercentFrom;
        set
        {
            if (_discountPercentFrom != value)
            {
                _discountPercentFrom = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(DiscountPercentFrom));
            }
        }
    }

    private double? _discountPercentTo;

    /// <summary>
    /// Gets or sets the maximum discount percentage for search.
    /// </summary>
    [JsonPropertyName("discountPercentTo")]
    public double? DiscountPercentTo
    {
        get => _discountPercentTo;
        set
        {
            if (_discountPercentTo != value)
            {
                _discountPercentTo = value.HasValue && double.IsNaN(value.Value) ? null : value;
                OnPropertyChanged(nameof(DiscountPercentTo));
            }
        }
    }

    private bool _businessStatus = true;

    /// <summary>
    /// Gets or sets the business status for search.
    /// </summary>
    [JsonPropertyName("businessStatus")]
    public bool BusinessStatus
    {
        get => _businessStatus;
        set
        {
            if (_businessStatus != value)
            {
                _businessStatus = value;
                OnPropertyChanged(nameof(BusinessStatus));
            }
        }
    }
}
