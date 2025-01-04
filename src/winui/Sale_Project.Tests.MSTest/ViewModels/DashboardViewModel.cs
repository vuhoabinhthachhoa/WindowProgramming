using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Employees;
using Sale_Project.ViewModels;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Core.Models.Products;

namespace Sale_Project.Tests.MSTest.ViewModels;
[TestClass]
public class DashboardViewModelTests
{
    [TestMethod]
    public async Task GetEmployeeByTotalInvoiceAsync_ShouldUpdateEmployeeTotalInvoicesAndSortedEmployees_WhenSuccess()
    {
        // Arrange
        var mockEmployeeService = new Mock<IEmployeeService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockProductService = new Mock<IProductService>();
        var mockInvoiceService = new Mock<IInvoiceService>();

        var employeeTotalInvoices = new List<EmployeeTotalInvoices>
    {
        new () { employeeResponse = new Employee { Name = "Employee1" }, invoiceCount = 10 },
        new () { employeeResponse = new Employee { Name = "Employee2" }, invoiceCount = 5 }
    };

        mockEmployeeService
            .Setup(service => service.GetEmployeeByTotalInvoice(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeTotalInvoices);

        var viewModel = new DashboardViewModel(mockProductService.Object, mockDialogService.Object, mockInvoiceService.Object, mockEmployeeService.Object);

        // Act
        await viewModel.GetEmployeeByTotalInvoiceAsync("2023-01-01", "2023-12-31");

        // Assert
        Assert.AreEqual(employeeTotalInvoices, viewModel.EmployeeTotalInvoices);
        Assert.AreEqual(2, viewModel.SortedEmployees.Count);
        Assert.AreEqual("Employee1", viewModel.SortedEmployees[0].Name);
        Assert.AreEqual("Employee2", viewModel.SortedEmployees[1].Name);
    }

    [TestMethod]
    public async Task GetInvoiceAggregationAsync_ShouldUpdateAggregation_WhenSuccess()
    {
        // Arrange
        var mockInvoiceService = new Mock<IInvoiceService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockProductService = new Mock<IProductService>();
        var mockEmployeeService = new Mock<IEmployeeService>();

        var expectedAggregation = new InvoiceAggregation
        {
            startDate = DateTimeOffset.Parse("2023-01-01"),
            endDate = DateTimeOffset.Parse("2023-12-31"),
            totalAmount = 1000,
            totalRealAmount = 900,
            totalDiscountAmount = 100
        };

        mockInvoiceService
            .Setup(service => service.GetInvoiceAggregationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(expectedAggregation);

        var viewModel = new DashboardViewModel(mockProductService.Object, mockDialogService.Object, mockInvoiceService.Object, mockEmployeeService.Object);

        // Act
        await viewModel.GetInvoiceAggregationAsync("2023-01-01", "2023-12-31");

        // Assert
        Assert.AreEqual(expectedAggregation, viewModel.Aggregation);
    }

    [TestMethod]
    public async Task GetProducts_ShouldUpdateProducts_WhenSuccess()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockDialogService = new Mock<IDialogService>();
        var mockInvoiceService = new Mock<IInvoiceService>();
        var mockEmployeeService = new Mock<IEmployeeService>();

        var expectedProducts = new List<Product>
    {
        new Product { Name = "Product1" },
        new Product { Name = "Product2" }
    };

        mockProductService
            .Setup(service => service.GetProductByName(It.IsAny<string>()))
            .ReturnsAsync(expectedProducts);

        var viewModel = new DashboardViewModel(mockProductService.Object, mockDialogService.Object, mockInvoiceService.Object, mockEmployeeService.Object);

        // Act
        await viewModel.GetProducts();

        // Assert
        Assert.AreEqual(expectedProducts, viewModel.Products);
    }

}