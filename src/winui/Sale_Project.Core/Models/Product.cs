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
    public int ID
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    public int CategoryID
    {
        get; set;
    }

    public int ImportPrice
    {
        get; set;
    }

    public int SellingPrice
    {
        get; set;
    }

    public string BranchID
    {
        get; set;
    }

    public int InventoryQuantity
    {
        get; set;
    }

    public string Images
    {
        get; set;
    } = "";

    public string BusinessStatus
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
    } = 0;

    public event PropertyChangedEventHandler PropertyChanged;
}
