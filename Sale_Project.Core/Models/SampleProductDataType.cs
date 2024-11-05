using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;
public class SampleProductDataType
{
    public string Product_Name { get; set; } = string.Empty;
    public string Product_id { get; set; } = string.Empty;
    public int Product_Amount { get; set; } = 0;
    public int Product_Price { get; set; } = 0;
    public double Product_discount { get; set; } = 0;
}
