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
    Task<Invoice> CreateInvoiceAsync(InvoiceCreationRequest invoiceCreationRequest);
    Task<IEnumerable<Invoice>> GetAllInvoices(DateOnly startDate, DateOnly endDate);
    Task GenerateInvoicesCsv(DateOnly startDate, DateOnly endDate);
}

