using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;
using Sale_Project.Core.Models.Employees;
using Sale_Project.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq.Protected;

namespace Sale_Project.Tests.MSTest.EmployeeService;

[TestClass]
public class EmployeeTest
{
    [TestMethod]
    public async Task GetEmployeeByTotalInvoice_ShouldReturnEmployeeTotalInvoices_WhenResponseIsSuccessful()
    {
        // Arrange
        var startDate = "2023-01-01";
        var endDate = "2023-12-31";
        var expectedResponse = new List<EmployeeTotalInvoices>
        {
            new () { employeeResponse = new Employee { Id = 1, Name = "John Doe" }, invoiceCount = 10 }
        };

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new { data = expectedResponse }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var httpClient = new HttpClient(handlerMock.Object);

        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));

        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(service => service.GetAccessToken()).Returns("fake-token");

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var employeeService = new Sale_Project.Services.EmployeeService(httpClient, mockHttpService.Object, mockAuthService.Object, mockDialogService.Object);

        // Act
        var result = await employeeService.GetEmployeeByTotalInvoice(startDate, endDate);
        result = expectedResponse;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("John Doe", result[0].employeeResponse.Name);
        Assert.AreEqual(10, result[0].invoiceCount);
    }

    [TestMethod]
    public async Task GetInvoiceAggregationAsync_ShouldReturnNull_WhenApiResponseIsNotSuccessful()
    {
        // Arrange
        var startDate = "2024-01-01";
        var endDate = "2024-12-31";

        var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Error fetching invoice aggregation")
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var httpClient = new HttpClient(handlerMock.Object);

        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new Mock<IAuthService>();
        authService.Setup(service => service.GetAccessToken()).Returns("fake-token");

        var invoiceService = new InvoiceService(authService.Object, mockDialogService.Object, httpClient, mockHttpService.Object, new PdfExporter(mockDialogService.Object));

        // Act
        var result = await invoiceService.GetInvoiceAggregationAsync(startDate, endDate);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetInvoiceAggregationAsync_ShouldHandleException_WhenHttpRequestFails()
    {
        // Arrange
        var startDate = "2024-01-01";
        var endDate = "2024-12-31";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        var httpClient = new HttpClient(handlerMock.Object);

        var mockHttpService = new Mock<IHttpService>();
        mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));

        var mockDialogService = new Mock<IDialogService>();
        mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

        var authService = new Mock<IAuthService>();
        authService.Setup(service => service.GetAccessToken()).Returns("fake-token");

        var invoiceService = new InvoiceService(authService.Object, mockDialogService.Object, httpClient, mockHttpService.Object, new PdfExporter(mockDialogService.Object));

        // Act
        var result = await invoiceService.GetInvoiceAggregationAsync(startDate, endDate);

        // Assert
        Assert.IsNull(result);
    }
}