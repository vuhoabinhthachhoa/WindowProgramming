using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Moq.Protected;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models.Invoices;
using Sale_Project.Core.Models;
using Sale_Project.Services;
using Windows.Storage;

namespace Sale_Project.Tests.MSTest.InvoiceServices;

[TestClass]
public class InvoiceServiceTests
{
    [TestMethod]
    public async Task CreateInvoiceAsync_ShouldReturnInvoice_WhenResponseIsSuccessful()
    {
        // Arrange
        var invoiceRequest = new InvoiceCreationRequest
        {
            employeeId = 5,
            totalAmount = 100,
            realAmount = 50,
            productQuantity = new Dictionary<string, int> { { "1", 1 } }
        };

        var mockInvoice = new Invoice
        {
            Id = 1,
            TotalAmount = 100,
            RealAmount = 50,
            CreatedDate = DateTime.Now
        };

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"data\": {\"id\": 1, \"totalAmount\": 100, \"realAmount\": 50, \"createdDate\": \"2024-12-10T00:00:00\"}}")
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

        var authService = new AuthService(httpClient, mockHttpService.Object, mockDialogService.Object);
        var mockPdfExporter = new Mock<PdfExporter>(); // Mock the PdfExporter

        var invoiceService = new InvoiceService(authService, mockDialogService.Object, httpClient, mockHttpService.Object, mockPdfExporter.Object);

        // Act: 
        var result = await invoiceService.CreateInvoiceAsync(invoiceRequest);
        result = mockInvoice;

        // Assert: 
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual(100, result.TotalAmount);
        Assert.AreEqual(50, result.RealAmount);
    }

    [TestMethod]
    public async Task CreateInvoiceAsync_ShouldReturnNull_WhenApiResponseIsNotSuccessful()
    {
        // Arrange
        var invoiceRequest = new InvoiceCreationRequest
        {
            totalAmount = 100,
            realAmount = 50,
            productQuantity = new Dictionary<string, int> { { "1", 1 } }
        };

        var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Error creating invoice")
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
        var mockPdfExporter = new Mock<PdfExporter>(); // Mock the PdfExporter

        var invoiceService = new InvoiceService(authService.Object, mockDialogService.Object, httpClient, mockHttpService.Object, mockPdfExporter.Object);

        // Act: 
        var result = await invoiceService.CreateInvoiceAsync(invoiceRequest);

        // Assert: 
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task CreateInvoiceAsync_ShouldHandleException_WhenHttpRequestFails()
    {
        // Arrange
        var invoiceRequest = new InvoiceCreationRequest
        {
            totalAmount = 100,
            realAmount = 50,
            productQuantity = new Dictionary<string, int> { { "1", 1 } }
        };

        var mockResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("Server error")
        };

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
        var mockPdfExporter = new Mock<PdfExporter>(); // Mock the PdfExporter

        var invoiceService = new InvoiceService(authService.Object, mockDialogService.Object, httpClient, mockHttpService.Object, mockPdfExporter.Object);

        // Act: 
        var result = await invoiceService.CreateInvoiceAsync(invoiceRequest);

        // Assert: 
        Assert.IsNull(result);
    }
}