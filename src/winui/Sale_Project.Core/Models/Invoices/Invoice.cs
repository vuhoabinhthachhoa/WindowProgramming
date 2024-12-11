using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models.Products;
using Sale_Project.Core.Models.Employees;
using System.Text.Json.Serialization;

namespace Sale_Project.Core.Models.Invoices;

public class Invoice
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private long id = 0;
    private Employee employee = new();
    private DateTime createdDate = DateTime.Now;
    private decimal totalAmount = 0.0m;
    private decimal realAmount = 0.0m;
    private List<InvoiceDetail> invoiceDetails = new();

    [JsonPropertyName("id")]
    public long Id
    {
        get; set;
    }

    [JsonPropertyName("employee")]
    public Employee Employee
    {
        get; set;
    }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate
    {
        get; set;
    }

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount
    {
        get; set;
    }

    [JsonPropertyName("realAmount")]
    public decimal RealAmount
    {
        get; set;
    }

    [JsonPropertyName("invoiceDetails")]
    public List<InvoiceDetail> InvoiceDetails
    {
        get; set;
    }
}

public class InvoiceDetail
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Product product = new();
    private decimal importPrice = 0.0m;
    private decimal sellingPrice = 0.0m;
    private decimal discountPercent = 0;
    private int quantity = 0;

    [JsonPropertyName("product")]
    public Product Product
    {
        get; set;
    }

    [JsonPropertyName("importPrice")]
    public decimal ImportPrice
    {
        get; set;
    }

    [JsonPropertyName("sellingPrice")]
    public decimal SellingPrice
    {
        get; set;
    }

    [JsonPropertyName("discountPercent")]
    public decimal DiscountPercent
    {
        get; set;
    }

    [JsonPropertyName("quantity")]
    public int Quantity
    {
        get; set;
    }
}
