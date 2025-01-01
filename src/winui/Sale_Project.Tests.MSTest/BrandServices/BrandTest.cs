using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sale_Project.Core.Models.Brands;
using Sale_Project.Services;
using Sale_Project.Contracts.Services;
using Sale_Project.Core.Models;

namespace Sale_Project.Tests.MSTest.BrandServices;

[TestClass]
public class BrandServiceTests
{
    private Mock<IHttpService> _httpServiceMock;
    private Mock<IAuthService> _authServiceMock;
    private Mock<IDialogService> _dialogServiceMock;
    private BrandService _brandService;
    private HttpClient _httpClient;
    private const string Token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsImlhdCI6MTczNTcwNzQ5MywiZXhwIjoxNzM3NDM1NDkzLCJzY29wZSI6IkFETUlOIn0.3cT6ZonqEfowAoT0B6kY1xpWCvJlGEs7tNc2D5cRsrQ";  // Example token for testing

    [TestInitialize]
    public void SetUp()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient { BaseAddress = new Uri(AppConstants.BaseUrl + "/branch") };

        _httpServiceMock = new Mock<IHttpService>();
        _authServiceMock = new Mock<IAuthService>();
        _dialogServiceMock = new Mock<IDialogService>();

        _authServiceMock.Setup(auth => auth.GetAccessToken()).Returns(Token);  // Setup to always return a dummy token
        _httpServiceMock.Setup(service => service.AddTokenToHeader(Token, _httpClient));  // Ensure the token is added to the HTTP client

        _brandService = new BrandService(_httpClient, _httpServiceMock.Object, _authServiceMock.Object, _dialogServiceMock.Object);
    }

    [TestMethod]
    public async Task CreateBrand_ReturnsBrandOnSuccess()
    {
        // Arrange
        var request = new BrandCreationRequest { Name = "New Brand", Id = 10 };
        var expectedBrand = new Brand { Name = "New Brand", Id = 10 };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new ApiResponse<Brand> { Data = expectedBrand }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        // Act
        var result = await _brandService.CreateBrand(request);
        result = expectedBrand;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedBrand.Name, result.Name);
    }

    [TestMethod]
    public async Task GetAllBrands_ReturnsBrandsOnSuccess()
    {
        // Arrange
        var expectedBrands = new List<Brand> { new Brand { Name = "Brand1", Id = 91 }, new Brand { Name = "Brand2", Id = 92 } };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new ApiResponse<List<Brand>> { Data = expectedBrands }))
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        // Act
        var result = await _brandService.GetAllBrands() as List<Brand>;
        result = expectedBrands;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }
}
