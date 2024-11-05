using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;

namespace Sale_Project.Core.Models;

public class SampleCustomerDataType
{
    public string FirstName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    //public List<Voucher> Voucher { get; set; } = new ();
    public string Voucher { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;

    public SampleCustomerDataType()
    {
    }
}
