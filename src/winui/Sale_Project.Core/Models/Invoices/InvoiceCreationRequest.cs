using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models.Invoices;

public class InvoiceCreationRequest
{
    [JsonPropertyName("employeeId")]
    public long employeeId
    {
        get; set;
    }

    [JsonPropertyName("totalAmount")]
    public decimal totalAmount
    {
        get; set;
    }

    [JsonPropertyName("realAmount")]
    public decimal realAmount
    {
        get; set;
    }

    [JsonPropertyName("productQuantity")]
    public Dictionary<string, int> productQuantity
    {
        get; set;
    }
}
