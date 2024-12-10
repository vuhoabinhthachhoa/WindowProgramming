using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.ViewModels;
using System.Text.Json;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.Tests.MSTest.ViewModels;

[TestClass]
public class SaleViewModelTest
{
    [TestMethod]
    public void LoadData_ShouldLoadCustomersCorrectly()
    {
        // Arrange
        var mockInvoiceService = new Mock<IInvoiceService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockProductService = new Mock<IProductService>();
        var viewModel = new SaleViewModel(mockInvoiceService.Object, mockDialogService.Object, mockProductService.Object);

        // Act
        viewModel.LoadData();

        // Assert
        Assert.IsNotNull(viewModel.customers);
    }

    [TestMethod]
    public async Task CreateInvoiceAsync_ShouldCreateInvoiceSuccessfully()
    {
        // Arrange
        var mockInvoiceService = new Mock<IInvoiceService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockProductService = new Mock<IProductService>();

        var viewModel = new SaleViewModel(mockInvoiceService.Object, mockDialogService.Object, mockProductService.Object);

        var invoiceRequest = new InvoiceCreationRequest { employeeId = 5, totalAmount = 100 };
        var expectedInvoice = new Invoice { Id = 4, RealAmount = 50, TotalAmount = 100 };

        mockInvoiceService
            .Setup(service => service.CreateInvoiceAsync(invoiceRequest))
            .ReturnsAsync(expectedInvoice);

        // Act
        await viewModel.CreateInvoiceAsync(invoiceRequest);

        // Assert
        Assert.IsNotNull(viewModel.Invoice);
        Assert.AreEqual(expectedInvoice.Id, viewModel.Invoice.Id);
        Assert.AreEqual(expectedInvoice.RealAmount, viewModel.Invoice.RealAmount);
    }

    [TestMethod]
    public async Task SearchProductByName_ShouldReturnCorrectProduct()
    {
        // Arrange
        var mockInvoiceService = new Mock<IInvoiceService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockProductService = new Mock<IProductService>();

        var viewModel = new SaleViewModel(mockInvoiceService.Object, mockDialogService.Object, mockProductService.Object);

        var productName = "Laptop";
        var expectedProduct = new Product { Id = 1, Name = "Laptop", SellingPrice = 1000 };

        mockProductService
            .Setup(service => service.GetProductByName(productName))
            .ReturnsAsync(expectedProduct);

        // Act
        await viewModel.SearchProductByName(productName);

        // Assert
        Assert.IsNotNull(viewModel.Product);
        Assert.AreEqual(expectedProduct.Id, viewModel.Product.Id);
        Assert.AreEqual(expectedProduct.Name, viewModel.Product.Name);
    }

    [TestMethod]
    public void GetJsonFilePath_ShouldReturnCorrectPath()
    {
        // Arrange
        var mockInvoiceService = new Mock<IInvoiceService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockProductService = new Mock<IProductService>();

        var viewModel = new SaleViewModel(mockInvoiceService.Object, mockDialogService.Object, mockProductService.Object);

        var fileName = "Customers.json";

        // Act
        var filePath = viewModel.GetJsonFilePath(fileName);

        // Assert
        Assert.IsTrue(filePath.Contains("Sale_Project\\MockData"));
        Assert.IsTrue(filePath.EndsWith(fileName));
    }
}
