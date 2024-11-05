using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models;

public class Voucher
{
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiredDate { get; set; } = DateTime.Now;
    public double Discount { get; set; } = 0;
    public double MaxDiscount { get; set; } = 0;
    public string Status { get; set; } = string.Empty;
}
