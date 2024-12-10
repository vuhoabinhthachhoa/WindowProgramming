using System;
using System.Collections.Generic;
using System.ComponentModel; // Required for INotifyPropertyChanged
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace Sale_Project.Core.Models.Products;

public class ProductSearchRequest : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // Helper method to raise the PropertyChanged event
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string _code;
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
    [JsonPropertyName("sellingPriceFrom")]
    public double? SellingPriceFrom
    {
        get => _sellingPriceFrom;
        set
        {
            if (_sellingPriceFrom != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _sellingPriceFrom = null;
                }
                else
                {
                    _sellingPriceFrom = value;
                }
                OnPropertyChanged(nameof(SellingPriceFrom));
            }
        }
    }

    private double? _sellingPriceTo;
    [JsonPropertyName("sellingPriceTo")]
    public double? SellingPriceTo
    {
        get => _sellingPriceTo;
        set
        {
            if (_sellingPriceTo != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _sellingPriceTo = null;
                }
                else
                {
                    _sellingPriceTo = value;
                }
                OnPropertyChanged(nameof(SellingPriceTo));
            }
        }
    }

    private double? _importPriceFrom;
    [JsonPropertyName("importPriceFrom")]
    public double? ImportPriceFrom
    {
        get => _importPriceFrom;
        set
        {
            if (_importPriceFrom != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _importPriceFrom = null;
                }
                else
                {
                    _importPriceFrom = value;
                }
                OnPropertyChanged(nameof(ImportPriceFrom));
            }
        }
    }

    private double? _importPriceTo;
    [JsonPropertyName("importPriceTo")]
    public double? ImportPriceTo
    {
        get => _importPriceTo;
        set
        {
            if (_importPriceTo != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _importPriceTo = null;
                }
                else
                {
                    _importPriceTo = value;
                }
                OnPropertyChanged(nameof(ImportPriceTo));
            }
        }
    }

    private double? _inventoryQuantityFrom;
    [JsonPropertyName("inventoryQuantityFrom")]
    public double? InventoryQuantityFrom
    {
        get => _inventoryQuantityFrom;
        set
        {
            if (_inventoryQuantityFrom != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _inventoryQuantityFrom = null;
                }
                else
                {
                    _inventoryQuantityFrom = value;
                }
                OnPropertyChanged(nameof(InventoryQuantityFrom));
            }
        }
    }

    private double? _inventoryQuantityTo;
    [JsonPropertyName("inventoryQuantityTo")]
    public double? InventoryQuantityTo
    {
        get => _inventoryQuantityTo;
        set
        {
            if (_inventoryQuantityTo != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _inventoryQuantityTo = null;
                }
                else
                {
                    _inventoryQuantityTo = value;
                }
                OnPropertyChanged(nameof(InventoryQuantityTo));
            }
        }
    }

    private double? _discountPercentFrom;
    [JsonPropertyName("discountPercentFrom")]
    public double? DiscountPercentFrom
    {
        get => _discountPercentFrom;
        set
        {
            if (_discountPercentFrom != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _discountPercentFrom = null;
                }
                else
                {
                    _discountPercentFrom = value;
                }
                OnPropertyChanged(nameof(DiscountPercentFrom));
            }
        }
    }

    private double? _discountPercentTo;
    [JsonPropertyName("discountPercentTo")]
    public double? DiscountPercentTo
    {
        get => _discountPercentTo;
        set
        {
            if (_discountPercentTo != value)
            {
                if (value.HasValue && double.IsNaN(value.Value))
                {
                    _discountPercentTo = null;
                }
                else
                {
                    _discountPercentTo = value;
                }
                OnPropertyChanged(nameof(DiscountPercentTo));
            }
        }
    }

    private bool _businessStatus = true;
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
