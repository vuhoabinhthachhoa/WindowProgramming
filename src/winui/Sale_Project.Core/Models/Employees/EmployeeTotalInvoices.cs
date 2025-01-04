using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models.Employees;

public class EmployeeTotalInvoices
{
    [JsonPropertyName("employeeResponse")]
    public Employee employeeResponse
    {
        get; set;
    }

    [JsonPropertyName("invoiceCount")]
    public int invoiceCount
    {
        get; set;
    }
}
