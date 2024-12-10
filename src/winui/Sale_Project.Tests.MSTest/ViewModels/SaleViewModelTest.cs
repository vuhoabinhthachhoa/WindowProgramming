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

    [TestMethod]
    public async Task GetProducts_ShouldLoadAllProducts()
    {
        // Arrange
        var mockInvoiceService = new Mock<IInvoiceService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockProductService = new Mock<IProductService>();

        var viewModel = new SaleViewModel(mockInvoiceService.Object, mockDialogService.Object, mockProductService.Object);

        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", SellingPrice = 100 },
            new Product { Id = 2, Name = "Product2", SellingPrice = 200 }
        };

        mockProductService
            .Setup(service => service.GetProductByName(" "))
            .ReturnsAsync(expectedProducts);

        // Act
        await viewModel.GetProducts();

        // Assert
        Assert.IsNotNull(viewModel.Products);
        Assert.AreEqual(expectedProducts.Count, viewModel.Products.Count);
        Assert.AreEqual(expectedProducts[0].Id, viewModel.Products[0].Id);
        Assert.AreEqual(expectedProducts[0].Name, viewModel.Products[0].Name);
        Assert.AreEqual(expectedProducts[0].SellingPrice, viewModel.Products[0].SellingPrice);
        Assert.AreEqual(expectedProducts[1].Id, viewModel.Products[1].Id);
        Assert.AreEqual(expectedProducts[1].Name, viewModel.Products[1].Name);
        Assert.AreEqual(expectedProducts[1].SellingPrice, viewModel.Products[1].SellingPrice);
    }
}
