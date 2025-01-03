using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Invoices;

namespace Sale_Project.Contracts.Services;

public interface IInvoiceService
{
    //Add a new invoice to the system
    Task<Invoice> CreateInvoiceAsync(InvoiceCreationRequest invoiceCreationRequest);

    //Get an invoice by its ID
    Task<InvoiceAggregation> GetInvoiceAggregationAsync(string startDate, string endDate);
}
