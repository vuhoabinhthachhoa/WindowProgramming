using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class Product
{
    public string Id
    {
        get; set;
    }

    public string Code
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    public string Category_id
    {
        get; set;
    }

    public int Import_price
    {
        get; set;
    }

    public int Selling_price
    {
        get; set;
    }

    public string Branch_id
    {
        get; set;
    }

    public int Inventory_quantity
    {
        get; set;
    }

    public string Images
    {
        get; set;
    }

    public string Business_status
    {
        get; set;
    }

    public string Size
    {
        get; set;
    }

    public double Discount_percent
    {
        get; set;
    }
}
