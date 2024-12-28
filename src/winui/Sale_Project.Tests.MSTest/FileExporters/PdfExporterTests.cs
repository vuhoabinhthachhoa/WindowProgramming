using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employees;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Core.Models.Products;
using Sale_Project.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sale_Project.Tests.MSTest.FileExporters;

[TestClass]
public class PdfExporterTests
{
    private Mock<IDialogService> _mockDialogService;
    private PdfExporter _pdfExporter;

    [TestInitialize]
    public void Setup()
    {
        _mockDialogService = new Mock<IDialogService>();
        _pdfExporter = new PdfExporter(_mockDialogService.Object);
    }

    [TestMethod]
    public void ExportInvoiceToPdf_ValidInvoice_CreatesPdf()
    {
        // Arrange
        var invoice = new Invoice
        {
            Id = 1,
            Employee = new Employee { Name = "John Doe" },
            CreatedDate = DateTime.Now,
            TotalAmount = 100.00m,
            RealAmount = 90.00m,
            InvoiceDetails = new List<InvoiceDetail>
            {
                new InvoiceDetail
                {
                    Product = new Product { Name = "T-Shirt", Size = "M", SellingPrice = 20.00, DiscountPercent = 10 },
                    Quantity = 2,
                    SellingPrice = 20.00M,
                    DiscountPercent = 10
                }
            }
        };
        string filePath = Path.Combine(Path.GetTempPath(), "test_invoice.pdf");

        // Act
        _pdfExporter.ExportInvoiceToPdf(invoice, filePath);

        // Assert
        Assert.IsTrue(File.Exists(filePath));
        File.Delete(filePath);
    }

    [TestMethod]
    public void ExportInvoiceToPdf_ExceptionThrown_ShowsErrorDialog()
    {
        // Arrange
        var invoice = new Invoice();
        string filePath = Path.Combine(Path.GetTempPath(), "test_invoice.pdf");

        // Act
        _pdfExporter.ExportInvoiceToPdf(invoice, filePath);

        // Assert
        _mockDialogService.Verify(ds => ds.ShowErrorAsync("Error", "An error occurred while exporting the invoice to PDF"), Times.Once);
    }
}
