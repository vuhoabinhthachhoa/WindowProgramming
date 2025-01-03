using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models.Invoices;
public class InvoiceAggregation
{
    [JsonPropertyName("startDate")]
    public DateTimeOffset startDate
    {
        get; set;
    }

    [JsonPropertyName("endDate")]
    public DateTimeOffset endDate
    {
        get; set;
    }

    [JsonPropertyName("totalAmount")]
    public double totalAmount
    {
        get; set;
    }

    [JsonPropertyName("totalRealAmount")]
    public double totalRealAmount
    {
        get; set;
    }

    [JsonPropertyName("totalDiscountAmount")]
    public double totalDiscountAmount
    {
        get; set;
    }

    public double MaxValue => new[] { totalAmount, totalRealAmount, totalDiscountAmount }.Max();
}
