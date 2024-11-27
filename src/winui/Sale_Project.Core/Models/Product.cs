using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class Product : INotifyPropertyChanged
{
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

    public int ID
    {
        get => id;
        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }
    }

    public string Code
    {
        get => code;
        set
        {
            if (code != value)
            {
                code = value;
                OnPropertyChanged(nameof(Code));
            }
        }
    }

    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public Category Category
    {
        get => category;
        set
        {
            if (category != value)
            {
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
    }

    public float ImportPrice
    {
        get => importPrice;
        set
        {
            if (importPrice != value)
            {
                importPrice = value;
                OnPropertyChanged(nameof(ImportPrice));
            }
        }
    }

    public float SellingPrice
    {
        get => sellingPrice;
        set
        {
            if (sellingPrice != value)
            {
                sellingPrice = value;
                OnPropertyChanged(nameof(SellingPrice));
            }
        }
    }

    public Branch Branch
    {
        get => branch;
        set
        {
            if (branch != value)
            {
                branch = value;
                OnPropertyChanged(nameof(Branch));
            }
        }
    }

    public int InventoryQuantity
    {
        get => inventoryQuantity;
        set
        {
            if (inventoryQuantity != value)
            {
                inventoryQuantity = value;
                OnPropertyChanged(nameof(InventoryQuantity));
            }
        }
    }

    public string ImageUrl
    {
        get => imageUrl;
        set
        {
            if (imageUrl != value)
            {
                imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }
    }

    public string CloudinaryImageId
    {
        get => cloudinaryImageId;
        set
        {
            if (cloudinaryImageId != value)
            {
                cloudinaryImageId = value;
                OnPropertyChanged(nameof(CloudinaryImageId));
            }
        }
    }

    public bool BusinessStatus
    {
        get => businessStatus;
        set
        {

            if (businessStatus != value)
            {
                businessStatus = value;
                OnPropertyChanged(nameof(BusinessStatus));
            }
        }
    }

    public string Size
    {
        get => size;
        set
        {
            if (size != value)
            {
                size = value;
                OnPropertyChanged(nameof(Size));
            }
        }
    }

    public double DiscountPercent
    {
        get => discountPercent;
        set
        {
            if (discountPercent != value)
            {
                discountPercent = value;
                OnPropertyChanged(nameof(DiscountPercent));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
