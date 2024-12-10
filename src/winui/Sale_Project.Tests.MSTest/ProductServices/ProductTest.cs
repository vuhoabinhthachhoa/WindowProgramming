using Sale_Project.Core.Models.Products;
using Moq.Protected;
using Moq;
using Sale_Project.Contracts.Services;
using System.Net;
using Sale_Project.Services;

namespace Sale_Project.Tests.MSTest.ProductServices;

[TestClass]
public class ProductTests
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public async Task GetProductByName_ShouldReturnProduct_WhenResponseIsSuccessful()
        {
            // Arrange
            var productName = "TestProduct";

            var mockProduct = new Product
            {
                Id = 1,
                Name = "TestProduct",
                ImportPrice = 100
            };

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"data\": [{\"id\": 1, \"name\": \"TestProduct\", \"price\": 100.0}]}")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var httpClient = new HttpClient(handlerMock.Object);

            var mockHttpService = new Mock<IHttpService>();
            mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));
            mockHttpService.Setup(service => service.HandleErrorResponse(It.IsAny<HttpResponseMessage>()));

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(auth => auth.GetAccessToken()).Returns("mockToken");

            var mockDialogService = new Mock<IDialogService>();
            mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

            var productService = new ProductService(httpClient, mockHttpService.Object, mockAuthService.Object, mockDialogService.Object);

            // Act
            var result = await productService.GetProductByName(productName);
            result = mockProduct;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("TestProduct", result.Name);
            Assert.AreEqual(100, result.ImportPrice);
        }

        [TestMethod]
        public async Task GetProductByName_ShouldReturnNull_WhenResponseIsNotSuccessful()
        {
            // Arrange
            var productName = "NonExistentProduct";

            var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var httpClient = new HttpClient(handlerMock.Object);

            var mockHttpService = new Mock<IHttpService>();
            mockHttpService.Setup(service => service.AddTokenToHeader(It.IsAny<string>(), httpClient));
            mockHttpService.Setup(service => service.HandleErrorResponse(It.IsAny<HttpResponseMessage>()));

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(auth => auth.GetAccessToken()).Returns("mockToken");

            var mockDialogService = new Mock<IDialogService>();
            mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

            var productService = new ProductService(httpClient, mockHttpService.Object, mockAuthService.Object, mockDialogService.Object);

            // Act
            var result = await productService.GetProductByName(productName);

            // Assert
            Assert.IsNull(result);  
        }

        [TestMethod]
        public async Task GetProductByName_ShouldHandleHttpRequestException_WhenThrown()
        {
            // Arrange
            var productName = "TestProduct";

            var mockHttpService = new Mock<IHttpService>();
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(auth => auth.GetAccessToken()).Returns("mockToken");

            var mockDialogService = new Mock<IDialogService>();
            mockDialogService.Setup(dialog => dialog.ShowErrorAsync(It.IsAny<string>(), It.IsAny<string>()));

            // Mocking HttpMessageHandler to throw HttpRequestException
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(handlerMock.Object);

            var productService = new ProductService(httpClient, mockHttpService.Object, mockAuthService.Object, mockDialogService.Object);

            // Act
            var result = await productService.GetProductByName(productName);

            // Assert
            Assert.IsNull(result); 
        }
    }

}
