using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sale_Project.Core.Models.Employees;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Helpers;

namespace Sale_Project.Tests.MSTest.FileExporters;

[TestClass]
public class CsvExporterTests
{

    [TestMethod]
    public void ExportInvoicesToCsv_EmptyInvoices_ThrowsArgumentException()
    {
        // Arrange
        var invoices = new List<Invoice>();
        var filePath = Path.Combine(Path.GetTempPath(), "invoices.csv");

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => CsvExporter.ExportInvoicesToCsv(invoices, filePath));
        Assert.AreEqual("The invoices list is empty. (Parameter 'invoices')", ex.Message);
    }

    [TestMethod]
    public void ExportInvoicesToCsv_NullInvoices_ThrowsArgumentException()
    {
        // Arrange
        List<Invoice> invoices = null;
        var filePath = Path.Combine(Path.GetTempPath(), "invoices.csv");

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => CsvExporter.ExportInvoicesToCsv(invoices, filePath));
        Assert.AreEqual("The invoices list is empty. (Parameter 'invoices')", ex.Message);
    }
}
